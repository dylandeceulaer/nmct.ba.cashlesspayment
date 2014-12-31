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
        public static List<Register> GetRegisters()
        {
            List<Register> res = new List<Register>();
            string sql = "SELECT * FROM Registers WHERE Active=1";
            DbDataReader data = Database.GetData(CONNSTR, sql);
            while (data.Read())
            {
                res.Add(new Register()
                {
                    Id = int.Parse(data["id"].ToString()),
                    RegisterName = data["RegisterName"].ToString(),
                    Device = data["Device"].ToString(),
                    ExpiresDate = Convert.ToDateTime(data["ExpiresDate"].ToString()),
                    PurchaseDate = Convert.ToDateTime(data["PurchaseDate"].ToString())
                });
            }
            return res;
        }
        public static Register GetRegisterById(int ID)
        {
            Register res = new Register();
            string sql = "SELECT * FROM Registers WHERE ID = @id";
            DbParameter par1 = Database.AddParameter(CONNSTR, "id", ID);
            DbDataReader data = Database.GetData(CONNSTR, sql ,par1);
            while (data.Read())
            {
                res = new Register()
                {
                    Id = int.Parse(data["id"].ToString()),
                    RegisterName = data["RegisterName"].ToString(),
                    Device = data["Device"].ToString(),
                    ExpiresDate = Convert.ToDateTime(data["ExpiresDate"].ToString()),
                    PurchaseDate = Convert.ToDateTime(data["PurchaseDate"].ToString())
                };
            }
            return res;
        }
        public static int InsertRegister(Register reg)
        {
            try
            {
                string sql = "INSERT INTO Registers(RegisterName, Device,PurchaseDate,ExpiresDate) VALUES(@RegisterName,@Device,@PurchaseDate,@ExpiresDate)";
                DbParameter par1 = Database.AddParameter(CONNSTR, "RegisterName", reg.RegisterName);
                DbParameter par2 = Database.AddParameter(CONNSTR, "Device", reg.Device);
                DbParameter par3 = Database.AddParameter(CONNSTR, "PurchaseDate", reg.PurchaseDate);
                DbParameter par4 = Database.AddParameter(CONNSTR, "ExpiresDate", reg.ExpiresDate);


                return Database.InsertData(CONNSTR, sql, par1, par2, par3, par4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static int InsertRegister(IEnumerable<Claim> claims, Register reg)
        {
            try
            {
                string sql = "INSERT INTO Registers(RegisterName, Device) VALUES(@RegisterName,@Device)";
                DbParameter par1 = Database.AddParameter(CONNSTR, "RegisterName", reg.RegisterName);
                DbParameter par2 = Database.AddParameter(CONNSTR, "Device", reg.Device);


                return Database.InsertData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static int UpdateRegister(Register reg)
        {
            try
            {
                string sql = "UPDATE Registers SET RegisterName=@RegisterName, Device=@Device,PurchaseDate=@PurchaseDate,ExpiresDate=@ExpiresDate WHERE ID = @id";
                DbParameter par1 = Database.AddParameter(CONNSTR, "RegisterName", reg.RegisterName);
                DbParameter par2 = Database.AddParameter(CONNSTR, "Device", reg.Device);
                DbParameter par3 = Database.AddParameter(CONNSTR, "PurchaseDate", reg.PurchaseDate);
                DbParameter par4 = Database.AddParameter(CONNSTR, "ExpiresDate", reg.ExpiresDate);
                DbParameter par5 = Database.AddParameter(CONNSTR, "id", reg.Id);


                return Database.ModifyData(CONNSTR, sql, par1, par2, par3, par4, par5);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static int UpdateRegister(IEnumerable<Claim> claims, Register reg)
        {
            try
            {
                string sql = "UPDATE Registers SET RegisterName=@RegisterName, Device=@Device WHERE ID = @id";
                DbParameter par1 = Database.AddParameter(CONNSTR, "RegisterName", reg.RegisterName);
                DbParameter par2 = Database.AddParameter(CONNSTR, "Device", reg.Device);
                DbParameter par3 = Database.AddParameter(CONNSTR, "id", reg.Id);

                return Database.ModifyData(Database.GetConnection(ConnectionString.Create(claims)), sql, par1, par2, par3);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static int DeleteRegister(int id)
        {
            string sql = "UPDATE Registers SET Active=0 WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(CONNSTR, sql, par);
        }
        public static int DeleteRegister(IEnumerable<Claim> claims,int id)
        {
            string sql = "DELETE FROM Registers WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(Database.GetConnection(ConnectionString.Create(claims)), sql, par);
        }

    }
}