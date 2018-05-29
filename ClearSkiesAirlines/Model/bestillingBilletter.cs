using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BestillingBilletter
    {
        public int BillettId { get; set; }
        public string Fra { get; set; }
        public string Til { get; set; }
        public DateTime Avreise { get; set; }
        public string PassasjerFornavn { get; set; }
        public string PassasjerEtternavn { get; set; }
    }
}
