using System.Threading.Tasks;
using MemoryApi.DbModels;
using FaunaDB.Client;
using FaunaDB.Query;
using FaunaDB.Types;
using static FaunaDB.Query.Language;

namespace MemoryApi.Storage.Impl
{
    public class FaunaDbUserRepository : IUserRepository
    {
        private readonly FaunaClient _client;

        public FaunaDbUserRepository(FaunaClient client)
        {
            _client = client;
        }

        public async Task<User> FindByIdentifier(string identifier)
        {
            var result = await _client.Query(Get(
                Union(
                    Match(Index("user_username"), identifier),
                    Match(Index("user_email"), identifier)
                )
            ));
            return result.Get(Field.At("data")).To<User>().Value;
        }

        public async Task<bool> CheckIfExists(string email, string username)
        {
            var result = await _client.Query(
                Or(
                    Exists(Match(Index("user_username"), username)),
                    Exists(Match(Index("user_email"), email))
                )
            );

            return result.To<bool>().Value;
        }

        public async Task<User> Create(User dbModel)
        {
            var result = await _client.Query(Language.Create(Class("user"), Obj("data", dbModel.ToFaunaObj())));
            return result.Get(Field.At("data")).To<User>().Value;
        }
    }
}