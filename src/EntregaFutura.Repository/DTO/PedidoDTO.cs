using EntregaFutura.Domain.Models;
using System;
using System.Collections.Generic;

namespace EntregaFutura.Domain.DTO
{
    public class PedidoDTO
    {

        public int PedidoId { get; set; }

        public string VendedorId { get; set; }

        public string ClienteId { get; set; }

        public int? ObservacaoId { get; set; }

        public int Status { get; set; }

        public DateTime DataInclusao { get; set; }

        public decimal PercentualDesconto { get; set; }

        public decimal ValorMercadoria { get; set; }

        public decimal ValorTotal { get; set; }

        public ICollection<PedidoItemModel> ItensPedido { get; set; }

        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        public virtual UsuarioModel UsuarioClienteModel { get; set; }

        public virtual ObservacaoModel Observacao { get; set; }

    }

}
