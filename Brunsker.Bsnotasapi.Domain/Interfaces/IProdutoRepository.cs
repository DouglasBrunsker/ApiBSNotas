using System.Collections.Generic;
using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> SelectProdutos(FiltroPesquisaProdutos filtro);
        Task<ICMS> ExibirICMS(string chave, int codigo_produto);
    }
}