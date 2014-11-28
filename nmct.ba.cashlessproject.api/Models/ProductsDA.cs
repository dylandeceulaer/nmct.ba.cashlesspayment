using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class ProductsDA
    {
        private const string CONNSTR = "ConnectionCashless";

        public static List<Product> GetProducts()
        {
            List<Product> res = new List<Product>();
            string sql = "SELECT ID, ProductName, Price,CategoryID FROM Products";
            DbDataReader data = Database.GetData(CONNSTR, sql);
            while (data.Read())
            {
                res.Add(new Product()
                {
                    ProductName = data["ProductName"].ToString(),
                    Price = float.Parse(data["Price"].ToString()),
                    Category = int.Parse(data["CategoryID"].ToString()),
                    Id = int.Parse(data["ID"].ToString())
                });
            }
            return res;
        }
        public static int UpdateProduct(Product prod)
        {

            string sql = "UPDATE Products SET ProductName=@naam, CategoryID=@cat, Price=@price WHERE ID=@id";
            DbParameter par4 = Database.AddParameter(CONNSTR, "id", prod.Id);
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", prod.ProductName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "cat", prod.Category);
            DbParameter par3 = Database.AddParameter(CONNSTR, "price", prod.Price);

            return Database.ModifyData(CONNSTR, sql, par1, par2, par3, par4);

        }
        public static int InsertProduct(Product prod)
        {
            string sql = "INSERT INTO Products(ProductName, CategoryID, Price) VALUES(@naam,@cat,@price)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", prod.ProductName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "cat", prod.Category);
            DbParameter par3 = Database.AddParameter(CONNSTR, "price", prod.Price);

            return Database.InsertData(CONNSTR, sql, par1, par2, par3);
        }
        public static int DeleteProduct(int id)
        {
            string sql = "DELETE FROM Products WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(CONNSTR, sql, par);
        }
    }
}