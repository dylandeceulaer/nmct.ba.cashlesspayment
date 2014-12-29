using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        #region Properties
        private ObservableCollection<Customer> _klanten;
        public ObservableCollection<Customer> Klanten
        {
            get { return _klanten; }
            set { _klanten = value; RaisePropertyChanged("Klanten"); }
        }

        private Customer _selected;
        public Customer Selected
        {
            get 
            { 
                if (_selected == null) 
                { 
                    Selected = new Customer() {Id=-1 }; 
                    return _selected; 
                } 
                else return _selected; 
            }
            set { _selected = value; RaisePropertyChanged("Selected"); RaisePropertyChanged("UpdateCustomerCommand"); ImagePath = ""; Alert = ""; }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; RaisePropertyChanged("ImagePath"); GetPhoto(); }
        }

        private string _alert;
        public string Alert
        {
            get { return _alert; }
            set { _alert = value; RaisePropertyChanged("Alert"); }
        }
        #endregion

        #region Icommands
        public ICommand TerugCommand
        {
            get { return new RelayCommand(Terug); }
        }

        public ICommand UpdateCustomerCommand
        {
            get { return new RelayCommand(UpdateCustomer, Selected.IsValid); }
        }

        public ICommand DeleteCustomerCommand
        {
            get { return new RelayCommand(DeleteCustomer, KanDelete); }
        }

        public ICommand NieuwCommand
        {
            get { return new RelayCommand(Nieuw, KanNieuw); }
        }

        public ICommand AddImageCommand
        {
            get { return new RelayCommand(AddImage); }
        }
        public ICommand WindowLoadedCommand
        {
            get { return new RelayCommand(WindowLoaded); }
        }
        #endregion

        #region CRUD
        private async void GetKlanten()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/customer");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Klanten = JsonConvert.DeserializeObject<ObservableCollection<Customer>>(json);
                    if(Klanten.Count != 0)Selected = Klanten[0];
                }
            }
        }
        
        private async void UpdateCustomer()
        {
            if (Selected != null)
            {
                if (Selected.Id == -1)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string json = JsonConvert.SerializeObject(Selected);
                        client.SetBearerToken(ApplicationVM.token.AccessToken);
                        HttpResponseMessage res = await client.PostAsync("http://localhost:5054/api/customer", new StringContent(json, Encoding.UTF8, "application/json"));
                        if (res.IsSuccessStatusCode)
                        {
                            string jsonres = await res.Content.ReadAsStringAsync();
                            int result = JsonConvert.DeserializeObject<int>(jsonres);
                            if (result > 0)
                            {
                                GetKlanten();
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
                        HttpResponseMessage res = await client.PutAsync("http://localhost:5054/api/customer", new StringContent(json, Encoding.UTF8, "application/json"));
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

        private async void DeleteCustomer()
        {
            if (Selected != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage res = await client.DeleteAsync("http://localhost:5054/api/customer/" + Selected.Id);
                    if (res.IsSuccessStatusCode)
                    {
                        string jsonres = await res.Content.ReadAsStringAsync();

                        int result = JsonConvert.DeserializeObject<int>(jsonres);
                        if (result == 1)
                        {
                            Alert = "Succesvol verwijderd.";
                            GetKlanten();
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
        private void Terug()
        {
            Customer.DoValidation = false;
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new MenuVM());
        }

        private bool KanNieuw()
        {
            if (Klanten == null || Klanten.Count == 0) return true;
            if (Klanten[Klanten.Count - 1].Id != -1) return true;
            return false;
        }

        private bool KanDelete()
        {
            if (Selected.Id > 0) return true;
            return false;
        } 

        private void Nieuw()
        {
            Klanten.Add(new Customer()
            {
                Id = -1
            });
            Selected = Klanten[Klanten.Count() - 1];
        }
        private void WindowLoaded()
        {
            Customer.DoValidation = true;
        }
        #endregion

        #region ImageStuff
        private void AddImage()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files |*.jpg;*.png;*.gif";
            if (fileDialog.ShowDialog() == true)
            {
                ImagePath = fileDialog.FileName;
            }
        }

        private void GetPhoto()
        {
            if (!string.IsNullOrWhiteSpace(ImagePath))
            {
                if (File.Exists(ImagePath))
                {
                    FileStream fs = new FileStream(ImagePath, FileMode.Open, FileAccess.Read);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    fs.Close();

                    Selected.Picture = data;
                    RaisePropertyChanged("Selected");
                }
                else
                {
                    Selected.Picture = new byte[0];
                    RaisePropertyChanged("Selected");
                }
            }
        }
        #endregion

    }
}
