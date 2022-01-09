using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebAppENSEK.Model;

namespace WebAppENSEK.Services
{
    public interface IDbAccessLayer
    {
        public bool ValidAccountID(int accountID);
        public string GetLatestMeterReadingTime(int accountID);

        public bool SaveMeterReading(MeterReading meter);
    }
}
