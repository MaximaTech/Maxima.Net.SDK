using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;

namespace Maxima.Net.SDK.Domain.Utils
{
    public class ConfiguracaoHangfire
    {


        public static readonly MemoryStorageOptions StorageOptions = new()
        {
            CountersAggregateInterval = TimeSpan.FromMinutes(30),
            JobExpirationCheckInterval = TimeSpan.FromMinutes(5)
        };

        public static DashboardOptions DashboardOptions() => new()
        {
            AppPath = "/",
            DisplayStorageConnectionString = false,
            DisplayNameFunc = (dsContext, job) => job.ToString(),
            StatsPollingInterval = 10 * 1000,
            DashboardTitle = "MaximaTech - Background Jobs",
            IgnoreAntiforgeryToken = true,
            Authorization = new[] { new DashboardNoAuthorizationFilter() }
        };

    }
    public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return true;
        }
    }
}