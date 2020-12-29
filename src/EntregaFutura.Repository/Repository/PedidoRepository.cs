using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class PedidoRepository : Repository<PedidoModel>, IPedidoRepository
    {
        public PedidoRepository(ApiDbContext contexto) : base(contexto)
        {

        }

    }
}
