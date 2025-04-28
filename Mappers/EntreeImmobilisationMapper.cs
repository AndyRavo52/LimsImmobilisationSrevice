using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Mappers
{
    public static class EntreeImmobilisationMapper
    {
        public static EntreeImmobilisationDto ToDto(EntreeImmobilisation entreeImmobilisation)
        {
            return new EntreeImmobilisationDto
            {
                IdEntreeImmobilisation = entreeImmobilisation.IdEntreeImmobilisation,
                Quantite = entreeImmobilisation.Quantite,
                PrixAchat = entreeImmobilisation.PrixAchat,
                DateEntree = entreeImmobilisation.DateEntree,
                BonReception = entreeImmobilisation.BonReception,
                BonDeCommande = entreeImmobilisation.BonDeCommande,
                NumeroFacture = entreeImmobilisation.NumeroFacture,
                IdFournisseur = entreeImmobilisation.IdFournisseur,
                Fournisseur = entreeImmobilisation.Fournisseur != null ? FournisseurMapper.ToDto(entreeImmobilisation.Fournisseur) : null,
                IdImmobilisation = entreeImmobilisation.IdImmobilisation,
                Immobilisation = entreeImmobilisation.Immobilisation != null ? ImmobilisationMapper.ToDto(entreeImmobilisation.Immobilisation) : null
            };
        }

        public static EntreeImmobilisation ToEntity(EntreeImmobilisationDto entreeImmobilisationDto)
        {
            return new EntreeImmobilisation
            {
                IdEntreeImmobilisation = entreeImmobilisationDto.IdEntreeImmobilisation,
                Quantite = entreeImmobilisationDto.Quantite,
                PrixAchat = entreeImmobilisationDto.PrixAchat,
                DateEntree = entreeImmobilisationDto.DateEntree,
                BonReception = entreeImmobilisationDto.BonReception,
                BonDeCommande = entreeImmobilisationDto.BonDeCommande,
                NumeroFacture = entreeImmobilisationDto.NumeroFacture,
                IdFournisseur = entreeImmobilisationDto.IdFournisseur,
                IdImmobilisation = entreeImmobilisationDto.IdImmobilisation
            };
        }
    }
}