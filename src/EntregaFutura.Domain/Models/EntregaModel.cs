using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("Entregas")]
    public class EntregaModel
    {
        [Key]
        public int EntregaId { get; set; }

        [Required(ErrorMessage = "O vendedor da entrega não foi informado")]
        public string VendedorId { get; set; }

        public int? ObservacaoId { get; set; }

        [Required(ErrorMessage = "O status da entrega não foi informado")]
        [Range(1, 9, ErrorMessage = "O status deve estar entre {1} e {2}")]
        public int Status { get; set; }

        [Required(ErrorMessage = "A descrição da entrega não foi informada")]
        [StringLength(90, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string Descricao { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(3,2)")]
        [Range(0, 100, ErrorMessage = "O percentual de desconto da entrega deve estar entre {1} e {2}")]
        public decimal PercentualDesconto { get; set; }

        [Required(ErrorMessage = "A data de inclusão da entrega não foi informada")]
        public DateTime DataInclusao { get; set; }

        public DateTime DataAbertura { get; set; }

        public DateTime DataEncerramento { get; set; }

        [ForeignKey("VendedorId")]
        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        [ForeignKey("ObservacaoId")]
        public virtual ObservacaoModel Observacao { get; set; }

    }

}
