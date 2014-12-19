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
    public class SalesDA
    {
        private const string CONNSTR = "ConnectionCashlessIT";

        public static List<Sales> GetSales(IEnumerable<Claim> claims)
        {
            List<Sales> res = new List<Sales>();
            string sql = "SELECT ID, CustomerID, RegisterID, ProductID,Amount,TotalPrice,[TimeStamp] FROM Sales";
            DbDataReader data = Database.GetData(Database.GetConnection(ConnectionString.Create(claims)), sql);
            while (data.Read())
            {
                res.Add(new Sales()
                {
                    Id = int.Parse(data["ID"].ToString()),
                    Amound = int.Parse(data["Amount"].ToString()),
                    CustomerID = int.Parse(data["CustomerID"].ToString()),
                    ProductID = int.Parse(data["ProductID"].ToString()),
                    RegisterID = int.Parse(data["RegisterID"].ToString()),
                    TimeStamp = ConvertUnixTimeStamp(data["TimeStamp"]),
                    TotalPrice = float.Parse(data["TotalPrice"].ToString())
                    
                });
            }
            return res;

        }
        private static DateTime ConvertUnixTimeStamp(object timestamp)
        {
            long timestamplong = Convert.ToInt64(timestamp);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamplong).ToLocalTime();
            return dtDateTime;
        }
        public static int UpdateSales(Sales Sale, IEnumerable<Claim> claims)
        {

            string sql = "UPDATE Sales SET CustomerID = @customer, RegisterID=@register, ProductID=@product,Amount=@amound,TotalPrice=@price WHERE ID=@id";
            DbParameter par6 = Database.AddParameter(CONNSTR, "id", Sale.Id);
            DbParameter par1 = Database.AddParameter(CONNSTR, "customer", Sale.CustomerID);
            DbParameter par2 = Database.AddParameter(CONNSTR, "register", Sale.RegisterID);
            DbParameter par3 = Database.AddParameter(CONNSTR, "product", Sale.ProductID);
            DbParameter par4 = Database.AddParameter(CONNSTR, "amound", Sale.Amound);
            DbParameter par5 = Database.AddParameter(CONNSTR, "price", Sale.TotalPrice);

            return Database.ModifyData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2, par3, par4, par5, par6);

        }
        public static int InsertSales(Sales Sale, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Sales(CustomerID,RegisterID,ProductID,Amount,TotalPrice) VALUES(CustomerID = @customer,@register,@product,@amound,@price)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "customer", Sale.CustomerID);
            DbParameter par2 = Database.AddParameter(CONNSTR, "register", Sale.RegisterID);
            DbParameter par3 = Database.AddParameter(CONNSTR, "product", Sale.ProductID);
            DbParameter par4 = Database.AddParameter(CONNSTR, "amound", Sale.Amound);
            DbParameter par5 = Database.AddParameter(CONNSTR, "price", Sale.TotalPrice);


            return Database.InsertData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2, par3, par4, par5);
        }
        public static int DeleteSales(int id, IEnumerable<Claim> claims)
        {
            string sql = "DELETE FROM Sales WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(Database.GetConnection(ConnectionString.Create(claims)), sql, par);
        }
    }
}