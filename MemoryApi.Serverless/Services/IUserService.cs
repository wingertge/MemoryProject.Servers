using System.Threading.Tasks;
using MemoryApi.DbModels;
using MemoryCore.JsonModels;

namespace MemoryApi.Services
{
    public interface IUserService
    {
        Task<ActionResult<User>> CreateUser(string email, string username, string password);
    }
}