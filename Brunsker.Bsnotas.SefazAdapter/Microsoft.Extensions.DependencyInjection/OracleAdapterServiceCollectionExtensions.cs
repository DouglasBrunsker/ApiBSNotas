
using Brunsker.Bsnotas.SefazAdapter;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Microsoft.Extensions.Configuration;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class SefazAdapterServiceCollectionExtensions
    {
        public static IServiceCollection AddSefazAdapterRespository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISefazApiAdapter, SefazApiAdapter>();

            services.AddScoped<ICteSefazAdapter, CteSefazAdapter>();

            return services;
        }
    }
}