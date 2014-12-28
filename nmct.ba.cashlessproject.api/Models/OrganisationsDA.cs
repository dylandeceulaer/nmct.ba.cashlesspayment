using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

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
        public static List<Organisation> GetOrganisations()
        {
            List<Organisation> res = new List<Organisation>();
            string sql = "SELECT * FROM Organisations WHERE Active=1";
            DbDataReader data = Database.GetData(CONNSTR, sql);
            while (data.Read())
            {
                res.Add(new Organisation()
                {
                    ID = int.Parse(data["ID"].ToString()),
                    Login = Cryptography.Decrypt(data["Login"].ToString()),
                    Password = Cryptography.Decrypt(data["Password"].ToString()),
                    DbName = Cryptography.Decrypt(data["DbName"].ToString()),
                    DbLogin = Cryptography.Decrypt(data["DbLogin"].ToString()),
                    DbPassword = Cryptography.Decrypt(data["DbPassword"].ToString()),
                    OrganisationName = data["OrganisationName"].ToString(),
                    Address = data["Address"].ToString(),
                    Email = data["Email"].ToString(),
                    Phone = data["Phone"].ToString()
                });
            }
            return res;
        }
        public static Organisation GetOrganisationById(int ID)
        {
            Organisation res = new Organisation();
            string sql = "SELECT * FROM Organisations WHERE ID=@id";
            DbParameter par1 = Database.AddParameter(CONNSTR, "id", ID);
            DbDataReader data = Database.GetData(CONNSTR, sql,par1);
            while (data.Read())
            {
                res = new Organisation()
                {
                    ID = int.Parse(data["ID"].ToString()),
                    Login = Cryptography.Decrypt(data["Login"].ToString()),
                    Password = Cryptography.Decrypt(data["Password"].ToString()),
                    DbName = Cryptography.Decrypt(data["DbName"].ToString()),
                    DbLogin = Cryptography.Decrypt(data["DbLogin"].ToString()),
                    DbPassword = Cryptography.Decrypt(data["DbPassword"].ToString()),
                    OrganisationName = data["OrganisationName"].ToString(),
                    Address = data["Address"].ToString(),
                    Email = data["Email"].ToString(),
                    Phone = data["Phone"].ToString()
                };
            }
            return res;
        }
        public static int UpdateOrganisation(Organisation organisation)
        {
            try
            {
                string sql = "UPDATE Organisations SET Login=@login, Password=@password,DbName=@name,DbLogin=@dblogin,Dbpassword=@dbpassword,OrganisationName=@organisationName,Address=@address,Email=@email,Phone=@phone WHERE ID=@id";
                DbParameter par1 = Database.AddParameter(CONNSTR, "login", organisation.Login);
                DbParameter par2 = Database.AddParameter(CONNSTR, "password", organisation.Password);
                DbParameter par3 = Database.AddParameter(CONNSTR, "name", organisation.DbName);
                DbParameter par4 = Database.AddParameter(CONNSTR, "dblogin", organisation.DbLogin);
                DbParameter par5 = Database.AddParameter(CONNSTR, "dbpassword", organisation.DbPassword);
                DbParameter par6 = Database.AddParameter(CONNSTR, "organisationName", organisation.OrganisationName);
                DbParameter par7 = Database.AddParameter(CONNSTR, "address", organisation.Address);
                DbParameter par8 = Database.AddParameter(CONNSTR, "email", organisation.Email);
                DbParameter par9 = Database.AddParameter(CONNSTR, "phone", organisation.Phone);
                DbParameter par10 = Database.AddParameter(CONNSTR, "id", organisation.ID);


                return Database.InsertData(CONNSTR, sql, par1, par2, par3, par4, par5, par6, par7, par8, par9, par10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static int DeleteOrganisation(int id)
        {
            string sql = "UPDATE Organisations SET Active=0 WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(CONNSTR, sql, par);
        }

        public static void CreateDatabase(Organisation o)
        {
            // create the actual database
            string create = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/create.txt"));// only for the web
            //string create = File.ReadAllText(@"..\..\Data\create.txt"); // only for desktop
            string sql = create.Replace("@@DbName", o.DbName).Replace("@@DbLogin", o.DbLogin).Replace("@@DbPassword", o.DbPassword);
            foreach (string commandText in RemoveGo(sql))
            {
                Database.ModifyData(Database.GetConnection(CONNSTR), commandText);
            }

            // create login, user and tables
            DbTransaction trans = null;
            try
            {
                trans = Database.BeginTransaction(CONNSTR);

                string fill = File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/fill.txt")); // only for the web
                //string fill = File.ReadAllText(@"..\..\Data\fill.txt"); // only for desktop
                string sql2 = fill.Replace("@@DbName", o.DbName).Replace("@@DbLogin", o.DbLogin).Replace("@@DbPassword", o.DbPassword);

                foreach (string commandText in RemoveGo(sql2))
                {
                    Database.ModifyData(trans, commandText);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
            }
        }

        private static string[] RemoveGo(string input)
        {
            //split the script on "GO" commands
            string[] splitter = new string[] { "\r\nGO\r\n" };
            string[] commandTexts = input.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            commandTexts[commandTexts.Length - 1] = commandTexts[commandTexts.Length - 1].Replace("\r\nGO", "");
            return commandTexts;
        }

    }
}