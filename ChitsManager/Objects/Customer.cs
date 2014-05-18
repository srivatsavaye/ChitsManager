using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitsManager.Objects
{
    public class Customer
    {
        private int _customerId;
        private string _customerName;
        private string _address;
        private string _city;
        private string _homePhone;
        private string _cellPhone;
        private bool _activeFlag;
        private bool _isDirty;

        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string HomePhone
        {
            get { return _homePhone; }
            set { _homePhone = value; }
        }

        public string CellPhone
        {
            get { return _cellPhone; }
            set { _cellPhone = value; }
        }

        public bool ActiveFlag
        {
            get { return _activeFlag; }
            set { _activeFlag = value; }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
    }
}
