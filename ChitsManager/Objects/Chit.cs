using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitsManager.Objects
{
    public class Chit
    {
        private int _chitId;
        private string _chitName;
        private int _premium;
        private int _numberOfMonths;
        private int _numberOfCustomers;
        private DateTime _startDate;
        private int _interestGracePeriod;
        private int _interestOnPremium;
        private int _multiplier;
        private bool _activeFlag;
       
        public int ChitId
        {
            get { return _chitId; }
            set { _chitId = value; }
        }

        public string ChitName
        {
            get { return _chitName; }
            set { _chitName = value; }
        }

        public int Premium
        {
            get { return _premium; }
            set { _premium = value; }
        }

        public int NumberOfMonths
        {
            get { return _numberOfMonths; }
            set { _numberOfMonths = value; }
        }

        public int NumberOfCustomers
        {
            get { return _numberOfCustomers; }
            set { _numberOfCustomers = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public int InterestGracePeriod
        {
            get { return _interestGracePeriod; }
            set { _interestGracePeriod = value; }
        }

        public int InterestOnPremium
        {
            get { return _interestOnPremium; }
            set { _interestOnPremium = value; }
        }

        public int Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }

        public bool ActiveFlag
        {
            get { return _activeFlag; }
            set { _activeFlag = value; }
        }

        public override string ToString()
        {
            return ChitId.ToString() + " - " + ChitName;
        }
        
    }
}
