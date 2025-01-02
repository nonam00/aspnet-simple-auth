using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Web.Models;

public record RegisterDto([Required, EmailAddress] string Email, [Required] string Password, [Required] RoleEnum Role);
