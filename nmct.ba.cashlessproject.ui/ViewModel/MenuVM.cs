using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    class MenuVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Menu"; }
        }
        public MenuVM(){
            Pages.Add(new ProductenVM());
            Pages.Add(new MedewerkersVM());
            Pages.Add(new KassasVM());
            Pages.Add(new StatestiekenVM());
        }
        private List<Ipage> _pages;
        public List<Ipage> Pages
        {
            get { if (_pages == null)_pages = new List<Ipage>(); return _pages; }
            set { _pages = value; }
        }
    }
}
