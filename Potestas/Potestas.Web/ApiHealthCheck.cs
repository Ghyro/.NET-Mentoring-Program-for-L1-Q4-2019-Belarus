using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Potestas.Web
{
    public class ApiHealthCheck : IHealthCheck
    {
        public readonly ILogger<ApiHealthCheck> Logger;

        public ApiHealthCheck(ILogger<ApiHealthCheck> logger)
        {
            Logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Logger.LogInformation("Try to get health info about database connection");

            var isAvailable = CheckDataBaseConnection();

            if (isAvailable)
                Logger.LogCritical("Impossible to get database info. Check connection.");

            return Task.FromResult(isAvailable ? HealthCheckResult.Healthy("Database connection is available for now")
                : HealthCheckResult.Unhealthy("Database connection is unavailable for now"));
        }

        private static bool CheckDataBaseConnection()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Observations");
            var isMongoLive = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(50);

            return isMongoLive;
        }
    }
}
