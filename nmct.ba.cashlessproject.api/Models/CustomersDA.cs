using nmct.ba.cashlessproject.api.helper;
using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.api.Models
{
    public class CustomersDA
    {
        private const string CONNSTR = "ConnectionCashlessIT";

        public static List<Customer> GetCustomers(IEnumerable<Claim> claims)
        {
            List<Customer> res = new List<Customer>();
            string sql = "SELECT ID, CustomerName, FirstName, Street,Number,PostalCode,City,Balance, Picture FROM Customers WHERE Active=1";
            DbDataReader data = Database.GetData(Database.GetConnection(ConnectionString.Create(claims)), sql);
            while (data.Read())
            {
                res.Add(new Customer()
                {
                    Id = int.Parse(data["ID"].ToString()),
                    FirstName = data["FirstName"].ToString(),
                    CustomerName = data["CustomerName"].ToString(),
                    Street = data["Street"].ToString(),
                    City = data["City"].ToString(),
                    PostalCode = data["PostalCode"].ToString(),
                    Number = data["Number"].ToString(),
                    Balance = float.Parse(data["Balance"].ToString()),
                    Picture = GetPicture(data["Picture"])
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
        public static int UpdateCustomer(Customer customer, IEnumerable<Claim> claims)
        {

            string sql = "UPDATE Customers SET CustomerName=@naam, FirstName=@voornaam, Street=@straat,Number=@nummer,City=@plaats,Balance=@balance,PostalCode=@postcode,Picture=@pic WHERE ID=@id";
            DbParameter par8 = Database.AddParameter(CONNSTR, "id", customer.Id);
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", customer.CustomerName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "voornaam", customer.FirstName);
            DbParameter par3 = Database.AddParameter(CONNSTR, "straat", customer.Street);
            DbParameter par4 = Database.AddParameter(CONNSTR, "nummer", customer.Number);
            DbParameter par5 = Database.AddParameter(CONNSTR, "plaats", customer.City);
            DbParameter par6 = Database.AddParameter(CONNSTR, "balance", customer.Balance);
            DbParameter par7 = Database.AddParameter(CONNSTR, "postcode", customer.PostalCode);
            DbParameter par9 = Database.AddParameter(CONNSTR, "pic", customer.Picture);


            return Database.ModifyData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);

        }
        public static int InsertCustomer(Customer customer, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Customers(CustomerName, FirstName, Street,Number,City,Balance,PostalCode,Picture) VALUES(@naam,@voornaam,@straat,@nummer,@plaats,@balance,@postcode,@pic)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", customer.CustomerName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "voornaam", customer.FirstName);
            DbParameter par3 = Database.AddParameter(CONNSTR, "straat", customer.Street);
            DbParameter par4 = Database.AddParameter(CONNSTR, "nummer", customer.Number);
            DbParameter par5 = Database.AddParameter(CONNSTR, "plaats", customer.City);
            DbParameter par6 = Database.AddParameter(CONNSTR, "balance", customer.Balance);
            DbParameter par7 = Database.AddParameter(CONNSTR, "postcode", customer.PostalCode);
            DbParameter par8;
            if (customer.Picture == null) par8 = Database.AddParameter(CONNSTR, "pic", new byte[0]);
            else par8 = Database.AddParameter(CONNSTR, "pic", customer.Picture);


            return Database.InsertData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2, par3, par4, par5, par6, par7,par8);
        }
        public static int DeleteCustomer(int id, IEnumerable<Claim> claims)
        {
            string sql = "UPDATE Customers SET Active=0 WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(Database.GetConnection(ConnectionString.Create(claims)), sql, par);
        }

    }
}