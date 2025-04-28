using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LimsImmobilisationService.Models
{
    [Table("Indisponibilité")] // Spécifie le nom de la table dans la base de données
    public class Indisponibilite
    {
        [Key] // Indique que cette propriété est la clé primaire
        [Column("id_indisponibilité")] // Spécifie le nom de la colonne dans la base de données
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indique que la valeur est auto-incrémentée
        public int IdIndisponibilite { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("date_debut")] // Spécifie le nom de la colonne dans la base de données
        public DateTime DateDebut { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("date_fin")] // Spécifie le nom de la colonne dans la base de données
        public DateTime DateFin { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("id_immobilisation_propre")] // Spécifie le nom de la colonne dans la base de données
        public int IdImmobilisationPropre { get; set; } // Clé étrangère vers la table ImmobilisationImmatriculation

        [Required] // Indique que la propriété est non nullable
        [Column("id_objet_indisponibilité")] // Spécifie le nom de la colonne dans la base de données
        public int IdObjetIndisponibilite { get; set; } // Clé étrangère vers la table ObjetIndisponibilite

        // Propriétés de navigation pour les relations
        [ForeignKey("IdImmobilisationPropre")] // Indique que cette propriété est une clé étrangère
        public ImmobilisationImmatriculation? ImmobilisationImmatriculation { get; set; } // Relation avec ImmobilisationImmatriculation

        [ForeignKey("IdObjetIndisponibilite")] // Indique que cette propriété est une clé étrangère
        public ObjetIndisponibilite? ObjetIndisponibilite { get; set; } // Relation avec ObjetIndisponibilite
    }
}