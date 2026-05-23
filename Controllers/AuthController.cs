using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartRancho.API.Auth;
using SmartRancho.API.Data;
using SmartRancho.API.Entities;
using SmartRancho.API.Services;
using Microsoft.AspNetCore.Identity;
using SmartRancho.API.Dtos;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SmartRanchoDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(UserManager<ApplicationUser> userManager, JwtService jwtService, SmartRanchoDbContext context)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var rancho = new Rancho
        {
            NombreRancho = dto.NombreRancho,
            FechaRegistro = DateTime.UtcNow,
            Activo = true
        };

        _context.Ranchos.Add(rancho);
        await _context.SaveChangesAsync();

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            RanchoId = rancho.RanchoId
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "Admin");

        return Ok("Usuario administrador creado");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Unauthorized("Usuario no existe");

        var result = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!result)
            return Unauthorized("Credenciales incorrectas");

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtService.GenerateToken(user, roles);

        return Ok(new
        {
            token,
            user.Email,
            user.RanchoId,
            roles
        });
    }
}