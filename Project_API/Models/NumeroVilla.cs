﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_API.Models
{
	public class NumeroVilla
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int VillaNo { get; set; }
		[Required]
        public int VillaId { get; set; }

		[ForeignKey("VillaId")]
        public Villa Villa { get; set; }

		public string DetallesEspecial { get; set; }	
		public DateTime FechaCreacion { get; set; }
		public DateTime FechaActualizacion { get; set; }
    }
}
