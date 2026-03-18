using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFastOrder.Data;
using EFastOrder.Models;

namespace EFastOrder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categorieId, string? recherche)
        {
            var categories = await _context.Categories.ToListAsync();
            var articlesQuery = _context.Articles
                .Include(a => a.Categorie)
                .Where(a => a.IsDisponible);

            if (categorieId.HasValue && categorieId.Value > 0)
            {
                articlesQuery = articlesQuery.Where(a => a.CategorieId == categorieId.Value);
            }

            if (!string.IsNullOrEmpty(recherche))
            {
                articlesQuery = articlesQuery.Where(a =>
                    a.Nom.Contains(recherche) || (a.Description != null && a.Description.Contains(recherche)));
            }

            var viewModel = new MenuViewModel
            {
                Categories = categories,
                Articles = await articlesQuery.OrderBy(a => a.CategorieId).ThenBy(a => a.Nom).ToListAsync(),
                CategorieSelectionnee = categorieId,
                Recherche = recherche
            };

            // Nombre d'articles dans le panier pour le badge
            var userId = GetUserId();
            var panier = await _context.Paniers
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.UserId == userId);
            ViewBag.NombreArticlesPanier = panier?.NombreArticles ?? 0;

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        private string GetUserId()
        {
            if (User.Identity?.IsAuthenticated == true)
                return User.Identity.Name ?? "anonymous";
            
            // Pour les utilisateurs non connectés, utiliser un cookie
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
