using DevIO.Business.Intefaces;
using System;
namespace EntregaFutura.Repository.DTO
{
    public class UserTokenDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public LevelUser Level { get; set; }
    }

    public class LoginTokenDTO
    {
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public UserTokenDTO UserToken { get; set; }
    }

}
