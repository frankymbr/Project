using Project_API.Models;
using Project_API.Repositorio.IRepositorio;

namespace Project_API.Repositorio
{
	public interface IVillaRepositorio : IRepositorio<Villa>
	{
		Task<Villa> Actualizar(Villa entidad);
	}
}
