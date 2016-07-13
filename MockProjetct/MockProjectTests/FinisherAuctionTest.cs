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
    public class FinisherAuctionTest
    {
        [TestMethod]
        public void ShouldFinishAuctionThatStartedLastWeek()
        {
            DateTime date = new DateTime(2016, 07, 01);

            Auction auction1 = new Auction("TV Stereo");
            auction1.InDate(date);

            Auction auction2 = new Auction("Video Game");
            auction2.InDate(date);

            List<Auction> oldAuctions = new List<Auction>();
            oldAuctions.Add(auction1);
            oldAuctions.Add(auction2);
            
            //create the mock
            var dao = new Mock<AuctionRepository>();

            //tell mock to returns old auctions when call the currents auctions
            dao.Setup(a => a.Opened()).Returns(oldAuctions);
            FinisherAuction finisher = new FinisherAuction(dao.Object);
            finisher.Finish();

            Assert.AreEqual(2, oldAuctions.Count);
            Assert.IsTrue(oldAuctions[0].closed);
            Assert.IsTrue(oldAuctions[1].closed);

        }
        
        [TestMethod]
        public void ShouldNotFinishAuctionThatStartToday()
        {
            DateTime date = DateTime.Today;

            Auction auction1 = new Auction("Skate board");
            auction1.InDate(date);

            Auction auction2 = new Auction("Star Wars blu-ray");
            auction2.InDate(date);

            List<Auction> oldAuctions = new List<Auction>();
            oldAuctions.Add(auction1);
            oldAuctions.Add(auction2);

            //create the mock
            var dao = new Mock<AuctionRepository>();

            //tell mock to returns old auctions when call the currents auctions
            dao.Setup(a => a.Opened()).Returns(oldAuctions);
            FinisherAuction finisher = new FinisherAuction(dao.Object);
            finisher.Finish();

            Assert.AreEqual(2, oldAuctions.Count);
            Assert.IsFalse(oldAuctions[0].closed);
            Assert.IsFalse(oldAuctions[1].closed);
        }

        [TestMethod]
        public void ShouldNotDoAnythingIfNotHaveAuction()
        {
            //create the mock
            var dao = new Mock<AuctionRepository>();

            //tell mock to returns old auctions when call the currents auctions
            dao.Setup(a => a.Opened()).Returns(new List<Auction>());
            FinisherAuction finisher = new FinisherAuction(dao.Object);
            finisher.Finish();

            Assert.AreEqual(0,finisher.total);
        }

    }
}
