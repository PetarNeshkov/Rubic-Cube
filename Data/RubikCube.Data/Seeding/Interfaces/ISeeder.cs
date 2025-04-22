namespace RubikCube.Data.Seeding.Interfaces;

public interface ISeeder
{
    Task SeedAsync(CubeDbContext dbContext);
}