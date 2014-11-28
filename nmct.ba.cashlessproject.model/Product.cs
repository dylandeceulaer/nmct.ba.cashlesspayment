using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.model
{
    public class Product
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }
        private float _price;

        public float Price
        {
            get { return _price; }
            set { _price = value; }
        }
        private int _category;

        public int Category
        {
            get { return _category; }
            set { _category = value; }
        }
        private BitmapImage _image;

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }
        
        
    }
}
