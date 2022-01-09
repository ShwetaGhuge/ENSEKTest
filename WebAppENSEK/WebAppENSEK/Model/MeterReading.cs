using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppENSEK.Model
{
    public class MeterReading
    {
       public int AccountID { get; set; }
        public DateTime ReadingTime { get; set; }

        public int MeterValue { get; set; }
    }
}
