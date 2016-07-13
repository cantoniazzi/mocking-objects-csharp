using System;

namespace MockProjetct.domain
{
    public class Payment
    {
        public double value { get; set; }
        public DateTime date { get; set; }

        public  Payment (double value, DateTime date)
        {
            this.value = value;
            this.date = date;
        }
    }
}
