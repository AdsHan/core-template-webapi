using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class ObservacaoRepository : Repository<ObservacaoModel>, IObservacaoRepository
    {
        public ObservacaoRepository(ApiDbContext contexto) : base(contexto)
        {

        }

    }
}
