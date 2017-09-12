using System.Threading.Tasks;

namespace MemoryApi.Storage
{
    public interface IAuthRepository
    {
        Task CreateToken(string userId, string token);
        Task<string> GetUserIdFromToken(string token);
    }
}