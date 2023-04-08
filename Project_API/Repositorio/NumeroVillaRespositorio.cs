using Project_API.Datos;
using Project_API.Models;

namespace Project_API.Repositorio
{
	public class NumeroVillaRespositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
	{
		private readonly ApplicationDbContext _db;

        public NumeroVillaRespositorio(ApplicationDbContext db) :base(db)
		{
			_db = db;
		}			
       
        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
		{
			entidad.FechaActualizacion = DateTime.Now;
			_db.NumeroVillas.Update(entidad);
			await _db.SaveChangesAsync();
			return entidad;
		}
	}
}
