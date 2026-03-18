using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFastOrder.Data;
using EFastOrder.Models;

namespace EFastOrder.Controllers
{
    public class CommandeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommandeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Commande
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var commandes = await _context.Commandes
                .Include(c => c.Items)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();
            return View(commandes);
        }

        // GET: Commande/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var commande = await _context.Commandes
                .Include(c => c.Items!)
                    .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (commande == null) return NotFound();

            // Vérifier que l'utilisateur a accès à cette commande
            var userId = GetUserId();
            if (commande.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return View(commande);
        }

        // GET: Commande/Passer
        public async Task<IActionResult> Passer()
        {
            var userId = GetUserId();
            var panier = await _context.Paniers
                .Include(p => p.Items!)
                    .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (panier == null || panier.Items == null || !panier.Items.Any())
            {
                TempData["Error"] = "Votre panier est vide. Ajoutez des articles avant de passer commande.";
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new PasserCommandeViewModel
            {
                Panier = panier
            };

            return View(viewModel);
        }

        // POST: Commande/Confirmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmer(string? notes)
        {
            var userId = GetUserId();
            var panier = await _context.Paniers
                .Include(p => p.Items!)
                    .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (panier == null || panier.Items == null || !panier.Items.Any())
            {
                TempData["Error"] = "Votre panier est vide.";
                return RedirectToAction("Index", "Home");
            }

            // Créer la commande
            var commande = new Commande
            {
                UserId = userId,
                NumeroCommande = $"CMD-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}",
                DateCommande = DateTime.Now,
                Statut = StatutCommande.EnAttente,
                Notes = notes,
                Items = new List<CommandeItem>()
            };

            decimal total = 0;
            foreach (var item in panier.Items)
            {
                var commandeItem = new CommandeItem
                {
                    ArticleId = item.ArticleId,
                    NomArticle = item.Article?.Nom ?? "Article inconnu",
                    Quantite = item.Quantite,
                    PrixUnitaire = item.PrixUnitaire
                };
                commande.Items.Add(commandeItem);
                total += item.PrixUnitaire * item.Quantite;
            }
            commande.MontantTotal = total;

            _context.Commandes.Add(commande);

            // Vider le panier
            _context.PanierItems.RemoveRange(panier.Items);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Commande {commande.NumeroCommande} passée avec succès ! Montant : {commande.MontantTotal:C}";
            return RedirectToAction(nameof(Details), new { id = commande.Id });
        }

        // POST: Commande/Annuler/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Annuler(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null) return NotFound();

            var userId = GetUserId();
            if (commande.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            if (commande.Statut == StatutCommande.EnAttente)
            {
                commande.Statut = StatutCommande.Annulee;
                await _context.SaveChangesAsync();
                TempData["Success"] = "La commande a été annulée.";
            }
            else
            {
                TempData["Error"] = "Cette commande ne peut plus être annulée.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Admin: Toutes les commandes
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> Toutes()
        {
            var commandes = await _context.Commandes
                .Include(c => c.Items)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();
            return View(commandes);
        }

        // Admin: Changer le statut
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangerStatut(int id, StatutCommande statut)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null) return NotFound();

            commande.Statut = statut;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Statut de la commande {commande.NumeroCommande} mis à jour.";
            return RedirectToAction(nameof(Toutes));
        }

        private string GetUserId()
        {
            if (User.Identity?.IsAuthenticated == true)
                return User.Identity.Name ?? "anonymous";

            var userId = HttpContext.Request.Cookies["EFastOrderUserId"];
            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString();
                HttpContext.Response.Cookies.Append("EFastOrderUserId", userId, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true
                });
            }
            return userId;
        }
    }
}
