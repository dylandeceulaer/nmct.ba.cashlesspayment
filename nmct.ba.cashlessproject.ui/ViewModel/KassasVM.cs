using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
using System.Windows.Input;

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
            set { _selectedRE = value; RaisePropertyChanged("SelectedRE"); SetSelectedEmpl(); }
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
        private Employee _selectedEmpl;

        public Employee SelectedEmpl
        {
            get { return _selectedEmpl; }
            set { _selectedEmpl = value; RaisePropertyChanged("SelectedEmpl"); }
        }

        private void SetSelectedEmpl()
        {
            if (SelectedRE != null && SelectedRE.EmployeeID >0)
            {
                var selected = from e in Medewerkers where e.Id == SelectedRE.EmployeeID select e;
                SelectedEmpl = selected.ToList()[0] as Employee;
            }
            else SelectedEmpl = null;
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

        public ICommand InsertRegEmpCommand
        {
            get { return new RelayCommand(InsertRegEmp); }
        }

        private async void InsertRegEmp()
        {
            if (Selected != null && SelectedEmpl != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    RegisterEmployee nieuw = SelectedRE;
                    nieuw.EmployeeID = SelectedEmpl.Id;

                    string json = JsonConvert.SerializeObject(nieuw);

                    HttpResponseMessage res = await client.PostAsync("http://localhost:5054/api/RegisterEmployee", new StringContent(json, Encoding.UTF8, "application/json"));
                    if (res.IsSuccessStatusCode)
                    {
                        GetKassaBediening();
                    }
                }
            }
        }

        public ICommand NieuwCommand
        {
            get { return new RelayCommand(Nieuw); }
        }

        private void Nieuw()
        {
            if (Selected != null && Selected.Id > 0)
            {
                KassasBediening.Add(new RegisterEmployee()
                {
                    RegisterID = Selected.Id,
                    From = DateTime.Now,
                    Until = DateTime.Now
                });
                SelectedRE = KassasBediening[KassasBediening.Count() - 1];
            }
        }

        public ICommand DeleteRegEmpCommand
        {
            get { return new RelayCommand(DeleteRegEmp); }
        }
        private async void DeleteRegEmp()
        {
            if (Selected != null)
            {

                using (HttpClient client = new HttpClient())
                {
                    string json = JsonConvert.SerializeObject(SelectedRE);

                    HttpResponseMessage res = await client.PutAsync("http://localhost:5054/api/RegisterEmployee", new StringContent(json, Encoding.UTF8, "application/json"));
                    if (res.IsSuccessStatusCode)
                    {
                        string jsonres = await res.Content.ReadAsStringAsync();
                        int result = JsonConvert.DeserializeObject<int>(jsonres);
                        if (result >= 1)
                        {
                            GetKassaBediening();
                        }
                    }
                }
            }
        }
    }
}
