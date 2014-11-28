using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.api.Models
{
    public class CustomersDA
    {
        private const string CONNSTR = "ConnectionCashless";

        public static List<Customer> GetCustomers()
        {
            List<Customer> res = new List<Customer>();
            string sql = "SELECT ID, CustomerName, FirstName, Street,Number,PostalCode,City,Balance FROM Customers";
            DbDataReader data = Database.GetData(CONNSTR, sql);
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
                    Balance = float.Parse(data["Balance"].ToString())
                });
            }
            return res;
        }
        public static int UpdateCustomer(Customer customer)
        {

            string sql = "UPDATE Customers SET CustomerName=@naam, FirstName=@voornaam, Street=@straat,Number=@nummer,City=@plaats,Balance=@balance,PostalCode=@postcode WHERE ID=@id";
            DbParameter par8 = Database.AddParameter(CONNSTR, "id", customer.Id);
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", customer.CustomerName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "voornaam", customer.FirstName);
            DbParameter par3 = Database.AddParameter(CONNSTR, "straat", customer.Street);
            DbParameter par4 = Database.AddParameter(CONNSTR, "nummer", customer.Number);
            DbParameter par5 = Database.AddParameter(CONNSTR, "plaats", customer.City);
            DbParameter par6 = Database.AddParameter(CONNSTR, "balance", customer.Balance);
            DbParameter par7 = Database.AddParameter(CONNSTR, "postcode", customer.PostalCode);

            return Database.ModifyData(CONNSTR, sql, par1, par2, par3, par4, par5, par6, par7, par8);

        }
        public static int InsertCustomer(Customer customer)
        {
            string sql = "INSERT INTO Customers(CustomerName, FirstName, Street,Number,City,Balance,PostalCode) VALUES(@naam,@voornaam,@straat,@nummer,@plaats,@balance,@postcode)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", customer.CustomerName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "voornaam", customer.FirstName);
            DbParameter par3 = Database.AddParameter(CONNSTR, "straat", customer.Street);
            DbParameter par4 = Database.AddParameter(CONNSTR, "nummer", customer.Number);
            DbParameter par5 = Database.AddParameter(CONNSTR, "plaats", customer.City);
            DbParameter par6 = Database.AddParameter(CONNSTR, "balance", customer.Balance);
            DbParameter par7 = Database.AddParameter(CONNSTR, "postcode", customer.PostalCode);


            return Database.InsertData(CONNSTR, sql, par1, par2, par3, par4, par5, par6, par7);
        }
        public static int DeleteCustomer(int id)
        {
            string sql = "DELETE FROM Customers WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(CONNSTR, sql, par);
        }



    }
}