using nmct.ba.cashlessproject.model;
using nmmct.ba.cashlessproject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.api.Models
{
    public class CategoryDA
    {
        private const string CONNSTR = "ConnectionCashless";
        public static List<Category> GetCategories()
        {
            List<Category> res = new List<Category>();
            string sql = "SELECT Name, ID FROM Categories";
            DbDataReader data = Database.GetData(CONNSTR, sql);

            while (data.Read())
            {
                res.Add(new Category()
                {
                    CategoryName = data["Name"].ToString(),
                    Id = int.Parse(data["ID"].ToString())
                });
            }
            return res;
        }
    }
}