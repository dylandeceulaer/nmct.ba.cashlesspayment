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
    public class ProductController : ApiController
    {
        public List<Product> Get()
        {
            return ProductsDA.GetProducts();
        }
        public int Put(Product Product)
        {
            return ProductsDA.UpdateProduct(Product);
        }            
        public int Delete(int id)
        {
            return ProductsDA.DeleteProduct(id);
        }
        public int Insert(Product Product)
        {
            return ProductsDA.InsertProduct(Product);
        }
    }
}
