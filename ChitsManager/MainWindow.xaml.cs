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
            LoadAuctions();
            #endregion

            #region Payments
            LoadPayments();
            #endregion

            LoadAllPayments();

            _customers = DataConversion.ConvertCustomerData(0);

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

        private bool IsListDirty(List<bool> dirtyFlags)
        {
            return dirtyFlags.Where(b => b == true).Count() > 0 ? true : false;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (Auction auction in Auctions.Where(a => a.IsDirty == true).ToList())
            {
                int premiumAmount;
                int bonusAmount;
                DateTime dueDate;

                bonusAmount = DataConversion.GetBonusAmount(AuctionSelectedChit, auction.AuctionAmount);
                premiumAmount = DataConversion.GetPremiumAmount(AuctionSelectedChit, bonusAmount);
                dueDate = DataConversion.GetDueDate(AuctionSelectedChit.ChitId, auction.Month);

                auction.BonusAmount = bonusAmount;
                auction.PremiumAmount = premiumAmount;
                auction.DueDate = dueDate.ToShortDateString();
            }
            DataConversion.UpdateAuctionData(Auctions);
            DataConversion.UpdatePaymentData(MonthlyPayments);
            DataConversion.UpdatePaymentData(AllPayments);
            DataConversion.UpsertCustomers(Customers);
            ReloadAuctions();
            MessageBox.Show("Changes Saved");
        }
        #endregion

        #endregion

        #region All Payments Region

        #region Properties

        private List<Payment> _allPayments = new List<Payment>();

        public List<Payment> AllPayments
        {
            get { return _allPayments; }
            set { _allPayments = value; }
        }

        #endregion

        #region Event Handlers

        private void dgAllPayments_Loaded(object sender, RoutedEventArgs e)
        {
            dgAllPayments_SetGridColumnProperties();
        }


        private void dgAllPayments_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditingElement.DataContext != null)
            {
                ((ChitsManager.Objects.Payment)e.EditingElement.DataContext).IsDirty = true;
            }
        }


        #endregion

        #region Constants

        private const int _allPaymentsPaymentIdColumnNumber = 0;
        private const int _allPaymentsCustomerNameColumnNumber = 1;
        private const int _allPaymentsChitNameColumnNumber = 2;
        private const int _allPaymentsMonthColumnNumber = 3;
        private const int _allPaymentsAmountDueColumnNumber = 4;
        private const int _allPaymentsDueDateColumnNumber = 5;
        private const int _allPaymentsPaidDateColumnNumber = 6;
        private const int _allPaymentsPaidColumnNumber = 7;
        private const int _allPaymentsIsDirtyColumnNumber = 8;


        #endregion

        #region Appearance

        private void dgAllPayments_SetGridColumnProperties()
        {
            if (dgAllPayments.Columns.Count > 0)
            {
                dgAllPayments.Columns[_allPaymentsPaymentIdColumnNumber].Visibility = Visibility.Hidden;
                //dgAllPayments.Columns[_allPaymentsChitNameColumnNumber].Visibility = Visibility.Hidden;
                //dgAllPayments.Columns[_allPaymentsMonthColumnNumber].Visibility = Visibility.Hidden;
                dgAllPayments.Columns[_allPaymentsIsDirtyColumnNumber].Visibility = Visibility.Hidden;
                dgAllPayments.Columns[_allPaymentsChitNameColumnNumber].IsReadOnly = true;
                dgAllPayments.Columns[_allPaymentsAmountDueColumnNumber].IsReadOnly = true;
                dgAllPayments.Columns[_allPaymentsDueDateColumnNumber].IsReadOnly = true;
                dgAllPayments.Columns[_allPaymentsCustomerNameColumnNumber].IsReadOnly = true;
                dgAllPayments.Columns[_allPaymentsMonthColumnNumber].IsReadOnly = true;
                // dgAllPayments.Columns[_allPaymentsPaidDateColumnNumber].IsReadOnly = true;
                double gridWidth = dgAllPayments.Width - 8;

                dgAllPayments.Columns[_allPaymentsCustomerNameColumnNumber].Width = gridWidth * 0.2;
                dgAllPayments.Columns[_allPaymentsChitNameColumnNumber].Width = gridWidth * 0.1;
                dgAllPayments.Columns[_allPaymentsPaidColumnNumber].Width = gridWidth * 0.1;
                dgAllPayments.Columns[_allPaymentsMonthColumnNumber].Width = gridWidth * 0.1;
                dgAllPayments.Columns[_allPaymentsAmountDueColumnNumber].Width = gridWidth * 0.2;
                dgAllPayments.Columns[_allPaymentsDueDateColumnNumber].Width = gridWidth * 0.15;
                dgAllPayments.Columns[_allPaymentsPaidDateColumnNumber].Width = gridWidth * 0.15;
            }
        }

        #endregion

        #region  Helpers
        private void LoadAllPayments()
        {
            _allPayments = DataConversion.ConvertAllPaymentData();
        }

        #endregion

        #endregion

        #region Payments Region

        #region Properties

        private List<Chit> _paymentChits = new List<Chit>();
        private Chit _paymentSelectedChit;
        private List<Payment> _payments = new List<Payment>();
        private List<Payment> _monthlyPayments = new List<Payment>();
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
                return _monthlyPayments;
            }
            set
            {
                _monthlyPayments = value;
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
                _monthlyPayments = Payments.Where(p => p.Month == SelectedMonth).ToList();
                OnPropertyChanged("MonthlyPayments");
                dgPayments_Loaded(new object(), new RoutedEventArgs());
            }
        }


        #endregion

        #region Event Handlers

        private void dgPayments_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditingElement.DataContext != null)
            {
                ((ChitsManager.Objects.Payment)e.EditingElement.DataContext).IsDirty = true;
            }
        }

        private void dgPayments_Loaded(object sender, RoutedEventArgs e)
        {
            dgPayments_SetGridColumnProperties();
        }

        private void dgPayments_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Constants

        private const int _paymentPaymentIdColumnNumber = 0;
        private const int _paymentCustomerNameColumnNumber = 1;
        private const int _paymentChitNameColumnNumber = 2;
        private const int _paymentMonthColumnNumber = 3;
        private const int _paymentAmountDueColumnNumber = 4;
        private const int _paymentDueDateColumnNumber = 5;
        private const int _paymentPaidDateColumnNumber = 6;
        private const int _paymentPaidColumnNumber = 7;
        private const int _paymentIsDirtyColumnNumber = 8;


        #endregion

        #region Appearance

        private void dgPayments_SetGridColumnProperties()
        {
            if (dgPayments.Columns.Count > 0)
            {
                dgPayments.Columns[_paymentPaymentIdColumnNumber].Visibility = Visibility.Hidden;
                dgPayments.Columns[_paymentChitNameColumnNumber].Visibility = Visibility.Hidden;
                dgPayments.Columns[_paymentMonthColumnNumber].Visibility = Visibility.Hidden;
                dgPayments.Columns[_paymentIsDirtyColumnNumber].Visibility = Visibility.Hidden;
                dgPayments.Columns[_paymentAmountDueColumnNumber].IsReadOnly = true;
                dgPayments.Columns[_paymentDueDateColumnNumber].IsReadOnly = true;
                dgPayments.Columns[_paymentCustomerNameColumnNumber].IsReadOnly = true;
               // dgPayments.Columns[_paymentPaidDateColumnNumber].IsReadOnly = true;
                double gridWidth = dgPayments.Width - 8;

                dgPayments.Columns[_paymentCustomerNameColumnNumber].Width = gridWidth * 0.45;
                dgPayments.Columns[_paymentPaidColumnNumber].Width = gridWidth * 0.1;
                dgPayments.Columns[_paymentAmountDueColumnNumber].Width = gridWidth * 0.15;
                dgPayments.Columns[_paymentDueDateColumnNumber].Width = gridWidth * 0.15;
                dgPayments.Columns[_paymentPaidDateColumnNumber].Width = gridWidth * 0.15;
            }
        }

        #endregion

        #region  Helpers
        private void LoadPayments()
        {
            _paymentChits = DataConversion.GetChits();
            if (PaymentChits.Count > 0)
            {
                _paymentSelectedChit = PaymentChits[0];
                _payments = DataConversion.ConvertPaymentData(PaymentSelectedChit.ChitId);
                if (Months.Count > 0)
                {
                    _selectedMonth = Months[0];
                    _monthlyPayments = Payments.Where(p => p.Month == SelectedMonth).ToList();
                }
                //if (Months.Count > 0)
                //    _selectedMonth = Months[0];
            }
        }

        #endregion

        #endregion

        #region Auctions Region

        #region Properties

        #region Constants

        private const int _auctionAuctionIdColumnNumber = 0;
        private const int _auctionCustomeridColumnNumber = 1;
        private const int _auctionCustomerNameColumnNumber = 2;
        private const int _auctionMonthColumnNumber = 3;
        private const int _auctionAmountColumnNumber = 4;
        private const int _auctionPremiumAmountColumnNumber = 5;
        private const int _auctionBonusAmountColumnNumber = 6;
        private const int _auctionDueDateColumnNumber = 7;
        private const int _auctionIsDirtyColumnNumber = 8;


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
                ReloadAuctions();
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

            //if (IsListDirty(Auctions.Select(a => a.IsDirty).ToList()))
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
                case _auctionMonthColumnNumber:
                    string number = tb.Text;
                    if (!NumberInRange(number, 1, _auctionSelectedChit.NumberOfMonths, Auctions.Where(i => i.Month != 0).Select(a => a.Month).ToList()))
                    {
                        tb.Text = "0";
                        e.EditingElement.Focus();
                        e.Cancel = true;
                        //dgAuctions.CurrentCell = new DataGridCellInfo(dgAuctions.Items[e.Row.AlternationIndex], dgAuctions.Columns[e.Column.DisplayIndex]);
                        //dgAuctions.BeginEdit();
                        return;
                    }
                    //else
                    //{

                    //}
                    break;
                case _auctionAmountColumnNumber:
                    if (!IsNumber(((TextBox)e.EditingElement).Text))
                    {
                        tb.Text = "0";
                        e.EditingElement.Focus();
                        e.Cancel = true;
                    }
                    //else
                    //{
                    //    //int nextMonth = GetNextMonth();
                    //    //((ChitsManager.Objects.Auction)e.EditingElement.DataContext).Month = nextMonth;
                    //    //((Auction)dgAuctions.Items[e.Row.AlternationIndex]).Month = nextMonth;
                    //    //Auctions[e.Row.AlternationIndex].Month = GetNextMonth();
                    //    //dgAuctions.Items.Refresh();
                    //    OnPropertyChanged("Auctions");
                    //}
                    break;
            }
        }

        #endregion Event Handlers

        #region Appearance

        private void dgAuctions_SetGridColumnProperties()
        {
            if (dgAuctions.Columns.Count > 0)
            {
                dgAuctions.Columns[_auctionAuctionIdColumnNumber].Visibility = Visibility.Hidden;
                dgAuctions.Columns[_auctionCustomeridColumnNumber].Visibility = Visibility.Hidden;
                dgAuctions.Columns[_auctionIsDirtyColumnNumber].Visibility = Visibility.Hidden;
                dgAuctions.Columns[_auctionCustomerNameColumnNumber].IsReadOnly = true;
                dgAuctions.Columns[_auctionPremiumAmountColumnNumber].IsReadOnly = true;
                dgAuctions.Columns[_auctionBonusAmountColumnNumber].IsReadOnly = true;
                dgAuctions.Columns[_auctionDueDateColumnNumber].IsReadOnly = true;
                //dgAuctions.Columns[_monthColumnNumber].IsReadOnly = true;
                double gridWidth = dgAuctions.Width - 8;

                dgAuctions.Columns[_auctionCustomerNameColumnNumber].Width = gridWidth * .4;
                dgAuctions.Columns[_auctionMonthColumnNumber].Width = gridWidth * .12;
                dgAuctions.Columns[_auctionAmountColumnNumber].Width = gridWidth * .12;
                dgAuctions.Columns[_auctionPremiumAmountColumnNumber].Width = gridWidth * .12;
                dgAuctions.Columns[_auctionBonusAmountColumnNumber].Width = gridWidth * .12;
                dgAuctions.Columns[_auctionDueDateColumnNumber].Width = gridWidth * .12;
            }
        }

        #endregion

        #region Helpers

        private int GetNextMonth()
        {
            return Auctions.Select(a => a.Month).Distinct().Max() + 1;
        }

        private void LoadAuctions()
        {
            _dicAuctions = DataConversion.ConvertAuctionData();
            if (AuctionSelectedChit != null)
                Auctions = (_dicAuctions.First(c => c.Key.ChitId == AuctionSelectedChit.ChitId)).Value;
            else
                Auctions = new List<Auction>();
        }

        private void ReloadAuctions()
        {
            _dicAuctions = DataConversion.ConvertAuctionData();
            if (AuctionSelectedChit != null)
                _auctions = (_dicAuctions.First(c => c.Key.ChitId == AuctionSelectedChit.ChitId)).Value;
            else
                _auctions = new List<Auction>();
            OnPropertyChanged("Auctions");
            dgAuctions_Loaded(new object(), new RoutedEventArgs());
        }

        #endregion

        #endregion

        #region Customers

        #region Properties

        private List<Customer> _customers;

        public List<Customer> Customers
        {
            get
            {
                return _customers;
            }
            set
            {
                _customers = value;
            }
        }

        #endregion

        #region Constants

        private const int _customerCustomerIdColumnNumber = 0;
        private const int _customerCustomerNameColumnNumber = 1;
        private const int _customerAddressColumnNumber = 2;
        private const int _customerCityColumnNumber = 3;
        private const int _customerHomePhoneColumnNumber = 4;
        private const int _customerCellPhoneColumnNumber = 5;
        private const int _customerActiveFlagColumnNumber = 6;
        private const int _customerIsDirtyColumnNumber = 7;


        #endregion

        #region Appearance

        private void dgCustomers_SetGridColumnProperties()
        {
            if (dgCustomers.Columns.Count > 0)
            {
                dgCustomers.Columns[_customerCustomerIdColumnNumber].Visibility = Visibility.Hidden;
                dgCustomers.Columns[_customerIsDirtyColumnNumber].Visibility = Visibility.Hidden;
                //dgAuctions.Columns[_monthColumnNumber].IsReadOnly = true;
                double gridWidth = dgCustomers.Width - 8;

                dgCustomers.Columns[_customerCustomerNameColumnNumber].Width = gridWidth * 0.3;
                dgCustomers.Columns[_customerAddressColumnNumber].Width = gridWidth * 0.3;
                dgCustomers.Columns[_customerCityColumnNumber].Width = gridWidth * 0.1;
                dgCustomers.Columns[_customerHomePhoneColumnNumber].Width = gridWidth * 0.1;
                dgCustomers.Columns[_customerCellPhoneColumnNumber].Width = gridWidth * 0.1;
                dgCustomers.Columns[_customerActiveFlagColumnNumber].Width = gridWidth * 0.1;
            }
        }

        #endregion

        #region Event Handlers

        private void dgCustomers_Loaded(object sender, RoutedEventArgs e)
        {
            dgCustomers_SetGridColumnProperties();
        }

        private void dgCustomers_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.DataContext != null)
            {
                ((ChitsManager.Objects.Customer)e.Row.DataContext).IsDirty = true;
            }
        }

        private void dgCustomers_LostFocus(object sender, RoutedEventArgs e)
        {
            Control ctrl = FocusManager.GetFocusedElement(this) as Control;
            if (ctrl.Parent != null && ctrl.Parent.GetType() != typeof(DataGridCell))
            {
                DataConversion.UpsertCustomers(Customers);
                _customers = DataConversion.ConvertCustomerData(0);
            }
        }

        #endregion

        
        #endregion




    }
}
