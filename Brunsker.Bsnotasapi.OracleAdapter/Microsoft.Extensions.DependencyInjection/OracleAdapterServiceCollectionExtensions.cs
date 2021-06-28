using Brunsker.Bsnotas.OracleAdapter;
using Brunsker.Bsnotas.Domain.Adapters;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OracleAdapterServiceCollectionExtensions
    {

        public static IServiceCollection AddOracleAdapterRespository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOracleRepositoryAdapter, OracleRepositoryAdapter>();

            return services;
        }
    }
}