﻿namespace Domain.Models;

public class User
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public ICollection<Role> Roles { get; set; } = [];
}