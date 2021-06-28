using Brunsker.Bsnotas.Domain.Adapters;
using Brunsker.Bsnotas.SefazAdapter;
using Microsoft.Extensions.Configuration;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class SefazAdapterServiceCollectionExtensions
    {
        public static IServiceCollection AddSefazAdapterRespository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISefazApiAdapter, SefazApiAdapter>();

            return services;
        }
    }
}