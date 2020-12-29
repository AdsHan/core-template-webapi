using EntregaFutura.Domain.Models;

namespace EntregaFutura.Domain.DTO
{
    public class PedidoItemDTO
    {
        public int PedidoId { get; set; }

        public int ItemId { get; set; }

        public int ProdutoId { get; set; }

        public int? ObservacaoId { get; set; }

        public decimal Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }

        public decimal PercentualDesconto { get; set; }

        public decimal ValorMercadoria { get; set; }

        public decimal ValorTotal { get; set; }

        public virtual ProdutoModel Produto { get; set; }

        public virtual ObservacaoModel Observacao { get; set; }

    }

}
