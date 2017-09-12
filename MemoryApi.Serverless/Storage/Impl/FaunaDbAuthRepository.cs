using System;
using System.Linq;
using System.Threading.Tasks;
using FaunaDB.Client;
using FaunaDB.Query;
using FaunaDB.Types;
using MemoryApi.Util;
using static FaunaDB.Query.Language;

namespace MemoryApi.Storage.Impl
{
    public class FaunaDbAuthRepository : IAuthRepository
    {
        private readonly FaunaClient _client;

        public FaunaDbAuthRepository(FaunaClient client)
        {
            _client = client;
        }

        public async Task CreateToken(string userId, string token)
        {
            await _client.Query(
                If(Exists(Match(Index("auth_id"), userId)),
                    Foreach(Paginate(Match(Index("auth_id"), userId)), @ref => Update(@ref, Obj("UserId", userId, "Token", token))),
                    Create(Class("auth_cache"), Obj("UserId", userId, "Token", token))
                )
            );
        }

        public async Task<string> GetUserIdFromToken(string token)
        {
            try
            {
                var result = await _client.Query(Get(Match(Index("auth_token"), token)));
                return result.Collect(Field.At("data", "UserId")).FirstOrDefault()?.To<string>().Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}