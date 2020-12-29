using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EntregaFutura.Domain.Models
{
    public class RegraModel : IdentityRole
    {
        public ICollection<UsuarioRegraModel> UsuarioRegras { get; set; }
    }
}
