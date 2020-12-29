using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class ListaPrecoRepository : Repository<ListaPrecoModel>, IListaPrecoRepository
    {
        public ListaPrecoRepository(ApiDbContext contexto) : base(contexto)
        {

        }

    }
}
