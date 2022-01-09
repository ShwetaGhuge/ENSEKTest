using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppENSEK.Comparator;
using WebAppENSEK.Model;
using WebAppENSEK.Db;
using Microsoft.Extensions.Logging;

namespace WebAppENSEK.Services
{
    public class Repository : IRepository
    {
        private readonly IDbAccessLayer _db;
        private readonly ILogger<Repository> _logger;

        public Repository(IDbAccessLayer db, ILogger<Repository> logger)
        {
            this._db = db;
            _logger = logger;
        }

        /// <summary>
        /// Read Csv file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultResponse MeterReadingUpload(IFormFile file)
        {
            var dic = new Dictionary<MeterReading, MeterReading>(new MeterReadingComparer());
            ResultResponse resultResponse = new ResultResponse();
            int totalRecords = 0;

            if (file != null && file.Length > 0)
            {
                if (file.FileName.EndsWith(".csv"))
                {
                    using var reader = new StreamReader(file.OpenReadStream());
                    var readCSV = reader.ReadToEnd();

                    foreach (var row in readCSV.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row.Trim()))
                        {

                            totalRecords++;
                            string[] cell = row.Split(',');

                            try
                            {
                                MeterReading meter = new MeterReading
                                {
                                    AccountID = Convert.ToInt32(cell[0]),
                                    ReadingTime = Convert.ToDateTime(cell[1]),
                                    MeterValue = Convert.ToInt32(cell[2])
                                };

                                if (cell[2].Trim().Length == 5)
                                {
                                    dic.Add(meter, meter);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogInformation("Record failed for AccountID :" + cell[0], ex.ToString());
                            }
                        }
                    }
                }
            }
            //Get Success Inserts
            var successInsert = GetSuccessInserts(dic);

            resultResponse.PassRecords = successInsert;
            resultResponse.FailRecords = (totalRecords - 1) - successInsert;

            return resultResponse;
        }

        /// <summary>
        /// Check for Valid Account
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        private bool IsValidAccount(int accountID)
        {
            var isValidAccount = _db.ValidAccountID(accountID);
            return isValidAccount;
        }

        /// <summary>
        /// Check for Latest Meter Reading time
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="readingTime"></param>
        /// <returns></returns>
        private bool IsReadingLatest(int accountID, DateTime readingTime)
        {
            var latestMeterReadingTime = _db.GetLatestMeterReadingTime(accountID);
            if (string.IsNullOrEmpty(latestMeterReadingTime))
            {
                return true;
            }
            else
            {
                return Convert.ToDateTime(latestMeterReadingTime) < readingTime;
            }
        }

        /// <summary>
        /// This method return succesful saved records in the database
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private int GetSuccessInserts(Dictionary<MeterReading, MeterReading> dic)
        {
            int successInsert = 0;

            foreach (var item in dic)
            {
                //Check for Valid Account
                var isValid = IsValidAccount(item.Key.AccountID);

                if (isValid)
                {
                    var isLatest = IsReadingLatest(item.Key.AccountID, item.Key.ReadingTime);

                    if (isLatest)
                    {
                        MeterReading meter = new MeterReading
                        {
                            AccountID = item.Value.AccountID,
                            ReadingTime = item.Value.ReadingTime,
                            MeterValue = item.Value.MeterValue
                        };

                        //Save into Database
                        var isSaved = _db.SaveMeterReading(meter);

                        if (isSaved)
                        {
                            successInsert++;
                        }
                    }
                }
            }
            return successInsert;
        }
    }
}
