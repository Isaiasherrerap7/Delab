using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Delab.AccessData.Data;

// 1.Para la clase DataContex heredamos de DbContext
// 2. Creamos el Constructor apartir del DbContext
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    // 3. Creamos las propiedades DbSet para cada entidad para usar en la base de datos
    public DbSet<Country> Countries => Set<Country>();

    public DbSet<State> States => Set<State>();
    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Para tomar los calores de ConfigEntities
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}