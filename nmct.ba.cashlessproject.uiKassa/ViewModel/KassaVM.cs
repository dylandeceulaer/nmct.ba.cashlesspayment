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
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;


namespace nmct.ba.cashlessproject.uiKassa.ViewModel
{
    class KassaVM : ObservableObject, Ipage
    {
        
        public Employee CurrentEmployee { get; set; }
        private bool eerste = true;
        private bool IsTransactie = false;
        private BEID_ReaderContext reader;
        private bool IsKaartIn = false;
        public string Name
        {
            get { return "Kassa"; }
        }
        public KassaVM()
        {
            GetProducten();
            GetCardReader();
            KlantNaam = "Geen Kaart";
            KnopText = "Afrekenen";
            TotaalBestelling = new ObservableCollection<Bestelling>();
        }

        void b_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Properties
        private List<Product> _producten;
        public List<Product> Producten
        {
            get { return _producten; }
            set { _producten = value; RaisePropertyChanged("Producten"); }
        }
        private List<Product> _selectedProducts;
	    public List<Product> SelectedProducts
	    {
		    get { return _selectedProducts;}
		    set { _selectedProducts = value; RaisePropertyChanged("SelectedProducts");}
	    }
        private List<Category> _categorien;
        public List<Category> Categorien
        {
            get { return _categorien; }
            set { _categorien = value; RaisePropertyChanged("Categorien"); }
        }
        private Customer _currentKlant;

