using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Mappers
{
    public static class FournisseurMapper
    {
        public static FournisseurDto ToDto(Fournisseur fournisseur)
        {
            return new FournisseurDto
            {
                IdFournisseur = fournisseur.IdFournisseur,
                Designation = fournisseur.Designation
            };
        }

        public static Fournisseur ToEntity(FournisseurDto fournisseurDto)
        {
            return new Fournisseur
            {
                IdFournisseur = fournisseurDto.IdFournisseur,
                Designation = fournisseurDto.Designation
            };
        }
    }
}