using Microsoft.Extensions.Options;
using NTierArchitectureServer.Entities.Options;

namespace NTierArchitectureServer.WebApi.Options
{
    public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
    {
        private readonly IConfiguration _configuration;

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOptions options)
        {
            options.MSSqlConnectionString = _configuration.GetConnectionString("SqlServer");
        }
    }
}
