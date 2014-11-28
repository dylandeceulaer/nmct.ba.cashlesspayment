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
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged("Selected"); SetSelectedCategory(); }
        }
        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged("SelectedCategory"); }
        }

        private void SetSelectedCategory()
        {
            if (Selected != null && Selected.Category > 0)
            {
                var selected = from e in Categorien where e.Id == Selected.Category select e;
                SelectedCategory = selected.ToList()[0] as Category;
            }
            else SelectedCategory = null;
        }

        private async void GetProducten()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/product");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Producten = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                }
            }
        }
        private async void GetCategorien()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/category");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Categorien = JsonConvert.DeserializeObject<List<Category>>(json);
                }
            }
        }

    }
}
