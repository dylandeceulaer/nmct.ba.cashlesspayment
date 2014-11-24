using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            Pages.Add(new MenuVM());
            Pages.Add(new AanmeldenVM());
            Pages.Add(new AccountVM());
            Pages.Add(new ProductenVM());
            Pages.Add(new MedewerkersVM());
            Pages.Add(new KassasVM());
            Pages.Add(new StatestiekenVM());

            CurrentPage = Pages[1];
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
        private void ChangePage(Ipage page)
        {
            CurrentPage = page;
        }
    }
}
