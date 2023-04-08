using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_API.Models
{
	public class Villa
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } 
		public string Name { get; set; }	
		public string Detail { get; set; }
		[Required]
		public double Tarifa { get; set; }
		public int Occupants { get; set; }
		public int Dimension { get; set; }
		public string ImagenUrl { get; set; }
		public string Amenidad { get; set; }
		public DateTime DateCreation { get; set; }
		public DateTime DateUpdate { get; set; }
	}
}