        public Customer CurrentKlant
        {
            get { return _currentKlant; }
            set { _currentKlant = value; RaisePropertyChanged("CurrentKlant"); }
        }
        private string _klantNaam;
        public string KlantNaam
        {
            get { return _klantNaam; }
            set { _klantNaam = value; RaisePropertyChanged("KlantNaam"); }
        }
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged("SelectedCategory"); GetSelectedProducts(); }
        }
        private ObservableCollection<Bestelling> _totaalBestelling;
        public ObservableCollection<Bestelling> TotaalBestelling
        {
            get { return _totaalBestelling; }
            set { _totaalBestelling = value; RaisePropertyChanged("TotaalBestelling"); BerekenTotaal(); }
        }
        private Bestelling _selectedBestelling;
        public Bestelling SelectedBestelling
        {
            get { return _selectedBestelling; }
            set { _selectedBestelling = value; RaisePropertyChanged("SelectedBestelling"); }
        }
        private float _totaalPrijs;
        public float TotaalPrijs
        {
            get { return _totaalPrijs; }
            set { _totaalPrijs = value; RaisePropertyChanged("TotaalPrijs"); }
        }
        private string _knopText;

        public string KnopText
        {
            get { return _knopText; }
            set { _knopText = value; RaisePropertyChanged("KnopText"); }
        }
        
        #endregion

        #region Icommands
        public ICommand BestelCommand
        {
            get { return new RelayCommand<int>(Bestel,KanBestellen); }
        }
        public ICommand VerhoogCommand
        {
            get { return new RelayCommand(Verhoog, KanVeranderen); }
        }
        public ICommand VerlaagCommand
        {
            get { return new RelayCommand(Verlaag, KanVeranderen); }
        }
        public ICommand DeleteBestellingCommand
        {
            get { return new RelayCommand<int>(DeleteBestelling); }
        }
        public ICommand AfrekenenCommand
        {
            get { return new RelayCommand(Afrekenen,KanAfrekenen); }
        }
        #endregion

        #region CRUD
        private async void GetProducten()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/product");
                    if (res.IsSuccessStatusCode)
                    {
                        string json = await res.Content.ReadAsStringAsync();
                        Producten = JsonConvert.DeserializeObject<List<Product>>(json);
                    }
                }
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/Category");
                    if (res.IsSuccessStatusCode)
                    {
                        string json = await res.Content.ReadAsStringAsync();
                        Categorien = JsonConvert.DeserializeObject<List<Category>>(json);
                        SelectedCategory = Categorien[0];
                    }
                }
            }
            catch(Exception ex)
            {
                Log(new Errorlog()
                {
                    Message = ex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = ex.StackTrace
                });
            }
        }
        private async void GetCategorien()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/Category");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Categorien = JsonConvert.DeserializeObject<List<Category>>(json);
                    SelectedCategory = Categorien[0];
                }
            }
        }
        private async void GetKlant(string code)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    string uri = "http://localhost:5054/api/Customer" + "?code=" + code;
                    HttpResponseMessage res = await client.GetAsync(uri);
                    if (res.IsSuccessStatusCode)
                    {
                        string json = await res.Content.ReadAsStringAsync();
                        Customer nieuw = JsonConvert.DeserializeObject<Customer>(json);

                        if (nieuw.Id == 0)
                        {
                            KlantNaam = "Niet Geldig.";
                            IsKaartIn = false;
                        }
                        else
                        {
                            CurrentKlant = nieuw;
                            KlantNaam = nieuw.FirstName + " " + nieuw.CustomerName;
                            RaisePropertyChanged("BestelCommand");
                            IsKaartIn = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log(new Errorlog()
                {
                    Message = ex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = ex.StackTrace
                });
            }
        }
        private async void DoAfrekening()
        {
            try
            {
                string kassaID = Properties.Settings.Default.ID;

                using (HttpClient client = new HttpClient())
                {
                    Purchase p = new Purchase()
                    {
                        Bestellingen = TotaalBestelling.ToList(),
                        Customer = CurrentKlant,
                        TotaalPrijs = TotaalPrijs,
                        KassaID = int.Parse(kassaID)
                    };

                    string json = JsonConvert.SerializeObject(p);
                    client.SetBearerToken(ApplicationVM.token.AccessToken);

                    HttpResponseMessage response = await client.PostAsync("http://localhost:5054/api/Sales", new StringContent(json, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonresponse = await response.Content.ReadAsStringAsync();
                        int result = JsonConvert.DeserializeObject<int>(jsonresponse);
                        if (result > 1)
                        {
                            CurrentKlant.Balance -= float.Parse(Math.Round(Convert.ToDouble(TotaalPrijs), 2).ToString());
                            TotaalBestelling = new ObservableCollection<Bestelling>();
                            IsTransactie = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(new Errorlog()
                {
                    Message = ex.Message,
                    RegisterID = int.Parse(Properties.Settings.Default.ID),
                    Stacktrace = ex.StackTrace
                });
            }
        }

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

        #endregion

        #region etc
        private void GetSelectedProducts()
        {
            if (Producten != null)
            {
                var res = from e in Producten where e.Category == SelectedCategory.Id select e;
                SelectedProducts = res.ToList();
            }
        }

        

        private void Bestel(int id)
        {
            var prods = from e in TotaalBestelling where e.Id == id select e;
            List<Bestelling> lijstProds = prods.ToList();

            if (lijstProds.Count == 0)
            {
                var prod = (from e in Producten where e.Id == id select e).FirstOrDefault();
                Product product = prod;
                Bestelling b = new Bestelling()
                {
                    Id = id,
                    Naam = product.ProductName,
                    Aantal = 1,
                    prijs = product.Price
                };
                b.PropertyChanged += bestelling_PropertyChanged;
                TotaalBestelling.Add(b);
                SelectedBestelling = b;
                BerekenTotaal();
            }
            else
            {
                TotaalBestelling.First(item => item.Id == id).Aantal++;
                SelectedBestelling = TotaalBestelling.First(item => item.Id == id);
                RaisePropertyChanged("TotaalBestelling");
            }

        }
        void bestelling_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            BerekenTotaal();
        }
        private void DeleteBestelling(int id)
        {
            TotaalBestelling.Remove(TotaalBestelling.First(item => item.Id == id));
            BerekenTotaal();
        }

        private bool KanVeranderen()
        {
            if (SelectedBestelling == null) return false;
            return true;
        }
            
        private bool KanBestellen(int id)
        {
            return IsKaartIn;
        }
        private void Afrekenen()
        {
            if (!IsTransactie)
            {
                DoAfrekening();
                IsTransactie = true;
            }

        }
        private bool KanAfrekenen()
        {
            if (CurrentKlant != null)
            {
                if (TotaalPrijs == 0)
                {
                    KnopText = "Afrekenen";
                    return false;
                    
                }
                if (TotaalPrijs < CurrentKlant.Balance)
                {
                    KnopText = "Afrekenen";
                    return true;
                }
                else
                {
                    KnopText = "Saldo ontoereikend";
                    return false;
                }
            }
            else return false;
        }

        private void Verhoog()
        {
            if (SelectedBestelling != null)  SelectedBestelling.Aantal++;
            BerekenTotaal();
        }
        private void Verlaag()
        {
            if (SelectedBestelling != null)
            {
                SelectedBestelling.Aantal--;
                if (SelectedBestelling.Aantal == 0)
                {
                    TotaalBestelling.Remove(SelectedBestelling);
                    BerekenTotaal();
                    if (TotaalBestelling.Count > 0) SelectedBestelling = TotaalBestelling[TotaalBestelling.Count - 1];
                }
            }
        }
        private void BerekenTotaal()
        {
            var totaal = (from e in TotaalBestelling select e.prijs * e.Aantal).Sum();
            TotaalPrijs = (float)totaal;
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
        private async void GetCardReader()
        {
            try
            {
                BEID_ReaderSet readerSet = BEID_ReaderSet.instance();
                var taskReader = Task.Factory.StartNew(() => readerSet.getReader());
                reader = await taskReader;
                AttachEvents();
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
                Console.WriteLine("Kaardlezer: " + ex.Message);
            }
        }
        public void CallBack(int lRe, uint lState, System.IntPtr p)
        {
            if (!eerste)
            {
                KlantNaam = "Kaart lezen";
                IsKaartIn = false;
                TotaalBestelling = new ObservableCollection<Bestelling>();
            }
            else
            {
                eerste = false;
            }
            GetInfo();
        }

        private void GetInfo()
        {
            try
            {
                if (reader.isCardPresent())
                {

                    KlantNaam = "Kaart lezen";
                    BEID_EIDCard card = reader.getEIDCard();
                    BEID_EId doc = card.getID();
                    GetKlant(doc.getNationalNumber());

                }
                else
                {
                    KlantNaam = "Geen kaart";
                    IsKaartIn = false;
                    TotaalBestelling = new ObservableCollection<Bestelling>();
                    CurrentKlant = null;
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
                KlantNaam = "Geen kaart";
                IsKaartIn = false;
                TotaalBestelling = new ObservableCollection<Bestelling>();
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
                KlantNaam = "Geen kaart";
                IsKaartIn = false;
                TotaalBestelling = new ObservableCollection<Bestelling>();
                Console.WriteLine(ex.Message);
            }
        }
        
        #endregion
    }

}
