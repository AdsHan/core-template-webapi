using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class ListaPrecoProdutoRepository : Repository<ListaPrecoProdutoModel>, IListaPrecoProdutoRepository
    {
        public ListaPrecoProdutoRepository(ApiDbContext contexto) : base(contexto)
        {

        }

    }
}
