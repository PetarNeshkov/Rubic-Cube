
using RubikCube.Models.Enums;

namespace RubikCube.Models.CubeTile;

public class CubeTileServiceModel
{
    public int Id { get; init; }
    
    public FaceName Face { get; set; }
    
    public int Row { get; init; }
    
    public int Column { get; init; }
    
    public string Color { get; init; }
}