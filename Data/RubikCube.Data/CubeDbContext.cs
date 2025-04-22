using Microsoft.EntityFrameworkCore;
using RubikCube.Data.Models;

namespace RubikCube.Data;

public class CubeDbContext(DbContextOptions<CubeDbContext> options)
    : DbContext(options)
{
    public DbSet<CubeTile> CubeTiles { get; init; }
}