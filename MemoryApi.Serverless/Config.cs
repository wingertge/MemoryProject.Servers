using System;

namespace MemoryApi
{
    public static class Config
    {
        public static Environment Environment { get; set; }
        public static string FaunaSecret { get; set; }
        public static string FaunaHost { get; set; }
        public static string FaunaScheme { get; set; }
        public static int FaunaPort { get; set; }
        public static string GoogleClientId { get; set; }

        static Config()
        {
            FaunaSecret = System.Environment.GetEnvironmentVariable("FAUNA_SECRET") ?? "secret";
            FaunaHost = System.Environment.GetEnvironmentVariable("FAUNA_HOST") ?? "db.fauna.com";
            FaunaScheme = System.Environment.GetEnvironmentVariable("FAUNA_SCHEME") ?? "https";
            FaunaPort = int.Parse(System.Environment.GetEnvironmentVariable("FAUNA_PORT") ?? "443");
            GoogleClientId = System.Environment.GetEnvironmentVariable("GOOGLE_ID") ?? "583067665397-2940k8nv5ofoblbj290db0bc7afhibtl.apps.googleusercontent.com";

            Environment =
                System.Environment.GetEnvironmentVariable("FUNCTION_ENV")
                    ?.Equals("Production", StringComparison.InvariantCultureIgnoreCase) ?? false
                    ? Environment.Production
                    : Environment.Development;
        }
    }

    public enum Environment
    {
        Development,
        Production
    }
}