using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsImmobilisationService.Models
{
    [Table("Localisation")]
    public class Localisation
    {
        [Key]
        [Column("id_localisation")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLocalisation { get; set; }

        [Required]
        [Column("designation")]
        [StringLength(50)]
        public required string Designation { get; set; }
    }
}