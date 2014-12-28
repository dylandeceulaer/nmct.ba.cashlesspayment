using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        #region Properties
        private Employee _selected;
        public Employee Selected
        {
            get
            {
                if (_selected == null)
                {
                    Selected = new Employee() { Id = -1 };
                    return _selected;
                }
                else return _selected;
            }
            set { _selected = value; RaisePropertyChanged("Selected"); Alert = ""; RaisePropertyChanged("UpdateEmployeeCommand"); }
        }

        private string _alert;
        public string Alert
        {
            get { return _alert; }
            set { _alert = value; RaisePropertyChanged("Alert"); }
        }

        private ObservableCollection<Employee> _medewerkers;
        public ObservableCollection<Employee> Medewerkers
        {
            get { return _medewerkers; }
            set { _medewerkers = value; RaisePropertyChanged("Medewerkers"); }
        }
        #endregion

        #region Icommands
        public ICommand UpdateEmployeeCommand
        {
            get { return new RelayCommand(UpdateEmployee, Selected.IsValid); }
        }

        public ICommand DeleteEmployeeCommand
        {
            get { return new RelayCommand(DeleteEmployee, KanDelete); }
        }

        public ICommand NieuwCommand
        {
            get { return new RelayCommand(Nieuw, KanNieuw); }
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
        private async void GetMedewerkers()
        {
            using(HttpClient client = new System.Net.Http.HttpClient()){
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:5054/api/employee");
                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();
                    Medewerkers = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                    if(Medewerkers.Count != 0)Selected = Medewerkers[0];
                }
            }
        }

        private async void UpdateEmployee()
        {
            if (Selected != null) { 
                if (Selected.Id == -1)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string json = JsonConvert.SerializeObject(Selected);
                        client.SetBearerToken(ApplicationVM.token.AccessToken);
                        HttpResponseMessage res = await client.PostAsync("http://localhost:5054/api/employee", new StringContent(json, Encoding.UTF8, "application/json"));
                        if (res.IsSuccessStatusCode)
                        {
                            string jsonres = await res.Content.ReadAsStringAsync();
                            int result = JsonConvert.DeserializeObject<int>(jsonres);
                            if (result > 0)
                            {
                                Selected.Id = result;
                                Alert = "De nieuwe medewerker is opgeslagen.";

                            }
                            else
                            {
                                Alert = "Fout bij het toevoegen.";
                            }
                        }
                    }
                }
                else
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string json = JsonConvert.SerializeObject(Selected);
                        client.SetBearerToken(ApplicationVM.token.AccessToken);
                        HttpResponseMessage res = await client.PutAsync("http://localhost:5054/api/employee", new StringContent(json, Encoding.UTF8, "application/json"));
                        if (res.IsSuccessStatusCode)
                        {
                            string jsonres = await res.Content.ReadAsStringAsync();
                            int result = JsonConvert.DeserializeObject<int>(jsonres);
                            if (result == 1)
                            {
                                Alert = "De wijzigingen zijn succesvol opgeslagen.";

                            }
                            else
                            {
                                Alert = "Fout bij het opslaan van de wijzigingen.";
                            }
                        }
                    }
                }
            }
        }
        
        private async void DeleteEmployee()
        {
            if (Selected != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage res = await client.DeleteAsync("http://localhost:5054/api/employee/" + Selected.Id);
                    if (res.IsSuccessStatusCode)
                    {
                        string jsonres = await res.Content.ReadAsStringAsync();
                        
                        int result = JsonConvert.DeserializeObject<int>(jsonres);
                        if (result == 1)
                        {
                            Alert = "Succesvol verwijderd.";
                            GetMedewerkers();
                        }
                        else
                        {
                            Alert = "Fout bij het verwijderen.";
                        }
                    }
                }
            }
        }
        #endregion

        #region etc
        private bool KanNieuw()
        {
            if (Medewerkers != null && Medewerkers[Medewerkers.Count - 1].Id != -1) return true;
            return false;
        }
        private void Nieuw()
        {
            Medewerkers.Add(new Employee()
            {
                Id = -1
            });
            Selected = Medewerkers[Medewerkers.Count() - 1];
        }
        private void Terug()
        {
            Employee.DoValidation = false;
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new MenuVM());
        }
        private bool KanDelete()
        {
            if (Selected.Id > 0) return true;
            return false;
        }
        private void WindowLoaded()
        {
            Employee.DoValidation = true;
        }
        #endregion
    }
}
