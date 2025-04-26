
using RubikCube.Models.Enums;

namespace RubikCube.Models.CubeTile;

public class CubeTileServiceModel
{
    public int Id { get; init; }
    
    public FaceName Face { get; init; }
    
    public int Row { get; set; }
    
    public int Column { get; set; }
    
    public string Color { get; set; }
}