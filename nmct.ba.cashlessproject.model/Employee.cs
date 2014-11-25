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
        private string _street;

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }private string _number;

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }
        private string _postalCode;

        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }
        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
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
