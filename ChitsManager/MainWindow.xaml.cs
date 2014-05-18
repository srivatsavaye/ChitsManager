using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using ChitsManager.DataAccess;
using ChitsManager.Objects;

namespace ChitsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public MainWindow()
        {
            #region Auctions
            _dicAuctions = DataConversion.ConvertAuctionData();
            if (AuctionSelectedChit != null)
                _auctions = (_dicAuctions.First(c => c.Key.ChitId == AuctionSelectedChit.ChitId)).Value;
            else
                _auctions = new List<Auction>();
            #endregion

            #region Payments
            _paymentChits = DataConversion.GetChits();
            if (PaymentChits.Count > 0)
            {
                _paymentSelectedChit = PaymentChits[0];
                //_payments = DataConversion.ConvertPaymentData(PaymentSelectedChit.ChitId);
                //if (Months.Count > 0)
                //    _selectedMonth = Months[0];
            }

            #endregion

            InitializeComponent();

            _warningDictionary.Add("MustBeANumber", "Only Number needs to be entered in this cell");
            _warningDictionary.Add("OutOfRange", "Number can only be between {0} and {1} and not include {2}");
        }

        #region Common

        #region Properties

        #region Constants

        private const string _webTextMustBeANumber = "MustBeANumber";
        private const string _webTextOutOfRange = "OutOfRange";

        #endregion

        private Dictionary<string, string> _warningDictionary = new Dictionary<string, string>();
        #endregion

        #region Helpers

        private bool IsNumber(string number)
        {
            if (number == "0")
                return true;
            int intNumber;
            int.TryParse(number, out intNumber);
            if (intNumber == 0)
            {
                MessageBox.Show(_warningDictionary[_webTextMustBeANumber]);
                return false;
            }
            return true;
        }

        private bool NumberInRange(string number, int minNumber, int maxNumber, List<int> existingNumbers)
        {
            bool ret = false;
            NumberRange numberRange = NumberRange.OTHER;
            if (IsNumber(number))
            {
                //if (number != "")
                //{
                int intNumber = int.Parse(number);
                if (intNumber < minNumber)
                {
                    numberRange = NumberRange.LESSTHANMINIMUM;
                }
                else if (intNumber > maxNumber)
                {
                    numberRange = NumberRange.GREATERTHANMAXIMUM;
                }
                else if (existingNumbers.Contains(intNumber))
                {
                    numberRange = NumberRange.ALREADYEXISTS;
                }
                //}
                //else
                //{
                //    return true;
                //}
            }
            else
            {
                return false;
            }
            string message = "";
            if (numberRange != NumberRange.OTHER)
                MessageBox.Show(string.Format(_warningDictionary[_webTextOutOfRange], minNumber, maxNumber, string.Join(",", existingNumbers.Select(n => n.ToString()).ToArray())));
            else
                ret = true;

            //switch(ret)
            //{
            //    case NumberRange.LESSTHANMINIMUM;
            //        message = 
            //}

            return ret;
        }

        #endregion

        #region Other

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        #endregion

        #region Auctions Region

        #region Properties

        #region Constants

        private const int _auctionIdColumnNumber = 0;
        private const int _customeridColumnNumber = 1;
        private const int _customerNameColumnNumber = 2;
        private const int _monthColumnNumber = 3;
        private const int _auctionAmountColumnNumber = 4;
        private const int _isDirtyColumnNumber = 5;


        #endregion

        private Chit _auctionSelectedChit;
        private Dictionary<Chit, List<Auction>> _dicAuctions = new Dictionary<Chit, List<Auction>>();
        private List<Auction> _auctions;

        public List<Auction> Auctions
        {
            get
            {
                return _auctions;
            }
            set
            {
                if (value != null)
                {
                    _auctions = value;
                }
            }
        }

        public Chit AuctionSelectedChit
        {
            get
            {
                if (_auctionSelectedChit == null)
                    _auctionSelectedChit = _dicAuctions.Select(c => c.Key).Distinct().First();
                return _auctionSelectedChit;
            }
            set
            {
                if (value != null)
                {
                    _auctionSelectedChit = value;
                }
                OnPropertyChanged("Auctions");
                dgAuctions_Loaded(new object(), new RoutedEventArgs());
            }
        }

        public List<Chit> AuctionChits
        {
            get
            {
                return _dicAuctions.Select(c => c.Key).Distinct().ToList();
            }
        }

        #endregion Properties

        #region Event Handlers

        private void dgAuctions_LostFocus(object sender, RoutedEventArgs e)
        {
            Control ctrl = FocusManager.GetFocusedElement(this) as Control;
            if (ctrl.Parent != null && ctrl.Parent.GetType() != typeof(DataGridCell))
                DataConversion.UpdateAuctionData(Auctions);
        }

        private void dgAuctions_Loaded(object sender, RoutedEventArgs e)
        {
            dgAuctions_SetGridColumnProperties();
        }

        private void dgAuctions_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            List<Auction> listAuctions = new List<Auction>();
            TextBox tb = new TextBox();

            if (e.EditingElement.DataContext != null)
            {
                ((ChitsManager.Objects.Auction)e.EditingElement.DataContext).IsDirty = true;
                tb = (TextBox)e.EditingElement;
            }

            switch (e.Column.DisplayIndex)
            {
                case _monthColumnNumber:
                    //string number = tb.Text;
                    //if (!NumberInRange(number, 1, _auctionSelectedChit.NumberOfMonths, Auctions.Where(i => i.Month != 0).Select(a => a.Month).ToList()))
                    //{
                    //    tb.Text = "0";
                    //    e.EditingElement.Focus();
                    //    e.Cancel = true;
                    //    //dgAuctions.CurrentCell = new DataGridCellInfo(dgAuctions.Items[e.Row.AlternationIndex], dgAuctions.Columns[e.Column.DisplayIndex]);
                    //    //dgAuctions.BeginEdit();
                    //    return;
                    //}
                    //else
                    //{

                    //}
                    break;
                case _auctionAmountColumnNumber:
                    if (IsNumber(((TextBox)e.EditingElement).Text))
                    {
                        ((ChitsManager.Objects.Auction)e.EditingElement.DataContext).Month = GetNextMonth();
                        ((Auction)dgAuctions.Items[e.Row.AlternationIndex]).Month = GetNextMonth();
                        //Auctions[e.Row.AlternationIndex].Month = GetNextMonth();
                        //dgAuctions.Items.Refresh();
                        //OnPropertyChanged("Auctions");
                    }
                    else
                    {
                        tb.Text = "0";
                        e.EditingElement.Focus();
                        e.Cancel = true;
                    }
                    break;
            }
        }

        #endregion Event Handlers

        #region Appearance

        private void dgAuctions_SetGridColumnProperties()
        {
            dgAuctions.Columns[_auctionIdColumnNumber].Visibility = Visibility.Hidden;
            dgAuctions.Columns[_customeridColumnNumber].Visibility = Visibility.Hidden;
            dgAuctions.Columns[_isDirtyColumnNumber].Visibility = Visibility.Hidden;
            dgAuctions.Columns[_customerNameColumnNumber].IsReadOnly = true;
            //dgAuctions.Columns[_monthColumnNumber].IsReadOnly = true;
            double gridWidth = dgAuctions.Width - 8;

            dgAuctions.Columns[_customerNameColumnNumber].Width = gridWidth / 2;
            dgAuctions.Columns[_monthColumnNumber].Width = gridWidth / 4;
            dgAuctions.Columns[_auctionAmountColumnNumber].Width = gridWidth / 4;
        }

        #endregion

        #region Helpers

        private int GetNextMonth()
        {
            return Auctions.Select(a => a.Month).Distinct().Max() + 1;
        }

        #endregion

        #endregion

        #region Payments Region

        #region Properties

        private List<Chit> _paymentChits = new List<Chit>();
        private Chit _paymentSelectedChit;
        private List<Payment> _payments = new List<Payment>();
        private int _selectedMonth;

        public List<Payment> Payments
        {
            get
            {
                if (PaymentChits.Count > 0)
                    return DataConversion.ConvertPaymentData(PaymentSelectedChit.ChitId);
                else
                    return new List<Payment>();
            }
        }

        public Chit PaymentSelectedChit
        {
            get
            {
                return _paymentSelectedChit;
            }
            set
            {
                _paymentSelectedChit = value;
                OnPropertyChanged("Months");
            }
        }

        public List<Chit> PaymentChits
        {
            get { return _paymentChits; }
        }

        public List<Payment> MonthlyPayments
        {
            get
            {
                return Payments.Where(p => p.Month == SelectedMonth).ToList();
            }
        }

        public List<int> Months
        {
            get { return Payments.Select(m => m.Month).Distinct().ToList(); }
        }

        public int SelectedMonth
        {
            get
            {
                if (_selectedMonth != 0)
                    return _selectedMonth;
                else
                    return Months[0];
            }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged("MonthlyPayments");
            }
        }


        #endregion

        #region Event Handlers

        private void dgPayments_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void dgPayments_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void dgPayments_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #endregion

        #region Customers

        #region Properties

        private List<Customer> _customers;

        public List<Customer> Customers
        {
            get { return DataConversion.ConvertCustomerData(0); }
            set { _customers = value; }
        }

        #endregion

        #region Constants

        private const int _customerIdColumnNumber = 0;
        private const int _customerNameCustomerColumnNumber = 1;
        private const int _addressColumnNumber = 2;
        private const int _cityColumnNumber = 3;
        private const int _homePhoneColumnNumber = 4;
        private const int _cellPhoneColumnNumber = 5;
        private const int _activeFlagColumnNumber = 6;
        private const int _isDirtyCustomerColumnNumber = 7;


        #endregion

        #region Appearance

        private void dgCustomers_SetGridColumnProperties()
        {
            dgCustomers.Columns[_customerIdColumnNumber].Visibility = Visibility.Hidden;
            dgCustomers.Columns[_isDirtyCustomerColumnNumber].Visibility = Visibility.Hidden; 
            //dgAuctions.Columns[_monthColumnNumber].IsReadOnly = true;
            double gridWidth = dgCustomers.Width - 8;

            dgCustomers.Columns[_customerNameCustomerColumnNumber].Width = gridWidth * 0.3;
            dgCustomers.Columns[_addressColumnNumber].Width = gridWidth * 0.3;
            dgCustomers.Columns[_cityColumnNumber].Width = gridWidth * 0.1;
            dgCustomers.Columns[_homePhoneColumnNumber].Width = gridWidth * 0.1;
            dgCustomers.Columns[_cellPhoneColumnNumber].Width = gridWidth * 0.1;
            dgCustomers.Columns[_activeFlagColumnNumber].Width = gridWidth * 0.1;
        }

        #endregion

        #region Event Handlers

        private void dgCustomers_Loaded(object sender, RoutedEventArgs e)
        {
            dgCustomers_SetGridColumnProperties();
        }

        #endregion

        #endregion




    }
}
