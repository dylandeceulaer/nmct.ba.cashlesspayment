using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.model
{
    class Customers
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
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _balance;

        public int Balance
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
        
    }
}
