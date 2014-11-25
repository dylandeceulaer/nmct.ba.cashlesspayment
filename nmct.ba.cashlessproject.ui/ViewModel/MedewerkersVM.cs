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
    class MedewerkersVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Medewerkers"; }
        }
        public MedewerkersVM()
        {
            GetMedewerkers();
        }

        private Employee _selected;

        public Employee Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged("Selected"); }
        }
        

        private ObservableCollection<Employee> _medewerkers;

        public ObservableCollection<Employee> Medewerkers
        {
            get { return _medewerkers; }
            set { _medewerkers = value; RaisePropertyChanged("Medewerkers"); }
        }
        private async void GetMedewerkers()
        {
            using(HttpClient client = new HttpClient()){
                HttpResponseMessage response = await client.GetAsync("http://localhost:5054/api/employee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Medewerkers = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }
        
    }
}
