using be.belgium.eid;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.uiKlanten.ViewModel
{
    class RegistreerVM : ObservableObject, Ipage
    {
        private BEID_ReaderContext reader;
        private uint stop;
        private string focused;
        public string Name
        {
            get { return "Registreer"; }

        }
        public RegistreerVM()
        {
            GetCardReader();
            Hoofding = "Registreer nieuwe kaart met kaartnummer: "+ ApplicationVM.Card;
        }

        private string _currentCard;

        public string CurrentCard
        {
            get { return _currentCard; }
            set { _currentCard = value; }
        }
        

        private Customer _klant;

        private string _hoofding;

        public string Hoofding
        {
            get { return _hoofding; }
            set { _hoofding = value; RaisePropertyChanged("Hoofding");  }
        }
        
        public Customer Klant
        {
            get
            {
                if (_klant == null)
                {
                    Klant = new Customer();
                    return _klant;
                }
                else return _klant;
            }
            set { _klant = value; RaisePropertyChanged("Klant"); RaisePropertyChanged("InsertCustomerCommand"); }
        }
        private string _alert;

        public string Alert
        {
            get { return _alert; }
            set { _alert = value; RaisePropertyChanged("Alert"); }
        }
        

        public ICommand InsertCustomerCommand
        {
            get {
                return new RelayCommand(InsertCustomer, Klant.IsValid);
            }
        }
        public ICommand WindowLoadedCommand
        {
            get { return new RelayCommand(WindowLoaded); }
        }

        public ICommand KeyboardCommand
        {
            get { return new RelayCommand<string>(KeyboardC); }
        }
        public ICommand GetFocusedElementCommand
        {
            get { return new RelayCommand<string>(GetFocusedElement); }
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

        private async void InsertCustomer()
        {
            using (HttpClient client = new HttpClient())
            {
                Klant.Card = ApplicationVM.Card;
                string json = JsonConvert.SerializeObject(Klant);
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.PostAsync("http://localhost:5054/api/customer", new StringContent(json, Encoding.UTF8, "application/json"));
                if (res.IsSuccessStatusCode)
                {
                    string jsonres = await res.Content.ReadAsStringAsync();
                    int result = JsonConvert.DeserializeObject<int>(jsonres);
                    if (result > 0)
                    {
                        Alert = "Uw kaart is succesvol geregistreerd. Neem de kaart van de kaartlezer om in te loggen.";
                    }
                    else
                    {
                        Alert = "Er is een fout opgetreden bij het registreren. Neem contact op met de verantwoordelijke.";
                    }
                }
            }
        }
             

        #endregion


        private void WindowLoaded()
        {
            Customer.DoValidation = true;
        }
        private void KeyboardC(string val)
        {
            switch (focused)
            {
                case "CustomerName":
                    if (val == "return") Klant.CustomerName = Klant.CustomerName.Remove(Klant.CustomerName.Length -1,1);
                    else Klant.CustomerName += val;
                    break;
                case "FirstName":
                    if (val == "return") Klant.FirstName = Klant.FirstName.Remove(Klant.FirstName.Length - 1, 1);
                    else Klant.FirstName += val;
                    break;
                case "Street":
                    if (val == "return") Klant.Street = Klant.Street.Remove(Klant.Street.Length - 1, 1);
                    else Klant.Street += val;
                    break;
                case "PostalCode":
                    if (val == "return") Klant.PostalCode = Klant.PostalCode.Remove(Klant.PostalCode.Length - 1, 1);
                    else Klant.PostalCode += val;
                    break;
                case "Number":
                    if (val == "return") Klant.Number = Klant.Number.Remove(Klant.Number.Length - 1, 1);
                    else Klant.Number += val;
                    break;
                case "City":
                    if (val == "return") Klant.City = Klant.City.Remove(Klant.City.Length - 1, 1);
                    else Klant.City += val;
                    break;
                default:
                    break;
            }
        }
        private void GetFocusedElement(string element)
        {
            focused = element;
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
                    ApplicationVM.CurrentCustomer = new Customer();
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                        Customer.DoValidation = false;
                        ApplicationVM.Card = "";
                        appvm.ChangePage(new AanmeldenVM());
                    });
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
                    ApplicationVM.CurrentCustomer = new Customer();
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
                    ApplicationVM.CurrentCustomer = new Customer();
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

