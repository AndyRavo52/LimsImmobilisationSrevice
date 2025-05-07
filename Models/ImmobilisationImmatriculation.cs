using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsImmobilisationService.Models
{
    [Table("Immobilisation_immatriculation")]
    public class ImmobilisationImmatriculation
    {
        [Key]
        [Column("id_immobilisation_propre")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdImmobilisationPropre { get; set; }

        [Required]
        [Column("matricule")]
        [StringLength(50)]
        public string? Matricule { get; set; }

        [Column("remarque")]
        public string? Remarque { get; set; }

        [Required]
        [Column("etat_initiale")]
        public string? EtatInitiale { get; set; }

        [Column("id_entree_immobilisation")]
        public int? IdEntreeImmobilisation { get; set; }

        [ForeignKey("IdEntreeImmobilisation")]
        public EntreeImmobilisation? EntreeImmobilisation { get; set; }
    }
}