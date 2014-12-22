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
    class ProductenVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Producten"; }
        }
        public ProductenVM()
        {
            GetProducten();
            GetCategorien();
        }
        #region Properties
        private ObservableCollection<Product> _producten;
        public ObservableCollection<Product> Producten
        {
            get { return _producten; }
            set { _producten = value; RaisePropertyChanged("Producten"); }
        }
        private List<Category> _categorien;
        public List<Category> Categorien
        {
            get { return _categorien; }
            set { _categorien = value; RaisePropertyChanged("Categorien"); }
        }
        private Product _selected;
        public Product Selected
        {
            get
            {
                if (_selected == null)
                {
                    Selected = new Product() { Id = -1 };
                    return _selected;
                }
                else return _selected;
            }
            set { _selected = value; RaisePropertyChanged("Selected"); Alert = ""; ImagePath = ""; RaisePropertyChanged("UpdateProductCommand"); }
        }
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged("SelectedCategory"); }
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
        public ICommand UpdateProductCommand
        {
            get { return new RelayCommand(UpdateProduct, Selected.IsValid); }
        }
        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProduct, KanDelete); }
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
        
        private async void GetProducten()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/product");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Producten = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                    Selected = Producten[0];
                }
            }
        }
        private async void GetCategorien()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/category");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Categorien = JsonConvert.DeserializeObject<List<Category>>(json);
                }
            }
        }
        
        

        
        private async void UpdateProduct()
        {
            if (Selected != null)
            {
                if (Selected.Id == -1)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string json = JsonConvert.SerializeObject(Selected);
                        client.SetBearerToken(ApplicationVM.token.AccessToken);
                        HttpResponseMessage res = await client.PostAsync("http://localhost:5054/api/Product", new StringContent(json, Encoding.UTF8, "application/json"));
                        if (res.IsSuccessStatusCode)
                        {
                            string jsonres = await res.Content.ReadAsStringAsync();
                            int result = JsonConvert.DeserializeObject<int>(jsonres);
                            if (result > 0)
                            {
                                Selected.Id = result;
                                Alert = "Het nieuwe product is opgeslagen.";

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
                        HttpResponseMessage res = await client.PutAsync("http://localhost:5054/api/Product", new StringContent(json, Encoding.UTF8, "application/json"));
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
        private async void DeleteProduct()
        {
            if (Selected != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage res = await client.DeleteAsync("http://localhost:5054/api/Product/" + Selected.Id);
                    if (res.IsSuccessStatusCode)
                    {
                        string jsonres = await res.Content.ReadAsStringAsync();

                        int result = JsonConvert.DeserializeObject<int>(jsonres);
                        if (result == 1)
                        {
                            Alert = "Succesvol verwijderd.";
                            GetProducten();
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
            if (Producten == null) return true;
            if (Producten[Producten.Count - 1].Id != -1) return true;
            return false;
        }
        
        private void WindowLoaded()
        {
            Product.DoValidation = true;
        }
        private void Nieuw()
        {
            Producten.Add(new Product()
            {
                Id = -1
            });
            Selected = Producten[Producten.Count() - 1];
        }
        private bool KanDelete()
        {
            if (Selected.Id > 0) return true;
            return false;
        }
        private void Terug()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new MenuVM());
            Product.DoValidation = false;
        }
        #endregion

        #region imagestuff
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

                    Selected.Image = data;
                    RaisePropertyChanged("Selected");
                }
                else
                {
                    Selected.Image = new byte[0];
                    RaisePropertyChanged("Selected");
                }
            }
        }
        #endregion
    }
}
