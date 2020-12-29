using Microsoft.AspNetCore.Identity;

namespace EntregaFutura.Domain.Models
{
    public class UsuarioRegraModel : IdentityUserRole<string>
    {
        public virtual UsuarioModel Usuario { get; set; }
        public virtual RegraModel Regra { get; set; }
    }
}
