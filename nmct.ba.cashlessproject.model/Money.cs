using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Money : ObservableObject
    {
        private int _count;

        public int count
        {
            get { return _count; }
            set { _count = value; RaisePropertyChanged("count"); }
        }
        
        public int value { get; set; }
    }
}
