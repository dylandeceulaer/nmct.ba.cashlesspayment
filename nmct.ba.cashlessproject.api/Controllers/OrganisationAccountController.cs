using nmct.ba.cashlessproject.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class OrganisationAccountController : ApiController
    {
        public int Put(List<string> wachtwoorden)
        {
            return AccountDA.UpdatePassword(wachtwoorden);
        }
    }
}
