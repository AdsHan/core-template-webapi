using System.Threading.Tasks;

namespace EntregaFutura.Repository.Repository
{
    public interface IUnitOfWork
    {
        IObservacaoRepository ObservacaoRepository { get; }
        IProdutoRepository ProdutoRepository { get; }
        IProdutoImagemRepository ProdutoImagemRepository { get; }
        IProdutoGrupoRepository ProdutoGrupoRepository { get; }
        IListaPrecoRepository ListaPrecoRepository { get; }
        IListaPrecoProdutoRepository ListaPrecoProdutoRepository { get; }
        IEntregaRepository EntregaRepository { get; }
        IPedidoRepository PedidoRepository { get; }
        IPedidoItemRepository PedidoItemRepository { get; }
        IImagemRepository ImagemRepository { get; }

        Task Commit();
    }
}
