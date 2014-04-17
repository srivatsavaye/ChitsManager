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
        private int _amountDue;
        private DateTime _dueDate;
        private DateTime _paidDate;
        private bool _paid;

        public int PaymentId
        {
            get { return _paymentId; }
            set { _paymentId = value; }
        }

        public int Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public int AmountDue
        {
            get { return _amountDue; }
            set { _amountDue = value; }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        public DateTime PaidDate
        {
            get { return _paidDate; }
            set { _paidDate = value; }
        }

        public bool Paid
        {
            get { return _paid; }
            set { _paid = value; }
        }
    }
}
