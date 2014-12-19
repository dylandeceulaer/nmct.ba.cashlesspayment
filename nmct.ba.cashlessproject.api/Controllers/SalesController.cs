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
    public class SalesController : ApiController
    {
        public List<Sales> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.GetSales(p.Claims);
        }
        public int Put(Sales Sales)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.UpdateSales(Sales, p.Claims);
        }
        public int Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.DeleteSales(id, p.Claims);
        }
        public int Insert(Sales Sales)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return SalesDA.InsertSales(Sales, p.Claims);
        }
    }
}
