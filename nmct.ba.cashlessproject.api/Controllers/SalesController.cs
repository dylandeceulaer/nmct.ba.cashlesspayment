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
    public class SalesController : ApiController
    {
        public List<Sales> Get()
        {
            return SalesDA.GetSales();
        }
        public int Put(Sales Sales)
        {
            return SalesDA.UpdateSales(Sales);
        }
        public int Delete(int id)
        {
            return SalesDA.DeleteSales(id);
        }
        public int Insert(Sales Sales)
        {
            return SalesDA.InsertSales(Sales);
        }
    }
}
