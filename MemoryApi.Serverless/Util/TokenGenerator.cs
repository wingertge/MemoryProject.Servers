using System;
using System.Security.Cryptography;
using NeoSmart.Utils;

namespace MemoryApi.Util
{
    public static class TokenGenerator
    {
        public static string GenerateAuthToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[256];
                rng.GetBytes(bytes);
                return UrlBase64.Encode(bytes);
            }
        }
    }
}