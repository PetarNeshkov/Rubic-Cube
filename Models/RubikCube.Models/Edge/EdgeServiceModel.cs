using RubikCube.Models.Enums;

namespace RubikCube.Models.Edge;

public class EdgeServiceModel(FaceName face, int index, bool isColumn)
{
    public FaceName Face { get; } = face;

    public int Index { get; } = index;

    public bool IsColumn { get; } = isColumn;
}