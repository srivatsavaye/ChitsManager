using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GlobalDataAccess;

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


        public bool UpdateAuctionByAuctionId(int auctionId, int month, int auctionAmount)
        {
            bool ret = false;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@AuctionId", auctionId));
                paramList.Add(new SqlParameter("@Month", month));
                paramList.Add(new SqlParameter("@AuctionAmount", auctionAmount));
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
    }
}
