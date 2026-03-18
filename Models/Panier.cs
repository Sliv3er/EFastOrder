using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFastOrder.Models
{
    public class Panier
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Date de création")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<PanierItem>? Items { get; set; }

        [NotMapped]
        [Display(Name = "Total")]
        public decimal Total => Items?.Sum(i => i.SousTotal) ?? 0;

        [NotMapped]
        [Display(Name = "Nombre d'articles")]
        public int NombreArticles => Items?.Sum(i => i.Quantite) ?? 0;
    }

    public class PanierItem
    {
        public int Id { get; set; }

        [Required]
        public int PanierId { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required]
        [Range(1, 99, ErrorMessage = "La quantité doit être entre 1 et 99")]
        [Display(Name = "Quantité")]
        public int Quantite { get; set; } = 1;

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Prix unitaire")]
        public decimal PrixUnitaire { get; set; }

        [NotMapped]
        [Display(Name = "Sous-total")]
        public decimal SousTotal => PrixUnitaire * Quantite;

        // Navigation
        [ForeignKey("PanierId")]
        public Panier? Panier { get; set; }

        [ForeignKey("ArticleId")]
        public Article? Article { get; set; }
    }
}
