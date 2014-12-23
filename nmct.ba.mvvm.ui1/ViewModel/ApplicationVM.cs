using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
//using nmct.ba.navigatiedemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.mvvm.ui1.ViewModel
{
    public class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            Pages.Add(new PageOneVM());
            Pages.Add(new PageTwoVM());
            Pages.Add(new PageThreeVM());

            CurrentPage = Pages[0];
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
