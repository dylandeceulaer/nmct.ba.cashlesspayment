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
    public class RegisterEmployeeController : ApiController
    {
        public List<RegisterEmployee> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterEmployeeDA.GetRegisterEmployees(p.Claims);
        }
        public List<RegisterEmployee> Get(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterEmployeeDA.GetRegisterEmployees(id, p.Claims);
        }
        public int Put(RegisterEmployee reg)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterEmployeeDA.DeleteRegisterEmployee(reg, p.Claims);
        }
        public void Post(RegisterEmployee reg)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            RegisterEmployeeDA.InsertRegisterEmployee(reg, p.Claims);
        }
    }
}
