using System.ComponentModel.DataAnnotations;

namespace Web;

public record LoginDto([Required, EmailAddress] string Email, [Required] string Password);