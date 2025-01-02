using Domain;

namespace Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}