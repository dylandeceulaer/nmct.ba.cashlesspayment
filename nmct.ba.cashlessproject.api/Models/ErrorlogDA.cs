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
        public static List<Errorlog> GetErrorlogsById(int id)
        {
            List<Errorlog> res = new List<Errorlog>();
            string sql = "SELECT * FROM Errorlog WHERE OrganisationID=@id ORDER BY [Timestamp] DESC";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            DbDataReader data = Database.GetData(CONNSTR, sql,par);
            while (data.Read())
            {
                res.Add(new Errorlog()
                {
                   Message = data["Message"].ToString(),
                   OrganisationID = id,
                   RegisterID = int.Parse(data["RegisterID"].ToString()),
                   Stacktrace = data["Stacktrace"].ToString(),
                   Timestamp = int.Parse(data["Timestamp"].ToString())
                });
            }
            return res;
        }
        public static int InsertErrorLog(Errorlog e)
        {
            string sql = "INSERT INTO ErrorLog(Message, OrganisationID, RegisterID,Stacktrace,Timestamp) VALUES(@Message,@OrganisationID,@RegisterID,@Stacktrace,@Timestamp)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "Message", e.Message);
            DbParameter par2 = Database.AddParameter(CONNSTR, "OrganisationID", e.OrganisationID);
            DbParameter par3 = Database.AddParameter(CONNSTR, "RegisterID", e.RegisterID);
            DbParameter par4 = Database.AddParameter(CONNSTR, "Stacktrace", e.Stacktrace);
            DbParameter par5 = Database.AddParameter(CONNSTR, "Timestamp", e.Timestamp);

            return Database.InsertData(CONNSTR, sql, par1, par2, par3, par4, par5);
        }

        public static List<Errorlog> GetNewErrorlogs(List<Claim> claims, int datemin, int orgID)
        {
            List<Errorlog> res = new List<Errorlog>();
            string sql = "SELECT * FROM Errorlog WHERE Timestamp>@datemin ORDER BY [Timestamp] DESC";
            DbParameter par = Database.AddParameter(CONNSTR, "datemin", datemin);
            DbDataReader data = Database.GetData(Database.GetConnection(ConnectionString.Create(claims)), sql,par);
            while (data.Read())
            {
                res.Add(new Errorlog()
                {
                    Message = data["Message"].ToString(),
                    RegisterID = int.Parse(data["RegisterID"].ToString()),
                    Stacktrace = data["Stacktrace"].ToString(),
                    Timestamp = int.Parse(data["Timestamp"].ToString()),
                    OrganisationID = orgID
                });
            }
            return res;
        }
        
    }
}