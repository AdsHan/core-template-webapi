using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("Pedidos")]
    public class PedidoModel
    {
        [Key]
        public int PedidoId { get; set; }

        [Required(ErrorMessage = "O vendedor da entrega não foi informado")]
        public string VendedorId { get; set; }

        [Required(ErrorMessage = "O cliente do pedido não foi informado")]
        public string ClienteId { get; set; }

        public int? ObservacaoId { get; set; }

        [Required(ErrorMessage = "O status do pedido não foi informado")]
        [Range(1, 9, ErrorMessage = "O status deve estar entre {1} e {2}")]
        public int Status { get; set; }

        [Required(ErrorMessage = "A data de inclusão do pedido não foi informado")]
        public DateTime DataInclusao { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(3,2)")]
        [Range(0, 100, ErrorMessage = "O percentual de desconto do pedido deve estar entre {1} e {2}")]
        public decimal PercentualDesconto { get; set; }

        [Required(ErrorMessage = "O valor da mercadoria do pedido não pode ser zeros")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        public decimal ValorMercadoria { get; set; }

        [Required(ErrorMessage = "O valor total do pedido não pode ser zeros")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        public decimal ValorTotal { get; set; }

        public ICollection<PedidoItemModel> ItensPedido { get; set; }

        [ForeignKey("VendedorId")]
        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        [ForeignKey("ClienteId")]
        public virtual UsuarioModel UsuarioClienteModel { get; set; }

        [ForeignKey("ObservacaoId")]
        public virtual ObservacaoModel Observacao { get; set; }

    }

}
