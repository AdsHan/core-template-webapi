using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntregaFutura.Repository.Repository
{
    public interface IProdutoRepository : IRepository<ProdutoModel>
    {

        Task<PagedList<ProdutoModel>> GetProdutosPaginados(ProdutosParameters produtosParameters);
        Task<IEnumerable<ProdutoModel>> GetImagens(int ProdutoId);
        Task<IEnumerable<ProdutoModel>> GetProdutosPorPreco();

    }
}
