using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFastOrder.Models
{
    public enum StatutCommande
    {
        [Display(Name = "En attente")]
        EnAttente,
        [Display(Name = "En préparation")]
        EnPreparation,
        [Display(Name = "Prête")]
        Prete,
        [Display(Name = "Livrée")]
        Livree,
        [Display(Name = "Annulée")]
        Annulee
    }

    public class Commande
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Numéro de commande")]
        public string NumeroCommande { get; set; } = string.Empty;

        [Display(Name = "Date de commande")]
        public DateTime DateCommande { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Montant total (DT)")]
        public decimal MontantTotal { get; set; }

        [Display(Name = "Statut")]
        public StatutCommande Statut { get; set; } = StatutCommande.EnAttente;

        [StringLength(500)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        // Navigation
        public ICollection<CommandeItem>? Items { get; set; }
    }

    public class CommandeItem
    {
        public int Id { get; set; }

        [Required]
        public int CommandeId { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required]
        [Display(Name = "Nom de l'article")]
        public string NomArticle { get; set; } = string.Empty;

        [Required]
        [Range(1, 99)]
        [Display(Name = "Quantité")]
        public int Quantite { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Prix unitaire (DT)")]
        public decimal PrixUnitaire { get; set; }

        [NotMapped]
        [Display(Name = "Sous-total")]
        public decimal SousTotal => PrixUnitaire * Quantite;

        // Navigation
        [ForeignKey("CommandeId")]
        public Commande? Commande { get; set; }

        [ForeignKey("ArticleId")]
        public Article? Article { get; set; }
    }
}
