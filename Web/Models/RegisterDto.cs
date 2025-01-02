using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public record RegisterDto([Required, EmailAddress] string Email, [Required] string Password);
