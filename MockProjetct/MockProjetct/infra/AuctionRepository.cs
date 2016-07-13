using MockProjetct.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockProjetct.infra
{
    public interface AuctionRepository
    {
        void Save(Auction auction);
        List<Auction> Closed();
        List<Auction> Opened();
        void Update(Auction auction);
    }
}
