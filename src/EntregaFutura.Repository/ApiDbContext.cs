
using EntregaFutura.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EntregaFutura.Repository.Model
{

    public class ApiDbContext : IdentityDbContext<UsuarioModel, RegraModel, string, IdentityUserClaim<string>, UsuarioRegraModel, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        public DbSet<EntregaModel> Entregas { get; set; }
        public DbSet<ImagemModel> Imagens { get; set; }
        public DbSet<ListaPrecoModel> ListasPreco { get; set; }
        public DbSet<ListaPrecoProdutoModel> ListasPrecoProduto { get; set; }
        public DbSet<ObservacaoModel> Observacoes { get; set; }
        public DbSet<PedidoItemModel> PedidosItem { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<ProdutoGrupoModel> ProdutosGrupo { get; set; }
        public DbSet<ProdutoImagemModel> ProdutosImagem { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioRegraModel>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Regra)
                    .WithMany(r => r.UsuarioRegras)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.Usuario)
                    .WithMany(r => r.UsuarioRegras)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            });

            modelBuilder.Entity<ListaPrecoProdutoModel>()
                .HasKey(t => new { t.ListaPrecoProdutoId, t.ProdutoId });

            modelBuilder.Entity<PedidoItemModel>()
                .HasKey(t => new { t.PedidoId, t.ItemId });

            modelBuilder.Entity<ProdutoImagemModel>()
                .HasKey(t => new { t.ProdutoId, t.ImagemId });

        }


    }
}
