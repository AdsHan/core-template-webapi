using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.Model;
using EntregaFutura.Repository.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntregaFutura.Repository.Repository
{
    public class ProdutoRepository : Repository<ProdutoModel>, IProdutoRepository
    {
        public ProdutoRepository(ApiDbContext contexto) : base(contexto)
        {
        }

        public async Task<IEnumerable<ProdutoModel>> GetImagens(int ProdutoId)
        {

            return await Get().Include(x => x.Imagens.Select(q => q.Imagem)).Where(x => x.ProdutoId == ProdutoId).ToListAsync();
            //return await Get().Include(a => a.Imagens).ThenInclude(c => c.Imagem).Where(x => x.ProdutoId == ProdutoId).ToListAsync();

        }

        public async Task<PagedList<ProdutoModel>> GetProdutosPaginados(ProdutosParameters produtosParameters)
        {
            //return Get()
            //    .OrderBy(on => on.Nome)
            //    .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
            //    .Take(produtosParameters.PageSize)
            //    .ToList();

            return await PagedList<ProdutoModel>.ToPagedList(Get().OrderBy(on => on.ProdutoId),
                produtosParameters.PageNumber, produtosParameters.PageSize);
        }

        public async Task<IEnumerable<ProdutoModel>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }

    }
}
