using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EFastOrder.Data;
using EFastOrder.Models;

namespace EFastOrder.Controllers
{
    /// <summary>
    /// Tableau de bord administrateur
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                NombreArticles = await _context.Articles.CountAsync(),
                NombreCategories = await _context.Categories.CountAsync(),
                NombreCommandes = await _context.Commandes.CountAsync(),
                RevenuTotal = await _context.Commandes
                    .Where(c => c.Statut != StatutCommande.Annulee)
                    .SumAsync(c => c.MontantTotal),
                DernieresCommandes = await _context.Commandes
                    .Include(c => c.Items)
                    .OrderByDescending(c => c.DateCommande)
                    .Take(10)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}
