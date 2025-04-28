using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Models
{
    [Table("Assignation")] // Spécifie le nom de la table dans la base de données
    public class Assignation
    {
        [Key] // Indique que cette propriété est la clé primaire
        [Column("id_assignation")] // Spécifie le nom de la colonne dans la base de données
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indique que la valeur est auto-incrémentée
        public int IdAssignation { get; set; }

        [Required] // Indique que la propriété est non nullable
        [Column("date_assignation")]
        public DateTime DateAssignation { get; set; } // Correspond à date

        [Column("id_localisation")] // Clé étrangère nullable
        public int? IdLocalisation { get; set; } // int? car le champ est nullable (YES)

        [Required] // Indique que la propriété est non nullable
        [Column("id_immobilisation_propre")]
        public int IdImmobilisationPropre { get; set; } // Clé étrangère non nullable

        [Required] // Indique que la propriété est non nullable
        [Column("id_employe")]
        public int IdEmploye { get; set; } // Clé étrangère non nullable

        // Propriétés de navigation pour les relations
        [ForeignKey("IdLocalisation")] // Indique que cette propriété est une clé étrangère
        public Localisation? Localisation { get; set; } // Relation avec la table Localisation (nullable)

        [ForeignKey("IdImmobilisationPropre")] // Indique que cette propriété est une clé étrangère
        public ImmobilisationImmatriculation? ImmobilisationImmatriculation { get; set; } // Relation corrigée avec la table immobilisation_immatriculation (non nullable)

        [ForeignKey("IdEmploye")] // Indique que cette propriété est une clé étrangère
        public Employe? Employe { get; set; } // Relation avec la table Employe (non nullable)
    }
}