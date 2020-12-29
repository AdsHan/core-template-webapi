using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;

namespace EntregaFutura.Repository.Repository
{
    public class ImagemRepository : Repository<ImagemModel>, IImagemRepository
    {
        public ImagemRepository(ApiDbContext contexto) : base(contexto)
        {

        }

    }
}
