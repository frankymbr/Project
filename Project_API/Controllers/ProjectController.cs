using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Datos;
using Project_API.Models;
using Project_API.Models.Dto;
using System.Xml.Linq;

namespace Project_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		private readonly ILogger<ProjectController> _logger;
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;
        public ProjectController(ILogger<ProjectController> logger, ApplicationDbContext db, IMapper mapper)
        {
			_logger = logger;
			_db = db;
			_mapper = mapper;
		}

        [HttpGet]
		[Route("GetVillas")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
		{
			_logger.LogInformation("Obtener todas las villas");

			IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

			return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
		}

		[HttpGet]
		[Route("GetVillaDto/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<VillaDto>> GetVillaDto(int id)
		{
			if (id == 0)
			{
				_logger.LogError("Error al traer Villa con  id " + id);
				return BadRequest();
			}


			//var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
			var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
			if(villa == null) return NotFound();			
			return Ok(_mapper.Map<VillaDto>(villa));
		}

		[HttpPost]
		[Route("CreateVilla")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto createDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			if ( await _db.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
			{
				ModelState.AddModelError("NombreExiste", "This name already exists");
				return BadRequest(ModelState);
			}				

			if (createDto == null) return BadRequest(createDto);		

			Villa modelo = _mapper.Map<Villa>(createDto);			

			await _db.Villas.AddAsync(modelo);
			await _db.SaveChangesAsync();

			return CreatedAtAction("CreateVilla", new {id = modelo.Id }, modelo); //con CreatedAtRoute sale status 500 por un bug de versiones de Microsoft
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteVillas(int id)
		{
			if (id == 0) return BadRequest();
			var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
			if (villa == null) return NotFound();

			//VillaStore.VillaList.Remove(villa);
			_db.Villas.Remove(villa);
			await _db.SaveChangesAsync();
			return NoContent();
		}

		[HttpPut]
		[Route("UpdateVilla/{id:int}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
		{
			if(updateDto == null || id != updateDto.Id) return BadRequest();
	
			Villa modelo  = _mapper.Map<Villa>(updateDto);			

			_db.Villas.Update(modelo);
			await _db.SaveChangesAsync();

			return NoContent();
		}

		[HttpPatch("{id:int}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public  async Task<IActionResult> UpdateVillaPartial(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
		{
			if (patchDto == null || id ==0 ) return BadRequest();
			var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

			VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);			

			if(villa == null) return BadRequest();

			patchDto.ApplyTo(villaDto, ModelState);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			Villa modelo = _mapper.Map<Villa>(villaDto);

			_db.Villas.Update(modelo);
			await _db.SaveChangesAsync();
			return NoContent();
		}
	}
}
