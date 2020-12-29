using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class ProdutoGrupoRepository : Repository<ProdutoGrupoModel>, IProdutoGrupoRepository
    {
        public ProdutoGrupoRepository(ApiDbContext contexto) : base(contexto)
        {

        }

    }
}
