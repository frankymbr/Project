using Project_API.Models;
using Project_API.Repositorio.IRepositorio;

namespace Project_API.Repositorio
{
	public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
	{
		Task<NumeroVilla> Actualizar(NumeroVilla entidad);
	}
}
