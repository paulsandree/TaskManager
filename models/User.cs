using System.ComponentModel.DataAnnotations;

namespace TaskManager.models;

public class User
{
    public int Id { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    public string? Role { get; set; }
    public string? Name { get; set; }
}