using System.ComponentModel.DataAnnotations;

namespace Project_API.Models.Dto
{
	public class VillaCreateDto
	{
		[Required]
		[MaxLength(30)]
		public string Name { get; set; }
		public string Detail { get; set; }
		[Required]
		public double Tarifa { get; set; }
		public int Occupants { get; set; }
		public int Dimension { get; set; }
		public string ImagenUrl { get; set; }
		public string Amenidad { get; set; }
	}
}
