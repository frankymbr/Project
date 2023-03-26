using Project_API.Models.Dto;

namespace Project_API.Datos
{
	public static class VillaStore
	{
		public static List<VillaDto> VillaList = new List<VillaDto>
		{
			new VillaDto { Id = 1, Name="Vista a la piscina", Occupants= 10, Dimension = 100 },
			new VillaDto { Id = 2, Name="Vista a la playa", Occupants= 20, Dimension = 200}
		};
	}
}
