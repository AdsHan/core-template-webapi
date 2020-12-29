using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class ProdutoImagemRepository : Repository<ProdutoImagemModel>, IProdutoImagemRepository
    {
        public ProdutoImagemRepository(ApiDbContext contexto) : base(contexto)
        {
        }

    }
}
