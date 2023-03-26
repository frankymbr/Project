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
        public ProjectController(ILogger<ProjectController> logger, ApplicationDbContext db)
        {
			_logger = logger;
			_db = db;
		}

        [HttpGet]
		[Route("GetVillas")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<VillaDto>> GetVillas()
		{
			_logger.LogInformation("Obtener todas las villas");
			return Ok(_db.Villas.ToList());
		}

		[HttpGet]
		[Route("GetVillaDto/{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<VillaDto> GetVillaDto(int id)
		{
			if (id == 0)
			{
				_logger.LogError("Error al traer Villa con  id " + id);
				return BadRequest();
			}


			//var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
			var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
			if(villa == null) return NotFound();			
			return Ok(villa);
		}

		[HttpPost]
		[Route("CreateVilla")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			if (_db.Villas.FirstOrDefault(v => v.Name.ToLower() == villaDto.Name.ToLower()) != null)
			{
				ModelState.AddModelError("NombreExiste", "This name already exists");
				return BadRequest(ModelState);
			}				

			if (villaDto == null) return BadRequest(villaDto);
			if (villaDto.Id > 0)  return StatusCode(StatusCodes.Status500InternalServerError);

			//villaDto.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
			//VillaStore.VillaList.Add(villaDto);

			Villa modelo = new()
			{				
				Name = villaDto.Name,
				Detail = villaDto.Detail,
				ImagenUrl = villaDto.ImagenUrl,
				Occupants = villaDto.Occupants,
				Tarifa = villaDto.Tarifa,
				Dimension = villaDto.Dimension,
				Amenidad = villaDto.Amenidad,
				DateCreation = DateTime.Now,
				DateUpdate = DateTime.Now
			};

			_db.Villas.Add(modelo);
			_db.SaveChanges();

			return CreatedAtRoute("GetVillaDto", new {id =villaDto.Id }, villaDto);
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeleteVillas(int id)
		{
			if (id == 0) return BadRequest();
			var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
			if (villa == null) return NotFound();

			//VillaStore.VillaList.Remove(villa);
			_db.Villas.Remove(villa);
			_db.SaveChanges();
			return NoContent();
		}

		[HttpPut]
		[Route("UpdateVilla/{id:int}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
		{
			if(villaDto == null || id != villaDto.Id) return BadRequest();
			//var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
			////if (villa == null) return NotFound();
			//villa.Name = villaDto.Name;
			//villa.Occupants = villaDto.Occupants;
			//villa.Dimension = villaDto.Dimension;

			Villa modelo = new()
			{
				Id = id,
				Name = villaDto.Name,
				Detail = villaDto.Detail,
				ImagenUrl = villaDto.ImagenUrl,
				Occupants = villaDto.Occupants,
				Tarifa = villaDto.Tarifa,
				Dimension = villaDto.Dimension,
				Amenidad = villaDto.Amenidad,
				DateCreation = DateTime.Now,
				DateUpdate = DateTime.Now
			};

			_db.Villas.Update(modelo);
			_db.SaveChanges();

			return NoContent();
		}

		[HttpPatch("{id:int}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult UpdateVillaPartial(int id, JsonPatchDocument<VillaDto> patchDto)
		{
			if (patchDto == null || id ==0 ) return BadRequest();
			//var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
			//if (villa == null) return NotFound();
			var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);
			VillaDto villaDto = new()
			{
				Id = villa.Id,
				Name = villa.Name,
				Detail = villa.Detail,
				ImagenUrl = villa.ImagenUrl,
				Occupants = villa.Occupants,
				Tarifa = (double)villa.Tarifa,
				Dimension = villa.Dimension,
				Amenidad = villa.Amenidad
			};

			if(villa == null) return BadRequest();

			patchDto.ApplyTo(villaDto, ModelState);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			Villa modelo = new()
			{
				Id = villaDto.Id,
				Name = villaDto.Name,
				Detail = villaDto.Detail,
				ImagenUrl = villaDto.ImagenUrl,
				Occupants = villaDto.Occupants,
				Tarifa = (double)villaDto.Tarifa,
				Dimension = villaDto.Dimension,
				Amenidad = villaDto.Amenidad,
				DateCreation = DateTime.Now,
				DateUpdate = DateTime.Now
			};

			_db.Villas.Update(modelo);
			_db.SaveChanges();
			return NoContent();
		}
	}
}
