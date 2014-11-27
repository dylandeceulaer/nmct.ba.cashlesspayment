using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class RegisterEmployee
    {
        private int _registerID;

        public int RegisterID
        {
            get { return _registerID; }
            set { _registerID = value; }
        }
        private int _employeeID;

        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }
        private DateTime _from;

        public DateTime From
        {
            get { return _from; }
            set { _from = value; }
        }
        private DateTime _until;

        public DateTime Until
        {
            get { return _until; }
            set { _until = value; }
        }

    }
}
