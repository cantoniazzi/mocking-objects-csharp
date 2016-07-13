using System;
using System.Collections.Generic;

namespace MockProjetct.domain
{
    public class Auction
    {
        public string description { get; set; }
        public DateTime date { get; set; }
        public List<Bid> bids{ get; private set; }
        public bool closed { get; set; }
        public int id { get; set; }

        public Auction (string description)
        {
            this.description = description;
            this.bids = new List<Bid>();
            this.date = DateTime.Today;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bid"></param>
        public void Propose (Bid bid)
        {
            if (bids.Count == 0 || CanGiveBid(bid.user))
            {
                bids.Add(bid);
            }
        }

        /// <summary>
        /// Check if the current user can gives bid
        /// </summary>
        /// <param name="user"></param>
        /// <returns> true/false</returns>
        private bool CanGiveBid(User user)
        {
            return !LastBid().user.Equals(user) && QtdBidsOf(user) < 5;
        }

        /// <summary>
        /// Return bid quantity from some user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>quantity</returns>
        private int QtdBidsOf (User user)
        {
            int total = 0;

            foreach (var b in this.bids)
            {
                if (b.user.Equals(user))
                    total++;
                
            }
            return total;
        }

        /// <summary>
        /// Return last bif of auction
        /// </summary>
        /// <returns></returns>
        private Bid LastBid()
        {
            return this.bids[this.bids.Count - 1];
        }

        /// <summary>
        /// Finish the auction
        /// </summary>
        public void Finish()
        {
            this.closed = true;
        }

        public void InDate(DateTime date)
        {
            this.date = date;
        }

    }
}
