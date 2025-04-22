using Microsoft.AspNetCore.Mvc;
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
}
    