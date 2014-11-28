using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.model
{
    public class Customer
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }

        private float _balance;

        public float Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        private BitmapImage _picture;

        public BitmapImage Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
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
        
    }
}
