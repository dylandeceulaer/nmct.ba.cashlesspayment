﻿using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class EmployeeController : ApiController
    {
        public List<Employee> Get()
        {
            return EmployeeDA.GetEployees();
        }
    }
}
