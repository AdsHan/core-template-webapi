
using EntregaFutura.Domain.Models;

namespace EntregaFutura.Repository.DTO
{
    public class ListaPrecoProdutoDTO
    {
        public int ListaPrecoProdutoId { get; set; }

        public int ProdutoId { get; set; }

        public int? ObservacaoId { get; set; }

        public string Descricao { get; set; }

        public decimal Preco { get; set; }

        public decimal QuantidadeMinima { get; set; }

        public decimal PercentualDesconto { get; set; }

        public virtual ProdutoModel Produto { get; set; }

        public virtual ObservacaoModel Observacao { get; set; }

    }

}
