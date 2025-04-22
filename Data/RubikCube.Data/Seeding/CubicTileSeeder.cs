using Microsoft.EntityFrameworkCore;
using RubikCube.Data.Models;
using RubikCube.Data.Seeding.Interfaces;
using RubikCube.Models.Enums;

namespace RubikCube.Data.Seeding;

public class CubicTileSeeder : ISeeder
{
    public async Task SeedAsync(CubeDbContext dbContext)
    {
        if (await dbContext.CubeTiles.AnyAsync())
        {
            return;
        }

        var colors = new Dictionary<FaceName, string>
        {
            { FaceName.Up, "White" },
            { FaceName.Down, "Yellow" },
            { FaceName.Left, "Orange" },
            { FaceName.Right, "Red" },
            { FaceName.Front, "Green" },
            { FaceName.Back, "Blue" }
        };

        var tiles = new List<CubeTile>();
        foreach (var face in Enum.GetValues<FaceName>())
        {
            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    tiles.Add(new CubeTile
                    {
                        Face = face,
                        Row = row,
                        Column = col,
                        Color = colors[face]
                    });
                }
            }
        }

        await dbContext.CubeTiles.AddRangeAsync(tiles);
        await dbContext.SaveChangesAsync();
    }
}