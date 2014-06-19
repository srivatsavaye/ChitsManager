using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitsManager.Objects
{
    public class Payment
    {
        private int _paymentId;
        private int _month;
        private string _customerName;
        private string _chitName;
        private string _amountDue;
        private string _dueDate;
        private string _amountPaid;
        private string _paidDate;
        private bool _paid;
        private bool _isDirty;

        public int PaymentId
        {
            get { return _paymentId; }
            set { _paymentId = value; }
        }

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }

        public string ChitName
        {
            get { return _chitName; }
            set { _chitName = value; }
        }

        public int Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public string AmountDue
        {
            get { return _amountDue; }
            set { _amountDue = value; }
        }

        public string DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        public string AmountPaid
        {
            get { return _amountPaid; }
            set { _amountPaid = value; }
        }

        public string PaidDate
        {
            get { return _paidDate; }
            set { _paidDate = value; }
        }

        public bool Paid
        {
            get { return _paid; }
            set { _paid = value; }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
    }
}
