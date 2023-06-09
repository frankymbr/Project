﻿using AutoMapper;
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
	public class ProjectController : ControllerBase
	{
		private readonly ILogger<ProjectController> _logger;
		private readonly IVillaRepositorio _villaRepo;
		private readonly IMapper _mapper;
		protected APIResponse _apiResponse;
        public ProjectController(ILogger<ProjectController> logger, IVillaRepositorio villaRepo, IMapper mapper)
        {
			_logger = logger;
			_villaRepo = villaRepo;
			_mapper = mapper;
			_apiResponse = new();
		}

        [HttpGet]
		[Route("GetVillas")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetVillas()
		{
			try
			{
				_logger.LogInformation("Obtener todas las villas");

				IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();

				_apiResponse.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
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
		[Route("GetVillaDto/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> GetVillaDto(int id)
		{
			try
			{
				if (id == 0)
				{
					_logger.LogError("Error al traer Villa con  id " + id);
					_apiResponse.StatusCode = HttpStatusCode.BadRequest;
					_apiResponse.IsExistoso = false;
					return BadRequest(_apiResponse);
				}

				var villa = await _villaRepo.Obtener(v => v.Id == id);
				if (villa == null)
				{
					_apiResponse.StatusCode = HttpStatusCode.NotFound;
					_apiResponse.IsExistoso = false;
					return NotFound(_apiResponse);
				}
				_apiResponse.Resultado = _mapper.Map<VillaDto>(villa);
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
		public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
		{
			try
			{
				if (!ModelState.IsValid) return BadRequest(ModelState);
				if (await _villaRepo.Obtener(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
				{
					ModelState.AddModelError("NombreExiste", "This name already exists");
					return BadRequest(ModelState);
				}

				if (createDto == null) return BadRequest(createDto);

				Villa modelo = _mapper.Map<Villa>(createDto);

				modelo.DateCreation = DateTime.Now;
				modelo.DateUpdate = DateTime.Now;
				await _villaRepo.Crear(modelo);
				_apiResponse.Resultado = modelo;
				_apiResponse.StatusCode = HttpStatusCode.Created;

				return CreatedAtAction("CreateVilla", new { id = modelo.Id }, _apiResponse); //con CreatedAtRoute sale status 500 por un bug de versiones de Microsoft
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
		public async Task<IActionResult> DeleteVillas(int id)
		{
			try
			{
				if (id == 0)
				{
					_apiResponse.IsExistoso = false;
					_apiResponse.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_apiResponse);
				}
				
				var villa = await _villaRepo.Obtener(v => v.Id == id);
				if (villa == null)
				{
					_apiResponse.IsExistoso = false;
					_apiResponse.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_apiResponse);
				}

				await _villaRepo.Remover(villa);
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
		public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
		{
			try
			{
				if (updateDto == null || id != updateDto.Id)
				{
					_apiResponse.IsExistoso = false;
					_apiResponse.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_apiResponse);
				}

				Villa modelo = _mapper.Map<Villa>(updateDto);

				await _villaRepo.Actualizar(modelo);
				_apiResponse.StatusCode = HttpStatusCode.NoContent;
			}
			catch (Exception ex)
			{
				_apiResponse.IsExistoso = false;
				_apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
			}

			return Ok(_apiResponse);
		}

		[HttpPatch("{id:int}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public  async Task<IActionResult> UpdateVillaPartial(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
		{
			try
			{
				if (patchDto == null || id == 0) return BadRequest();
				var villa = await _villaRepo.Obtener(v => v.Id == id, tracked: false);

				VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

				if (villa == null) return BadRequest();

				patchDto.ApplyTo(villaDto, ModelState);
				if (!ModelState.IsValid) return BadRequest(ModelState);

				Villa modelo = _mapper.Map<Villa>(villaDto);

				await _villaRepo.Actualizar(modelo);
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
