using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Bestelling : ObservableObject
    {
        public int Id { get; set; }

        private int _aantal;

        public int Aantal
        {
            get { return _aantal; }
            set { _aantal = value; RaisePropertyChanged("Aantal"); }
        }
        
        public string Naam { get; set; }
        public float prijs { get; set; }
    }
}
