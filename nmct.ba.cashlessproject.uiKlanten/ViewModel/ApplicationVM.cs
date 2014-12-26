using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using be.belgium.eid;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.uiKlanten.ViewModel
{
    
    public class ApplicationVM : ObservableObject
    {
        public static TokenResponse token = null;
        public static int CurrentCustomer = -1;
        public static string Card;
        public ApplicationVM()
        {
            GetToken();
            CurrentPage = new AanmeldenVM();
        }
        private void GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:5054/token"));
            token = client.RequestResourceOwnerPasswordAsync(Properties.Settings.Default.Username, Properties.Settings.Default.Password).Result;
        }
        private Ipage _currentPage;
        public Ipage CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; RaisePropertyChanged("CurrentPage"); }
        }
        private List<Ipage> _pages;
        public List<Ipage> Pages
        {
            get { if (_pages == null)_pages = new List<Ipage>(); return _pages; }
            set { _pages = value; }
        }
        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<Ipage>(ChangePage); }
        }
        public void ChangePage(Ipage page)
        {
            CurrentPage = page;
        }
    }
}
