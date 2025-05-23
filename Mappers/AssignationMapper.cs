using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;
using System.Text.Json.Serialization;

namespace LimsImmobilisationService.Mappers
{
    public static class AssignationMapper
    {
        public static AssignationDto? ToDto(Assignation? assignation)
        {
            if (assignation == null) return null;

            return new AssignationDto
            {
                IdAssignation = assignation.IdAssignation,
                DateAssignation = assignation.DateAssignation,
                IdLocalisation = assignation.IdLocalisation,
                IdImmobilisationPropre = assignation.IdImmobilisationPropre,
                IdEmploye = assignation.IdEmploye,
                Localisation = assignation.Localisation != null ? LocalisationMapper.ToDto(assignation.Localisation) : null,
                ImmobilisationImmatriculation = assignation.ImmobilisationImmatriculation != null ? ImmobilisationImmatriculationMapper.ToDto(assignation.ImmobilisationImmatriculation) : null,
                Employe = assignation.Employe != null ? EmployeMapper.ToDto(assignation.Employe) : null
            };
        }

        public static Assignation? ToEntity(AssignationDto? assignationDto)
        {
            if (assignationDto == null)
                return null;

            return new Assignation
            {
                IdAssignation = assignationDto.IdAssignation,
                DateAssignation = assignationDto.DateAssignation,
                IdLocalisation = assignationDto.IdLocalisation,
                IdImmobilisationPropre = assignationDto.IdImmobilisationPropre,
                IdEmploye = assignationDto.IdEmploye,
                Localisation = assignationDto.Localisation != null 
                    ? LocalisationMapper.ToEntity(assignationDto.Localisation) 
                    : null,
                ImmobilisationImmatriculation = assignationDto.ImmobilisationImmatriculation != null 
                    ? ImmobilisationImmatriculationMapper.ToEntity(assignationDto.ImmobilisationImmatriculation) 
                    : null,
                Employe = assignationDto.Employe != null 
                    ? EmployeMapper.ToEntity(assignationDto.Employe) 
                    : null
            };
        }

        public static Assignation? ToEntitySimple(AssignationDto? assignationDto)
        {
            if (assignationDto == null)
                return null;

            return new Assignation
            {
                // IdAssignation n'est pas défini explicitement pour la création (auto-incrémenté par la DB)
                DateAssignation = assignationDto.DateAssignation,
                IdLocalisation = assignationDto.IdLocalisation,
                IdImmobilisationPropre = assignationDto.IdImmobilisationPropre,
                IdEmploye = assignationDto.IdEmploye
            };
        }
    }
}