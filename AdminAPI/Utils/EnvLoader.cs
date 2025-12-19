using DotNetEnv;

namespace AdminAPI.Utils
{
    public class EnvLoader
    {
        public static void Load()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var envFiles = new[]
            {
                $".env.{environment.ToLower()}.local",
                $".env.{environment.ToLower()}",
                ".env.local",
                ".env"
            };

            foreach (var envFile in envFiles)
            {
                if (File.Exists(envFile))
                {
                    Console.WriteLine($"Loading environment from: {envFile}");
                    Env.Load(envFile);
                }
            }

            Console.WriteLine($"Environment: {environment}");
        }
    }
}
