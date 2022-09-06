using Brunsker.Bsnotas.Application.AutoMapperConfigurations;
using Brunsker.Bsnotas.Application.Interfaces;
using Brunsker.Bsnotas.Application.Services;
using Brunsker.Bsnotasapi.Application.Services;
using Brunsker.Bsnotasapi.Domain.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static void addApplicationService(this IServiceCollection services)
        {
            services.AddScoped<ICteService, CteService>();
            services.AddScoped<INFService, NFService>();
            services.AddScoped<IUsuarioServices, UsuarioServices>();
            services.AddScoped<INfseServicoService, NfseServicoService>();

            AutoMapperConfig.Inicialize();
        }
    }

}
