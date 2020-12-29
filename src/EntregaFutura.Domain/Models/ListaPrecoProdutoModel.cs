using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("ListasPrecoProduto")]
    public class ListaPrecoProdutoModel
    {
        [Key]
        [Column(Order = 1)]
        public int ListaPrecoProdutoId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ProdutoId { get; set; }

        public int? ObservacaoId { get; set; }

        [Required(ErrorMessage = "A descrição do produto da lista de preço não foi informada")]
        [StringLength(90, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço do produto da lista de preço não foi informada")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,3)")]
        public decimal QuantidadeMinima { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(3,2)")]
        [Range(0, 100, ErrorMessage = "O percentual de desconto deve estar entre {1} e {2}")]
        public decimal PercentualDesconto { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual ProdutoModel Produto { get; set; }

        [ForeignKey("ObservacaoId")]
        public virtual ObservacaoModel Observacao { get; set; }

    }

}
