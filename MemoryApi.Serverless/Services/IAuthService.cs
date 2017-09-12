using System.Threading.Tasks;
using MemoryApi.DbModels;
using MemoryCore.JsonModels;

namespace MemoryApi.Services
{
    public interface IAuthService
    {
        Task<ActionResult<string>> Login(string identifier, string password);
        Task<ActionResult<string>> ValidateToken(string token);
    }
}