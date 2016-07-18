using MockProjetct.domain;
using MockProjetct.infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockProjetct.service
{
    public class PaymentGenerator
    {
        private FakeAuctionDAO auctionDAO;
        private PaymentDAO paymentDAO;
        private Rater rater;
        private SystemWatch watch;

        public PaymentGenerator(FakeAuctionDAO auctionDAO,
                                PaymentDAO paymentDAO, 
                                Rater rater)
        {
            this.auctionDAO = auctionDAO;
            this.paymentDAO = paymentDAO;
            this.rater = rater;
            this.watch = new SystemWatch();
        }

        public PaymentGenerator(FakeAuctionDAO auctionDAO, Rater rater, PaymentDAO paymentDAO, Relogio relogio)
        {
            this.auctionDAO = auctionDAO;
            this.rater = rater;
            this.paymentDAO = paymentDAO;
            this.watch = watch;
        }


        public virtual void Generate()
        {
            List<Auction> closed = auctionDAO.Closed();

            foreach (var a in closed)
            {
                this.rater.Rate(a);

                Payment payment = new Payment(this.rater.highestValue, DateTime.Today);

                this.paymentDAO.Save(payment);
            }

        }

        public virtual DateTime NextWorkingDay()
        {
            DateTime date = DateTime.Today;
            DayOfWeek dayOfWeek = date.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(2);

            if (dayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1);

            return date;
        }

    }
}
