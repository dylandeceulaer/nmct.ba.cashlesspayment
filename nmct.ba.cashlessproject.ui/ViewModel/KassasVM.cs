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
            GetMedewerkers();
        }

        private ObservableCollection<Register> _kassas;

        public ObservableCollection<Register> Kassas
        {
            get { return _kassas; }
            set { _kassas = value; RaisePropertyChanged("Kassas"); }
        }

        private ObservableCollection<RegisterEmployee> _kassasBediening;

        public ObservableCollection<RegisterEmployee> KassasBediening
        {
            get { return _kassasBediening; }
            set { _kassasBediening = value; RaisePropertyChanged("KassasBediening"); }
        }

        private RegisterEmployee _selectedRE;

        public RegisterEmployee SelectedRE
        {
            get { return _selectedRE; }
            set { _selectedRE = value; RaisePropertyChanged("KassaBediening"); SetBediening(); }
        }

        private void SetBediening()
        {
            SelectedIndexEmpl = SelectedRE.EmployeeID;
        }     

        private Register _selected;

        public Register Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged("Selected"); GetKassaBediening(); }
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
        private async void GetKassaBediening()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/RegisterEmployee/"+Selected.Id);
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    KassasBediening = JsonConvert.DeserializeObject<ObservableCollection<RegisterEmployee>>(json);
                    
                }
            }
        }
        private int _selectedIndexEmpl;

        public int SelectedIndexEmpl
        {
            get { return _selectedIndexEmpl; }
            set { _selectedIndexEmpl = value; RaisePropertyChanged("SelectedIndexEmpl"); }
        }
        

        private ObservableCollection<Employee> _medewerkers;

        public ObservableCollection<Employee> Medewerkers
        {
            get { return _medewerkers; }
            set { _medewerkers = value; RaisePropertyChanged("Medewerkers"); }
        }
        private async void GetMedewerkers()
        {
            using (HttpClient client = new HttpClient())
            {
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
