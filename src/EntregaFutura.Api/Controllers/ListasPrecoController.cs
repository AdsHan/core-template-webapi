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
    public class ListasPrecoController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly DataService _dataService;

        public ListasPrecoController(
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

        // GET: api/listaspreco
        /// <summary>
        /// Obtém as listas de preço
        /// </summary>
        /// <returns>Coleção de objetos listas de preço</returns>        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ListaPrecoDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ListaPrecoDTO>>> Get()
        {

            List<ListaPrecoModel> listasPreco = new List<ListaPrecoModel>();

            LevelUser level = await _usuarioService.GetCurrentUserLevel();

            switch (level)
            {
                case LevelUser.Admin:
                    {
                        listasPreco = await _uof.ListaPrecoRepository.Get().
                                                Include(x => x.UsuarioVendedorModel).
                                                Include(x => x.Observacao).
                                                AsNoTracking().
                                                ToListAsync();
                        break;
                    }
                case LevelUser.Vendedor:
                    {
                        listasPreco = await _uof.ListaPrecoRepository.Get().
                                                Where(x => x.IsAtivo && x.UsuarioVendedorModel.UserName == User.Identity.Name).
                                                Include(x => x.UsuarioVendedorModel).
                                                Include(x => x.Observacao).
                                                ToListAsync();
                        break;
                    }
                case LevelUser.Cliente:
                    {
                        var usuarioVendedor = await _usuarioService.GetUsuarioVendedor();
                        if (usuarioVendedor == null) break;

                        listasPreco = await _uof.ListaPrecoRepository.Get().
                                                Where(x => x.IsAtivo && x.UsuarioVendedorModel.UserName == usuarioVendedor.UserName).
                                                Include(x => x.UsuarioVendedorModel).
                                                Include(x => x.Observacao).
                                                ToListAsync();
                        break;
                    }
            }

            var listasPrecoDTO = _mapper.Map<List<ListaPrecoDTO>>(listasPreco);
            return listasPrecoDTO;

        }

        // GET: api/listaspreco/5
        /// <summary>
        /// Obtém a lista de preço pelo seu ID
        /// </summary>
        /// <param name="id">Código da lista de preço</param>
        /// <returns>Objeto lista de preço </returns>        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ListaPrecoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ListaPrecoDTO>> GetById(int id)
        {
            var listaPrecoModel = await _uof.ListaPrecoRepository.GetById(p => p.ListaPrecoId == id);

            if (listaPrecoModel == null)
            {
                return NotFound();
            }

            var listaPrecoDTO = _mapper.Map<ListaPrecoDTO>(listaPrecoModel);
            return listaPrecoDTO;
        }

        // POST: api/listaspreco
        /// <summary>
        /// Inclui a lista de Preço
        /// </summary>
        /// <param name="listaPrecoDto">Objeto lista de preço</param>
        /// <returns>O objeto lista preço incluído</returns>        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ActionName("GravaListaPreco")]
        public async Task<ActionResult<ListaPrecoDTO>> Post([FromBody] ListaPrecoDTO listaPrecoDto)
        {

            var listaPrecoModel = _mapper.Map<ListaPrecoModel>(listaPrecoDto);
            listaPrecoModel.DataInclusao = _dataService.getDataHoraBrasilia();

            _uof.ListaPrecoRepository.Add(listaPrecoModel);

            await _uof.Commit();

            var listaPrecoDTO = _mapper.Map<ListaPrecoDTO>(listaPrecoModel);

            return CreatedAtAction("GravaListaPreco", new { id = listaPrecoDTO.ListaPrecoId }, listaPrecoDTO);
        }

        // PUT: api/listaspreco/5
        /// <summary>
        /// Atualiza a lista de preço pelo ID
        /// </summary>
        /// <param name="id">Código da lista de preço</param>
        /// <param name="listaPrecoDto">Objeto lista de preço</param>
        /// <returns>O objeto lista de preço alterado</returns>        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ListaPrecoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ListaPrecoDTO listaPrecoDto)
        {
            if (id != listaPrecoDto.ListaPrecoId)
            {
                return BadRequest();
            }

            var ListaPrecoModel = _mapper.Map<ListaPrecoModel>(listaPrecoDto);

            _uof.ListaPrecoRepository.Update(ListaPrecoModel);

            try
            {
                await _uof.Commit();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListaPrecoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // DELETE: api/listaspreco/5        
        /// <summary>
        /// Deleta uma lista de preço pelo seu ID
        /// </summary>
        /// <param name="id">Código da lista de preço</param>
        /// <returns>O objeto lista de preço excluído</returns>        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ListaPrecoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListaPrecoDTO>> Delete(int id)
        {
            var listaPrecoModel = await _uof.ListaPrecoRepository.GetById(p => p.ListaPrecoId == id);

            if (listaPrecoModel == null)
            {
                return NotFound();
            }

            _uof.ListaPrecoRepository.Delete(listaPrecoModel);
            await _uof.Commit();

            var listaPrecoDto = _mapper.Map<ListaPrecoDTO>(listaPrecoModel);
            return listaPrecoDto;
        }

        // POST: api/listaspreco/adiciona-produto
        /// <summary>
        /// Inclui o produto na lista de Preço
        /// </summary>
        /// <param name="listaPrecoProdutoDto">Objeto produto da lista de preço</param>
        /// <returns>O objeto de produto da lista preço incluído</returns>        
        [HttpPost("adiciona-produto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ActionName("GravaListaPrecoProduto")]
        public async Task<ActionResult<ListaPrecoProdutoDTO>> PostProduto([FromBody] ListaPrecoProdutoDTO listaPrecoProdutoDto)
        {

            var listaPrecoProdutoModel = _mapper.Map<ListaPrecoProdutoModel>(listaPrecoProdutoDto);
            _uof.ListaPrecoProdutoRepository.Add(listaPrecoProdutoModel);

            await _uof.Commit();

            var listaPrecoProdutoDTO = _mapper.Map<ListaPrecoProdutoDTO>(listaPrecoProdutoModel);

            return CreatedAtAction("GravaListaPrecoProduto", new { id = listaPrecoProdutoDTO.ListaPrecoProdutoId, idProduto = listaPrecoProdutoDTO.ProdutoId }, listaPrecoProdutoDTO);
        }

        // PUT: api/listaspreco/altera-produto/5/1
        /// <summary>
        /// Atualiza o produto na lista de preço pelo ID
        /// </summary>
        /// <param name="id">Código da lista de preço</param>
        /// <param name="idProduto">Código do produto</param>
        /// <param name="listaPrecoProdutoDto">Objeto produto da lista de preço</param>
        /// <returns>O objeto de produto da lista preço alterado</returns>        
        [HttpPut("altera-produto/{id}/{idProduto}")]
        [ProducesResponseType(typeof(ListaPrecoProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProduto([FromRoute] int id, [FromRoute] int idProduto, [FromBody] ListaPrecoProdutoDTO listaPrecoProdutoDto)
        {
            if (id != listaPrecoProdutoDto.ListaPrecoProdutoId || idProduto != listaPrecoProdutoDto.ProdutoId)
            {
                return BadRequest();
            }

            var ListaPrecoProdutoModel = _mapper.Map<ListaPrecoProdutoModel>(listaPrecoProdutoDto);

            _uof.ListaPrecoProdutoRepository.Update(ListaPrecoProdutoModel);

            try
            {
                await _uof.Commit();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListaPrecoProdutoModelExists(id, idProduto))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // DELETE: api/listaspreco/deleta-produto/5/1
        /// <summary>
        /// Deleta um produto da lista de preço pelo seu ID
        /// </summary>
        /// <param name="id">Código da lista de preço</param>
        /// <param name="idProduto">Código do produto</param>
        /// <returns>O objeto lista de preço excluído</returns>        
        [HttpPut("deleta-produto/{id}/{idProduto}")]
        [ProducesResponseType(typeof(ListaPrecoProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListaPrecoProdutoDTO>> DeleteProduto(int id, int idProduto)
        {
            var listaPrecoProdutoModel = await _uof.ListaPrecoProdutoRepository.GetById(p => p.ListaPrecoProdutoId == id && p.ProdutoId == idProduto);

            if (listaPrecoProdutoModel == null)
            {
                return NotFound();
            }

            _uof.ListaPrecoProdutoRepository.Delete(listaPrecoProdutoModel);
            await _uof.Commit();

            var listaPrecoProdutoDto = _mapper.Map<ListaPrecoProdutoDTO>(listaPrecoProdutoModel);
            return listaPrecoProdutoDto;
        }

        private bool ListaPrecoModelExists(int id)
        {
            var listaPreco = _uof.ListaPrecoRepository.GetById(e => e.ListaPrecoId == id);
            return listaPreco != null ? true : false;
        }

        private bool ListaPrecoProdutoModelExists(int id, int idProduto)
        {
            var listaPrecoProduto = _uof.ListaPrecoProdutoRepository.GetById(e => e.ListaPrecoProdutoId == id && e.ProdutoId == idProduto);
            return listaPrecoProduto != null ? true : false;
        }

    }
}
