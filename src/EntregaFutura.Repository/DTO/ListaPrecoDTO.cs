using EntregaFutura.Domain.Models;
using System;
using System.Collections.Generic;

namespace EntregaFutura.Repository.DTO
{

    public class ListaPrecoDTO
    {
        public int ListaPrecoId { get; set; }

        public string VendedorId { get; set; }

        public int? ObservacaoId { get; set; }

        public string Referencia { get; set; }

        public string Descricao { get; set; }

        public DateTime DataValidade { get; set; }

        public ICollection<ListaPrecoProdutoModel> ProdutosLista { get; set; }

        public virtual UsuarioModel UsuarioVendedorDTO { get; set; }

        public virtual ObservacaoModel Observacao { get; set; }

    }

}
