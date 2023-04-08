using System.ComponentModel.DataAnnotations;

namespace Project_API.Models.Dto
{
	public class NumeroVillaCreateDto
	{
		[Required]
		public int VillaNo { get; set; }
		[Required]
		public int VillaId { get; set; }
		public string DetallesEspecial { get; set; }
	}
}
