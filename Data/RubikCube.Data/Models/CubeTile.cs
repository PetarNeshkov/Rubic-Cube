using System.ComponentModel.DataAnnotations;
using RubikCube.Data.Enums;
using static RubikCube.Data.Constraints.DatabaseConstants;
using static RubikCube.Data.Constraints.DatabaseConstants.CubeTile;

namespace RubikCube.Data.Models;

public class CubeTile
{
    [Key]
    [MaxLength(KeyMaxLength)]
    public int Id { get; init; }
    
    public FaceName Face { get; init; }     
    
    public int Row { get; init; }          
    
    public int Column { get; init; }  
    
    [Required]
    [MaxLength(ColorMaxLength)]
    public string Color { get; init; }  
}