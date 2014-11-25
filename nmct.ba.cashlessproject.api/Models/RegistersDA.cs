using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System.Data.Common;

namespace nmct.ba.cashlessproject.api.Models
{
    public class RegistersDA
    {
        private const string CONNSTR = "ConnectionCashless";

        public static List<Register> GetRegisters()
        {
            List<Register> res = new List<Register>();
            string sql = "SELECT ID, RegisterName, Device FROM Registers";
            DbDataReader data = Database.GetData(CONNSTR, sql);
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