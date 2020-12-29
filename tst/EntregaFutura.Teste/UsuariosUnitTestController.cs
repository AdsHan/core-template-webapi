using AutoMapper;
using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.DTO.Mappings;
using EntregaFutura.Repository.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApiCatalogoxUnitTests
{
    public class UsuariosUnitTestController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<UsuarioModel> _userManager;

        public static DbContextOptions<ApiDbContext> dbContextOptions { get; }

        public static string connectionString =
           "server=localhost;userid=root;password=root;database=entregafutura";

        static UsuariosUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<ApiDbContext>()
               .UseMySql(connectionString)
               .Options;
        }

        public UsuariosUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();

            var context = new ApiDbContext(dbContextOptions);
        }


        [Fact]
        public void GetUsuarios_Return_OkResult()
        {

        }

    }
}
