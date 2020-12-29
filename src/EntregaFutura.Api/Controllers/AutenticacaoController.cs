using AutoMapper;
using DevIO.Business.Intefaces;
using EntregaFutura.Api.Services;
using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EntregaFutura.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[Controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<UsuarioModel> _userManager;
        private readonly SignInManager<UsuarioModel> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        private readonly DataService _dataService;

        public AutenticacaoController(
            IMapper mapper,
            UserManager<UsuarioModel> userManager,
            SignInManager<UsuarioModel> signInManager,
            IConfiguration configuration,
            DataService dataService,
            IUsuarioService usuarioService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dataService = dataService;
            _usuarioService = usuarioService;
        }

        // GET: api/autenticacao
        /// <summary>
        /// Obtém a lista de usuários
        /// </summary>
        /// <returns>Coleção de objetos Usuários</returns>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> Get()
        {

            List<UsuarioModel> usuarios = new List<UsuarioModel>();

            LevelUser level = await _usuarioService.GetCurrentUserLevel();

            switch (level)
            {
                case LevelUser.Admin:
                    {
                        usuarios = await _userManager.Users.
                                            Include(x => x.Observacao).
                                            Include(x => x.UsuarioRegras).
                                            ThenInclude(r => r.Regra).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
                case LevelUser.Vendedor:
                    {
                        usuarios = await _userManager.Users.
                                            Where(x => x.UsuarioVendedorModel.UserName == User.Identity.Name).
                                            Include(x => x.Observacao).
                                            Include(x => x.UsuarioRegras).
                                            ThenInclude(r => r.Regra).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
                case LevelUser.Cliente:
                    {
                        var usuarioVendedor = await _usuarioService.GetUsuarioVendedor();
                        if (usuarioVendedor == null) break;

                        usuarios = await _userManager.Users.
                                            Where(x => x.IsAtivo == true && x.UsuarioVendedorModel.UserName == usuarioVendedor.UserName).
                                            Include(x => x.Observacao).
                                            Include(x => x.UsuarioRegras).
                                            ThenInclude(r => r.Regra).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
            }

            var usuariosDTO = _mapper.Map<List<UsuarioDTO>>(usuarios);
            return usuariosDTO;

        }

        // GET: api/autenticacao/1
        /// <summary>
        /// Obtém um usuários pelo seu identificador UsuarioID
        /// </summary>
        /// <param name="name">Código do usuário</param>
        /// <returns>Objeto Usuário</returns>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioDTO>> GetById(string name)
        {
            // return await um?.Users?.SingleOrDefaultAsync(x => x.UserName == name);
            var applicationUserModel = await _userManager.FindByNameAsync(name);
            if (applicationUserModel == null)
            {
                return NotFound();
            }

            var usuarioDTO = _mapper.Map<UsuarioDTO>(applicationUserModel);
            return usuarioDTO;

        }

        // POST: api/autenticacao/incluir        
        /// <summary>
        /// Inclui um novo usuário
        /// </summary>
        /// <param name="usuarioDto">Objeto Usuário</param>
        /// <returns>Token</returns>        
        [HttpPost("incluir")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO usuarioDto)
        {

            var user = new UsuarioModel
            {
                Email = usuarioDto.Email,
                UserName = usuarioDto.UserName,
                NormalizedUserName = usuarioDto.UserName.ToUpper(),
                PhoneNumber = usuarioDto.PhoneNumber,
                Descricao = usuarioDto.Descricao,
                Contato = usuarioDto.Contato,
                DataInclusao = _dataService.getDataHoraBrasilia(),
                DataUltimoAcesso = _dataService.getDataHoraBrasilia(),
                VendedorId = usuarioDto.VendedorId,
                ListaPrecoPadraoId = usuarioDto.ListaPrecoPadraoId,
                Observacao = usuarioDto.Observacao,
                IsAtivo = true,
                IsDeleted = false,
                IsContratoAtivo = false,
            };

            var result = await _userManager.CreateAsync(user, usuarioDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);

            return Ok(GeraTokenAsync(usuarioDto));

        }

        // GET: api/autenticacao/login
        /// <summary>
        /// Loga o usuário no sistma
        /// </summary>
        /// <param name="usuarioDTO">Objeto Usuário</param>        
        /// <returns>Token</returns>        
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO usuarioDTO)
        {

            // Verifica as credenciais do usuário e retorna um valor
            var result = await _signInManager.PasswordSignInAsync(
                usuarioDTO.UserName,
                usuarioDTO.Password,
                isPersistent: false,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // Atualiza a data do último acesso
                var usuario = await _userManager.FindByNameAsync(usuarioDTO.UserName);
                usuario.DataUltimoAcesso = _dataService.getDataHoraBrasilia();

                await _userManager.UpdateAsync(usuario);

                return Ok(GeraTokenAsync(usuarioDTO));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido...");
                return BadRequest(ModelState);
            }
        }

        // GET: api/autenticacao/logout
        /// <summary>
        /// Desloga o usuário no sistma
        /// </summary>        
        /// <returns>Token</returns>        
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return Ok();

        }

        private async Task<LoginTokenDTO> GeraTokenAsync(UsuarioDTO usuarioDTO)
        {

            var user = await _userManager.FindByEmailAsync(usuarioDTO.UserName);
            LevelUser level = await _usuarioService.GetCurrentUserLevel(user);

            // Define claims do usuário (nao é obrigatorio, mas melhora a segurança(cria mais chaves no PAYLOAD))
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, usuarioDTO.UserName),
                 new Claim("meuPet", "pipoca"),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            // Gera uma chave com base em um algoritmo simétrico
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            // Gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tempo de expiracão do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // Classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credenciais);

            // Retorna os dados com o token e informacoes
            var response = new LoginTokenDTO
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK",
                UserToken = new UserTokenDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Level = level
                }
            };

            return response;

        }
    }
}
