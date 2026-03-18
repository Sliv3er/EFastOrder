using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFastOrder.Data;
using EFastOrder.Models;

namespace EFastOrder.Controllers
{
    public class PanierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PanierController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Panier
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var panier = await GetOrCreatePanier(userId);

            var viewModel = new PanierViewModel
            {
                Panier = panier,
                Total = panier.Total,
                NombreArticles = panier.NombreArticles
            };

            return View(viewModel);
        }

        // POST: Panier/Ajouter
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(int articleId, int quantite = 1)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null || !article.IsDisponible)
            {
                TempData["Error"] = "Cet article n'est pas disponible.";
                return RedirectToAction("Index", "Home");
            }

            var userId = GetUserId();
            var panier = await GetOrCreatePanier(userId);

            // Vérifier si l'article est déjà dans le panier
            var existingItem = panier.Items?.FirstOrDefault(i => i.ArticleId == articleId);
            if (existingItem != null)
            {
                existingItem.Quantite += quantite;
                _context.PanierItems.Update(existingItem);
            }
            else
            {
                var panierItem = new PanierItem
                {
                    PanierId = panier.Id,
                    ArticleId = articleId,
                    Quantite = quantite,
                    PrixUnitaire = article.Prix
                };
                _context.PanierItems.Add(panierItem);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"\"{article.Nom}\" ajouté au panier !";
            return RedirectToAction("Index", "Home");
        }

        // POST: Panier/ModifierQuantite
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifierQuantite(int itemId, int quantite)
        {
            var item = await _context.PanierItems
                .Include(i => i.Article)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null) return NotFound();

            if (quantite <= 0)
            {
                _context.PanierItems.Remove(item);
                TempData["Success"] = $"\"{item.Article?.Nom}\" retiré du panier.";
            }
            else
            {
                item.Quantite = quantite;
                _context.PanierItems.Update(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Panier/Retirer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retirer(int itemId)
        {
            var item = await _context.PanierItems
                .Include(i => i.Article)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null) return NotFound();

            _context.PanierItems.Remove(item);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"\"{item.Article?.Nom}\" retiré du panier.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Panier/Vider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vider()
        {
            var userId = GetUserId();
            var panier = await _context.Paniers
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (panier?.Items != null)
            {
                _context.PanierItems.RemoveRange(panier.Items);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Le panier a été vidé.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Panier> GetOrCreatePanier(string userId)
        {
            var panier = await _context.Paniers
                .Include(p => p.Items!)
                    .ThenInclude(i => i.Article)
                        .ThenInclude(a => a!.Categorie)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (panier == null)
            {
                panier = new Panier { UserId = userId };
                _context.Paniers.Add(panier);
                await _context.SaveChangesAsync();
                panier.Items = new List<PanierItem>();
            }

            return panier;
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
