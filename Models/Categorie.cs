using System.ComponentModel.DataAnnotations;

namespace EFastOrder.Models
{
    public class Categorie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la catégorie est requis")]
        [StringLength(100)]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Icône")]
        [StringLength(50)]
        public string? Icone { get; set; }

        // Navigation
        public ICollection<Article>? Articles { get; set; }
    }
}
