using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockProjetct.domain;
using MockProjetct.infra;
using MockProjetct.service;
using Moq;
using System;
using System.Collections.Generic;

namespace MockProjectTests
{
    [TestClass]
    public class PaymentGeneratorTest
    {
        [TestMethod]
        public void ShouldGeneratePaymentToAuctionClosed()
        {
            var auctionDAO = new Mock<FakeAuctionDAO>();
            var rater = new Rater();
            var paymentDAO = new Mock<PaymentDAO>();

            Auction auction1 = new Auction("Xbox");
            auction1.Propose(new Bid(new User("Jorge"), 2000));
            auction1.Propose(new Bid(new User("Maria"), 2500));
            auction1.InDate(new DateTime(1999, 5, 1));

            List<Auction> auctions = new List<Auction>();
            auctions.Add(auction1);

            auctionDAO.Setup(a => a.Closed()).Returns(auctions);
            //rater.Setup(r => r.highestValue).Returns(2500);

            Payment paymentReturn = null;

            paymentDAO.Setup(p => p.Save(It.IsAny<Payment>())).Callback<Payment>(r => paymentReturn = r);

            PaymentGenerator generator = new PaymentGenerator(auctionDAO.Object, paymentDAO.Object, rater);
            generator.Generate();

            Assert.AreEqual(2500,paymentReturn.value);
        }

        [TestMethod]
        public void ShouldPassToNextWorkingDay()
        {
            var auctionDAO = new Mock<FakeAuctionDAO>();
            var paymentDAO = new Mock<PaymentDAO>();

            Auction auction1 = new Auction("GameBoy");
            auction1.Propose(new Bid(new User("Bob"), 500));
            auction1.Propose(new Bid(new User("Carl"), 1500));

            List<Auction> auctions = new List<Auction>();
            auctions.Add(auction1);

            auctionDAO.Setup(l => l.Closed()).Returns(auctions);

            Payment payment = null;

            paymentDAO.Setup(p => p.Save(It.IsAny<Payment>())).Callback<Payment>(r => payment = r);

            PaymentGenerator generator = new PaymentGenerator(auctionDAO.Object, paymentDAO.Object, new Rater());
            generator.Generate();

            Assert.AreEqual(DayOfWeek.Monday, payment.date.DayOfWeek);

        }

    }
}
