
using EntregaFutura.Domain.Models;
using System.Collections.Generic;

namespace EntregaFutura.Repository.DTO
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Descricao { get; set; }
        public string PhoneNumber { get; set; }
        public string Contato { get; set; }
        public string? VendedorId { get; set; }
        public int? ListaPrecoPadraoId { get; set; }
        public int? ObservacaoId { get; set; }
        public virtual UsuarioModel UsuarioVendedorModel { get; set; }
        public virtual ObservacaoModel Observacao { get; set; }
        public virtual ICollection<UsuarioRegraModel> UsuarioRegras { get; set; }
    }
}
