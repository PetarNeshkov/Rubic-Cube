using Microsoft.EntityFrameworkCore;
using RubikCube.Data;
using RubikCube.Models.CubeTile;
using RubikCube.Service.Interfaces;

namespace RubikCube.Service;

public class CubeService(CubeDbContext cubeDbContext) : ICubeService
{
    public async Task<IEnumerable<CubeTileServiceModel>> GetInitialCubeForm()
    {
        return await cubeDbContext.CubeTiles
            .OrderBy(t => t.Face)
            .ThenBy(t => t.Row)
            .ThenBy(t => t.Column)
            .Select(t => new CubeTileServiceModel
            {
                Id = t.Id,
                Face = t.Face,
                Row = t.Row,
                Column = t.Column,
                Color = t.Color,
            })
            .ToListAsync();
    }
}