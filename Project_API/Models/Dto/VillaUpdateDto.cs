using System.ComponentModel.DataAnnotations;

namespace Project_API.Models.Dto
{
	public class VillaUpdateDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		[MaxLength(30)]
		public string Name { get; set; }
		public string Detail { get; set; }
		[Required]
		public double Tarifa { get; set; }
		[Required]
		public int Occupants { get; set; }
		[Required]
		public int Dimension { get; set; }
		[Required]
		public string ImagenUrl { get; set; }
		public string Amenidad { get; set; }
	}
}
