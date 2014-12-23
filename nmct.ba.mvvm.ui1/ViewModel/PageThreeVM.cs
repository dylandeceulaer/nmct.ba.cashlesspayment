using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.mvvm.ui1.ViewModel
{
    public class PageThreeVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Page Three"; }
        }
    }
}
