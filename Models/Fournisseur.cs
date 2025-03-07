using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsImmobilisationService.Models
{
    [Table("Fournisseur")]
    public class Fournisseur
    {
        [Key]
        [Column("id_fournisseur")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFournisseur { get; set; }

        [Required]
        [Column("designation")]
        [StringLength(50)]
        public required string Designation { get; set; }
    }
}