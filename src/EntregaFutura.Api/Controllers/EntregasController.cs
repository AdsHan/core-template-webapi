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
    public class EntregasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly DataService _dataService;

        public EntregasController(
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

        // GET: api/entregas
        /// <summary>
        /// Obtém a lista de entregas
        /// </summary>
        /// <returns>Coleção de objetos entregas</returns>        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EntregaDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EntregaDTO>>> Get()
        {

            List<EntregaModel> entregas = new List<EntregaModel>();

            LevelUser level = await _usuarioService.GetCurrentUserLevel();

            switch (level)
            {
                case LevelUser.Admin:
                    {
                        entregas = await _uof.EntregaRepository.Get().
                                            Include(x => x.UsuarioVendedorModel).
                                            Include(x => x.Observacao).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
                case LevelUser.Vendedor:
                    {
                        entregas = await _uof.EntregaRepository.Get().
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

                        entregas = await _uof.EntregaRepository.Get().
                                            Where(x => x.UsuarioVendedorModel.UserName == usuarioVendedor.UserName).
                                            Include(x => x.UsuarioVendedorModel).
                                            Include(x => x.Observacao).
                                            AsNoTracking().
                                            ToListAsync();
                        break;
                    }
            }

            var entregasDTO = _mapper.Map<List<EntregaDTO>>(entregas);
            return entregasDTO;

        }

        // GET: api/entregas/5
        /// <summary>
        /// Obtém a entrega pelo seu ID
        /// </summary>
        /// <param name="id">Código da entrega</param>
        /// <returns>Objeto entrega </returns>        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EntregaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<EntregaDTO>> GetById(int id)
        {
            var entregaModel = await _uof.EntregaRepository.GetById(p => p.EntregaId == id);

            if (entregaModel == null)
            {
                return NotFound();
            }

            var entregasDTO = _mapper.Map<EntregaDTO>(entregaModel);
            return entregasDTO;
        }

        // POST: api/entregas
        /// <summary>
        /// Inclui a entrega
        /// </summary>
        /// <param name="entregaDto">Objeto entrega</param>
        /// <returns>O objeto entrega incluído</returns>        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ActionName("GravaEntrega")]
        public async Task<ActionResult<EntregaDTO>> Post([FromBody] EntregaDTO entregaDto)
        {

            var entregaModel = _mapper.Map<EntregaModel>(entregaDto);
            entregaModel.DataInclusao = _dataService.getDataHoraBrasilia();

            _uof.EntregaRepository.Add(entregaModel);

            await _uof.Commit();

            var entregaDTO = _mapper.Map<EntregaDTO>(entregaModel);

            return CreatedAtAction("GravaEntrega", new { id = entregaDTO.EntregaId }, entregaDTO);
        }

        // PUT: api/entregas/5
        /// <summary>
        /// Atualiza a entrega pelo ID
        /// </summary>
        /// <param name="id">Código da entrega</param>
        /// <param name="entregaDto">Objeto entrega</param>
        /// <returns>O objeto entrega alterado</returns>        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EntregaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] EntregaDTO entregaDto)
        {
            if (id != entregaDto.EntregaId)
            {
                return BadRequest();
            }

            var entregaModel = _mapper.Map<EntregaModel>(entregaDto);

            _uof.EntregaRepository.Update(entregaModel);

            try
            {
                await _uof.Commit();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntregaModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // DELETE: api/entregas/5        
        /// <summary>
        /// Deleta uma entrega pelo seu ID
        /// </summary>
        /// <param name="id">Código da entrega</param>
        /// <returns>O objeto entrega excluído</returns>        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(EntregaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EntregaDTO>> Delete(int id)
        {
            var entregaModel = await _uof.EntregaRepository.GetById(p => p.EntregaId == id);

            if (entregaModel == null)
            {
                return NotFound();
            }

            _uof.EntregaRepository.Delete(entregaModel);
            await _uof.Commit();

            var entregaDto = _mapper.Map<EntregaDTO>(entregaModel);
            return entregaDto;
        }

        private bool EntregaModelExists(int id)
        {
            var entrega = _uof.EntregaRepository.GetById(e => e.EntregaId == id);
            return entrega != null ? true : false;
        }
    }
}
