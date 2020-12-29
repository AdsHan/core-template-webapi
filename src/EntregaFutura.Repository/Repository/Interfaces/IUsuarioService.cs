using EntregaFutura.Domain.Models;
using System.Threading.Tasks;

namespace DevIO.Business.Intefaces
{
    public enum LevelUser
    {
        NaoLocalizado = 0,
        Admin,
        Vendedor,
        Cliente
    }

    public interface IUsuarioService
    {
        string Name { get; }
        Task<LevelUser> GetCurrentUserLevel(UsuarioModel user = null);
        Task<UsuarioModel> GetUsuarioVendedor();
    }
}