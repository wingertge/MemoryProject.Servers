using System.Threading.Tasks;
using MemoryApi.DbModels;

namespace MemoryApi.Storage
{
    public interface IUserRepository
    {
        Task<User> FindByIdentifier(string identifier);
        Task<bool> CheckIfExists(string email, string username);
        Task<User> Create(User dbModel);
    }
}