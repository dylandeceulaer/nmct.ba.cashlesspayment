using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class EmployeeDA
    {
        private const string CONNSTR = "ConnectionCashless";

        public static List<Employee> GetEployees()
        {
            List<Employee> res = new List<Employee>();
            string sql = "SELECT ID, EmployeeName, Street,Number,PostalCode,City, Phone, Email FROM Employee";
            DbDataReader data = Database.GetData(CONNSTR, sql);
            while (data.Read())
            {
                res.Add(new Employee()
                {
                    Id = int.Parse(data["ID"].ToString()),
                    EmployeeName = data["EmployeeName"].ToString(),
                    Street = data["Street"].ToString(),
                    City=data["City"].ToString(),
                    PostalCode=data["PostalCode"].ToString(),
                    Number=data["Number"].ToString(),
                    Phone = data["Phone"].ToString(),
                    Email = data["Email"].ToString()
                });
            }
            return res;
        }
    }
}