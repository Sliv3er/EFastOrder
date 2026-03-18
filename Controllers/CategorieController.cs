using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EFastOrder.Data;
using EFastOrder.Models;

namespace EFastOrder.Controllers
{
    /// <summary>
    /// Contrôleur CRUD pour les catégories - Utilise ADO.NET (pas Entity Framework)
    /// Ceci démontre l'utilisation d'ADO.NET comme exigé dans le cahier des charges.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class CategorieController : Controller
    {
        private readonly CategorieAdoRepository _repository;

        public CategorieController(CategorieAdoRepository repository)
        {
            _repository = repository;
        }

        // GET: Categorie
        [AllowAnonymous]
        public IActionResult Index()
        {
            var categories = _repository.GetAll();
            return View(categories);
        }

        // GET: Categorie/Details/5
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var categorie = _repository.GetById(id);
            if (categorie == null) return NotFound();
            return View(categorie);
        }

        // GET: Categorie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                _repository.Create(categorie);
                TempData["Success"] = $"La catégorie \"{categorie.Nom}\" a été créée avec succès !";
                return RedirectToAction(nameof(Index));
            }
            return View(categorie);
        }

        // GET: Categorie/Edit/5
        public IActionResult Edit(int id)
        {
            var categorie = _repository.GetById(id);
            if (categorie == null) return NotFound();
            return View(categorie);
        }

        // POST: Categorie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categorie categorie)
        {
            if (id != categorie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var success = _repository.Update(categorie);
                if (!success) return NotFound();
                TempData["Success"] = $"La catégorie \"{categorie.Nom}\" a été modifiée avec succès !";
                return RedirectToAction(nameof(Index));
            }
            return View(categorie);
        }

        // GET: Categorie/Delete/5
        public IActionResult Delete(int id)
        {
            var categorie = _repository.GetById(id);
            if (categorie == null) return NotFound();

            ViewBag.HasArticles = _repository.HasArticles(id);
            return View(categorie);
        }

        // POST: Categorie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_repository.HasArticles(id))
            {
                TempData["Error"] = "Impossible de supprimer cette catégorie car elle contient des articles.";
                return RedirectToAction(nameof(Index));
            }

            var categorie = _repository.GetById(id);
            var success = _repository.Delete(id);
            if (success)
                TempData["Success"] = $"La catégorie \"{categorie?.Nom}\" a été supprimée avec succès !";
            
            return RedirectToAction(nameof(Index));
        }
    }
}
