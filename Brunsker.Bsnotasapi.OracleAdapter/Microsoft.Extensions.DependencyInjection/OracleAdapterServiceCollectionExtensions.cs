using Brunsker.Bsnotas.Domain.Interfaces;
using Brunsker.Bsnotas.OracleAdapter.Repositories;
using Brunsker.Bsnotas.OracleAdapter.Repositories.RepositoryBase;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OracleAdapterServiceCollectionExtensions
    {
        public static void AddOracleAdapterRespository(this IServiceCollection services)
        {
            services.AddTransient<INFEntradaRepository, NFEntradaRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<INFSaidaRepository, NFSaidaRepository>();
            services.AddTransient<IFornecedorRepository, FornecedorRepository>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<ICteRepository, CteRepository>();
            services.AddTransient<ICadastroCFOPRepository, CadastroCFOPRepository>();
            services.AddTransient<INfseServiceRepository, NfseServicoRepository>();
        }
    }
}