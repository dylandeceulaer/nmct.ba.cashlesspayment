using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class RegisterEmployeeDA
    {
        private const string CONNSTR = "ConnectionCashless";
        public static List<RegisterEmployee> GetRegisterEmployees()
        {
            List<RegisterEmployee> res = new List<RegisterEmployee>();
            string sql = "SELECT RegisterID, EmployeeID, From, Until FROM Register_Employee";
            DbDataReader data = Database.GetData(CONNSTR, sql);

            while (data.Read())
            {
                res.Add(new RegisterEmployee()
                {
                    EmployeeID = int.Parse(data["EmployeeID"].ToString()),
                    RegisterID = int.Parse(data["RegisterID"].ToString()),
                    From = DateTime.Parse(data["From"].ToString()),
                    Until = DateTime.Parse(data["Until"].ToString())
                });
            }
            return res;
        }
        public static List<RegisterEmployee> GetRegisterEmployees(int id)
        {
            List<RegisterEmployee> res = new List<RegisterEmployee>();
            string sql = "SELECT * FROM Register_Employee Where RegisterID = @id";
            DbParameter para = Database.AddParameter(CONNSTR, "id", id);
            DbDataReader data = Database.GetData(CONNSTR, sql, para);

            while (data.Read())
            {
                res.Add(new RegisterEmployee()
                {
                    EmployeeID = int.Parse(data["EmployeeID"].ToString()),
                    RegisterID = int.Parse(data["RegisterID"].ToString()),
                    From = DateTime.Parse(data["From"].ToString()),
                    Until = DateTime.Parse(data["Until"].ToString())
                });
            }
            return res;
        }
    }
}