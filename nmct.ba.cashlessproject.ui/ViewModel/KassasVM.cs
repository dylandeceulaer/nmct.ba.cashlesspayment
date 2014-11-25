using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    class KassasVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Kassas"; }
        }

        public KassasVM()
        {
            GetKassas();
        }

        private ObservableCollection<Register> _kassas;

        public ObservableCollection<Register> Kassas
        {
            get { return _kassas; }
            set { _kassas = value; RaisePropertyChanged("Kassas"); }
        }

        private Register _selected;

        public Register Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged("Selected"); }
        }

        private async void GetKassas()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/register");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Kassas = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                }
            }
        }

        

    }
}
