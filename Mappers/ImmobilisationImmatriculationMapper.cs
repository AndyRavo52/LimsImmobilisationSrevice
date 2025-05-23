using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Mappers
{
    public static class ImmobilisationImmatriculationMapper
    {
        public static ImmobilisationImmatriculationDto? ToDto(ImmobilisationImmatriculation? immobilisationImmatriculation)
        {
            if (immobilisationImmatriculation == null)
                return null;

            return new ImmobilisationImmatriculationDto
            {
                IdImmobilisationPropre = immobilisationImmatriculation.IdImmobilisationPropre,
                Matricule = immobilisationImmatriculation.Matricule,
                Remarque = immobilisationImmatriculation.Remarque,
                EtatInitiale = immobilisationImmatriculation.EtatInitiale,
                IdEntreeImmobilisation = immobilisationImmatriculation.IdEntreeImmobilisation,
                EntreeImmobilisation = immobilisationImmatriculation.EntreeImmobilisation != null 
                    ? EntreeImmobilisationMapper.ToDto(immobilisationImmatriculation.EntreeImmobilisation) 
                    : null
            };
        }

        public static ImmobilisationImmatriculation? ToEntity(ImmobilisationImmatriculationDto? immobilisationImmatriculationDto)
        {
            if (immobilisationImmatriculationDto == null)
                return null;

            return new ImmobilisationImmatriculation
            {
                IdImmobilisationPropre = immobilisationImmatriculationDto.IdImmobilisationPropre,
                Matricule = immobilisationImmatriculationDto.Matricule,
                Remarque = immobilisationImmatriculationDto.Remarque,
                EtatInitiale = immobilisationImmatriculationDto.EtatInitiale,
                // IdImmobilisation = immobilisationImmatriculationDto.IdImmobilisation,
                IdEntreeImmobilisation = immobilisationImmatriculationDto.IdEntreeImmobilisation
            };
        }
    }
}