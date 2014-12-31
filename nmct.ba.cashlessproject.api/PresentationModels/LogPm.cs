using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.PresentationModels
{
    public class LogPM
    {
        public string Register { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public bool IsNew { get; set; }
    }
}