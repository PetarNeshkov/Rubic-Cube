using RubikCube.Data;

public interface IDatabaseSeeder
{
    Task SeedDatabase(CubeDbContext dbContext, IServiceProvider serviceProvider);
}