using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Datos
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
		public DbSet<Villa> Villas { get; set; }
		public DbSet<NumeroVilla> NumeroVillas { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Villa>().HasData(
				new Villa()
				{
					Id = 1,
					Name = "Villa Real",
					Detail="Detalle de la villa...",
					ImagenUrl="",
					Occupants=5,
					Dimension=50,
					Tarifa=200,
					Amenidad="",
					DateCreation = DateTime.Now,
					DateUpdate=DateTime.Now
				},
					new Villa()
					{
						Id = 2,
						Name = "Villa Caraz",
						Detail = "Detalle de la villa Caraz...",
						ImagenUrl = "",
						Occupants = 10,
						Dimension = 100,
						Tarifa = 400,
						Amenidad = "",
						DateCreation = DateTime.Now,
						DateUpdate = DateTime.Now
					}
			);
		}
	}
}

