using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class KundeView
	{
		[Required(ErrorMessage = "Fornavn må oppgis")]
		[RegularExpression(@"[a-zA-ZæøåÆØÅ.\-\ ]{1,30}", ErrorMessage = "Kan kun inneholde bokstaver")]
		public string Fornavn { get; set; }

		[Required(ErrorMessage = "Etternavn må oppgis")]
		[RegularExpression(@"[a-zA-ZæøåÆØÅ\-\ ]{1,30}", ErrorMessage = "Kan kun innholde bokstaver")]
		public string Etternavn { get; set; }

		[Required(ErrorMessage = "Epost må oppgis")]
		[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$", ErrorMessage = "Skriv inn gyldig epost-adresse")]
		public string Epost { get; set; }

		[Required(ErrorMessage = "Adresse må oppgis")]
		[RegularExpression(@"[0-9a-zA-ZæøåÆØÅ\-\ ]{1,30}", ErrorMessage = "Oppgi full adresse")]
		public string Adresse { get; set; }

		[Required(ErrorMessage = "PostNr må oppgis")]
		[RegularExpression(@"[0-9]{4}", ErrorMessage = "Postnr må være 4 siffer")]
		public string PostNr { get; set; }

		[Required(ErrorMessage = "Poststed må oppgis")]
		[RegularExpression(@"[a-zA-ZæøåÆØÅ.\-\ ]{2,30}", ErrorMessage = "Kan kun inneholde bokstaver")]
		public string PostSted { get; set; }

		[Required(ErrorMessage = "Telefon må oppgis")]
		[RegularExpression(@"[0-9]{6,12}", ErrorMessage = "Telefonnummer må være 6-12 siffer")]
		public string Telefon { get; set; }

        public int kundeId { get; set; }
	}
}
