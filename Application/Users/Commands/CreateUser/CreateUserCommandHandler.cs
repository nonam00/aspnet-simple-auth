﻿using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(
    IAppDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
    : IRequestHandler<CreateUserCommand, string>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<string> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        if (await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            throw new Exception("User with this email already exits");
        }

        var hashedPassword = _passwordHasher.Generate(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = hashedPassword
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }
}