using Brunsker.Bsnotasapi.Application.Services;
using Brunsker.Bsnotasapi.Domain.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection addApplicationService(this IServiceCollection services)
        {
            services.AddScoped<INfeEntradaService, NfeEntradaService>();
            services.AddScoped<IUsuarioServices, UsuarioServices>();

            return services;
        }
    }

}
