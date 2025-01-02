using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public record LoginDto([Required, EmailAddress] string Email, [Required] string Password);