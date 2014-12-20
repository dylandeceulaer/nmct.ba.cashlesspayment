using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    class AccountVM : ObservableObject, Ipage
    {
        

        public string Name
        {
            get { return "Account"; }
        }
        public ICommand AfmeldenCommand
        {
            get { return new RelayCommand(Afmelden); }
        }
        private string _oudPaswoord;

        public string OudPaswoord
        {
            get { return _oudPaswoord; }
            set { _oudPaswoord = value; RaisePropertyChanged("OudPaswoord"); }
        }
        private string _nieuwPasswoord;

        public string NieuwPaswoord
        {
            get { return _nieuwPasswoord; }
            set { _nieuwPasswoord = value; RaisePropertyChanged("NieuwPaswoord"); }
        }
        private string _nieuwPaswoordBevestiging;
                
        public string NieuwPaswoordBevestiging
        {
            get { return _nieuwPaswoordBevestiging; }
            set { _nieuwPaswoordBevestiging = value; RaisePropertyChanged("NieuwPaswoordBevestiging"); }
        }
        private string _alert;

        public string Alert
        {
            get { return _alert; }
            set { _alert = value; RaisePropertyChanged("Alert"); }
        }

        private void Afmelden()
        {
            ApplicationVM.token = null;
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new AanmeldenVM());
        }

        public ICommand UpdatePasswordCommand
        {
            get { return new RelayCommand(UpdatePassword); }
        }
        private async void UpdatePassword()
        {
            if (OudPaswoord == ApplicationVM.password)
            {
                if(NieuwPaswoord == NieuwPaswoordBevestiging){
                    using (HttpClient client = new HttpClient())
                    {
                        List<string> Wachtwoorden = new List<string>();
                        Wachtwoorden.Add(ApplicationVM.user);
                        Wachtwoorden.Add(ApplicationVM.password);
                        Wachtwoorden.Add(NieuwPaswoord);

                        string json = JsonConvert.SerializeObject(Wachtwoorden);

                        HttpResponseMessage res = await client.PutAsync("http://localhost:5054/api/organisationAccount", new StringContent(json, Encoding.UTF8, "application/json"));
                        if (res.IsSuccessStatusCode)
                        {
                            Alert = "Nieuw wachtwoord Succesvol opgeslagen";
                        }
                    }
                }
                else
                {
                    Alert = "De wachtwoorden komen niet overeen.";
                }
            }else
            {
                Alert = "Het oude wachtwoord klopt niet.";
            }
        }

    }
}
