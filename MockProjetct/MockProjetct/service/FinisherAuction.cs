using MockProjetct.domain;
using MockProjetct.infra;
using System;
using System.Collections.Generic;

namespace MockProjetct.service
{
    public class FinisherAuction
    {
        public int total { get; set; }
        private AuctionRepository dao;
        private SendEmail sendEmail;

        public FinisherAuction(AuctionRepository dao, SendEmail sendEmail)
        {
            this.dao = dao;
            this.sendEmail = sendEmail;
            this.total = 0;
        }

        public virtual void Finish()
        {
            List<Auction> allCurrentAuctions = dao.Opened();

            foreach (var a in allCurrentAuctions)
            {
                if (this.StartLasWeek(a))
                {
                    a.Finish();
                    this.total++;
                    dao.Update(a);
                    this.sendEmail.Send(a);
                }
            }
        }

        private bool StartLasWeek(Auction auction)
        {
            return DaysBetween(auction.date, DateTime.Now) >= 7;
        }

        private int DaysBetween(DateTime start, DateTime end)
        {
            int days = (int) (end - start).TotalDays;

            return days;
        }
    }
}
