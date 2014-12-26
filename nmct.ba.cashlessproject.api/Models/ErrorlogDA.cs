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
    public class ErrorlogDA
    {
        private const string CONNSTR = "ConnectionCashlessIT";
        public static int InsertError(Errorlog error, IEnumerable<Claim> claims)
        {
            string sql = "INSERT INTO Errorlog(RegisterID,Timestamp,Message,Stacktrace) VALUES(@RegisterID,@Timestamp,@Message,@Stacktrace)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "RegisterID", error.RegisterID);
            DbParameter par2 = Database.AddParameter(CONNSTR, "Timestamp", Convert.ToInt32(Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds)));
            DbParameter par3 = Database.AddParameter(CONNSTR, "Message", error.Message);
            DbParameter par4 = Database.AddParameter(CONNSTR, "Stacktrace", error.Stacktrace);


            return Database.InsertData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2, par3, par4);
        }
    }
}