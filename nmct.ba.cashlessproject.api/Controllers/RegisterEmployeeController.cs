using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class RegisterEmployeeController : ApiController
    {
        public List<RegisterEmployee> Get()
        {
            return RegisterEmployeeDA.GetRegisterEmployees();
        }
        public List<RegisterEmployee> Get(int id)
        {
            return RegisterEmployeeDA.GetRegisterEmployees(id);
        }
        public int Put(RegisterEmployee reg)
        {
            return RegisterEmployeeDA.DeleteRegisterEmployee(reg);
        }
        public void Post(RegisterEmployee reg)
        {
            RegisterEmployeeDA.InsertRegisterEmployee(reg);
        }
    }
}
