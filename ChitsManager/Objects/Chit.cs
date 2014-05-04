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
        private int _numberOfMonths;
        private int _numberOfCustomers;
        private string _monthStarted;
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

        public string MonthStarted
        {
            get { return _monthStarted; }
            set { _monthStarted = value; }
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
