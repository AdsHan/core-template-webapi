using EntregaFutura.Repository.Model;
using EntregaFutura.Repository.Repository;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ObservacaoRepository _observacaoRepo;
        private ProdutoRepository _produtoRepo;
        private ProdutoImagemRepository _produtoImagemRepo;
        private ProdutoGrupoRepository _produtoGrupoRepo;
        private ListaPrecoRepository _listaPrecoRepo;
        private ListaPrecoProdutoRepository _listaPrecoProdutoRepo;
        private EntregaRepository _entregaRepo;
        private PedidoRepository _pedidoRepo;
        private PedidoItemRepository _pedidoItemRepo;
        private ImagemRepository _imagemRepo;

        public ApiDbContext _context;
        public UnitOfWork(ApiDbContext contexto)
        {
            _context = contexto;
        }

        public IObservacaoRepository ObservacaoRepository
        {
            get
            {
                return _observacaoRepo = _observacaoRepo ?? new ObservacaoRepository(_context);
            }
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
            }
        }

        public IProdutoImagemRepository ProdutoImagemRepository
        {
            get
            {
                return _produtoImagemRepo = _produtoImagemRepo ?? new ProdutoImagemRepository(_context);
            }
        }

        public IProdutoGrupoRepository ProdutoGrupoRepository
        {
            get
            {
                return _produtoGrupoRepo = _produtoGrupoRepo ?? new ProdutoGrupoRepository(_context);
            }
        }

        public IListaPrecoRepository ListaPrecoRepository
        {
            get
            {
                return _listaPrecoRepo = _listaPrecoRepo ?? new ListaPrecoRepository(_context);
            }
        }

        public IListaPrecoProdutoRepository ListaPrecoProdutoRepository
        {
            get
            {
                return _listaPrecoProdutoRepo = _listaPrecoProdutoRepo ?? new ListaPrecoProdutoRepository(_context);
            }
        }

        public IEntregaRepository EntregaRepository
        {
            get
            {
                return _entregaRepo = _entregaRepo ?? new EntregaRepository(_context);
            }
        }

        public IPedidoRepository PedidoRepository
        {
            get
            {
                return _pedidoRepo = _pedidoRepo ?? new PedidoRepository(_context);
            }
        }

        public IPedidoItemRepository PedidoItemRepository
        {
            get
            {
                return _pedidoItemRepo = _pedidoItemRepo ?? new PedidoItemRepository(_context);
            }
        }

        public IImagemRepository ImagemRepository
        {
            get
            {
                return _imagemRepo = _imagemRepo ?? new ImagemRepository(_context);
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
