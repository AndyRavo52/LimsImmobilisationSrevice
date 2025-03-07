using Microsoft.EntityFrameworkCore;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Data
{
    public class ImmobilisationContext : DbContext
    {
        public ImmobilisationContext(DbContextOptions<ImmobilisationContext> options)
            : base(options)
        {
        }

        public DbSet<Immobilisation> Immobilisations { get; set; }
        //public DbSet<ObjetIndisponibilite> ObjetIndisponibilites { get; set; }
        //public DbSet<Indisponibilite> Indisponibilites { get; set; }
        //public DbSet<ReportImmobilisation> ReportImmobilisations { get; set; }
        public DbSet<Localisation> Localisations { get; set; }
        //public DbSet<Assignation> Assignations { get; set; }
        //public DbSet<Anomalie> Anomalies { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        //public DbSet<EntreeImmobilisation> EntreeImmobilisations { get; set; }
        //public DbSet<ReformeImmob> ReformeImmobs { get; set; }
        //public DbSet<ImmobilisationImmatriculation> ImmobilisationImmatriculations { get; set; }
        //public DbSet<Employe> Employes { get; set; }
        public DbSet<Marque> Marques { get; set; }
    }
}
