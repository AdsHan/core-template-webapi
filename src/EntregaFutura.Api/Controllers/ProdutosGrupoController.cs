
using AutoMapper;
using DevIO.Business.Intefaces;
using EntregaFutura.Api.Services;
using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.DTO;
using EntregaFutura.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntregaFutura.Api.Controllers
{
    [ApiVersion("1.0")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ProdutosGrupoController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly DataService _dataService;

        public ProdutosGrupoController(
            IUnitOfWork uof,
            IMapper mapper,
            DataService dataService,
            IUsuarioService usuarioService)
        {
            _uof = uof;
            _mapper = mapper;
            _dataService = dataService;
            _usuarioService = usuarioService;
        }

        // GET: api/produtosgrupo
        /// <summary>
        /// Obtém a lista de grupos de produtos
        /// </summary>
        /// <returns>Coleção de objetos grupos de produtos</returns>        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProdutoGrupoDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProdutoGrupoDTO>>> Get()
        {

            List<ProdutoGrupoModel> produtosGrupo = new List<ProdutoGrupoModel>();

            LevelUser level = await _usuarioService.GetCurrentUserLevel();

            switch (level)
            {
                case LevelUser.Admin:
                    {
                        produtosGrupo = await _uof.ProdutoGrupoRepository.Get().
                                                Include(x => x.UsuarioVendedorModel).
                                                Include(x => x.Observacao).
                                                AsNoTracking().
                                                ToListAsync();
                        break;
                    }
                case LevelUser.Vendedor:
                    {
                        produtosGrupo = await _uof.ProdutoGrupoRepository.Get().
                                                Where(x => x.UsuarioVendedorModel.UserName == User.Identity.Name).
                                                Include(x => x.UsuarioVendedorModel).
                                                Include(x => x.Observacao).
                                                AsNoTracking().
                                                ToListAsync();
                        break;
                    }
                case LevelUser.Cliente:
                    {
                        var usuarioVendedor = await _usuarioService.GetUsuarioVendedor();
                        if (usuarioVendedor == null) break;

                        produtosGrupo = await _uof.ProdutoGrupoRepository.Get().
                                                Where(x => x.UsuarioVendedorModel.UserName == usuarioVendedor.UserName).
                                                Include(x => x.UsuarioVendedorModel).
                                                Include(x => x.Observacao).
                                                AsNoTracking().
                                                ToListAsync();
                        break;
                    }
            }

            var produtosGrupoDTO = _mapper.Map<List<ProdutoGrupoDTO>>(produtosGrupo);
            return produtosGrupoDTO;

        }

        // GET: api/produtosgrupo/5
        /// <summary>
        /// Obtém o grupo de produto pelo seu ID
        /// </summary>
        /// <param name="id">Código do grupo de grupo</param>
        /// <returns>Objeto grupo de produto </returns>        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutoGrupoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProdutoGrupoDTO>> GetById(int id)
        {
            var produtoGrupoModel = await _uof.ProdutoGrupoRepository.GetById(p => p.ProdutoGrupoId == id);

            if (produtoGrupoModel == null)
            {
                return NotFound();
            }

            var produtoGrupoDTO = _mapper.Map<ProdutoGrupoDTO>(produtoGrupoModel);
            return produtoGrupoDTO;
        }

        // POST: api/produtosgrupo       
        /// <summary>
        /// Inclui o novo grupo de produto
        /// </summary>
        /// <param name="produtoGrupoDto">Objeto grupo de produto</param>
        /// <returns>O objeto grupo de produto incluído</returns>        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ActionName("GravaProdutoGrupo")]
        public async Task<ActionResult<ProdutoGrupoDTO>> Post([FromBody] ProdutoGrupoDTO produtoGrupoDto)
        {

            var produtoGrupoModel = _mapper.Map<ProdutoGrupoModel>(produtoGrupoDto);
            produtoGrupoModel.DataInclusao = _dataService.getDataHoraBrasilia();

            _uof.ProdutoGrupoRepository.Add(produtoGrupoModel);

            await _uof.Commit();

            var produtoGrupoDTO = _mapper.Map<ProdutoGrupoDTO>(produtoGrupoModel);

            return CreatedAtAction("GravaProdutoGrupo", new { id = produtoGrupoDTO.ProdutoGrupoId }, produtoGrupoDTO);
        }

        // PUT: api/produtosgrupo/5
        /// <summary>
        /// Atualiza o grupo de produto pelo ID
        /// </summary>
        /// <param name="id">Código do grupo de produto</param>
        /// <param name="produtoGrupoDto">Objeto grupo de produto</param>
        /// <returns>O objeto grupo de produto alterado</returns>        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProdutoGrupoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ProdutoGrupoDTO produtoGrupoDto)
        {
            if (id != produtoGrupoDto.ProdutoGrupoId)
            {
                return BadRequest();
            }

            var produtoGrupoModel = _mapper.Map<ProdutoGrupoModel>(produtoGrupoDto);

            _uof.ProdutoGrupoRepository.Update(produtoGrupoModel);

            try
            {
                await _uof.Commit();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoGrupoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // DELETE: api/produtosgrupo/5        
        /// <summary>
        /// Deleta um produto pelo seu ID
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>O objeto grupo de produto exvluído</returns>        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProdutoGrupoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProdutoGrupoDTO>> Delete(int id)
        {
            var produtoGrupoModel = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produtoGrupoModel == null)
            {
                return NotFound();
            }

            _uof.ProdutoRepository.Delete(produtoGrupoModel);
            await _uof.Commit();

            var produtoGrupoDto = _mapper.Map<ProdutoGrupoDTO>(produtoGrupoModel);
            return produtoGrupoDto;
        }

        private bool ProdutoGrupoModelExists(int id)
        {
            var produtoGrupo = _uof.ProdutoGrupoRepository.GetById(e => e.ProdutoGrupoId == id);
            return produtoGrupo != null ? true : false;
        }
    }
}
