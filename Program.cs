using LimsImmobilisationService.Data;
using LimsImmobilisationService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ajouter le service DbContext
builder.Services.AddDbContext<ImmobilisationContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// Enregistre le service ImmobilisationService
builder.Services.AddScoped<IImmobilisationService, ImmobilisationService>();

// Enregistre le service MarqueService
builder.Services.AddScoped<IMarqueService, MarqueService>();

// Enregistre le service FournisseurService
builder.Services.AddScoped<IFournisseurService, FournisseurService>();

// Enregistre le service LocalisationService
builder.Services.AddScoped<ILocalisationService, LocalisationService>();





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
