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
    public class CustomerController : ApiController
    {
        public List<Customer> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomersDA.GetCustomers(p.Claims);
        }
        public Customer Get(string code)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomersDA.GetCustomerByCode(p.Claims, code);
        }
        
        public int Put(Customer customer)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomersDA.UpdateCustomer(customer, p.Claims);
        }
        public int Delete(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomersDA.DeleteCustomer(id, p.Claims);
        }
        public int Insert(Customer customer)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomersDA.InsertCustomer(customer, p.Claims);
        }
    }
}
