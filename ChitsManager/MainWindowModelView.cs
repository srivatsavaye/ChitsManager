using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitsManager.Objects;
using ChitsManager.DataAccess;

namespace ChitsManager
{
    class MainWindowModelView
    {
        private Dictionary<Chit, List<Auction>> _dicAuctions = new Dictionary<Chit, List<Auction>>();
        private Chit _selectedChit;

        public MainWindowModelView()
        {
            _dicAuctions = DataConversion.ConvertAuctionData();
        }

        public List<Auction> Auctions
        {
            get
            {
                if (SelectedChit != null)
                    return (_dicAuctions.First(c => c.Key.ChitId == SelectedChit.ChitId)).Value;
                else
                    return new List<Auction>();
            }
            set
            {
                if (value != null)
                {
                    var savedAuctions = value;
                }
            }
        }

        public Chit SelectedChit
        {
            get
            {
                if (_selectedChit == null)
                    _selectedChit = _dicAuctions.Select(c => c.Key).Distinct().First();
                return _selectedChit;
            }
            set
            {
                if (value != null)
                {
                    _selectedChit = value;
                }
            }
        }

        public List<Chit> Chits
        {
            get
            {
                return _dicAuctions.Select(c => c.Key).Distinct().ToList();
            }
        }
    }
}
