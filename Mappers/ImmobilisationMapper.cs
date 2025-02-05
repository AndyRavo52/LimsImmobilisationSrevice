using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Mappers
{
    public static class ImmobilisationMapper
    {
        // Convertit une entité Immobilisation en ImmobilisationDto
        public static ImmobilisationDto ToDto(Immobilisation immobilisation)
        {
            return new ImmobilisationDto
            {
                IdImmobilisation = immobilisation.IdImmobilisation,
                Reference = immobilisation.Reference,
                Designation = immobilisation.Designation,
                IdMarque = immobilisation.IdMarque,
                Marque = immobilisation.Marque != null ? new MarqueDto
                {
                    IdMarque = immobilisation.Marque.IdMarque,
                    Designation = immobilisation.Marque.Designation
                } : null
            };
        }

        // Convertit un ImmobilisationDto en entité Immobilisation
        public static Immobilisation ToEntity(ImmobilisationDto immobilisationDto)
        {
            return new Immobilisation
            {
                IdImmobilisation = immobilisationDto.IdImmobilisation,
                Reference = immobilisationDto.Reference,
                Designation = immobilisationDto.Designation,
                IdMarque = immobilisationDto.IdMarque
            };
        }
    }
}