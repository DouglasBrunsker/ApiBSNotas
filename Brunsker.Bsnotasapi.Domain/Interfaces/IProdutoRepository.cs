using System.Collections.Generic;
using System.Threading.Tasks;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> SelectProdutos(FiltroPesquisaProdutos filtro);
        //Task<ICMS> ExibirICMS(string chave, string CODPROD);
        //Task<IEnumerable<CodProd>> SearchCPROD(string chave);
    }
}