using System.Collections.Generic;
using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface IFornecedorRepository
    {
        Task<IEnumerable<Fornecedor>> SelectFornecedores(FiltroPesquisaFornecedor filtro);
    }
}