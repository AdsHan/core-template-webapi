using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    [Table("ListasPreco")]
    public class ListaPrecoModel
    {
        [Key]
        public int ListaPrecoId { get; set; }

        [Required(ErrorMessage = "O vendedor da lista de preço não foi informado")]
        public string VendedorId { get; set; }

        public int? ObservacaoId { get; set; }

        [Required(ErrorMessage = "A referência da lista de preço não foi informada")]
        [StringLength(30, ErrorMessage = "A Referencia deve ter no máximo {1}")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "A descrição da lista de preço não foi informada")]
        [StringLength(90, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de inclusão da lista de preço não foi informada")]
        public DateTime DataInclusao { get; set; }

        public DateTime DataValidade { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAtivo { get; set; }

        public ICollection<ListaPrecoProdutoModel> ProdutosLista { get; set; }

        [ForeignKey("VendedorId")]
        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        [ForeignKey("ObservacaoId")]
        public virtual ObservacaoModel Observacao { get; set; }

    }

}
