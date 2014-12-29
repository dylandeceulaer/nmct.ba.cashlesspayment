using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class Organistion_RegisterDA
    {
        
        private const string CONNSTR = "ConnectionCashlessIT";


        public static List<Organistion_Register> GetOrganistion_Registers()
        {
            List<Organistion_Register> res = new List<Organistion_Register>();
            string sql = "SELECT * FROM Organistion_Register";
            DbDataReader data = Database.GetData(CONNSTR, sql);
            while (data.Read())
            {
                res.Add(new Organistion_Register()
                {
                    OrganisationID = int.Parse(data["OrganisationID"].ToString()),
                    RegisterID = int.Parse(data["RegisterID"].ToString()),
                    UntilDate = Convert.ToDateTime(data["UntilDate"].ToString()),
                    FromDate = Convert.ToDateTime(data["FromDate"].ToString())
                });
            }
            return res;
        }
        public static Organistion_Register GetOrganistion_RegisterById(int ID)
        {
            Organistion_Register res = new Organistion_Register();
            string sql = "SELECT * FROM Organistion_Register WHERE RegisterID = @id";
            DbParameter par1 = Database.AddParameter(CONNSTR, "id", ID);
            DbDataReader data = Database.GetData(CONNSTR, sql ,par1);
            while (data.Read())
            {
                res = new Organistion_Register()
                {
                    OrganisationID = int.Parse(data["OrganisationID"].ToString()),
                    RegisterID = int.Parse(data["RegisterID"].ToString()),
                    UntilDate = Convert.ToDateTime(data["UntilDate"].ToString()),
                    FromDate = Convert.ToDateTime(data["FromDate"].ToString())
                };
            }
            return res;
        }
        public static int InsertOrganistion_Register(Organistion_Register reg)
        {
            try
            {
                string sql = "INSERT INTO Organistion_Register(RegisterID, OrganisationID,FromDate,UntilDate) VALUES(@RegisterID,@OrganisationID,@FromDate,@UntilDate)";
                DbParameter par1 = Database.AddParameter(CONNSTR, "RegisterID", reg.RegisterID);
                DbParameter par2 = Database.AddParameter(CONNSTR, "OrganisationID", reg.OrganisationID);
                DbParameter par3 = Database.AddParameter(CONNSTR, "FromDate", reg.FromDate);
                DbParameter par4 = Database.AddParameter(CONNSTR, "UntilDate", reg.UntilDate);


                return Database.InsertData(CONNSTR, sql, par1, par2, par3, par4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static int UpdateOrganistion_Register(Organistion_Register reg)
        {
            try
            {
                string sql = "UPDATE Organistion_Register SET RegisterID=@RegisterID, OrganisationID=@OrganisationID,FromDate=@FromDate,UntilDate=@UntilDate WHERE RegisterID = @RegisterID AND OrganisationID=@OrganisationID";
                DbParameter par1 = Database.AddParameter(CONNSTR, "RegisterID", reg.RegisterID);
                DbParameter par2 = Database.AddParameter(CONNSTR, "OrganisationID", reg.OrganisationID);
                DbParameter par3 = Database.AddParameter(CONNSTR, "FromDate", reg.FromDate);
                DbParameter par4 = Database.AddParameter(CONNSTR, "UntilDate", reg.UntilDate);


                return Database.ModifyData(CONNSTR, sql, par1, par2, par3, par4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static int DeleteOrganistion_Register(Organistion_Register reg)
        {
            string sql = "DELETE FROM Organistion_Register WHERE RegisterID=@RegisterID AND OrganisationID=@OrganisationID";
            DbParameter par1 = Database.AddParameter(CONNSTR, "RegisterID", reg.RegisterID);
            DbParameter par2 = Database.AddParameter(CONNSTR, "OrganisationID", reg.OrganisationID);
            return Database.ModifyData(CONNSTR, sql, par1, par2);
        }

    }
}