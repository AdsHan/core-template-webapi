using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class EntregaRepository : Repository<EntregaModel>, IEntregaRepository
    {
        public EntregaRepository(ApiDbContext contexto) : base(contexto)
        {

        }

    }
}
