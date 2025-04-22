using RubikCube.Data.Seeding.Interfaces;

namespace RubikCube.Data.Seeding;

public class CubeDbContextSeeder : IDatabaseSeeder
{
    public async Task SeedDatabase(CubeDbContext dbContext, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        ArgumentNullException.ThrowIfNull(serviceProvider);

        var seeders = new List<ISeeder>
        {
            new CubicTileSeeder(),
        };

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(dbContext);

            await dbContext.SaveChangesAsync();
        }
    }
}