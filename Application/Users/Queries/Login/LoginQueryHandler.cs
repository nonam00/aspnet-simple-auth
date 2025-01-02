using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.Login;

public class LoginQueryHandler(
    IAppDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
    : IRequestHandler<LoginQuery, string>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<string> Handle(LoginQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken) 
            ?? throw new Exception("Invalid email or password. Please try again.");

        var check = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!check)
        {
            throw new Exception("Invalid email or password. Please try again.");
        }

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }
}