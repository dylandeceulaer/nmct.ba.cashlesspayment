using nmct.ba.cashlessproject.helper;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class AccountDA
    {
        private const string CONNSTR = "ConnectionCashlessIT";

        public static int UpdatePassword(List<string> wachtwoorden)
        {
            string sql = "UPDATE Organisations SET Password=@pass WHERE Login=@login AND Password=@passOud";
            DbParameter par1 = Database.AddParameter(CONNSTR, "login", Cryptography.Encrypt(wachtwoorden[0]));
            DbParameter par2 = Database.AddParameter(CONNSTR, "pass", Cryptography.Encrypt(wachtwoorden[2]));
            DbParameter par3 = Database.AddParameter(CONNSTR, "passOud", Cryptography.Encrypt(wachtwoorden[1]));
            return Database.ModifyData(CONNSTR, sql, par1,par2, par3);

        }

    }
}