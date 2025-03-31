using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user == null)
        {
            return Unauthorized("Email o contraseña incorrectos.");
        }

        bool isPasswordValid = VerifyPassword(loginRequest.Password, user.Password);
        if (!isPasswordValid)
        {
            return Unauthorized("Email o contraseña incorrectos.");
        }

        return Ok(user);
    }

    private bool VerifyPassword(string enteredPassword, string storedPassword)
    {
        return enteredPassword == storedPassword;
    }
}
