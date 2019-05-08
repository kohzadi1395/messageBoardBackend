using MessageBoardBackend.Models;

namespace MessageBoardBackend.Services
{
    public interface IJwtService
    {
        string Generate(User user);
    }
}