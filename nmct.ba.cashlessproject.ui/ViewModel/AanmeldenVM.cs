using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    class AanmeldenVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Aanmelden"; }
        }

        private string _naam;

        public string Naam
        {
            get { return _naam; }
            set { _naam = value; RaisePropertyChanged("Naam"); }
        }
        private string _wachtwoord;

        public string Wachtwoord
        {
            get { return _wachtwoord; }
            set { _wachtwoord = value; RaisePropertyChanged("Wachtwoord"); }
        } 

    }
}
