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
    class StatestiekenVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Statestieken"; }
        }
        public StatestiekenVM()
        {
            GetKassas();
            GetProducten();
            GetStatestieken();
        }
        private List<Sales> _statestieken;

        public List<Sales> Statestieken
        {
            get { return _statestieken; }
            set { _statestieken = value; RaisePropertyChanged("Statestieken"); }
        }
        private async void GetStatestieken()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/Sales");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Statestieken = JsonConvert.DeserializeObject<List<Sales>>(json);
                }
            }
        }
        private double _stuks;

        public double Stuks
        {
            get { return _stuks; }
            set { _stuks = value; RaisePropertyChanged("Stuks"); }
        }

        private double _opbrengst;

        public double Opbrengst
        {
            get { return _stuks; }
            set { _stuks = value; RaisePropertyChanged("Opbrengst"); }
        }

        private void ToonStatestieken()
        {
            if (Selected is Register)
            {
                Register reg = Selected as Register;
                var res = from e in Statestieken where e.RegisterID == reg.Id select e;
                List<Sales> lstSales = res.ToList();
                Stuks = (from sale in lstSales select sale.Amound).Sum();

                res = from e in Statestieken where e.RegisterID == reg.Id select e;
                lstSales = res.ToList();
                Opbrengst = Math.Round((from sale in lstSales select sale.TotalPrice).Sum(),2);
            }
            if (Selected is Product)
            {
                Product reg = Selected as Product;
                var res = from e in Statestieken where e.ProductID == reg.Id select e;
                List<Sales> lstSales = res.ToList();
                Stuks = (from sale in lstSales select sale.Amound).Sum();

                res = from e in Statestieken where e.ProductID == reg.Id select e;
                lstSales = res.ToList();
                Opbrengst = Math.Round((from sale in lstSales select sale.TotalPrice).Sum(), 2);
            }
        }

        private List<Register> _kassas;

        public List<Register> Kassas
        {
            get { return _kassas; }
            set { _kassas = value; RaisePropertyChanged("Kassas"); }
        }
        private List<Product> _producten;

        public List<Product> Producten
        {
            get { return _producten; }
            set { _producten = value; RaisePropertyChanged("Producten"); }
        }
        private async void GetProducten()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/product");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Producten = JsonConvert.DeserializeObject<List<Product>>(json);
                }
            }
        }
        private async void GetKassas()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/register");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Kassas = JsonConvert.DeserializeObject<List<Register>>(json);
                }
            }
        }
        private object _selected;

        public object Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged("Selected"); ToonStatestieken(); }
        }
        
    }
}
