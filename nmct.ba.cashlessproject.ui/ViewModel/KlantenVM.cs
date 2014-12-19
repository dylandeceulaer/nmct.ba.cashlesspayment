using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
            set { _selected = value; RaisePropertyChanged("Selected"); ImagePath = ""; Alert = ""; }
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
                }
            }
        }
        public ICommand TerugCommand
        {
            get { return new RelayCommand(Terug); }
        }
        private void Terug()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new MenuVM());
        }
        public ICommand UpdateCustomerCommand
        {
            get { return new RelayCommand(UpdateCustomer); }
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
        private bool KanNieuw()
        {
            if (Klanten == null) return true;
            if (Klanten[Klanten.Count - 1].Id != -1) return true;
            return false;
        }

        public ICommand DeleteCustomerCommand
        {
            get { return new RelayCommand(DeleteCustomer); }
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

        public ICommand NieuwCommand
        {
            get { return new RelayCommand(Nieuw, KanNieuw); }
        }

        private void Nieuw()
        {
            Klanten.Add(new Customer()
            {
                Id = -1
            });
            Selected = Klanten[Klanten.Count() - 1];
        }

        public ICommand AddImageCommand
        {
            get { return new RelayCommand(AddImage); }
        }

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

    }
}
