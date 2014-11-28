using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Sales
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _customerID;

        public int CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }
        private int _registerID;

        public int RegisterID
        {
            get { return _registerID; }
            set { _registerID = value; }
        }
        private int _productID;

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }
        private int _amound;

        public int Amound
        {
            get { return _amound; }
            set { _amound = value; }
        }
        private DateTime _timeStamp;

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
        private float _totalPrice;

        public float TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }
        
        
    }
}
