using Project_API.Datos;
using Project_API.Models;

namespace Project_API.Repositorio
{
	public class VillaRespositorio : Repositorio<Villa>, IVillaRepositorio
	{
		private readonly ApplicationDbContext _db;

        public VillaRespositorio(ApplicationDbContext db) :base(db)
		{
			_db = db;
		}			
       
        public async Task<Villa> Actualizar(Villa entidad)
		{
			entidad.DateUpdate = DateTime.Now;
			_db.Villas.Update(entidad);
			await _db.SaveChangesAsync();
			return entidad;
		}
	}
}
