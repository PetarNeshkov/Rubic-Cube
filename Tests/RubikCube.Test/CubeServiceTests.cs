using Microsoft.EntityFrameworkCore;
using RubikCube.Data;
using RubikCube.Data.Models;
using RubikCube.Models.CubeTile;
using RubikCube.Models.Enums;
using RubikCube.Service;
using Xunit;

namespace RubikCube.Test;

public class CubeServiceTests
{
    private readonly CubeDbContext context;
    private readonly CubeService cubeService;

    public CubeServiceTests()
    {
        context = CreateDbContext();
        cubeService = new CubeService(context);
    }

    private static CubeDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<CubeDbContext>()
            .UseInMemoryDatabase(databaseName: "TestRubikCubeDB")
            .Options;

        return new CubeDbContext(options);
    }

    private static List<CubeTileServiceModel> CreateInitialCube()
    {
        var tiles = new List<CubeTileServiceModel>();

        AddFaceTiles(tiles, FaceName.Up, "White");
        AddFaceTiles(tiles, FaceName.Down, "Yellow");
        AddFaceTiles(tiles, FaceName.Left, "Orange");
        AddFaceTiles(tiles, FaceName.Right, "Red");
        AddFaceTiles(tiles, FaceName.Front, "Green");
        AddFaceTiles(tiles, FaceName.Back, "Blue");

        return tiles;
    }

    private static void AddFaceTiles(List<CubeTileServiceModel> tiles, FaceName face, string color)
    {
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                tiles.Add(new CubeTileServiceModel
                {
                    Face = face,
                    Row = row,
                    Column = col,
                    Color = color
                });
            }
        }
    }

    [Fact]
    public async Task GetInitialCubeForm_ShouldReturnOrderedTiles()
    {
        var tiles = CreateInitialCube()
            .Select(t => new CubeTile
            {
                Face = t.Face,
                Row = t.Row,
                Column = t.Column,
                Color = t.Color
            })
            .ToList();

        await context.CubeTiles.AddRangeAsync(tiles);
        await context.SaveChangesAsync();

        var initialCube = await cubeService.GetInitialCubeForm();
        var cube = initialCube.ToList();

        Assert.Equal(54, cube.Count);

        Assert.Equal(FaceName.Up, cube[0].Face);
        Assert.Equal(FaceName.Down, cube[9].Face);
        Assert.Equal(FaceName.Left, cube[18].Face);
        Assert.Equal(FaceName.Right, cube[27].Face);
        Assert.Equal(FaceName.Front, cube[36].Face);
        Assert.Equal(FaceName.Back, cube[45].Face);

        Assert.Equal("White", cube[0].Color);
        Assert.Equal("Yellow", cube[9].Color);
        Assert.Equal("Orange", cube[18].Color);
        Assert.Equal("Red", cube[27].Color);
        Assert.Equal("Green", cube[36].Color);
        Assert.Equal("Blue", cube[45].Color);
    }

    [Fact]
    public async Task RotateFormFrontMoveShouldRotateCorrectly()
    {
        var initialTiles = CreateInitialCube();
        var request = new RotateCubeRequest
        {
            Tiles = initialTiles,
            Move = "F"
        };

        var rotated = await cubeService.RotateForm(request);
        var cube = rotated.ToList();

        // Check UP bottom row -> moves to Left face right column (Top to Bottom)
        Assert.Equal("Orange", cube.First(t => t is { Face: FaceName.Left, Row: 0, Column: 2 }).Color);
        Assert.Equal("Orange", cube.First(t => t is { Face: FaceName.Left, Row: 1, Column: 2 }).Color);
        Assert.Equal("Orange", cube.First(t => t is { Face: FaceName.Left, Row: 2, Column: 2 }).Color);

        // Check LEFT right column -> moves to Down top row (Right to Left)
        Assert.Equal("Yellow", cube.First(t => t is { Face: FaceName.Down, Row: 0, Column: 2 }).Color);
        Assert.Equal("Yellow", cube.First(t => t is { Face: FaceName.Down, Row: 0, Column: 1 }).Color);
        Assert.Equal("Yellow", cube.First(t => t is { Face: FaceName.Down, Row: 0, Column: 0 }).Color);

        // Check DOWN top row -> moves to Right face left column (Bottom to Top)
        Assert.Equal("Red", cube.First(t => t is { Face: FaceName.Right, Row: 2, Column: 0 }).Color);
        Assert.Equal("Red", cube.First(t => t is { Face: FaceName.Right, Row: 1, Column: 0 }).Color);
        Assert.Equal("Red", cube.First(t => t is { Face: FaceName.Right, Row: 0, Column: 0 }).Color);

        // Check RIGHT left column -> moves to Up bottom row (Left to Right)
        Assert.Equal("White", cube.First(t => t is { Face: FaceName.Up, Row: 2, Column: 0 }).Color);
        Assert.Equal("White", cube.First(t => t is { Face: FaceName.Up, Row: 2, Column: 1 }).Color);
        Assert.Equal("White", cube.First(t => t is { Face: FaceName.Up, Row: 2, Column: 2 }).Color);
    }

    [Fact]
    public async Task RotateFormRightPrimeMoveShouldRotateCorrectly()
    {
        var initialTiles = CreateInitialCube();

        var request = new RotateCubeRequest
        {
            Tiles = initialTiles,
            Move = "R'"
        };

        var rotated = await cubeService.RotateForm(request);
        var cube = rotated.ToList();

        Assert.Equal("Blue", cube.First(t => t is { Face: FaceName.Up, Row: 0, Column: 2 }).Color);
        Assert.Equal("Blue", cube.First(t => t is { Face: FaceName.Up, Row: 1, Column: 2 }).Color);
        Assert.Equal("White", cube.First(t => t is { Face: FaceName.Front, Row: 0, Column: 2 }).Color);
        Assert.Equal("White", cube.First(t => t is { Face: FaceName.Front, Row: 1, Column: 2 }).Color);
        Assert.Equal("Yellow", cube.First(t => t is { Face: FaceName.Back, Row: 0, Column: 0 }).Color);
        Assert.Equal("Yellow", cube.First(t => t is { Face: FaceName.Back, Row: 1, Column: 0 }).Color);
        Assert.Equal("Green", cube.First(t => t is { Face: FaceName.Down, Row: 2, Column: 2 }).Color);
        Assert.Equal("Green", cube.First(t => t is { Face: FaceName.Down, Row: 1, Column: 2 }).Color);

        var rightFace = cube.Where(t => t.Face == FaceName.Right).ToList();
        Assert.Equal("Red", rightFace.First(t => t is { Row: 0, Column: 2 }).Color);
        Assert.Equal("Red", rightFace.First(t => t is { Row: 2, Column: 2 }).Color);
        Assert.Equal("Red", rightFace.First(t => t is { Row: 2, Column: 0 }).Color);
        Assert.Equal("Red", rightFace.First(t => t is { Row: 0, Column: 0 }).Color);
    }

    [Fact]
    public async Task RotateFormShouldRotateUpFaceClockwise()
    {
        var initialTiles = CreateInitialCube();
        var request = new RotateCubeRequest
        {
            Tiles = initialTiles,
            Move = "U"
        };

        var result = await cubeService.RotateForm(request);
        var tiles = result.ToList();

        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Front, Row: 0, Column: 0 }).Color);
        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Front, Row: 0, Column: 1 }).Color);
        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Front, Row: 0, Column: 2 }).Color);

        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Right, Row: 0, Column: 0 }).Color);
        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Right, Row: 0, Column: 1 }).Color);
        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Right, Row: 0, Column: 2 }).Color);

        Assert.Equal("Orange", tiles.First(t => t is { Face: FaceName.Back, Row: 0, Column: 0 }).Color);
        Assert.Equal("Orange", tiles.First(t => t is { Face: FaceName.Back, Row: 0, Column: 1 }).Color);
        Assert.Equal("Orange", tiles.First(t => t is { Face: FaceName.Back, Row: 0, Column: 2 }).Color);

        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Left, Row: 0, Column: 0 }).Color);
        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Left, Row: 0, Column: 1 }).Color);
        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Left, Row: 0, Column: 2 }).Color);

        var upFace = tiles.Where(t => t.Face == FaceName.Up).ToList();
        Assert.All(upFace, tile => Assert.Equal("White", tile.Color));
    }

    [Fact]
    public async Task RotateFormShouldRotateBackFaceCounterClockwise()
    {
        var initialTiles = CreateInitialCube();
        var request = new RotateCubeRequest
        {
            Tiles = initialTiles,
            Move = "B'"
        };

        var result = await cubeService.RotateForm(request);
        var tiles = result.ToList();

        Assert.Equal("Orange", tiles.First(t => t is { Face: FaceName.Up, Row: 0, Column: 0 }).Color);
        Assert.Equal("Orange", tiles.First(t => t.Face == FaceName.Up && t is { Row: 0, Column: 1 }).Color);
        Assert.Equal("Orange", tiles.First(t => t.Face == FaceName.Up && t is { Row: 0, Column: 2 }).Color);

        Assert.Equal("Yellow", tiles.First(t => t is { Face: FaceName.Left, Row: 0, Column: 0 }).Color);
        Assert.Equal("Yellow", tiles.First(t => t is { Face: FaceName.Left, Row: 1, Column: 0 }).Color);
        Assert.Equal("Yellow", tiles.First(t => t is { Face: FaceName.Left, Row: 2, Column: 0 }).Color);

        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Down, Row: 2, Column: 2 }).Color);
        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Down, Row: 2, Column: 1 }).Color);
        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Down, Row: 2, Column: 0 }).Color);

        Assert.Equal("White", tiles.First(t => t is { Face: FaceName.Right, Row: 0, Column: 2 }).Color);
        Assert.Equal("White", tiles.First(t => t is { Face: FaceName.Right, Row: 1, Column: 2 }).Color);
        Assert.Equal("White", tiles.First(t => t is { Face: FaceName.Right, Row: 2, Column: 2 }).Color);

        var backFace = tiles.Where(t => t.Face == FaceName.Back).ToList();
        Assert.All(backFace, tile => Assert.Equal("Blue", tile.Color));
    }

    [Fact]
    public async Task RotateFormShouldRotateLeftFaceClockwise()
    {
        var initialTiles = CreateInitialCube();
        var request = new RotateCubeRequest
        {
            Tiles = initialTiles,
            Move = "L"
        };

        var result = await cubeService.RotateForm(request);
        var tiles = result.ToList();

        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Up, Row: 0, Column: 0 }).Color);
        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Up, Row: 1, Column: 0 }).Color);
        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Up, Row: 2, Column: 0 }).Color);

        Assert.Equal("White", tiles.First(t => t is { Face: FaceName.Front, Row: 0, Column: 0 }).Color);
        Assert.Equal("White", tiles.First(t => t is { Face: FaceName.Front, Row: 1, Column: 0 }).Color);
        Assert.Equal("White", tiles.First(t => t is { Face: FaceName.Front, Row: 2, Column: 0 }).Color);

        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Down, Row: 0, Column: 0 }).Color);
        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Down, Row: 1, Column: 0 }).Color);
        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Down, Row: 2, Column: 0 }).Color);

        Assert.Equal("Yellow", tiles.First(t => t is { Face: FaceName.Back, Row: 2, Column: 2 }).Color);
        Assert.Equal("Yellow", tiles.First(t => t is { Face: FaceName.Back, Row: 1, Column: 2 }).Color);
        Assert.Equal("Yellow", tiles.First(t => t is { Face: FaceName.Back, Row: 0, Column: 2 }).Color);

        var leftFace = tiles.Where(t => t.Face == FaceName.Left).ToList();
        Assert.All(leftFace, tile => Assert.Equal("Orange", tile.Color));
    }

    [Fact]
    public async Task RotateFormShouldRotateDownFaceCounterClockwise()
    {
        var initialTiles = CreateInitialCube();
        var request = new RotateCubeRequest
        {
            Tiles = new List<CubeTileServiceModel>(initialTiles),
            Move = "D'"
        };

        var result = await cubeService.RotateForm(request);
        var tiles = result.ToList();

        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Front, Row: 2, Column: 0 }).Color);
        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Front, Row: 2, Column: 1 }).Color);
        Assert.Equal("Red", tiles.First(t => t is { Face: FaceName.Front, Row: 2, Column: 2 }).Color);
        
        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Right, Row: 2, Column: 0 }).Color);
        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Right, Row: 2, Column: 1 }).Color);
        Assert.Equal("Blue", tiles.First(t => t is { Face: FaceName.Right, Row: 2, Column: 2 }).Color);
        
        Assert.Equal("Orange", tiles.First(t => t is { Face: FaceName.Back, Row: 2, Column: 0 }).Color);
        Assert.Equal("Orange", tiles.First(t => t is { Face: FaceName.Back, Row: 2, Column: 1 }).Color);
        Assert.Equal("Orange", tiles.First(t => t is { Face: FaceName.Back, Row: 2, Column: 2 }).Color);
        
        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Left, Row: 2, Column: 0 }).Color);
        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Left, Row: 2, Column: 1 }).Color);
        Assert.Equal("Green", tiles.First(t => t is { Face: FaceName.Left, Row: 2, Column: 2 }).Color);
    }
    
    [Fact]
    public async Task RotateFormShouldFailWhenExpectingWrongColors()
    {
        var initialTiles = CreateInitialCube();
        var request = new RotateCubeRequest
        {
            Tiles = new List<CubeTileServiceModel>(initialTiles),
            Move = "D'"
        };

        var result = await cubeService.RotateForm(request);
        var tiles = result.ToList();
        
        Assert.NotEqual("Yellow", tiles.First(t => t is { Face: FaceName.Front, Row: 2, Column: 0 }).Color);
    }
    
    [Fact]
    public async Task RotateFormShouldThrowArgumentExceptionWhenMoveIsInvalid()
    {
        var initialTiles = CreateInitialCube();
        var request = new RotateCubeRequest
        {
            Tiles = new List<CubeTileServiceModel>(initialTiles),
            Move = "P" 
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await cubeService.RotateForm(request);
        });

        Assert.Equal("move", exception.ParamName); 
    }
}