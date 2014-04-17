using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ChitsManager.Objects;

namespace ChitsManager.DataAccess
{
    public static class DataConversion
    {
        private static DataController _dataController = new DataController();

        public static Dictionary<Chit, List<Auction>> ConvertAuctionData()
        {
            Dictionary<Chit, List<Auction>> dic = new Dictionary<Chit, List<Auction>>();
            DataSet ds = _dataController.GetAuctionsbyChitId();

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var distinctChits = ds.Tables[0].DefaultView.ToTable(true, "ChitId", "ChitName");
                    foreach (DataRow drChit in distinctChits.Rows)
                    {
                        int chitId;
                        int.TryParse(drChit["ChitId"].ToString(), out chitId);
                        Chit chit = new Chit();
                        chit.ChitId = chitId;
                        chit.ChitName = drChit["ChitName"].ToString();

                        List<Auction> listAuctions = new List<Auction>();
                        var auctions = ds.Tables[0].Select(String.Format("ChitId = {0}", chitId));
                        foreach (DataRow dr in auctions)
                        {
                            int auctionId;
                            int customerId;
                            int month;
                            int auctionAmount;
                            int.TryParse(dr["AuctionId"].ToString(), out auctionId);
                            int.TryParse(dr["CustomerId"].ToString(), out customerId);
                            int.TryParse(dr["Month"].ToString(), out month);
                            int.TryParse(dr["AuctionAmount"].ToString(), out auctionAmount);

                            Auction auction = new Auction();
                            auction.AuctionId = auctionId;
                            auction.Customerid = customerId;
                            auction.CustomerName = dr["CustomerName"].ToString();
                            auction.Month = month;
                            auction.AuctionAmount = auctionAmount;

                            listAuctions.Add(auction);
                        }
                        dic.Add(chit, listAuctions);
                    }
                }
            }
            return dic;
        }
    }
}
