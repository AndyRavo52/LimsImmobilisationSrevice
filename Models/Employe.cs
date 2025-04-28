using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LimsImmobilisationService.Models
{
    [Table("Employe")] // Spécifie le nom de la table dans la base de données
    public class Employe
    {
        [Key] // Indique que cette propriété est la clé primaire
        [Column("id_employe")] // Spécifie le nom de la colonne dans la base de données
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indique que la valeur est auto-incrémentée
        public int IdEmploye { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("matricule")]
        [StringLength(50)] // Correspond à varchar(50)
        public required string Matricule { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("nom")]
        [StringLength(50)] // Correspond à varchar(50)
        public required string Nom { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("prenom")]
        [StringLength(50)] // Correspond à varchar(50)
        public required string Prenom { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("genre")]
        public bool Genre { get; set; } // bit(1) est mappé à bool en C#

        [Required] // Indique que la propriété est non nullable
        [Column("cin")]
        [StringLength(12, MinimumLength = 12)] // Correspond à char(12)
        public required string Cin { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("contact")]
        [StringLength(10, MinimumLength = 10)] // Correspond à char(10)
        public required string Contact { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("adresse")]
        [StringLength(50)] // Correspond à varchar(50)
        public required string Adresse { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("manager")]
        [StringLength(50)] // Correspond à varchar(50)
        public required string Manager { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("statut")]
        public int Statut { get; set; } // Par défaut 0


    }
}
