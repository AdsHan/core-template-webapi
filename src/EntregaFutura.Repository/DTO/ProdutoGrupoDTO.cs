

using EntregaFutura.Domain.Models;

namespace EntregaFutura.Repository.DTO
{
    public class ProdutoGrupoDTO
    {

        public int ProdutoGrupoId { get; set; }

        public string VendedorId { get; set; }

        public string Referencia { get; set; }

        public string Descricao { get; set; }

        public decimal PercentualDesconto { get; set; }

        public virtual UsuarioModel UsuarioVendedorModel { get; set; }

        public virtual ObservacaoModel Observacao { get; set; }

    }

}
