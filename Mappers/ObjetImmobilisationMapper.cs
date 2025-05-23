using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;
namespace LimsImmobilisationService.Mappers
{
    public static class ObjetIndisponibiliteMapper
    {
        public static ObjetIndisponibiliteDto? ToDto(ObjetIndisponibilite? entity)
        {
            if (entity == null)
                return null;
            return new ObjetIndisponibiliteDto
            {
                IdObjetIndisponibilite = entity.IdObjetIndisponibilite,
                Designation = entity.Designation
            };
        }

        public static ObjetIndisponibilite? ToEntity(ObjetIndisponibiliteDto? dto)
        {
            if (dto == null)
                return null;
            return new ObjetIndisponibilite
            {
                IdObjetIndisponibilite = dto.IdObjetIndisponibilite,
                Designation = dto.Designation ?? string.Empty
            };
        }
    }
}