using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Datos;
using Project_API.Models;
using Project_API.Models.Dto;
using Project_API.Repositorio;
using System.Net;
using System.Xml.Linq;

namespace Project_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NumeroVillaController : ControllerBase
	{
		private readonly ILogger<NumeroVillaController> _logger;
		private readonly IVillaRepositorio _villaRepo;
		private readonly INumeroVillaRepositorio _numeroRepo;
		private readonly IMapper _mapper;
		protected APIResponse _apiResponse;
        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, INumeroVillaRepositorio numeroRepo, IMapper mapper)
        {
			_logger = logger;
			_villaRepo = villaRepo;
			_numeroRepo = numeroRepo;
			_mapper = mapper;
			_apiResponse = new();
		}

        [HttpGet]
		[Route("GetVillas")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetNumeroVillas()
		{
			try
			{
				_logger.LogInformation("Obtener todas las villas");

				IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos();

				_apiResponse.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
				_apiResponse.StatusCode = HttpStatusCode.OK;

				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.IsExistoso = false;
				_apiResponse.ErrorMessages = new List<string>() { ex.Message };
			}
			return _apiResponse;
		}

		[HttpGet]
		[Route("GetNumeroVilla/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
		{
			try
			{
				if (id == 0)
				{
					_logger.LogError("Error al traer número de Villa con  id " + id);
					_apiResponse.StatusCode = HttpStatusCode.BadRequest;
					_apiResponse.IsExistoso = false;
					return BadRequest(_apiResponse);
				}

				var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
				if (numeroVilla == null)
				{
					_apiResponse.StatusCode = HttpStatusCode.NotFound;
					_apiResponse.IsExistoso = false;
					return NotFound(_apiResponse);
				}
				_apiResponse.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
				_apiResponse.StatusCode = HttpStatusCode.OK;
				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.IsExistoso = false;
				_apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
			}

			return _apiResponse;
		}

		[HttpPost]
		[Route("CreateVilla")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> CreateNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
		{
			try
			{
				if (!ModelState.IsValid) return BadRequest(ModelState);
				if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
				{
					ModelState.AddModelError("NombreExiste", "This number already exists");
					return BadRequest(ModelState);
				}
				if (await _villaRepo.Obtener(v=>v.Id ==createDto.VillaId)==null)
				{
					ModelState.AddModelError("ClaveForanea", "The id does not exist");
					return BadRequest(ModelState);
				}

				if (createDto == null) return BadRequest(createDto);

				NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);

				modelo.FechaCreacion = DateTime.Now;
				modelo.FechaActualizacion = DateTime.Now;
				await _numeroRepo.Crear(modelo);
				_apiResponse.Resultado = modelo;
				_apiResponse.StatusCode = HttpStatusCode.Created;

				return CreatedAtAction("CreateVilla", new { id = modelo.VillaNo }, _apiResponse); //con CreatedAtRoute sale status 500 por un bug de versiones de Microsoft
			}
			catch (Exception ex)
			{
				_apiResponse.IsExistoso = false;
				_apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _apiResponse;
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteNumeroVillas(int id)
		{
			try
			{
				if (id == 0)
				{
					_apiResponse.IsExistoso = false;
					_apiResponse.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_apiResponse);
				}
				
				var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
				if (numeroVilla == null)
				{
					_apiResponse.IsExistoso = false;
					_apiResponse.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_apiResponse);
				}

				await _numeroRepo.Remover(numeroVilla);
				_apiResponse.StatusCode=HttpStatusCode.NotFound;
				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.IsExistoso = false;
				_apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return BadRequest(_apiResponse);
		}

		[HttpPut]
		[Route("UpdateVilla/{id:int}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
		{
			try
			{
				if (updateDto == null || id != updateDto.VillaNo)
				{
					_apiResponse.IsExistoso = false;
					_apiResponse.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_apiResponse);
				}

				if (await _villaRepo.Obtener(v=>v.Id == updateDto.VillaId) == null)
				{
					ModelState.AddModelError("Clave foranea", "The id does not exists");
					return BadRequest(ModelState);
				}

				NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

				await _numeroRepo.Actualizar(modelo);
				_apiResponse.StatusCode = HttpStatusCode.NoContent;
			}
			catch (Exception ex)
			{
				_apiResponse.IsExistoso = false;
				_apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
			}

			return Ok(_apiResponse);
		}
	
	}
}
