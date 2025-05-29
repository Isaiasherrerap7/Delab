using Delab.AccessData.Data;
using Delab.Shared.Entities;

namespace Delab.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    // Cuando arranque el backend valida si existe la base de datos y si no existe la crea
    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountries();
    }

    private async Task CheckCountries()
    {
        if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country
            {
                Name = "Colombia",
                CodPhone = "+57",
                States = new List<State>
                {
                    new State
                    {
                        Name = "Antioquia",
                        Cities = new List<City>
                        {
                            new City { Name = "Medellin" },
                            new City { Name = "Bello" },
                            new City { Name = "Itagui" },
                            new City { Name = "Envigado" },
                            new City { Name = "Sabaneta" },
                        }
                    },
                    new State
                    {
                        Name = "Cundinamarca",
                        Cities = new List<City>
                        {
                            new City { Name = "Bogota" },
                            new City { Name = "Chia" },
                            new City { Name = "Cajica" },
                            new City { Name = "Zipaquira" },
                            new City { Name = "Soacha" }
                        }
                    }
                }
            });

            _context.Countries.Add(new Country
            {
                Name = "Mexico",
                CodPhone = "+52",
                States = new List<State>
                {
                    new State
                    {
                        Name = "Jalisco",
                        Cities = new List<City>
                        {
                            new City { Name = "Guadalajara" },
                            new City { Name = "Zapopan" },
                            new City { Name = "Tlaquepaque" },
                            new City { Name = "Tlajomulco de Zuniga" },
                            new City { Name = "El Salto" }
                        }
                    },
                    new State
                    {
                        Name = "Nuevo Leon",
                        Cities = new List<City>
                        {
                            new City { Name = "Monterrey" },
                            new City { Name = "San Pedro Garza Garcia" },
                            new City { Name = "Santa Catarina" },
                            new City { Name = "Apodaca" },
                            new City { Name = "Escobedo" }
                        }
                    }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}