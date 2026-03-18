using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFastOrder.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de l'article est requis")]
        [StringLength(200)]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Le prix est requis")]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Prix (DT)")]
        [Range(0.01, 9999.99, ErrorMessage = "Le prix doit être entre 0.01 et 9999.99")]
        public decimal Prix { get; set; }

        [Display(Name = "Image URL")]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Disponible")]
        public bool IsDisponible { get; set; } = true;

        [Required(ErrorMessage = "La catégorie est requise")]
        [Display(Name = "Catégorie")]
        public int CategorieId { get; set; }

        [Display(Name = "Temps de préparation (min)")]
        public int? TempPreparation { get; set; }

        // Navigation
        [ForeignKey("CategorieId")]
        public Categorie? Categorie { get; set; }
    }
}
