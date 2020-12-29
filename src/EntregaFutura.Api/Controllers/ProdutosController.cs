using AutoMapper;
using DevIO.Business.Intefaces;
using EntregaFutura.Api.Services;
using EntregaFutura.Domain.Models;
using EntregaFutura.Repository.DTO;
using EntregaFutura.Repository.Pagination;
using EntregaFutura.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntregaFutura.Api.Controllers
{
    [ApiVersion("1.0")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly DataService _dataService;

        public ProdutosController(
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

        // GET: api/produtos
        /// <summary>
        /// Obtém a lista de produtos
        /// </summary>
        /// <returns>Coleção de objetos Produtos</returns>        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProdutoDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {

            List<ProdutoModel> produtos = new List<ProdutoModel>();

            LevelUser level = await _usuarioService.GetCurrentUserLevel();

            switch (level)
            {
                case LevelUser.Admin:
                    {
                        produtos = await _uof.ProdutoRepository.Get().
                                            Include(x => x.Imagens).ThenInclude(i => i.Imagem).
                                            Include(x => x.UsuarioVendedorModel).
                                            Include(x => x.ProdutoGrupo).
                                            Include(x => x.Observacao).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
                case LevelUser.Vendedor:
                    {
                        produtos = await _uof.ProdutoRepository.Get().
                                            Where(x => x.UsuarioVendedorModel.UserName == User.Identity.Name).
                                            Include(x => x.Imagens).ThenInclude(i => i.Imagem).
                                            Include(x => x.UsuarioVendedorModel).
                                            Include(x => x.ProdutoGrupo).
                                            Include(x => x.Observacao).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
                case LevelUser.Cliente:
                    {
                        var usuarioVendedor = await _usuarioService.GetUsuarioVendedor();
                        if (usuarioVendedor == null) break;

                        produtos = await _uof.ProdutoRepository.Get().
                                            Where(x => x.IsAtivo == true && x.UsuarioVendedorModel.UserName == usuarioVendedor.UserName).
                                            Include(x => x.Imagens).ThenInclude(i => i.Imagem).
                                            Include(x => x.UsuarioVendedorModel).
                                            Include(x => x.ProdutoGrupo).
                                            Include(x => x.Observacao).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
            }

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;

        }

        // GET: api/produtos/produtos-paginados       
        /// <summary>
        /// Obtém a lista de produtos
        /// </summary>
        /// <returns>Coleção de objetos Produtos</returns>        
        [HttpGet("produtos-paginados")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPaginados([FromQuery] ProdutosParameters produtosParameters)
        {

            var produtos = await _uof.ProdutoRepository.GetProdutosPaginados(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }

        // GET: api/produtos/5
        /// <summary>
        /// Obtém o produto pelo seu ID
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>Objeto Produto</returns>        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProdutoDTO>> GetById(int id)
        {
            // O Find eu vou primeiro na memoria e só posso usar se o Id é a chave primaria
            // FisrtOrDefault(p => o.ProdutoId == id) busca sempre do banco
            //var produto = await _uof.Produtos.FindAsync(id);
            var produtoModel = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produtoModel == null)
            {
                return NotFound();
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produtoModel);
            return produtoDTO;
        }

        // GET: api/produtos/imagens/5
        /// <summary>
        /// Obtém as imagens do produto pelo seu ID
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>Coleção de objetos Imagens</returns>        
        [HttpGet("imagens/{id:int}")]
        [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProdutoImagemDTO>>> GetImagens(int id)
        {
            var imagens = await _uof.ProdutoImagemRepository.GetById(c => c.ProdutoId == id);

            if (imagens == null)
            {
                return NotFound();
            }

            var iamgensDTO = _mapper.Map<List<ProdutoImagemDTO>>(imagens);
            return iamgensDTO;
        }

        // POST: api/produtos       
        /// <summary>
        /// Inclui o novo produto
        /// </summary>
        /// <param name="produtoDto">Objeto produto</param>
        /// <returns>O objeto produto incluído</returns>        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ActionName("GravaProduto")]
        public async Task<ActionResult<ProdutoDTO>> Post([FromBody] ProdutoDTO produtoDto)
        {

            // Se eu nao informace a um campo obrigatorio por exemplo (modelo inválido), até a versão 2.1 eu tinha que testar na mao e retornar 404
            // Agora o o proprio CORE responde um 404 bad request. Antigamente eu tinha que colocar.
            //if (!ModelState.IsValid)
            //{
            //return BadRequest(ModelState);
            //}

            //_uof.Produtos.Add(produto);
            //await _uof.SaveChangesAsync();

            var produtoModel = _mapper.Map<ProdutoModel>(produtoDto);
            produtoModel.DataInclusao = _dataService.getDataHoraBrasilia();

            _uof.ProdutoRepository.Add(produtoModel);

            await _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produtoModel);

            return CreatedAtAction("GravaProduto", new { id = produtoDTO.ProdutoId }, produtoDTO);
        }

        // PUT: api/produtos/5
        /// <summary>
        /// Atualiza o produto pelo ID
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <param name="produtoDto">Objeto produto</param>
        /// <returns>O objeto produto alterado</returns>        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produtoModel = _mapper.Map<ProdutoModel>(produtoDto);

            _uof.ProdutoRepository.Update(produtoModel);

            try
            {
                await _uof.Commit();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // DELETE: api/produtos/5        
        /// <summary>
        /// Deleta o produto pelo seu ID
        /// </summary>
        /// <param name="name">Código do produto</param>
        /// <returns>O objeto produto excluído</returns>        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produtoModel = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produtoModel == null)
            {
                return NotFound();
            }

            _uof.ProdutoRepository.Delete(produtoModel);
            await _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produtoModel);
            return produtoDto;
        }

        private bool ProdutoModelExists(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(e => e.ProdutoId == id);
            return produto != null ? true : false;
        }
    }
}
