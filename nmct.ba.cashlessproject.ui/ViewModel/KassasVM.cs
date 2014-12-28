using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

        #region properties
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
            get
            {
               return _selectedRE;
            }
            set { _selectedRE = value; RaisePropertyChanged("SelectedRE"); RaisePropertyChanged("InsertRegEmpCommand"); }
        }  

        private Register _selected;
        public Register Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged("Selected"); GetKassaBediening(); }
        }
        private Employee _selectedEmpl;
        public Employee SelectedEmpl
        {
            get { return _selectedEmpl; }
            set { _selectedEmpl = value; RaisePropertyChanged("SelectedEmpl"); }
        }
        private ObservableCollection<Employee> _medewerkers;
        public ObservableCollection<Employee> Medewerkers
        {
            get { return _medewerkers;}
            set { _medewerkers = value; RaisePropertyChanged("Medewerkers"); }
        }
        private string _alert;
        public string Alert
        {
            get { return _alert; }
            set { _alert = value; RaisePropertyChanged("Alert"); }
        }
        #endregion

        #region Icommands
        public ICommand InsertRegEmpCommand
        {
            get {
                if (SelectedRE != null) return new RelayCommand(InsertRegEmp, SelectedRE.IsValid);
                else return null;
            }
        }
        public ICommand NieuwCommand
        {
            get { return new RelayCommand(Nieuw); }
        }
        public ICommand DeleteRegEmpCommand
        {
            get { return new RelayCommand(DeleteRegEmp, KanDelete); }
        }
        public ICommand TerugCommand
        {
            get { return new RelayCommand(Terug); }
        }
        public ICommand WindowLoadedCommand
        {
            get { return new RelayCommand(WindowLoaded); }
        }
        #endregion

        #region CRUD
        private async void GetKassas()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/register");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Kassas = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                    if(Kassas.Count!=0) Selected = Kassas[0];
                }
            }
        }
        private async void GetKassaBediening()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/RegisterEmployee/"+Selected.Id);
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    KassasBediening = JsonConvert.DeserializeObject<ObservableCollection<RegisterEmployee>>(json);
                    if (KassasBediening.Count > 0)
                    {
                        SelectedRE = KassasBediening[0];
                    }
                    else
                    {
                        SelectedRE = new RegisterEmployee() { From = DateTime.Now, Until = DateTime.Now, RegisterID = Selected.Id };
                    }
                }
            }
        }
        
        private async void GetMedewerkers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:5054/api/employee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Medewerkers = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }
        private async void InsertRegEmp()
        {
            if (Selected != null && SelectedRE !=null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    SelectedRE.From = SelectedRE.From.AddMilliseconds(-SelectedRE.From.Millisecond);
                    SelectedRE.Until = SelectedRE.Until.AddMilliseconds(-SelectedRE.Until.Millisecond);

                    string json = JsonConvert.SerializeObject(SelectedRE);

                    HttpResponseMessage res = await client.PostAsync("http://localhost:5054/api/RegisterEmployee", new StringContent(json, Encoding.UTF8, "application/json"));
                    if (res.IsSuccessStatusCode)
                    {
                        GetKassaBediening();
                        Alert = "Succesvol toegevoegd.";
                    }
                    else
                    {
                        Alert = "Fout bij het toevoegen.";
                    }
                }
            }
        }
        private async void DeleteRegEmp()
        {
            if (Selected != null)
            {

                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    string json = JsonConvert.SerializeObject(SelectedRE);

                    HttpResponseMessage res = await client.PutAsync("http://localhost:5054/api/RegisterEmployee", new StringContent(json, Encoding.UTF8, "application/json"));
                    if (res.IsSuccessStatusCode)
                    {
                        string jsonres = await res.Content.ReadAsStringAsync();
                        int result = JsonConvert.DeserializeObject<int>(jsonres);
                        if (result >= 1)
                        {
                            GetKassaBediening();
                            Alert = "Sucesvol verwijderd.";
                        }
                        else
                        {
                            Alert = "Je kan items die niet zijn opgeslagen niet verwijderen.";
                        }
                    }
                }
            }
        }
        #endregion

        #region etc
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
        private void Terug()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new MenuVM());
            RegisterEmployee.DoValidation = false;
        }
        private bool KanDelete()
        {
            if (SelectedRE != null) return true;
            return false;
        }
        private void WindowLoaded()
        {
            RegisterEmployee.DoValidation = true;
        }
        #endregion
    }
}
