using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsImmobilisationService.Models
{
    [Table("Immobilisation")] // Spécifie le nom de la table dans la base de données
    public class Immobilisation
    {
        [Key] // Indique que cette propriété est la clé primaire
        [Column("id_immobilisation")] // Spécifie le nom de la colonne dans la base de données
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indique que la valeur est auto-incrémentée
        public int IdImmobilisation { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("reference")]
        public required string Reference { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("designation")]
        public required string Designation { get; set; }

        [Column("id_marque")] // Spécifie le nom de la colonne dans la base de données
        public int IdMarque { get; set; } // Clé étrangère vers la table Marque

        // Propriété de navigation pour la relation avec Marque
        [ForeignKey("IdMarque")] // Indique que cette propriété est une clé étrangère
        public  Marque? Marque { get; set; }
    }
}