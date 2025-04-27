using Microsoft.EntityFrameworkCore;
using RubikCube.Data;
using RubikCube.Models.CubeTile;
using RubikCube.Models.Edge;
using RubikCube.Models.Enums;
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

    public Task<IEnumerable<CubeTileServiceModel>> RotateForm(RotateCubeRequest requestMove)
    {
        ApplyMove(requestMove.Tiles, requestMove.Move);

        return Task.FromResult<IEnumerable<CubeTileServiceModel>>(requestMove.Tiles);
    }

    private static void ApplyMove(IList<CubeTileServiceModel> tiles, string move)
    {
        switch (move)
        {
            case "F":
                RotateFaceClockwise(tiles, FaceName.Front);
                RotateFrontSide(tiles);
                break;
            case "R'":
                RotateFaceCounterClockwise(tiles, FaceName.Right);
                RotateRightSide(tiles);
                break;
            case "U":
                RotateFaceClockwise(tiles, FaceName.Up);
                RotateUpSide(tiles);
                break;
            case "B'":
                RotateFaceCounterClockwise(tiles, FaceName.Back);
                RotateBackSide(tiles);
                break;
            case "L":
                RotateFaceClockwise(tiles, FaceName.Left);
                RotateLeftSide(tiles);
                break;
            case "D'":
                RotateFaceCounterClockwise(tiles, FaceName.Down);
                RotateDownSide(tiles);
                break;
            default: throw new ArgumentException("Invalid move", nameof(move));
        }
    }

    private static void RotateFaceClockwise(IList<CubeTileServiceModel> tiles, FaceName face)
    {
        var faceTiles = tiles.Where(t => t.Face == face).ToList();

        foreach (var tile in faceTiles)
        {
            var oldRow = tile.Row;
            tile.Row = tile.Column;
            tile.Column = 2 - oldRow;
        }
    }

    private static void RotateFaceCounterClockwise(IList<CubeTileServiceModel> tiles, FaceName face)
    {
        var faceTiles = tiles.Where(t => t.Face == face).ToList();

        foreach (var tile in faceTiles)
        {
            var oldColumn = tile.Column;
            tile.Column = tile.Row;
            tile.Row = 2 - oldColumn;
        }
    }

    private static void RotateUpSide(IList<CubeTileServiceModel> tiles) =>
        SwapEdges(tiles, [
            new EdgeServiceModel(FaceName.Back, 0, false),
            new EdgeServiceModel(FaceName.Right, 0, false),
            new EdgeServiceModel(FaceName.Front, 0, false),
            new EdgeServiceModel(FaceName.Left, 0, false)
        ], clockwise: true);

    private static void RotateDownSide(IList<CubeTileServiceModel> tiles) =>
        SwapEdges(tiles, [
            new EdgeServiceModel(FaceName.Front, 2, false),
            new EdgeServiceModel(FaceName.Right, 2, false),
            new EdgeServiceModel(FaceName.Back, 2, false),
            new EdgeServiceModel(FaceName.Left, 2, false)
        ], clockwise: false);

    private static void RotateFrontSide(IList<CubeTileServiceModel> tiles) =>
        SwapEdges(tiles, [
            new EdgeServiceModel(FaceName.Up, 2, false),
            new EdgeServiceModel(FaceName.Right, 0, true),
            new EdgeServiceModel(FaceName.Down, 0, false),
            new EdgeServiceModel(FaceName.Left, 2, true)
        ], clockwise: true);

    private static void RotateBackSide(IList<CubeTileServiceModel> tiles) =>
        SwapEdges(tiles, [
            new EdgeServiceModel(FaceName.Up, 0, false),
            new EdgeServiceModel(FaceName.Left, 0, true),
            new EdgeServiceModel(FaceName.Down, 2, false),
            new EdgeServiceModel(FaceName.Right, 2, true)
        ], clockwise: false);

    private static void RotateRightSide(IList<CubeTileServiceModel> tiles) =>
        SwapEdges(tiles, [
            new EdgeServiceModel(FaceName.Up, 2, true),
            new EdgeServiceModel(FaceName.Back, 0, true),
            new EdgeServiceModel(FaceName.Down, 2, true),
            new EdgeServiceModel(FaceName.Front, 2, true)
        ], clockwise: false);

    private static void RotateLeftSide(IList<CubeTileServiceModel> tiles) =>
        SwapEdges(tiles, [
            new EdgeServiceModel(FaceName.Up, 0, true),
            new EdgeServiceModel(FaceName.Front, 0, true),
            new EdgeServiceModel(FaceName.Down, 0, true),
            new EdgeServiceModel(FaceName.Back, 2, true)
        ], clockwise: true);

    private static void SwapEdges(IList<CubeTileServiceModel> tiles, EdgeServiceModel[] edges, bool clockwise)
    {
        var edgeColors = new string[edges.Length][];
        for (int i = 0; i < edges.Length; i++)
        {
            edgeColors[i] = GetEdge(tiles, edges[i].Face, edges[i].Index, edges[i].IsColumn);
        }

        if (clockwise)
        {
            for (int i = 0; i < edges.Length; i++)
            {
                var sourceIndex = (i - 1 + edges.Length) % edges.Length;

                var colors = edgeColors[sourceIndex].ToArray();
                if (edges[i].IsColumn != edges[sourceIndex].IsColumn)
                {
                    Array.Reverse(colors);
                }

                SetEdge(tiles, edges[i].Face, edges[i].Index, edges[i].IsColumn, colors);
            }
        }
        else
        {
            for (int i = 0; i < edges.Length; i++)
            {
                var sourceIndex = (i + 1) % edges.Length;

                var colors = edgeColors[sourceIndex].ToArray();
                if (edges[i].IsColumn != edges[sourceIndex].IsColumn)
                {
                    Array.Reverse(colors);
                }

                SetEdge(tiles, edges[i].Face, edges[i].Index, edges[i].IsColumn, colors);
            }
        }
    }

    private static string[] GetEdge(IList<CubeTileServiceModel> tiles, FaceName face, int index, bool isColumn)
    {
        var colors = new string[3];

        for (int i = 0; i < 3; i++)
        {
            var row = isColumn ? i : index;
            var column = isColumn ? index : i;

            var tile = tiles.First(t => t.Face == face && t.Row == row && t.Column == column);
            colors[i] = tile.Color;
        }

        return colors;
    }

    private static void SetEdge(IList<CubeTileServiceModel> tiles, FaceName face, int index, bool isColumn,
        string[] colors)
    {
        for (int i = 0; i < 3; i++)
        {
            var row = isColumn ? i : index;
            var column = isColumn ? index : i;

            var tile = tiles.First(t => t.Face == face && t.Row == row && t.Column == column);
            tile.Color = colors[i];
        }
    }
}