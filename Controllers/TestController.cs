using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SmartRancho.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Este endpoint es público 👀");
    }

    [Authorize]
    [HttpGet("private")]
    public IActionResult Private()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var ranchoId = User.FindFirst("RanchoId")?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            message = "Acceso autorizado 🔐",
            email,
            ranchoId,
            role
        });
    }
}