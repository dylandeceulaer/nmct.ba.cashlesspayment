using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Employee
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _employeeName;

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }
        private string _adress;

        public string Adress
        {
            get { return _adress; }
            set { _adress = value; }
        }
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        
        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        
    }
}
