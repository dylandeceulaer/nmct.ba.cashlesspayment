using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    class AccountVM : ObservableObject, Ipage
    {
        

        public string Name
        {
            get { return "Account"; }
        }
        public ICommand AfmeldenCommand
        {
            get { return new RelayCommand(Afmelden); }
        }

        private void Afmelden()
        {
            ApplicationVM.token = null;
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new AanmeldenVM());
        }
        

    }
}
