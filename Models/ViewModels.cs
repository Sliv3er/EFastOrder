namespace EFastOrder.Models
{
    public class MenuViewModel
    {
        public List<Categorie> Categories { get; set; } = new();
        public List<Article> Articles { get; set; } = new();
        public int? CategorieSelectionnee { get; set; }
        public string? Recherche { get; set; }
    }

    public class PanierViewModel
    {
        public Panier? Panier { get; set; }
        public decimal Total { get; set; }
        public int NombreArticles { get; set; }
    }

    public class DashboardViewModel
    {
        public int NombreArticles { get; set; }
        public int NombreCategories { get; set; }
        public int NombreCommandes { get; set; }
        public decimal RevenuTotal { get; set; }
        public List<Commande> DernieresCommandes { get; set; } = new();
        public List<Article> ArticlesPopulaires { get; set; } = new();
    }

    public class PasserCommandeViewModel
    {
        public Panier? Panier { get; set; }
        public string? Notes { get; set; }
    }
}
