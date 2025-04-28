using LimsImmobilisationService.Data;
using LimsImmobilisationService.Services;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // Ajoutez cette ligne pour utiliser MySqlServerVersion

var builder = WebApplication.CreateBuilder(args);

// Ajouter le service DbContext avec EnableStringComparisonTranslations
builder.Services.AddDbContext<ImmobilisationContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)),
        mySqlOptions => mySqlOptions.EnableStringComparisonTranslations() // Ajoutez cette option
    ));

// Enregistre le service ImmobilisationService
builder.Services.AddScoped<IImmobilisationService, ImmobilisationService>();

// Enregistre le service MarqueService
builder.Services.AddScoped<IMarqueService, MarqueService>();

// Enregistre le service FournisseurService
builder.Services.AddScoped<IFournisseurService, FournisseurService>();

// Enregistre le service LocalisationService
builder.Services.AddScoped<ILocalisationService, LocalisationService>();

// Enregistre le service EmployeService
builder.Services.AddScoped<IEmployeService, EmployeService>();

// Enregistre le service ObjetIndisponibiliteService
builder.Services.AddScoped<IObjetIndisponibiliteService, ObjetIndisponibiliteService>();

// Enregistre le service IndisponibiliteService
builder.Services.AddScoped<IIndisponibiliteService, IndisponibiliteService>();

// Enregistre le service AssignationService
builder.Services.AddScoped<IAssignationService, AssignationService>();

// Enregistre le service EntreeImmobilisationService
builder.Services.AddScoped<IEntreeImmobilisationService, EntreeImmobilisationService>();

// Enregistre le service ReportImmobilisationService
builder.Services.AddScoped<IReportImmobilisationService, ReportImmobilisationService>();

// Enregistre le service ImmobilisationImmobilisationService
builder.Services.AddScoped<IImmobilisationImmatriculationService, ImmobilisationImmatriculationService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();