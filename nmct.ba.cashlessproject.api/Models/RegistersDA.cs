using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System.Data.Common;
using System.Security.Claims;
using nmct.ba.cashlessproject.api.helper;

namespace nmct.ba.cashlessproject.api.Models
{
    public class RegistersDA
    {
        private const string CONNSTR = "ConnectionCashlessIT";

        public static List<Register> GetRegisters(IEnumerable<Claim> claims)
        {
            List<Register> res = new List<Register>();
            string sql = "SELECT ID, RegisterName, Device FROM Registers";
            DbDataReader data = Database.GetData(Database.GetConnection(ConnectionString.Create(claims)), sql);
            while (data.Read())
            {
                res.Add(new Register()
                {
                    Id = int.Parse(data["id"].ToString()),
                    RegisterName = data["RegisterName"].ToString(),
                    Device = data["Device"].ToString()
                });
            }
            return res;
        }

    }
}