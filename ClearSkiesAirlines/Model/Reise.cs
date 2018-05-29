using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Reise
	{
		[Required(ErrorMessage = "Velg startpunkt")]
		public string Fra { get; set; }

		[Required(ErrorMessage = "Velg destinasjon")]
		public string Destinasjon { get; set; }

		[Required(ErrorMessage = "Velg kapasitet")]
		[RegularExpression(@"[0-9]{1,4}", ErrorMessage = "Kan kun inneholde tall")]
		public int Kapasitet { get; set; }

		[Required(ErrorMessage = "Velg pris")]
		[RegularExpression(@"[0-9]{1,5}", ErrorMessage = "Kan kun inneholde tall")]
		public int Pris { get; set; }

		[Required(ErrorMessage = "Velg dato")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime AvreiseDato { get; set; }

		[Required(ErrorMessage = "Velg dato")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime ReturDato { get; set; }

		public int ReiseId { get; set; }
	}
}
