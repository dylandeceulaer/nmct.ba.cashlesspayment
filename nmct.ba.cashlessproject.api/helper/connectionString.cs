using nmct.ba.cashlessproject.helper;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace nmct.ba.cashlessproject.api.helper
{
    public class ConnectionString
    {
        public static ConnectionStringSettings Create(IEnumerable<Claim> claims)
        {
            try
            {
                string dblogin = claims.FirstOrDefault(c => c.Type == "dblogin").Value;
                string dbpass = claims.FirstOrDefault(c => c.Type == "dbpass").Value;
                string dbname = claims.FirstOrDefault(c => c.Type == "dbname").Value;

                return Database.CreateConnectionString("System.Data.SqlClient", @"DYLAN-PC\SERVER", Cryptography.Decrypt(dbname), Cryptography.Decrypt(dblogin), Cryptography.Decrypt(dbpass));
                //return Database.CreateConnectionString("System.Data.SqlClient", @"DYLAN-PC\SQLEXPRESS", dbname, dblogin, dbpass);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
    }
}