using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Purchase
    {
        public List<Bestelling> Bestellingen { get; set; }
        public Customer Customer { get; set; }
        public float TotaalPrijs { get; set; }
        public int KassaID { get; set; }
    }
}
