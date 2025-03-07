using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Mappers
{
    public static class LocalisationMapper
    {
        public static LocalisationDto ToDto(Localisation localisation)
        {
            return new LocalisationDto
            {
                IdLocalisation = localisation.IdLocalisation,
                Designation = localisation.Designation
            };
        }

        public static Localisation ToEntity(LocalisationDto localisationDto)
        {
            return new Localisation
            {
                IdLocalisation = localisationDto.IdLocalisation,
                Designation = localisationDto.Designation
            };
        }
    }
}