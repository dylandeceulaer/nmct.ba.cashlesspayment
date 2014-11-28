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
    public class CustomerController : ApiController
    {
        public List<Customer> Get()
        {
            return CustomersDA.GetCustomers();
        }
        public int Put(Customer customer)
        {
            return CustomersDA.UpdateCustomer(customer);
        }
        public int Delete(int id)
        {
            return CustomersDA.DeleteCustomer(id);
        }
        public int Insert(Customer customer)
        {
            return CustomersDA.InsertCustomer(customer);
        }
    }
}
