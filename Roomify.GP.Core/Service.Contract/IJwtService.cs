using Roomify.GP.Core.Entities;


namespace Roomify.GP.Core.Service.Contract
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
