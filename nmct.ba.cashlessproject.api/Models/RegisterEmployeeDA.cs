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
        public static void InsertRegisterEmployee(RegisterEmployee reg)
        {

            string sql = "INSERT INTO Register_Employee(EmployeeId,RegisterID,[From],[Until]) VALUES(@eid,@regid,@from,@until)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "eid", reg.EmployeeID);
            DbParameter par2 = Database.AddParameter(CONNSTR, "regid", reg.RegisterID);
            DbParameter par3 = Database.AddParameter(CONNSTR, "from", reg.From);
            DbParameter par4 = Database.AddParameter(CONNSTR, "until", reg.Until);

            Database.InsertData(CONNSTR, sql, par1, par2, par3, par4);

        }
        public static int DeleteRegisterEmployee(RegisterEmployee reg)
        {
            string sql = "DELETE FROM Register_Employee WHERE EmployeeId=@eid AND RegisterID=@regid AND [From]=@from AND [Until]=@until";
            DbParameter par1 = Database.AddParameter(CONNSTR, "eid", reg.EmployeeID);
            DbParameter par2 = Database.AddParameter(CONNSTR, "regid", reg.RegisterID);
            DbParameter par3 = Database.AddParameter(CONNSTR, "from", reg.From);
            DbParameter par4 = Database.AddParameter(CONNSTR, "until", reg.Until);

            return Database.ModifyData(CONNSTR, sql, par1, par2, par3, par4);
        }
    }
}