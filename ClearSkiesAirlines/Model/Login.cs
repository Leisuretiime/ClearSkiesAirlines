using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Login
    {
        [Display(Name = "Brukernavn")]
        [RegularExpression(@"[a-zA-Z0-9_-]{3,20}", ErrorMessage = " ")]
        [Required(ErrorMessage = "Brukernavn må oppgis")]
        public string Brukernavn { get; set; }

        [Display(Name = "Passord")]
        [Required(ErrorMessage = "Passord må oppgis")]
        [DataType(DataType.Password)]
        public string Passord { get; set; }
    }
}
