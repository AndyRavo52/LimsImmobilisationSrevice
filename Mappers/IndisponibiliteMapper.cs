using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Mappers
{
    public static class IndisponibiliteMapper
    {
        // Convertit une entité Indisponibilite en IndisponibiliteDto
        public static IndisponibiliteDto ToDto(Indisponibilite indisponibilite)
        {
                return new IndisponibiliteDto
        {
            IdIndisponibilite = indisponibilite.IdIndisponibilite,
            DateDebut = indisponibilite.DateDebut,
            DateFin = indisponibilite.DateFin,
            IdImmobilisationPropre = indisponibilite.IdImmobilisationPropre,
            IdObjetIndisponibilite = indisponibilite.IdObjetIndisponibilite,
            ImmobilisationImmatriculation = indisponibilite.ImmobilisationImmatriculation != null 
                ? ImmobilisationImmatriculationMapper.ToDto(indisponibilite.ImmobilisationImmatriculation) 
                : null,
            ObjetIndisponibilite = indisponibilite.ObjetIndisponibilite != null ? new ObjetIndisponibiliteDto
            {
                IdObjetIndisponibilite = indisponibilite.ObjetIndisponibilite.IdObjetIndisponibilite,
                Designation = indisponibilite.ObjetIndisponibilite.Designation
            } : null
        };
        }

        // Convertit un IndisponibiliteDto en entité Indisponibilite
        public static Indisponibilite ToEntity(IndisponibiliteDto indisponibiliteDto)
        {
            return new Indisponibilite
            {
                IdIndisponibilite = indisponibiliteDto.IdIndisponibilite,
                DateDebut = indisponibiliteDto.DateDebut,
                DateFin = indisponibiliteDto.DateFin,
                IdImmobilisationPropre = indisponibiliteDto.IdImmobilisationPropre,
                IdObjetIndisponibilite = indisponibiliteDto.IdObjetIndisponibilite
                // Les propriétés de navigation (ImmobilisationImmatriculation et ObjetIndisponibilite) ne sont pas mappées ici,
                // car elles sont généralement gérées par EF Core via les clés étrangères
            };
        }
    }
}