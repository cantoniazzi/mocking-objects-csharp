using MockProjetct.domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MockProjetct.service
{
    public class Rater
    {
        public virtual double highestValue { get; private set; }
        public virtual double lowestValue { get; private set; }

        private List<Bid> highestValues;
        
        public Rater ()
        {
            highestValue = Double.MinValue;
            lowestValue = Double.MaxValue;
        }

        public void Rate(Auction auction)
        {

            if (auction.bids.Count == 0)
            {
                throw new Exception("The auction don't have bids");
            }

            foreach (var a in auction.bids)
            {
                if (a.value > this.highestValue)
                    this.highestValue = a.value;

                if (a.value < this.lowestValue)
                    this.lowestValue = a.value;
            }

        }

        private void threeHighest(Auction auction)
        {
            this.highestValues = new List<Bid>(auction.bids
                .OrderByDescending(x => x.value));

            this.highestValues = this.highestValues
                .GetRange(0, this.highestValues.Count < 3 ? this.highestValues.Count : 3);
        }

    }
}
