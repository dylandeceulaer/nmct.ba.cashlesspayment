using nmct.ba.cashlessproject.api.helper;
using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class ProductsDA
    {
        private const string CONNSTR = "ConnectionCashlessIT";

        public static List<Product> GetProducts(IEnumerable<Claim> claims)
        {
            List<Product> res = new List<Product>();
            string sql = "SELECT ID, ProductName, Price,CategoryID, Image FROM Products WHERE Active=1";
            DbDataReader data = Database.GetData(Database.GetConnection(ConnectionString.Create(claims)), sql);
            while (data.Read())
            {
                res.Add(new Product()
                {
                    ProductName = data["ProductName"].ToString(),
                    Price = float.Parse(data["Price"].ToString()),
                    Category = int.Parse(data["CategoryID"].ToString()),
                    Id = int.Parse(data["ID"].ToString()),
                    Image = GetPicture(data["Image"])
                });
            }
            return res;
        }
        private static byte[] GetPicture(object pic)
        {
            if (!DBNull.Value.Equals(pic))
                return (byte[])pic;
            else return new byte[0];
        }
        public static int UpdateProduct(Product prod, IEnumerable<Claim> claims)
        {

            string sql = "UPDATE Products SET ProductName=@naam, CategoryID=@cat, Price=@price, Image=@pic WHERE ID=@id";
            DbParameter par4 = Database.AddParameter(CONNSTR, "id", prod.Id);
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", prod.ProductName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "cat", prod.Category);
            DbParameter par3 = Database.AddParameter(CONNSTR, "price", prod.Price);
            DbParameter par5;
            if (prod.Image == null) par5 = Database.AddParameter(CONNSTR, "pic", new byte[0]);
            else par5 = Database.AddParameter(CONNSTR, "pic", prod.Image);

            return Database.ModifyData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2, par3, par4, par5);

        }
        public static int InsertProduct(Product prod, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Products(ProductName, CategoryID, Price, Image) VALUES(@naam,@cat,@price,@pic)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", prod.ProductName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "cat", prod.Category);
            DbParameter par3 = Database.AddParameter(CONNSTR, "price", prod.Price);
            DbParameter par4;
            if (prod.Image == null) par4 = Database.AddParameter(CONNSTR, "pic", new byte[0]);
            else par4 = Database.AddParameter(CONNSTR, "pic", prod.Image);
            


            return Database.InsertData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2, par3, par4);
        }
        public static int DeleteProduct(int id, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Products SET Active=0 WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(Database.GetConnection(ConnectionString.Create(claims)), sql, par);
        }
    }
}