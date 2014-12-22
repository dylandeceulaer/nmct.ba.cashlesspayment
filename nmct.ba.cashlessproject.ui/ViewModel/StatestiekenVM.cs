using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    class StatestiekenVM : ObservableObject, Ipage
    {
        public string Name
        {
            get { return "Statestieken"; }
        }
        public StatestiekenVM()
        {
            GetKassas();
            GetProducten();
            GetStatestieken();
        }

        #region properties
        private List<Sales> _statestieken;
        public List<Sales> Statestieken
        {
            get { return _statestieken; }
            set { _statestieken = value; RaisePropertyChanged("Statestieken"); }
        }
        private double _stuks;
        public double Stuks
        {
            get { return _stuks; }
            set { _stuks = value; RaisePropertyChanged("Stuks"); }
        }

        private double _opbrengst;
        public double Opbrengst
        {
            get { return _opbrengst; }
            set { _opbrengst = value; RaisePropertyChanged("Opbrengst"); }
        }

        private List<Sales> _geselecteerdeSales;
        public List<Sales> GeselecteerdeSales
        {
            get { return _geselecteerdeSales; }
            set { _geselecteerdeSales = value; RaisePropertyChanged("GeselecteerdeSales"); }
        }
        private List<Register> _kassas;
        public List<Register> Kassas
        {
            get { return _kassas; }
            set { _kassas = value; RaisePropertyChanged("Kassas"); }
        }
        private List<Product> _producten;
        public List<Product> Producten
        {
            get { return _producten; }
            set { _producten = value; RaisePropertyChanged("Producten"); }
        }
        private object _selected;
        public object Selected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged("Selected"); ToonStatestieken(); }
        }
        private bool _totaal;
        public bool Totaal
        {
            get { return _totaal; }
            set { _totaal = value; RaisePropertyChanged("Totaal"); ToonStatestieken(); }
        }
        private DateTime _datumVan;
        public DateTime DatumVan
        {
            get { return _datumVan; }
            set { _datumVan = value; RaisePropertyChanged("DatumVan"); ToonStatestieken(); }
        }
        private DateTime _datumTot;
        public DateTime DatumTot
        {
            get { return _datumTot; }
            set { _datumTot = value; RaisePropertyChanged("DatumTot"); ToonStatestieken(); }
        }
        #endregion

        #region Icommands
        public ICommand ExportCommand
        {
            get { return new RelayCommand(Export); }
        }
        public ICommand TerugCommand
        {
            get { return new RelayCommand(Terug); }
        }
        #endregion

        #region CRUD
        private async void GetStatestieken()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/Sales");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Statestieken = JsonConvert.DeserializeObject<List<Sales>>(json);
                }
            }
            var datemin = (from e in Statestieken select e.TimeStamp).Min();
            DatumVan = (DateTime)datemin;
            var datemax = (from e in Statestieken select e.TimeStamp).Max();
            DatumTot = (DateTime)datemax;
        }
        private async void GetProducten()
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
        }
        private async void GetKassas()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage res = await client.GetAsync("http://localhost:5054/api/register");
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    Kassas = JsonConvert.DeserializeObject<List<Register>>(json);
                }
            }
        }
        #endregion

        #region Statestieken
        private void ToonStatestieken()
        {
            if (Totaal)
            {
                var res = from e in Statestieken where e.TimeStamp >= DatumVan && e.TimeStamp <= DatumTot select e;
                GeselecteerdeSales = res.ToList();
                Stuks = (from sale in GeselecteerdeSales select sale.Amound).Sum();
                Opbrengst = Math.Round((from sale in GeselecteerdeSales select sale.TotalPrice).Sum(), 2);
            }
            else
            {
                if (Selected is Register)
                {
                    Register reg = Selected as Register;
                    var res = from e in Statestieken where e.RegisterID == reg.Id && e.TimeStamp >= DatumVan && e.TimeStamp <= DatumTot select e;
                    GeselecteerdeSales = res.ToList();
                    Stuks = (from sale in GeselecteerdeSales select sale.Amound).Sum();
                    Opbrengst = Math.Round((from sale in GeselecteerdeSales select sale.TotalPrice).Sum(), 2);
                }
                if (Selected is Product)
                {
                    Product reg = Selected as Product;
                    var res = from e in Statestieken where e.ProductID == reg.Id && e.TimeStamp >= DatumVan && e.TimeStamp <= DatumTot select e;
                    GeselecteerdeSales = res.ToList();
                    Stuks = (from sale in GeselecteerdeSales select sale.Amound).Sum();
                    Opbrengst = Math.Round((from sale in GeselecteerdeSales select sale.TotalPrice).Sum(), 2);
                }
            }
        }
        private void Export()
        {
            string FileName;
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Excel Files|*.xlsx";
            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    if (!fileDialog.FileName.EndsWith(".xlsx")) FileName = fileDialog.FileName + ".xlsx";
                    else FileName = fileDialog.FileName;
                    SpreadsheetDocument doc = SpreadsheetDocument.Create(FileName, SpreadsheetDocumentType.Workbook);

                    WorkbookPart wbp = doc.AddWorkbookPart();
                    wbp.Workbook = new Workbook();

                    WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
                    SheetData data = new SheetData();
                    wsp.Worksheet = new Worksheet(data);

                    Sheets sheets = doc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet()
                    {
                        Id = doc.WorkbookPart.GetIdOfPart(wsp),
                        SheetId = 1,
                        Name = "verkoop"
                    };
                    sheets.Append(sheet);

                    Row header = new Row() { RowIndex = 1 };
                    Cell date = new Cell() { CellReference = "A1", DataType = CellValues.String, CellValue = new CellValue("Datum") };
                    Cell register = new Cell() { CellReference = "B1", DataType = CellValues.String, CellValue = new CellValue("Kassa") };
                    Cell product = new Cell() { CellReference = "C1", DataType = CellValues.String, CellValue = new CellValue("Product") };
                    Cell amount = new Cell() { CellReference = "D1", DataType = CellValues.String, CellValue = new CellValue("Stuks") };
                    Cell total = new Cell() { CellReference = "E1", DataType = CellValues.String, CellValue = new CellValue("Totaalprijs") };

                    header.Append(date, register, product, amount, total);
                    data.Append(header);

                    UInt32 teller = 2;
                    foreach (Sales sale in GeselecteerdeSales)
                    {
                        var productSelect = (from e in Producten where e.Id == sale.ProductID select e).ToList();
                        string productName = productSelect[0].ProductName;

                        var kassaSelect = (from e in Kassas where e.Id == sale.RegisterID select e).ToList();
                        string kassaName = kassaSelect[0].RegisterName;

                        Row rij = new Row() { RowIndex = teller };
                        Cell datum = new Cell() { CellReference = "A" + teller, DataType = CellValues.String, CellValue = new CellValue(sale.TimeStamp.ToShortDateString()) };
                        Cell kassa = new Cell() { CellReference = "B" + teller, DataType = CellValues.String, CellValue = new CellValue(kassaName) };
                        Cell produkt = new Cell() { CellReference = "C" + teller, DataType = CellValues.String, CellValue = new CellValue(productName) };
                        Cell stuks = new Cell() { CellReference = "D" + teller, DataType = CellValues.String, CellValue = new CellValue(sale.Amound.ToString()) };
                        Cell totaal = new Cell() { CellReference = "E" + teller, DataType = CellValues.String, CellValue = new CellValue(sale.TotalPrice.ToString()) };

                        rij.Append(datum, kassa, produkt, stuks, totaal);
                        data.Append(rij);

                        teller++;
                    }

                    Row eind = new Row() { RowIndex = teller + 1 };
                    Cell totaalText = new Cell() { CellReference = "A" + (teller + 1), DataType = CellValues.String, CellValue = new CellValue("Totaal") };
                    Cell totaalStuks = new Cell() { CellReference = "D" + (teller + 1), DataType = CellValues.String, CellValue = new CellValue(Stuks.ToString()) };
                    Cell totaalTotaal = new Cell() { CellReference = "E" + (teller + 1), DataType = CellValues.String, CellValue = new CellValue(Opbrengst.ToString()) };

                    eind.Append(totaalText, totaalStuks, totaalTotaal);
                    data.Append(eind);

                    wbp.Workbook.Save();
                    doc.Close();
                }
                catch (Exception ex)
                {
                    MessageBoxResult msg = MessageBox.Show("Fout bij het wegschrijven van het bestand.", "Fout", MessageBoxButton.OK);

                }
            }
        }
        #endregion

        #region etc
        private void Terug()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new MenuVM());
        }
        #endregion
    }
}
