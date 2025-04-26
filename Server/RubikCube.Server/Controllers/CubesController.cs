using Microsoft.AspNetCore.Mvc;
using RubikCube.Models.CubeTile;
using RubikCube.Server.Controllers.API;
using RubikCube.Server.Infrastructure.Extensions;
using RubikCube.Service.Interfaces;

namespace RubikCube.Server.Controllers;

public class CubesController(ICubeService cubeService) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCube()
        => await cubeService.GetInitialCubeForm()
            .ToOkResult();

    [HttpPost]
    public async Task<IActionResult> RotateCube([FromBody] RotateCubeRequest requestMove)
        => await cubeService.RotateForm(requestMove)
            .ToOkResult();
}
    