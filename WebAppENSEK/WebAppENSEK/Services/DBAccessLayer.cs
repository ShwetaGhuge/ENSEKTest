using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppENSEK.Db;
using WebAppENSEK.Model;
using WebAppENSEK.Services;

namespace WebAppENSEK.Services
{
    public class DBAccessLayer : IDbAccessLayer
    {
        private readonly ILogger<DBAccessLayer> _logger;

        public DBAccessLayer(ILogger<DBAccessLayer> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Select Account ID from the database
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public bool ValidAccountID(int accountID)
        {
            try
            {
                string sqlStr = "SELECT ACCOUNTID FROM ACCOUNT WHERE ACCOUNTID=" + accountID;
                var result = SqlHelper.GetScalar(sqlStr);

                return !string.IsNullOrEmpty(Convert.ToString(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Select top MeterReadingtime from the database
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public string GetLatestMeterReadingTime(int accountID)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("SELECT top 1 MeterReadingDateTime FROM MeterReading " +
                    " WHERE AccountID=" + accountID + " ORDER BY MeterReadingDateTime DESC");

                var result = SqlHelper.GetScalar(stringBuilder.ToString());
                return result != null ? result.ToString() : null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Insert MeterReading into database
        /// </summary>
        /// <param name="meter"></param>
        /// <returns></returns>
        public bool SaveMeterReading(MeterReading meter)
        {
            try
            {
                string commandText = " INSERT INTO MeterReading(AccountID,MeterReadingDateTime,MeterReadValue)" +
              " VALUES(@AccountID,@MeterDateTime,@MeterValue)";

                SqlCommand command = new SqlCommand
                {
                    CommandText = commandText
                };

                SqlParameter paramAccountID = new SqlParameter("@AccountID", SqlDbType.Int, 0)
                {
                    Value = meter.AccountID
                };
                command.Parameters.Add(paramAccountID);

                SqlParameter paramMeterDtTime = new SqlParameter("@MeterDateTime", SqlDbType.DateTime)
                {
                    Value = meter.ReadingTime
                };
                command.Parameters.Add(paramMeterDtTime);

                SqlParameter paramMeterValue = new SqlParameter("@MeterValue", SqlDbType.Int, 0)
                {
                    Value = meter.MeterValue
                };
                command.Parameters.Add(paramMeterValue);


                SqlCommand sqlCommand = SqlHelper.SplCmd(command);

                return sqlCommand != null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return false;
            }

        }
    }
}
