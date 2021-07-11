using Brunsker.Bsnotas.OracleAdapter;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.OracleAdapter;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OracleAdapterServiceCollectionExtensions
    {

        public static IServiceCollection AddOracleAdapterRespository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<INFEntradaRepository, NFEntradaRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<INFSaidaRepository, NFSaidaRepository>();

            return services;
        }
    }
}