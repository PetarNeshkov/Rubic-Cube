namespace RubikCube.Data.Seeding.Interfaces;

public interface IDatabaseSeeder
{
    Task SeedDatabase(CubeDbContext dbContext, IServiceProvider serviceProvider);
}