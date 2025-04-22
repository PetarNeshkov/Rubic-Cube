using Microsoft.AspNetCore.Mvc;

namespace RubikCube.Server.Controllers.API;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class BaseApiController : ControllerBase;