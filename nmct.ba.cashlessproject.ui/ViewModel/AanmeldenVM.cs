using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

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
        public ICommand LoginCommand
        {
            get { return new RelayCommand(Login); }
        }
        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; RaisePropertyChanged("Error"); }
        }

        private void Login()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();

            if (!ApplicationVM.token.IsError)
            {
                appvm.ChangePage(new MenuVM());
            }
            else
            {
                Error = "Naam of paswoord kloppen niet";
            }
        }

        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:5054/token"));
            return client.RequestResourceOwnerPasswordAsync(Naam, Wachtwoord).Result;
        }

    }
}
