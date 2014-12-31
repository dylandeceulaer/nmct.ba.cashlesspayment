using be.belgium.eid;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.uiKlanten.ViewModel
{
    
    class KlantBeheerVM : ObservableObject, Ipage
    {
        private BEID_ReaderContext reader;
        private uint stop;
        public string Name
        {
            get { return "KlantBeheer"; }
        }
        public KlantBeheerVM()
        {
            GetCardReader();
            Klant = ApplicationVM.CurrentCustomer;
            Alert = "Voeg Biljetten toe om uw kaart op te waarderen.";

            Geld = new ObservableCollection<Money>();
            GetTotaal();
        }

        private Customer _klant;

        public Customer Klant
        {
            get { return _klant; }
            set { _klant = value; }
        }
        private string _alert;

        public string Alert
        {
            get { return _alert; }
            set { _alert = value; RaisePropertyChanged("Alert"); }
        }
        private ObservableCollection<Money> _geld;

        public ObservableCollection<Money> Geld
        {
            get { return _geld; }
            set { _geld = value; RaisePropertyChanged("Geld"); GetTotaal(); }
        }
        private int _totaal;

        public int Totaal
        {
            get { return _totaal; }
            set { _totaal = value; RaisePropertyChanged("Totaal"); }
        }

        public ICommand AddMoneyCommand
        {
            get { return new RelayCommand(AddMoney, KanUpdaten); }
        }

        public ICommand AddNotesCommand
        {
            get { return new RelayCommand<int>(AddNotes); }
        }
        #region CRUD

        private async void Log(Errorlog e)
        {
            using (HttpClient client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(e);
                client.SetBearerToken(ApplicationVM.token.AccessToken);

                HttpResponseMessage response = await client.PostAsync("http://localhost:5054/api/Errorlog", new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error has been logged");
                }
            }
        }

        private async void AddMoney()
        {
            using (HttpClient client = new HttpClient())
            {
                Klant.Balance += Totaal;
                string json = JsonConvert.SerializeObject(Klant);
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.PutAsync("http://localhost:5054/api/customer", new StringContent(json, Encoding.UTF8, "application/json"));
                if (res.IsSuccessStatusCode)
                {
                    string jsonres = await res.Content.ReadAsStringAsync();
                    int result = JsonConvert.DeserializeObject<int>(jsonres);
                    if (result == 1)
                    {
                        Geld = new ObservableCollection<Money>();
                        Alert = "Uw balans is bijgewerkt, u kan uw kaart nu wegnemen.";

                    }
                    else
                    {
                        Alert = "Fout bij het opslaan, neem contact op met de beheerder.";
                    }
                }
            }
        }

        #endregion
        

        private void GetTotaal()
        {
            Totaal = 0;
            foreach (Money g in Geld)
            {
                Totaal = Totaal + (g.count * g.value);
            }
        }

        private bool KanUpdaten()
        {
            if (Geld.Count == 0) return false;
            else return true;
        }

        private void AddNotes(int waarde)
        {
            var g = from e in Geld where e.value == waarde select e;
            List<Money> lijstProds = g.ToList();

            if (lijstProds.Count == 0)
            {
                Geld.Add(new Money()
                {
                    count = 1,
                    value = waarde
                });
                //b.PropertyChanged += bestelling_PropertyChanged;
                GetTotaal();
            }
            else
            {
                Geld.First(item => item.value == waarde).count++;
                GetTotaal();
            }

        }

        #region CardReader
        private void AttachEvents()
        {
            try
            {
                BEID_ReaderSet.initSDK();
                BEID_ReaderSet readerSet = BEID_ReaderSet.instance();

                string readerName = readerSet.getReaderName(0);

                BEID_SetEventDelegate MyCallback = new BEID_SetEventDelegate(CallBack);
                stop = reader.SetEventCallback(MyCallback, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(readerName));
                
            }
            catch (BEID_Exception beex)
            {
                Log(new Errorlog()
                {
                    Message = beex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = beex.StackTrace
                });
                Console.WriteLine(beex.Message);
            }
            catch (Exception ex)
            {
                Log(new Errorlog()
                {
                    Message = ex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = ex.StackTrace
                });
                Console.WriteLine(ex.Message);
            }
        }
        [HandleProcessCorruptedStateExceptions] 
        private async void GetCardReader()
        {
            try
            {
                BEID_ReaderSet readerSet = BEID_ReaderSet.instance();
                var taskReader = Task.Factory.StartNew(() => readerSet.getReader());
                reader = await taskReader;
                AttachEvents();
            }
            catch (BEID_Exception ex)
            {
                Log(new Errorlog()
                {
                    Message = ex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = ex.StackTrace
                });
                Console.WriteLine("Kaardlezer: " + ex.Message);
            }

            catch (Exception ex)
            {
                Log(new Errorlog()
                {
                    Message = ex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = ex.StackTrace
                });
                Console.WriteLine("Kaardlezer: " + ex.Message);
            }
        }
        public void CallBack(int lRe, uint lState, System.IntPtr p)
        {
            GetInfo();
        }

        private void GetInfo()
        {
            try
            {
                if (!reader.isCardPresent())
                {
                    BEID_ReaderSet.releaseSDK();
                    ApplicationVM.CurrentCustomer =  new Customer() ;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                        Customer.DoValidation = false;
                        ApplicationVM.Card = "";
                        appvm.ChangePage(new AanmeldenVM());
                    });
                }
                else
                {
                    
                }
            }
            catch (BEID_Exception beex)
            {
                Log(new Errorlog()
                {
                    Message = beex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = beex.StackTrace
                });
                Console.WriteLine(beex.Message);
                BEID_ReaderSet.releaseSDK();
                ApplicationVM.CurrentCustomer = new Customer() ;
                App.Current.Dispatcher.Invoke(() =>
                {
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    Customer.DoValidation = false;
                    ApplicationVM.Card = "";
                    appvm.ChangePage(new AanmeldenVM());
                });

            }

            catch (Exception ex)
            {
                Log(new Errorlog()
                {
                    Message = ex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = ex.StackTrace
                });
                Console.WriteLine(ex.Message);
                BEID_ReaderSet.releaseSDK();
                ApplicationVM.CurrentCustomer =  new Customer() ;
                App.Current.Dispatcher.Invoke(() =>
                {
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    Customer.DoValidation = false;
                    ApplicationVM.Card = "";
                    appvm.ChangePage(new AanmeldenVM());
                });

            }
        }

        #endregion
    }
}
