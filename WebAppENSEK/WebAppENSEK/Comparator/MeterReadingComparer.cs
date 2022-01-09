using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebAppENSEK.Model;

namespace WebAppENSEK.Comparator
{
    public class MeterReadingComparer : IEqualityComparer<MeterReading>
    {

        public bool Equals([AllowNull] MeterReading x, [AllowNull] MeterReading y)
        {
            return x.AccountID == y.AccountID && x.ReadingTime == y.ReadingTime;
        }

        public int GetHashCode([DisallowNull] MeterReading obj)
        {
            return obj.AccountID.GetHashCode() + obj.ReadingTime.GetHashCode();
        }
    }
}
