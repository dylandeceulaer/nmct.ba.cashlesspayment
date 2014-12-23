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
    public class EmployeeController : ApiController
    {
        public List<Employee> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEployees(p.Claims);
        }
        public Employee Get(string code)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEployeeByCode(p.Claims,code);
        }
        public int Put(Employee empl)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.UpdateEmployee(empl, p.Claims);
        }
        public int Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.DeleteEmployee(id, p.Claims);
        }
        public int Insert(Employee empl)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.InsertEmployee(empl, p.Claims);
        }
    }

}
