using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    [Authorize]
    public class ErrorlogController : ApiController
    {
        
        public int Insert(Errorlog e)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ErrorlogDA.InsertError(e, p.Claims);
        }
    }
}
