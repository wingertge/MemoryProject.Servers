using System.Threading.Tasks;
using MemoryApi.Storage;
using MemoryApi.Util;
using MemoryCore.JsonModels;
using Sodium;

namespace MemoryApi.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;

        public AuthService(IUserRepository userRepository, IAuthRepository authRepository)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        public async Task<ActionResult<string>> Login(string identifier, string password)
        {
            var user = await _userRepository.FindByIdentifier(identifier);
            if (user == null)
                return new ActionResult<string>(false, ("", "Invalid credentials"));

            var passwordCorrect = PasswordHash.ArgonHashStringVerify(user.PasswordHash, password);
            if(!passwordCorrect)
                return new ActionResult<string>(false, ("", "Invalid credentials"));

            var token = TokenGenerator.GenerateAuthToken();
            await _authRepository.CreateToken(user.Id, token);
            return new ActionResult<string>(true, token);
        }

        public async Task<ActionResult<string>> ValidateToken(string token)
        {
            var result = await _authRepository.GetUserIdFromToken(token);
            return new ActionResult<string>(result != null, result);
        }
    }
}