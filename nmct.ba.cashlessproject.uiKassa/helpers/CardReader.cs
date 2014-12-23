using be.belgium.eid;
using nmct.ba.cashlessproject.uiKassa.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.uiKassa.helpers
{
    public class CardReader
    {
        //public void GetCard(BEID_ReaderContext reader)
        //{
        //    try
        //    {
        //        BEID_ReaderSet readerSett = BEID_ReaderSet.instance();
        //        BEID_ReaderContext Reader = readerSett.getReader();

        //        if (Reader.isCardPresent())
        //        {
        //            BEID_EIDCard card = Reader.getEIDCard();
        //            BEID_EId doc = card.getID();

        //            string firstname = doc.getFirstName();
        //            Console.WriteLine("Firstname: " + firstname);
        //            string lastname = doc.getSurname();
        //            Console.WriteLine("Lastname: " + lastname);
        //            string birthdate = doc.getDateOfBirth();
        //            Console.WriteLine("Birthdate: " + birthdate);
        //        }

        //        BEID_ReaderSet.releaseSDK();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }

        //}
    }
}
