using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GlobalDataAccess;
using ChitsManager.Objects;

namespace ChitsManager.DataAccess
{
    public class DataController
    {
        private string _connectionString;
        private ADODataAccess _adoDataAccess;

        public DataController()
        {
            _connectionString = Properties.Settings.Default.ConnectionString;
            _adoDataAccess = new ADODataAccess(_connectionString);
        }

        public DataSet GetAuctionsbyChitId(int chitId = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                if (chitId != 0)
                {
                    SqlParameter param = new SqlParameter("@ChitId", chitId);
                    paramList.Add(param);
                }
                if (!_adoDataAccess.ExecuteStoredProc(ds, "sp_GetAuctionsByChitId", paramList))
                {
                    throw new Exception("GetAuctionsbyChitId Could not return Data");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetAuctionsbyChitId method failed", ex);
            }

            return ds;
        }

        public DataSet GetPaymentsbyChitId(int chitId = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                if (chitId != 0)
                {
                    SqlParameter param = new SqlParameter("@ChitId", chitId);
                    paramList.Add(param);
                }
                if (!_adoDataAccess.ExecuteStoredProc(ds, "sp_GetPaymentsByChitId", paramList))
                {
                    throw new Exception("GetPaymentsbyChitId Could not return Data");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetPaymentsbyChitId method failed", ex);
            }

            return ds;
        }

        public DataSet GetAllPayments()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                if (!_adoDataAccess.ExecuteStoredProc(ds, "sp_GetAllPayments", paramList))
                {
                    throw new Exception("GetAllPayments Could not return Data");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllPayments method failed", ex);
            }

            return ds;
        }

        public DataSet GetChits()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                if (!_adoDataAccess.ExecuteStoredProc(ds, "sp_GetChits", paramList))
                {
                    throw new Exception("GetChits Could not return Data");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetChits method failed", ex);
            }

            return ds;
        }

        public bool UpdateAuctionByAuctionId(int auctionId, int month, int auctionAmount, int premiumAmount, int bonusAmount, string dueDate, string customerNote)
        {
            bool ret = false;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@AuctionId", auctionId));
                paramList.Add(new SqlParameter("@Month", month));
                paramList.Add(new SqlParameter("@AuctionAmount", auctionAmount));
                paramList.Add(new SqlParameter("@PremiumAmount", premiumAmount));
                paramList.Add(new SqlParameter("@BonusAmount", bonusAmount));
                paramList.Add(new SqlParameter("@DueDate", dueDate));
                paramList.Add(new SqlParameter("@CustomerNote", customerNote));
                if (!_adoDataAccess.ExecuteNonQuery("sp_UpdateAuctionByAuctionId", paramList))
                    throw new Exception("UpdateAuctionByAuctionId could not run");
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateAuctionByAuctionId method failed", ex);
            }

            return ret;
        }

        public bool UpdatePaymentByPaymentId(int paymentId, bool paid, string amountPaid, string paidDate)
        {
            bool ret = false;
            try
            {
                int amountPaidInt;
                int.TryParse(amountPaid, out amountPaidInt);
                if (String.IsNullOrEmpty(paidDate))
                    paidDate = DateTime.Now.ToShortDateString();
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@PaymentId", paymentId));
                paramList.Add(new SqlParameter("@Paid", paid));
                paramList.Add(new SqlParameter("@AmountPaid", amountPaidInt));
                paramList.Add(new SqlParameter("@PaidDate", paidDate));
                if (!_adoDataAccess.ExecuteNonQuery("sp_UpdatePaymentByPaymentId", paramList))
                    throw new Exception("UpdatePaymentByPaymentId could not run");
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdatePaymentByPaymentId method failed", ex);
            }

            return ret;
        }

        public DataSet GetCustomers(int customerId = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                if (customerId != 0)
                {
                    paramList.Add(new SqlParameter("@CustomerId", customerId));
                }
                if (!_adoDataAccess.ExecuteStoredProc(ds, "sp_GetCustomers", paramList))
                {
                    throw new Exception("GetCustomers did not return Data");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetCustomers method failed", ex);
            }

            return ds;
        }

        public bool UpdateCustomer(Customer customer)
        {
            bool ret = false;

            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@CustomerId", customer.CustomerId));
                paramList.Add(new SqlParameter("@CustomerName", customer.CustomerName));
                paramList.Add(new SqlParameter("@Address", customer.Address));
                paramList.Add(new SqlParameter("@City", customer.City));
                paramList.Add(new SqlParameter("@HomePhone", customer.HomePhone));
                paramList.Add(new SqlParameter("@CellPhone", customer.CellPhone));
                paramList.Add(new SqlParameter("@ActiveFlag", customer.ActiveFlag));

                if (!_adoDataAccess.ExecuteNonQuery("sp_UpdateCustomer", paramList))
                    throw new Exception("UpdateCustomer could not run");
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateCustomer method failed", ex);
            }



            return ret;
        }

        public bool InsertCustomer(Customer customer)
        {
            bool ret = false;

            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@CustomerName", customer.CustomerName));
                paramList.Add(new SqlParameter("@Address", customer.Address));
                paramList.Add(new SqlParameter("@City", customer.City));
                paramList.Add(new SqlParameter("@HomePhone", customer.HomePhone));
                paramList.Add(new SqlParameter("@CellPhone", customer.CellPhone));

                if (!_adoDataAccess.ExecuteNonQuery("sp_InsertCustomer", paramList))
                    throw new Exception("UpdateCustomer could not run");
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateCustomer method failed", ex);
            }
            return ret;
        }
    }
}
