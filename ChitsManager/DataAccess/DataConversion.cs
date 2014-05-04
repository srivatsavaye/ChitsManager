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

            List<Chit> listChits = GetChits();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var distinctChits = ds.Tables[0].DefaultView.ToTable(true, "ChitId");
                    foreach (DataRow drChit in distinctChits.Rows)
                    {
                        int chitId;
                        int.TryParse(drChit["ChitId"].ToString(), out chitId);
                        Chit chit = listChits.Where(c => c.ChitId == chitId).First();

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

        public static List<Chit> GetChits()
        {
            List<Chit> listChits = new List<Chit>();

            DataSet ds = _dataController.GetChits();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int chitId;
                        int numberofMonths;
                        int numberofCustomers;
                        int multiplier;
                        bool activeFlag;
                        int.TryParse(dr["ChitId"].ToString(), out chitId);
                        int.TryParse(dr["NumberofMonths"].ToString(), out numberofMonths);
                        int.TryParse(dr["NumberOfCustomers"].ToString(), out numberofCustomers);
                        int.TryParse(dr["Multiplier"].ToString(), out multiplier);
                        bool.TryParse(dr["ActiveFlag"].ToString(), out activeFlag);
                        Chit chit = new Chit();
                        chit.ChitId = chitId;
                        chit.ChitName = dr["ChitName"].ToString();
                        chit.MonthStarted = dr["MonthStarted"].ToString();
                        chit.Multiplier = multiplier;
                        chit.NumberOfCustomers = numberofCustomers;
                        chit.NumberOfMonths = numberofMonths;
                        chit.ActiveFlag = activeFlag;

                        listChits.Add(chit);
                    }
                }
            }

            return listChits;
        }

        public static void UpdateAuctionData(List<Auction> listAuctions)
        {
            foreach (Auction auction in listAuctions.Where(a => a.IsDirty == true).ToList())
            {
                _dataController.UpdateAuctionByAuctionId(auction.AuctionId, auction.Month, auction.AuctionAmount);
            }
        }

        public static List<Payment> ConvertPaymentData(int chitId = 0)
        {
            DataSet ds = _dataController.GetPaymentsbyChitId(chitId);
            List<Payment> listPayments = new List<Payment>();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int paymentId;
                        int month;
                        int amountDue;
                        DateTime dueDate;
                        DateTime paidDate;
                        bool paid;
                        int.TryParse(dr["PaymentId"].ToString(), out paymentId);
                        int.TryParse(dr["Month"].ToString(), out month);
                        int.TryParse(dr["AmountDue"].ToString(), out amountDue);
                        DateTime.TryParse(dr["PaidDate"].ToString(), out paidDate);
                        DateTime.TryParse(dr["DueDate"].ToString(), out dueDate);
                        bool.TryParse(dr["Paid"].ToString(), out paid);

                        Payment payment = new Payment();
                        payment.PaymentId = paymentId;
                        payment.Month = month;
                        payment.AmountDue = amountDue;
                        payment.DueDate = dueDate;
                        payment.PaidDate = paidDate;
                        payment.Paid = paid;

                        listPayments.Add(payment);
                    }
                }
            }
            return listPayments;
        }
    }
}
