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
        private static List<Chit> _chits = new List<Chit>();

        public static Dictionary<Chit, List<Auction>> ConvertAuctionData()
        {
            Dictionary<Chit, List<Auction>> dic = new Dictionary<Chit, List<Auction>>();
            try
            {
                DataSet ds = _dataController.GetAuctionsbyChitId();

                _chits = GetChits();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var distinctChits = ds.Tables[0].DefaultView.ToTable(true, "ChitId");
                        foreach (DataRow drChit in distinctChits.Rows)
                        {
                            int chitId;
                            int.TryParse(drChit["ChitId"].ToString(), out chitId);
                            Chit chit = _chits.Where(c => c.ChitId == chitId).First();

                            List<Auction> listAuctions = new List<Auction>();
                            var auctions = ds.Tables[0].Select(String.Format("ChitId = {0}", chitId));
                            foreach (DataRow dr in auctions)
                            {
                                int auctionId;
                                int customerId;
                                int month;
                                int auctionAmount;
                                int premiumAmount;
                                int bonusAmount;
                                int sortOrder;
                                bool isDirty = false;
                                string dueDate;
                                int.TryParse(dr["AuctionId"].ToString(), out auctionId);
                                int.TryParse(dr["CustomerId"].ToString(), out customerId);
                                int.TryParse(dr["Month"].ToString(), out month);
                                int.TryParse(dr["AuctionAmount"].ToString(), out auctionAmount);
                                int.TryParse(dr["PremiumAmount"].ToString(), out premiumAmount);
                                int.TryParse(dr["BonusAmount"].ToString(), out bonusAmount);
                                int.TryParse(dr["SortOrder"].ToString(), out sortOrder);
                                dueDate = dr["DueDate"].ToString();

                                if (premiumAmount == 0 && (month != 0 || auctionAmount != 0))
                                {
                                    bonusAmount = GetBonusAmount(chit, auctionAmount);
                                    premiumAmount = GetPremiumAmount(chit, bonusAmount);
                                    dueDate = GetDueDate(chitId, month).ToShortDateString();
                                    isDirty = true;
                                }

                                Auction auction = new Auction();
                                auction.AuctionId = auctionId;
                                auction.Customerid = customerId;
                                auction.CustomerName = dr["CustomerName"].ToString();
                                auction.Month = month;
                                auction.SortOrder = sortOrder;
                                auction.AuctionAmount = auctionAmount;
                                auction.PremiumAmount = premiumAmount;
                                auction.BonusAmount = bonusAmount;
                                auction.DueDate = dueDate;
                                auction.CustomerNote = dr["CustomerNote"].ToString();
                                auction.IsDirty = isDirty;

                                listAuctions.Add(auction);
                            }
                            dic.Add(chit, listAuctions);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ConvertAuctionData() Method", ex);
            }
            return dic;
        }

        public static List<Chit> GetChits()
        {
            List<Chit> listChits = new List<Chit>();

            try
            {
                DataSet ds = _dataController.GetChits();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int chitId;
                            int premium;
                            int numberofMonths;
                            int numberofCustomers;
                            int multiplier;
                            int interestGracePeriod;
                            int interestOnPremium;
                            DateTime startDate;
                            bool activeFlag;
                            int.TryParse(dr["ChitId"].ToString(), out chitId);
                            int.TryParse(dr["Premium"].ToString(), out premium);
                            int.TryParse(dr["NumberofMonths"].ToString(), out numberofMonths);
                            int.TryParse(dr["NumberOfCustomers"].ToString(), out numberofCustomers);
                            DateTime.TryParse(dr["StartDate"].ToString(), out startDate);
                            int.TryParse(dr["InterestGracePeriod"].ToString(), out interestGracePeriod);
                            int.TryParse(dr["InterestOnPremium"].ToString(), out interestOnPremium);
                            int.TryParse(dr["Multiplier"].ToString(), out multiplier);
                            bool.TryParse(dr["ActiveFlag"].ToString(), out activeFlag);
                            Chit chit = new Chit();
                            chit.ChitId = chitId;
                            chit.ChitName = dr["ChitName"].ToString();
                            chit.Multiplier = multiplier;
                            chit.Premium = premium;
                            chit.NumberOfCustomers = numberofCustomers;
                            chit.NumberOfMonths = numberofMonths;
                            chit.StartDate = startDate;
                            chit.InterestGracePeriod = interestGracePeriod;
                            chit.InterestOnPremium = interestOnPremium;
                            chit.ActiveFlag = activeFlag;

                            listChits.Add(chit);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetChits() Method", ex);
            }

            return listChits;
        }

        public static void UpdateAuctionData(List<Auction> listAuctions)
        {
            try
            {
                foreach (Auction auction in listAuctions.Where(a => a.IsDirty == true).ToList())
                {
                    _dataController.UpdateAuctionByAuctionId(auction.AuctionId, auction.Month, auction.AuctionAmount, auction.PremiumAmount, auction.BonusAmount, auction.DueDate, auction.CustomerNote);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateAuctionData() Method", ex);
            }
        }

        public static void UpdatePaymentData(List<Payment> listPayments)
        {
            try
            {
                foreach (Payment payment in listPayments.Where(a => a.IsDirty == true).ToList())
                {
                    _dataController.UpdatePaymentByPaymentId(payment.PaymentId, payment.Paid, payment.AmountPaid, payment.PaidDate);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateAuctionData() Method", ex);
            }
        }



        public static List<Payment> ConvertPaymentData(int chitId = 0)
        {
            List<Payment> listPayments = new List<Payment>();
            try
            {
                DataSet ds = _dataController.GetPaymentsbyChitId(chitId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int paymentId;
                            int month;
                            int amountDue;
                            int amountPaid;
                            bool paid;
                            int.TryParse(dr["PaymentId"].ToString(), out paymentId);
                            int.TryParse(dr["Month"].ToString(), out month);
                            int.TryParse(dr["AmountDue"].ToString(), out amountDue);
                            int.TryParse(dr["AmountPaid"].ToString(), out amountPaid);
                            bool.TryParse(dr["Paid"].ToString(), out paid);

                            Payment payment = new Payment();
                            payment.PaymentId = paymentId;
                            payment.Month = month;
                            payment.AmountDue = GetAmountDue(chitId, month, amountDue, paid, dr["PaidDate"].ToString());
                            payment.DueDate = dr["DueDate"].ToString();
                            payment.AmountPaid = amountPaid.ToString();
                            payment.PaidDate = dr["PaidDate"].ToString();
                            payment.CustomerName = dr["CustomerName"].ToString();
                            payment.Paid = paid;

                            listPayments.Add(payment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ConvertPaymentData() Method", ex);
            }
            return listPayments;
        }

        public static List<Payment> ConvertAllPaymentData()
        {
            List<Payment> listPayments = new List<Payment>();
            try
            {
                DataSet ds = _dataController.GetAllPayments();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int paymentId;
                            int month;
                            int amountDue;
                            int amountPaid;
                            bool paid;
                            int chitId;
                            int.TryParse(dr["PaymentId"].ToString(), out paymentId);
                            int.TryParse(dr["Month"].ToString(), out month);
                            int.TryParse(dr["AmountDue"].ToString(), out amountDue);
                            int.TryParse(dr["AmountPaid"].ToString(), out amountPaid);
                            int.TryParse(dr["ChitId"].ToString(), out chitId);
                            bool.TryParse(dr["Paid"].ToString(), out paid);

                            Payment payment = new Payment();
                            payment.PaymentId = paymentId;
                            payment.Month = month;
                            payment.AmountDue = GetAmountDue(chitId, month, amountDue, paid, dr["PaidDate"].ToString());
                            payment.DueDate = dr["DueDate"].ToString();
                            payment.AmountPaid = amountPaid.ToString();
                            payment.PaidDate = dr["PaidDate"].ToString();
                            payment.CustomerName = dr["CustomerName"].ToString();
                            payment.ChitName = dr["ChitName"].ToString();
                            payment.Paid = paid;

                            listPayments.Add(payment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ConvertAllPaymentData() Method", ex);
            }
            return listPayments;
        }

        public static List<Customer> ConvertCustomerData(int customerId = 0)
        {
            List<Customer> listCustomers = new List<Customer>();
            try
            {
                DataSet ds = _dataController.GetCustomers(customerId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int customerId1;
                            bool activeFlag;
                            int.TryParse(dr["CustomerId"].ToString(), out customerId1);
                            bool.TryParse(dr["ActiveFlag"].ToString(), out activeFlag);

                            Customer customer = new Customer();
                            customer.CustomerId = customerId1;
                            customer.CustomerName = dr["CustomerName"].ToString();
                            customer.Address = dr["Address"].ToString();
                            customer.City = dr["City"].ToString();
                            customer.HomePhone = dr["HomePhone"].ToString();
                            customer.CellPhone = dr["CellPhone"].ToString();
                            customer.ActiveFlag = activeFlag;

                            listCustomers.Add(customer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ConvertCustomerData() Method", ex);
            }
            return listCustomers;
        }

        public static bool UpsertCustomers(List<Customer> listCustomers)
        {
            bool ret = false;
            DataController controller = new DataController();
            try
            {
                foreach (Customer customer in listCustomers)
                {
                    if (customer.IsDirty)
                    {
                        if (customer.CustomerId == 0)
                            controller.InsertCustomer(customer);
                        else
                            controller.UpdateCustomer(customer);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpsertCustomers() Method", ex);
            }
            return ret;
        }


        public static int GetPremiumAmount(Chit chit, int bonusAmount)
        {
            int ret = 0;

            if (chit.Premium == 0 || bonusAmount == 0)
                return 0;

            ret = chit.Premium - bonusAmount;

            return ret;
        }

        public static int GetBonusAmount(Chit chit, int auctionAmount)
        {
            int ret = 0;

            if (auctionAmount == 0 || chit.Premium == 0 || chit.NumberOfCustomers == 0)
                return 0;

            ret = (auctionAmount - chit.Premium) / chit.NumberOfCustomers;

            return ret;
        }

        public static DateTime GetDueDate(int chitId, int currentMonth)
        {
            DateTime ret;
            Chit chit = _chits.Where(c => c.ChitId == chitId).FirstOrDefault();

            ret = chit.StartDate.AddMonths(currentMonth + 1).AddDays(-1);

            return ret;
        }

        public static int GetInterest(int chitId)
        {
            int ret = 0;
            Chit chit = _chits.Where(c => c.ChitId == chitId).FirstOrDefault();
            ret = chit.InterestOnPremium;
            return ret;
        }

        public static double GetInterestAmount(int chitId, int principle)
        {
            double ret = 0.0;
            ret = GetInterest(chitId) * principle / 100;
            return ret;
        }


        public static string GetAmountDue(int chitId, int month, int amountDue, bool paid, string paidDate)
        {
            DateTime dueDate = GetDueDate(chitId, month);
            string ret = amountDue.ToString();

            DateTime paidDateTime;

            DateTime.TryParse(paidDate, out paidDateTime);

            if (!paid || paidDateTime > dueDate.AddDays(5))
            {
                if (dueDate < DateTime.Now)
                {
                    double interestAmount = GetInterestAmount(chitId, amountDue);
                    int interestAmountAsInt = int.Parse(Math.Round(interestAmount, 0).ToString());

                    if (interestAmountAsInt == 0)
                        ret = amountDue.ToString();
                    else
                        ret = string.Format("{0} + {1} = {2}", amountDue, interestAmountAsInt, amountDue + interestAmountAsInt);
                }
            }

            return ret;
        }
    }
}
