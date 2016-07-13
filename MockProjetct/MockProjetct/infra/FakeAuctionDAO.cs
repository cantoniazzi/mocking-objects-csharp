using MockProjetct.domain;
using System.Collections.Generic;
using System;

namespace MockProjetct.infra
{
    public class FakeAuctionDAO : AuctionRepository
    {
        private static List<Auction> auctions = new List<Auction>();

        public List<Auction> Closed()
        {
            List<Auction> filtered = new List<Auction>();
            foreach (var a in auctions)
            {
                if (a.closed)
                    filtered.Add(a);
            }
            return filtered;
        }

        public List<Auction> Opened()
        {
            List<Auction> opened = new List<Auction>();

            foreach (var a in auctions)
            {
                if (!a.closed)
                    opened.Add(a);
            }

            return opened;
        }

        public void Save(Auction auction)
        {
            auctions.Add(auction);
        }

        public virtual void Update(Auction auction) { }

    }
}
