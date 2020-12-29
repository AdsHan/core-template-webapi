using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("ProdutosGrupo")]
    public class ProdutoGrupoModel
    {
        [Key]
        public int ProdutoGrupoId { get; set; }

        [Required(ErrorMessage = "O vendedor do grupo de produto não foi informado")]
        public string VendedorId { get; set; }

        public int? ObservacaoId { get; set; }

        [StringLength(30, ErrorMessage = "A Referencia deve ter no máximo {1}")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "A descrição do grupo de produto não foi informada")]
        [StringLength(90, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string Descricao { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(3,2)")]
        [Range(0, 100, ErrorMessage = "O percentual de desconto deve estar entre {1} e {2}")]
        public decimal PercentualDesconto { get; set; }

        [Required(ErrorMessage = "A data de inclusão do grupo de produto não foi informada")]
        public DateTime DataInclusao { get; set; }

        [ForeignKey("VendedorId")]
        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        [ForeignKey("ObservacaoId")]
        public virtual ObservacaoModel Observacao { get; set; }

    }

}
