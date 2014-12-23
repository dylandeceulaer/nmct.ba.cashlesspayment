using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.uiKassa.helpers
{
    class CardReaderOud
    {
        //private class ReaderRef
        //{
        //    public BEID_ReaderContext reader;
        //    public uint eventHandle;
        //    public IntPtr ptr;
        //    public uint cardId;
        //}
        //private ReaderRef readerRef;
        //private BEID_ReaderContext reader;
        //private BEID_ReaderSet readerSet;
        //private void AttachEvents()
        //{
        //    try
        //    {
        //        BEID_ReaderSet readerSet = BEID_ReaderSet.instance();
        //        reader = readerSet.getReader();
        //        string readerName = readerSet.getReaderName(0);

        //        readerRef = new ReaderRef();
        //        readerRef.reader = reader;
        //        readerRef.ptr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(readerName);

        //        BEID_SetEventDelegate MyCallback = new BEID_SetEventDelegate(CallBack);
        //        readerRef.eventHandle = reader.SetEventCallback(MyCallback, readerRef.ptr);


        //    }
        //    catch (Exception ex) {


        //    }
        //}
        //private void AttachEvents()
        //{
        //    try
        //    {
        //        BEID_ReaderSet.initSDK();
        //        readerSet = BEID_ReaderSet.instance();
        //        reader = readerSet.getReader();
        //        string readerName = readerSet.getReaderName(0);


        //        //readerRef = new ReaderRef();
        //        //readerRef.reader = reader;
        //        //readerRef.ptr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(readerName);

        //        BEID_SetEventDelegate MyCallback = new BEID_SetEventDelegate(CallBack);
        //        reader.SetEventCallback(MyCallback, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(readerName));


        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}





        //    InitCardReaderTimer();
        //}

        //private void InitCardReaderTimer()
        //{
            
        //    System.Timers.Timer aTimer = new System.Timers.Timer();
        //    aTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        //    aTimer.Interval = 10000;
        //    aTimer.Enabled = true;
        //}



        //void timer_Elapsed(object sender, ElapsedEventArgs e)
        //{

        //    card();
        //}
        //private void card()
        //{
        //    if (Reader != null)
        //    {
        //        try
        //        {

        //            if (Reader.isCardPresent())
        //            {
        //                BEID_EIDCard card = Reader.getEIDCard();
        //                BEID_EId doc = card.getID();
        //                string code = doc.getNationalNumber();
        //                Console.WriteLine(code);
        //            }
        //            BEID_ReaderSet.releaseSDK();

        //        }
        //        catch (System.AccessViolationException ex) { }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("KAARTLEZER:" + ex);
        //        }
        //    }
        //}

        //private async void GetCardReader()
        //{
        //    try
        //    {
        //        BEID_ReaderSet readerSet = BEID_ReaderSet.instance();
        //        var reader = Task.Factory.StartNew(() => readerSet.getReader());
        //        Reader = await reader;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Kaardlezer: " + ex.Message);
        //    }
        //}
    }
}
