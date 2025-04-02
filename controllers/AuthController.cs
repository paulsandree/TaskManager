using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.dbcontext;
using TaskManager.models;
using TaskManager.services;

namespace TaskManager.controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { Message = "Email et mot de passe sont requis" });
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            return Unauthorized(new { Message = "Identifiants invalides" });
        }
        
        if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { Message = "Identifiants invalides" });
        }

        // Générer un token
        var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { Message = "Email et mot de passe sont requis" });
        }

        // Vérifier si l'utilisateur existe déjà
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return BadRequest(new { Message = "Cet email est déjà utilisé" });
        }

        // Hacher le mot de passe
        var hashedPassword = PasswordHasher.HashPassword(request.Password);

        // Créer l'utilisateur
        var newUser = new User
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            Role = "User"  // Optionnel : définir un rôle par défaut
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Utilisateur enregistré avec succès" });
    }
}

public class LoginRequest
{
    public LoginRequest(string? email, string? password)
    {
        Email = email;
        Password = password;
    }

    public string? Email { get; }
    public string? Password { get; }
}

public class RegisterRequest
{
    public RegisterRequest(string? email, string? password)
    {
        Email = email;
        Password = password;
    }

    public string? Email { get; }
    public string? Password { get; }
}