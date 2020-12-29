using DevIO.Business.Intefaces;
using EntregaFutura.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EntregaFutura.Api.Services
{

    public class UsuarioService : IUsuarioService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<UsuarioModel> _userManager;

        public UsuarioService(IHttpContextAccessor accessor, UserManager<UsuarioModel> userManager)
        {
            _accessor = accessor;
            _userManager = userManager;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public async Task<LevelUser> GetCurrentUserLevel(UsuarioModel user = null)
        {

            UsuarioModel usuario = new UsuarioModel() { };

            if (user == null)
            {
                //var userInRole = await _userManager.IsInRoleAsync(user, role);
                usuario = await _userManager.FindByNameAsync(Name);
            }
            else
            {
                usuario = user;
            }

            if (usuario == null) return LevelUser.NaoLocalizado;
            var regras = await _userManager.GetRolesAsync(usuario);
            if (regras.Count == 0) return LevelUser.NaoLocalizado;

            switch (regras[0])
            {
                case "Admin":
                    {
                        return LevelUser.Admin;
                        break;
                    }
                case "Vendedor":
                    {
                        return LevelUser.Vendedor;
                        break;
                    }
                case "Cliente":
                    {
                        return LevelUser.Cliente;
                        break;
                    }

                default: return LevelUser.NaoLocalizado;
            }
        }

        public async Task<bool> GetCurrentUserAdmin()
        {
            //var userInRole = await _userManager.IsInRoleAsync(user, role);
            var usuario = await _userManager.FindByNameAsync(Name);
            if (usuario == null) return false;
            var regras = await _userManager.GetRolesAsync(usuario);
            if (regras.Count == 0) return false;
            if (regras[0] == "Admin" || regras[0] == "Vendedor") return true;
            return false;
        }

        public async Task<UsuarioModel> GetUsuarioVendedor()
        {
            var usuario = await _userManager.FindByNameAsync(Name);
            if (usuario == null) return usuario;
            return usuario.UsuarioVendedorModel;
        }

    }

}
