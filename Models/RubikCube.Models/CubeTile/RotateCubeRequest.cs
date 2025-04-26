namespace RubikCube.Models.CubeTile;

public class RotateCubeRequest
{
    public IList<CubeTileServiceModel> Tiles { get; set; }
    
    public string Move { get; set; }
}