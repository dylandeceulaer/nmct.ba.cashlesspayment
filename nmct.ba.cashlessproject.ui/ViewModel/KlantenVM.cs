using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    class KlantenVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Klanten"; }
        }
        public KlantenVM()
        {
            GetKlanten();
        }
        private ObservableCollection<Customer> _klanten;

        public ObservableCollection<Customer> Klanten
        {
            get { return _klanten; }
            set { _klanten = value; RaisePropertyChanged("Klanten"); }
        }
        private Customer _selected;

        public Customer Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged("Selected"); }
        }
        private async void GetKlanten()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/customer");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Klanten = JsonConvert.DeserializeObject<ObservableCollection<Customer>>(json);
                }
            }
        }
        
        
    }
}
