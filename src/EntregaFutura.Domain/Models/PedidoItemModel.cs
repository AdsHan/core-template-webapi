using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("PedidosItem")]
    public class PedidoItemModel
    {
        [Key]
        [Column(Order = 1)]
        public int PedidoId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "O produto do item do pedido não foi informado")]
        public int ProdutoId { get; set; }

        public int? ObservacaoId { get; set; }

        [Required(ErrorMessage = "A quantidade do item do pedido não foi informada")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,3)")]
        public decimal Quantidade { get; set; }

        [Required(ErrorMessage = "O valor unitário do item do pedido não foi informada")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        public decimal ValorUnitario { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(3,2)")]
        [Range(0, 100, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        public decimal PercentualDesconto { get; set; }

        [Required(ErrorMessage = "O valor da mercadoria do item do pedido não pode ser zeros")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        public decimal ValorMercadoria { get; set; }

        [Required(ErrorMessage = "O valor total do item do pedido não pode ser zeros")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        public decimal ValorTotal { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual ProdutoModel Produto { get; set; }

        [ForeignKey("ObservacaoId")]
        public virtual ObservacaoModel Observacao { get; set; }

    }

}
