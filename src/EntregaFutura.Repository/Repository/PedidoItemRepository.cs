using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class PedidoItemRepository : Repository<PedidoItemModel>, IPedidoItemRepository
    {
        public PedidoItemRepository(ApiDbContext contexto) : base(contexto)
        {

        }

    }
}
