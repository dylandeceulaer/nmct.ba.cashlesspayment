using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class PageOneVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Page One"; }
        }
    }
}
