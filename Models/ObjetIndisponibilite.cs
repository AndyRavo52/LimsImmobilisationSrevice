using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsImmobilisationService.Models
{
    [Table("objet_indisponibilité")] // Spécifie le nom de la table dans la base de données
    public class ObjetIndisponibilite
    {
        [Key] // Indique que cette propriété est la clé primaire
        [Column("id_objet_indisponibilité")] // Spécifie le nom de la colonne dans la base de données
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indique que la valeur est auto-incrémentée
        public int IdObjetIndisponibilite { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("designation")] // Spécifie le nom de la colonne dans la base de données
        public required string Designation { get; set; }
    }
}