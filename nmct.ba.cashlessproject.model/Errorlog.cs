using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Errorlog
    {
        public int RegisterID { get; set; }
        public int Timestamp { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public int OrganisationID { get; set; }
    }
}
