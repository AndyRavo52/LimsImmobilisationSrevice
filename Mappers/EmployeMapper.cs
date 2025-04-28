using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Mappers
{
    public static class EmployeMapper
    {
        // Convertit une entité Employe en EmployeDto
        public static EmployeDto ToDto(Employe employe)
        {
            return new EmployeDto
            {
                IdEmploye = employe.IdEmploye,
                Matricule = employe.Matricule,
                Nom = employe.Nom,
                Prenom = employe.Prenom,
             
                Cin = employe.Cin,
                Contact = employe.Contact,
                Adresse = employe.Adresse,
                Manager = employe.Manager,
                Statut = employe.Statut
            };
        }

        // Convertit un EmployeDto en entité Employe
        public static Employe ToEntity(EmployeDto employeDto)
        {
            return new Employe
            {
                IdEmploye = employeDto.IdEmploye,
                Matricule = employeDto.Matricule,
                Nom = employeDto.Nom,
                Prenom = employeDto.Prenom,
               
                Cin = employeDto.Cin,
                Contact = employeDto.Contact,
                Adresse = employeDto.Adresse,
                Manager = employeDto.Manager,
                Statut = employeDto.Statut
            };
        }
    }
}