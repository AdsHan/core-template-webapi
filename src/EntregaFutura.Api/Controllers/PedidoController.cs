using AutoMapper;
using DevIO.Business.Intefaces;
using EntregaFutura.Api.Services;
using EntregaFutura.Domain.DTO;
using EntregaFutura.Domain.Models;
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
    public class PedidosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly DataService _dataService;

        public PedidosController(
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

        // GET: api/pedidos
        /// <summary>
        /// Obtém os pedidos
        /// </summary>
        /// <returns>Coleção de objetos pedidos</returns>        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PedidoDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> Get()
        {

            List<PedidoModel> pedidos = new List<PedidoModel>();

            LevelUser level = await _usuarioService.GetCurrentUserLevel();

            switch (level)
            {
                case LevelUser.Admin:
                    {
                        pedidos = await _uof.PedidoRepository.Get().
                                            Include(x => x.ItensPedido).
                                            Include(x => x.Observacao).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
                case LevelUser.Vendedor:
                    {
                        pedidos = await _uof.PedidoRepository.Get().
                                            Where(x => x.UsuarioVendedorModel.UserName == User.Identity.Name).
                                            Include(x => x.ItensPedido).
                                            Include(x => x.Observacao).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
                case LevelUser.Cliente:
                    {
                        var usuarioVendedor = await _usuarioService.GetUsuarioVendedor();
                        if (usuarioVendedor == null) break;

                        pedidos = await _uof.PedidoRepository.Get().
                                            Where(x => x.UsuarioClienteModel.UserName == User.Identity.Name &&
                                                       x.UsuarioVendedorModel.UserName == usuarioVendedor.UserName).
                                            Include(x => x.ItensPedido).
                                            Include(x => x.Observacao).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
            }

            var pedidosDTO = _mapper.Map<List<PedidoDTO>>(pedidos);
            return pedidosDTO;

        }

        // GET: api/pedido/5
        /// <summary>
        /// Obtém o pedido pelo seu ID
        /// </summary>
        /// <param name="id">Código do pedido</param>
        /// <returns>Objeto pedido </returns>        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<PedidoDTO>> GetById(int id)
        {
            var pedidoModel = await _uof.PedidoRepository.GetById(p => p.PedidoId == id);

            if (pedidoModel == null)
            {
                return NotFound();
            }

            var pedidoDTO = _mapper.Map<PedidoDTO>(pedidoModel);
            return pedidoDTO;
        }

        // POST: api/pedido
        /// <summary>
        /// Inclui o pedido
        /// </summary>
        /// <param name="pedidoDto">Objeto pedido</param>
        /// <returns>O objeto pedido incluído</returns>        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ActionName("GravaPedido")]
        public async Task<ActionResult<PedidoDTO>> Post([FromBody] PedidoDTO pedidoDto)
        {

            var pedidoModel = _mapper.Map<PedidoModel>(pedidoDto);
            pedidoModel.DataInclusao = _dataService.getDataHoraBrasilia();

            _uof.PedidoRepository.Add(pedidoModel);

            await _uof.Commit();

            var pedidoDTO = _mapper.Map<PedidoDTO>(pedidoModel);

            return CreatedAtAction("GravaPedido", new { id = pedidoDTO.PedidoId }, pedidoDTO);
        }

        // PUT: api/pedido/5
        /// <summary>
        /// Atualiza o pedido pelo ID
        /// </summary>
        /// <param name="id">Código do pedido</param>
        /// <param name="pedidoDto">Objeto pedido</param>
        /// <returns>O objeto pedido alterado</returns>        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PedidoDTO pedidoDto)
        {
            if (id != pedidoDto.PedidoId)
            {
                return BadRequest();
            }

            var pedidoModel = _mapper.Map<PedidoModel>(pedidoDto);

            _uof.PedidoRepository.Update(pedidoModel);

            try
            {
                await _uof.Commit();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // DELETE: api/pedido/5        
        /// <summary>
        /// Deleta um pedido pelo seu ID
        /// </summary>
        /// <param name="id">Código do pedido</param>
        /// <returns>O objeto pedido excluído</returns>        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoDTO>> Delete(int id)
        {
            var pedidoModel = await _uof.PedidoRepository.GetById(p => p.PedidoId == id);

            if (pedidoModel == null)
            {
                return NotFound();
            }

            _uof.PedidoRepository.Delete(pedidoModel);
            await _uof.Commit();

            var pedidoDto = _mapper.Map<PedidoDTO>(pedidoModel);
            return pedidoDto;
        }

        // POST: api/pedidos/adiciona-item
        /// <summary>
        /// Inclui o item no pedido
        /// </summary>
        /// <param name="pedidoItemDto">Objeto item do pedido</param>
        /// <returns>O objeto item do pedido incluído</returns>        
        [HttpPost("adiciona-item")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ActionName("GravaPedidoItem")]
        public async Task<ActionResult<PedidoItemDTO>> PostItem([FromBody] PedidoItemDTO pedidoItemDto)
        {

            var pedidoItemModel = _mapper.Map<PedidoItemModel>(pedidoItemDto);
            _uof.PedidoItemRepository.Add(pedidoItemModel);

            await _uof.Commit();

            var pedidoItemDTO = _mapper.Map<PedidoItemDTO>(pedidoItemModel);

            return CreatedAtAction("GravaItemPedido", new { id = pedidoItemDTO.PedidoId, idItem = pedidoItemDTO.ItemId }, pedidoItemDTO);
        }

        // PUT: api/pedidos/altera-item/5/1
        /// <summary>
        /// Atualiza o item no pedido pelo ID
        /// </summary>
        /// <param name="id">Código do pedido</param>
        /// <param name="idItem">Item do pedido</param>
        /// <param name="pedidoItemDto">Objeto item do pedido</param>
        /// <returns>O objeto de item do pedido alterado</returns>        
        [HttpPut("altera-item/{id}/{idProduto}")]
        [ProducesResponseType(typeof(PedidoItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutItem([FromRoute] int id, [FromRoute] int idItem, [FromBody] PedidoItemDTO pedidoItemDto)
        {
            if (id != pedidoItemDto.PedidoId || idItem != pedidoItemDto.ItemId)
            {
                return BadRequest();
            }

            var PedidoItemModel = _mapper.Map<PedidoItemModel>(pedidoItemDto);

            _uof.PedidoItemRepository.Update(PedidoItemModel);

            try
            {
                await _uof.Commit();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoItemModelExists(id, idItem))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // DELETE: api/pedido/deleta-item/5/1
        /// <summary>
        /// Deleta um item do pedido pelo seu ID
        /// </summary>
        /// <param name="id">Código do pedido</param>
        /// <param name="idItem">Item do pedido</param>
        /// <returns>O objeto item do pedido excluído</returns>        
        [HttpPut("deleta-item/{id}/{idProduto}")]
        [ProducesResponseType(typeof(PedidoItemDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoItemDTO>> DeleteProduto(int id, int idItem)
        {
            var pedidoItemModel = await _uof.PedidoItemRepository.GetById(p => p.PedidoId == id && p.ItemId == idItem);

            if (pedidoItemModel == null)
            {
                return NotFound();
            }

            _uof.PedidoItemRepository.Delete(pedidoItemModel);
            await _uof.Commit();

            var pedidoItemDto = _mapper.Map<PedidoItemDTO>(pedidoItemModel);
            return pedidoItemDto;
        }

        private bool PedidoModelExists(int id)
        {
            var pedido = _uof.PedidoRepository.GetById(e => e.PedidoId == id);
            return pedido != null ? true : false;
        }

        private bool PedidoItemModelExists(int id, int idItem)
        {
            var pedidoItem = _uof.PedidoItemRepository.GetById(e => e.PedidoId == id && e.ItemId == idItem);
            return pedidoItem != null ? true : false;
        }

    }
}
