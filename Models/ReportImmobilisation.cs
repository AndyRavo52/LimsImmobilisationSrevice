using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsImmobilisationService.Models
{
    [Table("report_immobilisation")] // Nom de la table dans la base
    public class ReportImmobilisation
    {
        [Key]
        [Column("id_report_immobilisation")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdReportImmobilisation { get; set; }

        [Required]
        [Column("date_report")]
        public DateTime DateReport { get; set; }

        [Required]
        [Column("quantite")]
        public double Quantite { get; set; }

        [Column("id_immobilisation")]
        public int? IdImmobilisation { get; set; }

        [ForeignKey("IdImmobilisation")]
        public Immobilisation? Immobilisation { get; set; }
    }
}
