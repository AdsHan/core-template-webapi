
using EntregaFutura.Domain.Models;
using System.Collections.Generic;

namespace EntregaFutura.Repository.DTO
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        public string Referencia { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal QuantidadeMinima { get; set; }
        public string VendedorId { get; set; }
        public int ProdutoGrupoId { get; set; }
        public int? ObservacaoId { get; set; }
        public virtual List<ProdutoImagemModel> Imagens { get; set; }
        public virtual UsuarioModel UsuarioVendedorModel { get; set; }
        public virtual ProdutoGrupoModel ProdutoGrupo { get; set; }
        public virtual ObservacaoModel Observacao { get; set; }
    }
}
