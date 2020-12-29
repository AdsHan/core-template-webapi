using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("Produtos")]
    public class ProdutoModel
    {
        public ProdutoModel()
        {
            Imagens = new Collection<ProdutoImagemModel>();
        }

        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O vendedor do produto não foi informado")]
        public string VendedorId { get; set; }

        public int ProdutoGrupoId { get; set; }

        public int? ObservacaoId { get; set; }

        [StringLength(30, ErrorMessage = "A Referencia deve ter no máximo {1}")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "A descrição do produto não foi informada")]
        [StringLength(90, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O preço do produto não foi informada")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Preco { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8,3)")]
        public decimal QuantidadeMinima { get; set; }

        [Required(ErrorMessage = "A data de inclusão do produto não foi informado")]
        public DateTime DataInclusao { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAtivo { get; set; }

        public virtual ICollection<ProdutoImagemModel> Imagens { get; set; }

        [ForeignKey("VendedorId")]
        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        [ForeignKey("ProdutoGrupoId")]
        public virtual ProdutoGrupoModel ProdutoGrupo { get; set; }

        [ForeignKey("ObservacaoId")]
        public virtual ObservacaoModel Observacao { get; set; }

    }

}
