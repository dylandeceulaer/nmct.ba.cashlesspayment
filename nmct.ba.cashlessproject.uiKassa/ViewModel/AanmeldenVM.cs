﻿using be.belgium.eid;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace nmct.ba.cashlessproject.uiKassa.ViewModel
{
    class AanmeldenVM : ObservableObject, Ipage
    {
        private BEID_ReaderContext reader;
        private uint stop;
        private bool eerste = true;
        public string Name
        {
            get { return "Aanmelden"; }
            
        }
        public AanmeldenVM()
        {
            GetCardReader();
            LoginText = "Leg uw kaart op de cardreader om in the loggen.";
        }
        private string _loginText;

        public string LoginText
        {
            get { return _loginText; }
            set { _loginText = value; RaisePropertyChanged("LoginText"); }
        }

        #region CRUD
        private async void GetEmployee(string code)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                string uri = "http://localhost:5054/api/Employee" + "?code="+code;
                HttpResponseMessage res = await client.GetAsync(uri);
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Employee nieuw = JsonConvert.DeserializeObject<Employee>(json);
                    if (nieuw.Id == 0)
                    {
                        LoginText = "De huidige kaart is niet geldig om in te loggen.";
                    }
                    else
                    {
                        reader.StopEventCallback(stop);
                        ApplicationVM.CurrentEmployee = nieuw.Id;
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            
                            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                            appvm.ChangePage(new KassaVM());
                        });
                    }
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
                stop = reader.SetEventCallback(MyCallback, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(readerName));

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
            stop = lState;
            if (!eerste)
            {
                LoginText = "Uw kaart word gelezen, even geduld.";
            }
            else
            {
                eerste = false;
            }
            GetInfo();
        }

        private void GetInfo()
        {
            if (reader.isCardPresent())
            {
                try
                {
                    LoginText = "Uw kaart word gelezen, even geduld.";
                    BEID_EIDCard card = reader.getEIDCard();
                    BEID_EId doc = card.getID();

                    GetEmployee(doc.getNationalNumber());
                    
                }
                catch (BEID_Exception beex)
                {
                    LoginText = "Leg uw kaart op de cardreader om in the loggen.";
                    Console.WriteLine(beex.Message);
                }
                catch (Exception ex)
                {
                    LoginText = "Foutieve kaart.";
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                LoginText = "Leg uw kaart op de cardreader om in the loggen.";
            }
        }
        #endregion

    }
}