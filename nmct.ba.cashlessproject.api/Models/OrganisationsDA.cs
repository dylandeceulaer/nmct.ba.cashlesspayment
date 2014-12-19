using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class OrganisationsDA
    {
        private const string CONNSTR = "ConnectionCashlessIT";
        public static Organisation CheckCredentials(string username, string password)
        {
            string sql = "SELECT * FROM Organisations WHERE Login=@Login AND Password=@Password";

            DbParameter par1 = Database.AddParameter(CONNSTR, "@Login", Cryptography.Encrypt(username));
            DbParameter par2 = Database.AddParameter(CONNSTR, "@Password", Cryptography.Encrypt(password));

            //DbParameter par1 = Database.AddParameter(CONNSTR, "@Login", username);
            //DbParameter par2 = Database.AddParameter(CONNSTR, "@Password", password);
            try
            {
                DbDataReader reader = Database.GetData(CONNSTR, sql, par1, par2);
                reader.Read();
                return new Organisation()
                {
                    ID = Int32.Parse(reader["ID"].ToString()),
                    Login = reader["Login"].ToString(),
                    Password = reader["Password"].ToString(),
                    DbName = reader["DbName"].ToString(),
                    DbLogin = reader["DbLogin"].ToString(),
                    DbPassword = reader["DbPassword"].ToString(),
                    OrganisationName = reader["OrganisationName"].ToString(),
                    Address = reader["Address"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public static int InsertOrganisation(Organisation organisation)
        {
            try
            {
                string sql = "INSERT INTO Organisations(Login, Password,DbName,DbLogin,Dbpassword,OrganisationName,Address,Email,Phone) VALUES(@login,@password,@name,@dblogin,@dbpassword,@organisationName,@address,@email,@phone)";
                DbParameter par1 = Database.AddParameter(CONNSTR, "login", organisation.Login);
                DbParameter par2 = Database.AddParameter(CONNSTR, "password", organisation.Password);
                DbParameter par3 = Database.AddParameter(CONNSTR, "name", organisation.DbName);
                DbParameter par4 = Database.AddParameter(CONNSTR, "dblogin", organisation.DbLogin);
                DbParameter par5 = Database.AddParameter(CONNSTR, "dbpassword", organisation.DbPassword);
                DbParameter par6 = Database.AddParameter(CONNSTR, "organisationName", organisation.OrganisationName);
                DbParameter par7 = Database.AddParameter(CONNSTR, "address", organisation.Address);
                DbParameter par8 = Database.AddParameter(CONNSTR, "email", organisation.Email);
                DbParameter par9 = Database.AddParameter(CONNSTR, "phone", organisation.Phone);


                return Database.InsertData(CONNSTR, sql, par1, par2, par3, par4, par5, par6, par7, par8, par9);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        
    }
}