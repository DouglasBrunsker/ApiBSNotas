using Brunsker.Bsnotas.Application;
using Brunsker.Bsnotas.Domain.Services;
using Brunsker.Bsnotasapi.Application.Services;
using Brunsker.Bsnotasapi.Domain.Services;

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
