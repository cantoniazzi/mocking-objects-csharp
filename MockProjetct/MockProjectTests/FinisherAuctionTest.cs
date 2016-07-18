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
            var sendEmail = new Mock<SendEmail>();

            //tell mock to returns old auctions when call the currents auctions
            dao.Setup(a => a.Opened()).Returns(oldAuctions);
            FinisherAuction finisher = new FinisherAuction(dao.Object, sendEmail.Object);
            finisher.Finish();

            Assert.AreEqual(2, oldAuctions.Count);
            Assert.IsTrue(oldAuctions[0].closed);
            Assert.IsTrue(oldAuctions[1].closed);

            //verify update auctions
            dao.Verify(a => a.Update(auction1),Times.Once());
            dao.Verify(a => a.Update(auction2),Times.Once());
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
            var sendEmail = new Mock<SendEmail>();

            //tell mock to returns old auctions when call the currents auctions
            dao.Setup(a => a.Opened()).Returns(oldAuctions);
            FinisherAuction finisher = new FinisherAuction(dao.Object, sendEmail.Object);
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
            var sendEmail = new Mock<SendEmail>();

            //tell mock to returns old auctions when call the currents auctions
            dao.Setup(a => a.Opened()).Returns(new List<Auction>());
            FinisherAuction finisher = new FinisherAuction(dao.Object, sendEmail.Object);
            finisher.Finish();

            Assert.AreEqual(0,finisher.total);
        }

        [TestMethod]
        public void ShouldNotUpdateFinishedAuctions()
        {
            DateTime date = DateTime.Today;

            Auction auction1 = new Auction("Dvd player");
            auction1.InDate(date);

            List<Auction> listReturn = new List<Auction>();
            listReturn.Add(auction1);

            var dao = new Mock<AuctionRepository>();
            var fakeSendEmail = new Mock<SendEmail>();

            dao.Setup(a => a.Opened()).Returns(listReturn);

            FinisherAuction finisher = new FinisherAuction(dao.Object, fakeSendEmail.Object);
            finisher.Finish();

            dao.Verify(a => a.Update(auction1),Times.Never());
        }

        [TestMethod]
        public void ShouldExecuteEvenWhenDAOFail()
        {
            DateTime date = new DateTime(2014, 05, 05);

            Auction auction1 = new Auction("TV");
            auction1.InDate(date);

            Auction auction2 = new Auction("DiskMan");
            auction2.InDate(date);

            List<Auction> listReturn = new List<Auction>();
            listReturn.Add(auction1);
            listReturn.Add(auction2);

            var dao = new Mock<AuctionRepository>();
            dao.Setup(a => a.Opened()).Returns(listReturn);

            var sendEmail = new Mock<SendEmail>();

            dao.Setup(a => a.Update(auction1)).Throws(new Exception());

            FinisherAuction finisher = new FinisherAuction(dao.Object, sendEmail.Object);
            finisher.Finish();

            dao.Verify(a => a.Update(auction2));
            sendEmail.Verify(s => s.Send(auction2));

            sendEmail.Verify(s => s.Send(auction1), Times.Never());
        }
    
        [TestMethod]
        public void ShouldExecuteEvenWhenSendEmailFail()
        {
            DateTime date = new DateTime(2014, 05, 05);

            Auction auction1 = new Auction("Playstation");
            auction1.InDate(date);
            Auction auction2 = new Auction("Super Nes");
            auction2.InDate(date);

            List<Auction> listReturn = new List<Auction>();
            listReturn.Add(auction1);
            listReturn.Add(auction2);

            var dao = new Mock<AuctionRepository>();
            dao.Setup(a => a.Opened()).Returns(listReturn);

            var sendEmail = new Mock<SendEmail>();
            sendEmail.Setup(s => s.Send(auction1)).Throws(new Exception());
            
            FinisherAuction finisher = new FinisherAuction(dao.Object, sendEmail.Object);
            finisher.Finish();

            dao.Verify(a => a.Update(auction2));
            sendEmail.Verify(s => s.Send(auction2));
        }

        [TestMethod]
        public void DontCallSendEmailWhenAllAuctionFail()
        {
            DateTime date = new DateTime(2014, 05, 05);

            Auction auction1 = new Auction("Playstation");
            auction1.InDate(date);
            Auction auction2 = new Auction("Super Nes");
            auction2.InDate(date);

            List<Auction> listReturn = new List<Auction>();
            listReturn.Add(auction1);
            listReturn.Add(auction2);

            var dao = new Mock<AuctionRepository>();
            dao.Setup(a => a.Opened()).Returns(listReturn);

            var sendEmail = new Mock<SendEmail>();

            dao.Setup(a => a.Update(auction1)).Throws(new Exception());
            dao.Setup(a => a.Update(auction2)).Throws(new Exception());

            FinisherAuction finisher = new FinisherAuction(dao.Object, sendEmail.Object);
            finisher.Finish();

            sendEmail.Verify(s => s.Send(auction1), Times.Never());
            sendEmail.Verify(s => s.Send(auction2), Times.Never());
        }

        [TestMethod]
        public void DontCallSendEmailWhenAnyAuctionFail()
        {
            DateTime date = new DateTime(2014, 05, 05);

            Auction auction1 = new Auction("Playstation");
            auction1.InDate(date);
            Auction auction2 = new Auction("Super Nes");
            auction2.InDate(date);

            List<Auction> listReturn = new List<Auction>();
            listReturn.Add(auction1);
            listReturn.Add(auction2);

            var dao = new Mock<AuctionRepository>();
            dao.Setup(a => a.Opened()).Returns(listReturn);

            var sendEmail = new Mock<SendEmail>();

            dao.Setup(a => a.Update(auction1)).Throws(new Exception());
            dao.Setup(a => a.Update(auction2)).Throws(new Exception());

            FinisherAuction finisher = new FinisherAuction(dao.Object, sendEmail.Object);
            finisher.Finish();

            sendEmail.Verify(s => s.Send(It.IsAny<Auction>()), Times.Never());
        }
    }
}
