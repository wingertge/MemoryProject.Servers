using System.Threading.Tasks;
using MemoryApi.DbModels;
using MemoryApi.Storage;
using MemoryCore.JsonModels;
using Sodium;

namespace MemoryApi.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ActionResult<User>> CreateUser(string email, string username, string password)
        {
            var exists = await _userRepository.CheckIfExists(email, username);
            if(exists)
                return new ActionResult<User>(false, ("", "Email or username already exists."));

            var dbModel = new User
            {
                Email = email,
                Username = username,
                PasswordHash = PasswordHash.ArgonHashString(password)
            };

            return new ActionResult<User>(true, await _userRepository.Create(dbModel));
        }
    }
}