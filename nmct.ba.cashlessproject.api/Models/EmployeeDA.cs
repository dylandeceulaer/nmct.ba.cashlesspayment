using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class EmployeeDA
    {
        private const string CONNSTR = "ConnectionCashless";

        public static List<Employee> GetEployees()
        {
            List<Employee> res = new List<Employee>();
            string sql = "SELECT ID, EmployeeName, FirstName, Street,Number,PostalCode,City, Phone, Email FROM Employee";
            DbDataReader data = Database.GetData(CONNSTR, sql);
            while (data.Read())
            {
                res.Add(new Employee()
                {
                    Id = int.Parse(data["ID"].ToString()),
                    FirstName = data["FirstName"].ToString(),
                    EmployeeName = data["EmployeeName"].ToString(),
                    Street = data["Street"].ToString(),
                    City=data["City"].ToString(),
                    PostalCode=data["PostalCode"].ToString(),
                    Number=data["Number"].ToString(),
                    Phone = data["Phone"].ToString(),
                    Email = data["Email"].ToString()
                });
            }
            return res;
        }
        public static int UpdateEmployee(Employee empl)
        {
            
            string sql = "UPDATE Employee SET EmployeeName=@naam, FirstName=@voornaam, Street=@straat,Number=@nummer,City=@plaats,Phone=@tel,Email=@mail,PostalCode=@postcode WHERE ID=@id";
            DbParameter par8 = Database.AddParameter(CONNSTR, "id", empl.Id);            
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", empl.EmployeeName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "voornaam", empl.FirstName);
            DbParameter par3 = Database.AddParameter(CONNSTR, "straat", empl.Street);
            DbParameter par4 = Database.AddParameter(CONNSTR, "nummer", empl.Number);
            DbParameter par5 = Database.AddParameter(CONNSTR, "plaats", empl.City);
            DbParameter par6 = Database.AddParameter(CONNSTR, "tel", empl.Phone);
            DbParameter par7 = Database.AddParameter(CONNSTR, "mail", empl.Email);
            DbParameter par9 = Database.AddParameter(CONNSTR, "postcode", empl.PostalCode);

            return Database.ModifyData(CONNSTR, sql, par1, par2, par3, par4, par5, par6, par7, par8,par9);

        }
        public static int InsertEmployee(Employee empl)
        {
            string sql = "INSERT INTO Employee(EmployeeName, FirstName, Street,Number,City,Phone,Email,PostalCode) VALUES(@naam,@voornaam,@straat,@nummer,@plaats,@tel,@mail,@postcode)";
            DbParameter par1 = Database.AddParameter(CONNSTR, "naam", empl.EmployeeName);
            DbParameter par2 = Database.AddParameter(CONNSTR, "voornaam", empl.FirstName);
            DbParameter par3 = Database.AddParameter(CONNSTR, "straat", empl.Street);
            DbParameter par4 = Database.AddParameter(CONNSTR, "nummer", empl.Number);
            DbParameter par5 = Database.AddParameter(CONNSTR, "plaats", empl.City);
            DbParameter par6 = Database.AddParameter(CONNSTR, "tel", empl.Phone);
            DbParameter par7 = Database.AddParameter(CONNSTR, "mail", empl.Email);
            DbParameter par8 = Database.AddParameter(CONNSTR, "postcode", empl.PostalCode);


            return Database.InsertData(CONNSTR, sql, par1, par2, par3, par4, par5, par6, par7,par8);
        }
        public static int DeleteEmployee(int id)
        {
            string sql = "DELETE FROM Employee WHERE ID=@id";
            DbParameter par = Database.AddParameter(CONNSTR, "id", id);
            return Database.ModifyData(CONNSTR, sql, par);
        }
    }
}