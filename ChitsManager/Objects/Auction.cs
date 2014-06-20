using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitsManager.Objects
{
    public class Auction
    {
        private int _auctionId;
        //private int _chitId;
        //private string _chitName;
        private int _sortOrder;
        private int _customerid;
        private string _customerName;
        private int _month;
        private int _auctionAmount;
        private int _premiumAmount;
        private int _bonusAmount;
        private string _dueDate;
        private string _customerNote;
        private bool _isDirty;

        public int AuctionId
        {
            get { return _auctionId; }
            set { _auctionId = value; }
        }

        //public int ChitId
        //{
        //    get { return _chitId; }
        //    set { _chitId = value; }
        //}

        //public string ChitName
        //{
        //    get { return _chitName; }
        //    set { _chitName = value; }
        //}

        public int Customerid
        {
            get { return _customerid; }
            set { _customerid = value; }
        }

        public int SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }

        public int Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public int AuctionAmount
        {
            get { return _auctionAmount; }
            set { _auctionAmount = value; }
        }

        public int PremiumAmount
        {
            get { return _premiumAmount; }
            set { _premiumAmount = value; }
        }

        public int BonusAmount
        {
            get { return _bonusAmount; }
            set { _bonusAmount = value; }
        }

        public string DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        public string CustomerNote
        {
            get { return _customerNote; }
            set { _customerNote = value; }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
    }
}
