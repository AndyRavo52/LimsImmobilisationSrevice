using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsImmobilisationService.Models
{
    [Table("Entree_immobilisation")]
    public class EntreeImmobilisation
    {
        [Key]
        [Column("id_entree_immobilisation")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEntreeImmobilisation { get; set; }

        [Column("quantite")]
        public int? Quantite { get; set; }

        [Column("prix_achat")]
        public decimal? PrixAchat { get; set; }

        [Column("date_entrée")]
        public DateTime? DateEntree { get; set; }

        [Required]
        [Column("bon_reception")]
        [StringLength(50)]
        public string? BonReception { get; set; }

        [Required]
        [Column("bon_de_commande")]
        [StringLength(50)]
        public string? BonDeCommande { get; set; }

        [Required]
        [Column("numero_facture")]
        [StringLength(50)]
        public string? NumeroFacture { get; set; }

        [Required]
        [Column("id_fournisseur")]
        public int IdFournisseur { get; set; }

        [ForeignKey("IdFournisseur")]
        public Fournisseur? Fournisseur { get; set; }

        [Column("id_immobilisation")]
        public int? IdImmobilisation { get; set; }

        [ForeignKey("IdImmobilisation")]
        public Immobilisation? Immobilisation { get; set; }
    }
}