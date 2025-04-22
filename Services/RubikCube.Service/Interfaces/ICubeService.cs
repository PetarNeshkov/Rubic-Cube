using RubikCube.Models;
using RubikCube.Models.CubeTile;

namespace RubikCube.Service.Interfaces;

public interface ICubeService
{
    Task<IEnumerable<CubeTileServiceModel>> GetInitialCubeForm();
}