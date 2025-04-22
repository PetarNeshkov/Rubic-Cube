using Microsoft.EntityFrameworkCore;
using RubikCube.Data;
using RubikCube.Data.Seeding;

namespace RubikCube.Server.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<CubeDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("CubeConnectionString")));
    
    public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<CubeDbContext>();

        dbContext.Database.Migrate();

        new CubeDbContextSeeder()
            .SeedDatabase(dbContext, scope.ServiceProvider)
            .GetAwaiter()
            .GetResult();

        return app;
    }
    
    public static IApplicationBuilder UseAnyCors(
        this IApplicationBuilder app)
        => app
            .UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
}