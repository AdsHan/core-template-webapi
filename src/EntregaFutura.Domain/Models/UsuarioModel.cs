using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntregaFutura.Domain.Models
{
    public class UsuarioModel : IdentityUser
    {
        public string? VendedorId { get; set; }

        public int? ListaPrecoPadraoId { get; set; }

        public int? ObservacaoId { get; set; }

        [Required(ErrorMessage = "A descrição do produto não foi informada")]
        [StringLength(90, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string Descricao { get; set; }

        [StringLength(30, ErrorMessage = "O Contato deve ter no máximo {1}")]
        public string Contato { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsContratoAtivo { get; set; }

        public string ImagemUrl { get; set; }

        public DateTime DataUltimoAcesso { get; set; }

        [Required(ErrorMessage = "A data de inclusão do grupo de produto não foi informada")]
        public DateTime DataInclusao { get; set; }

        public ICollection<UsuarioRegraModel> UsuarioRegras { get; set; }

        [ForeignKey("VendedorId")]
        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        [ForeignKey("ListaPrecoPadraoId")]
        public virtual ListaPrecoModel ListaPreco { get; set; }

        [ForeignKey("ObservacaoId")]
        public virtual ObservacaoModel Observacao { get; set; }

    }
}
