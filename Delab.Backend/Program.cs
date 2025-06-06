using Delab.AccessData.Data;
using Delab.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Agregamos el servicio para controlar las referencias ciclicas
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Servicio de Swagger funcion

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

builder.Services.AddTransient<SeedDb>(); // Agregamos el servicio para inicializar la base de datos

// Conexion a Base de datos
builder.Services.AddDbContext<DataContext>(x =>
    x.UseSqlServer("name=DefaultConnection", option => option.MigrationsAssembly("Delab.Backend")));

var app = builder.Build();

SeedData(app);

void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (IServiceScope? scope = scopedFactory?.CreateScope())
    {
        SeedDb? seedDb = scope?.ServiceProvider.GetService<SeedDb>();
        seedDb?.SeedAsync().Wait();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    string swaggerUrl = "https://localhost:7254/swagger"; // URL de Swagger
    Task.Run(() => OpenBrowser(swaggerUrl));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// M�todo para abrir el navegador
static void OpenBrowser(string url)
{
    try
    {
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        };
        System.Diagnostics.Process.Start(psi);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al abrir el navegador: {ex.Message}");
    }
}