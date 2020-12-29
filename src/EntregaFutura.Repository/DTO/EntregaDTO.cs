using EntregaFutura.Domain.Models;
using System;

namespace EntregaFutura.Repository.DTO
{
    public class EntregaDTO
    {

        public int EntregaId { get; set; }

        public string VendedorId { get; set; }

        public int? ObservacaoId { get; set; }

        public int Status { get; set; }

        public string Descricao { get; set; }

        public decimal PercentualDesconto { get; set; }

        public DateTime DataAbertura { get; set; }

        public DateTime DataEncerramento { get; set; }

        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        public virtual ObservacaoModel Observacao { get; set; }

    }

}
