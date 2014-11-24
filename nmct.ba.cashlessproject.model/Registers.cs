using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    class Registers
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _registerName;

        public string RegisterName
        {
            get { return _registerName; }
            set { _registerName = value; }
        }
        private string _device;

        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }

    }
}
