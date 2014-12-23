using be.belgium.eid;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.uiKassa.helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;


namespace nmct.ba.cashlessproject.uiKassa.ViewModel
{
    class KassaVM : ObservableObject, Ipage
    {
        private BEID_ReaderContext reader;
        public string Name
        {
            get { return "Kassa"; }
        }
        public KassaVM()
        {
            GetCardReader();
            GetProducten();
            
        }

        #region Properties
        private List<Product> _producten;
        public List<Product> Producten
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
                    Producten = JsonConvert.DeserializeObject<List<Product>>(json);
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

                    appvm.ChangePage(new KassaVM());
                }
            }
        }
        private async void GetCategorien()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/product");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Producten = JsonConvert.DeserializeObject<List<Product>>(json);
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

                    appvm.ChangePage(new KassaVM());
                }
            }
        }
        #endregion

        #region CardReader
        private void AttachEvents()
        {
            try
            {
                BEID_ReaderSet.initSDK();
                BEID_ReaderSet readerSet = BEID_ReaderSet.instance();
                
                string readerName = readerSet.getReaderName(0);

                BEID_SetEventDelegate MyCallback = new BEID_SetEventDelegate(CallBack);
                reader.SetEventCallback(MyCallback, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(readerName));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void GetCardReader()
        {
            try
            {
                BEID_ReaderSet readerSet = BEID_ReaderSet.instance();
                var taskReader = Task.Factory.StartNew(() => readerSet.getReader());
                reader = await taskReader;
                AttachEvents();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kaardlezer: " + ex.Message);
            }
        }
        public void CallBack(int lRe, uint lState, System.IntPtr p)
        {
            Console.WriteLine("Kaart erin/eruit");
            GetInfo();
        }

        private void GetInfo()
        {
            try
            {
                BEID_EIDCard card = reader.getEIDCard();
                BEID_EId doc = card.getID();

                string code = doc.getNationalNumber();
                Console.WriteLine(code);
                }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }

}
