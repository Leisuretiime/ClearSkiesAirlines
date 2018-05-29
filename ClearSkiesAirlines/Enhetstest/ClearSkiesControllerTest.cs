using System;
using System.Web.Mvc;
using ClearSkiesAirlines.Controllers;
using ClearSkiesAirlines.DAL;
using ClearSkiesAirlines.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.TestHelper;
using PagedList.Mvc;

namespace Enhetstest
{
    [TestClass]
    public class ClearSkiesControllerTest
    {

        //Login
        [TestMethod]
        public void Login()
        {
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);

            Model.Login info = new Model.Login()
            {
                Brukernavn = "Admin",
                Passord = "Admin"
            };

            // Act
            var result = (RedirectToRouteResult)controller.Login(info);

            // Assert
            result.AssertActionRedirect().ToAction("homePage");
            Assert.AreEqual(true, controller.Session["LoggetInn"]);

        }

        [TestMethod]
        public void Login_IKKE_OK()
        {
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);

            Model.Login info = new Model.Login()
            {
                Brukernavn = "",
                Passord = ""
            };

            // Act
            var result = (ViewResult)controller.Login(info);

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewBag.Innlogget);
            Assert.AreEqual(false, controller.Session["LoggetInn"]);
        }

        [TestMethod]
        public void testLagSalt()
        {
            //Arrange
            var DBSTUB = new AirlineDALStub();

            //Act
            var result = DBSTUB.lagSalt();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void testLagHash()
        {
            //Arrange
            var DBSTUB = new AirlineDALStub();

            //Act
            var result = DBSTUB.lagHash("testPassord");

            //Assert
            Assert.IsNotNull(result);
        }


        //Reise
        [TestMethod]
		public void RegistrerReise_OK()
		{
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            DateTime dato = new DateTime(2017, 10, 20);
			DateTime tid1 = new DateTime(2017, 10, 20, 12, 00, 0);
			DateTime tid2 = new DateTime(2017, 10, 20, 14, 00, 0);
			var reise = new Model.ReiseReg()
			{
				Fra = "Oslo",
				Destinasjon = "Bergen",
				Avreise = dato,
				AvreiseTid = tid1,
				Ankomst = dato,
				AnkomstTid = tid2,
				Kapasitet = 150,
				Pris = 299
			};

            //Act
            var actionResult = (RedirectToRouteResult)controller.RegistrerReise(reise);

            //Assert
            actionResult.AssertActionRedirect().ToAction("AdministrerReiser");


        }

        [TestMethod]
		public void RegistrerReise_IKKE_OK()
		{
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            DateTime dato = new DateTime(2017, 10, 20);
            DateTime tid1 = new DateTime(2017, 10, 20, 12, 00, 0);
            DateTime tid2 = new DateTime(2017, 10, 20, 14, 00, 0);
            var reise = new Model.ReiseReg()
            {
                Fra = "London",
                Destinasjon = "Bergen",
                Avreise = dato,
                AvreiseTid = tid1,
                Ankomst = dato,
                AnkomstTid = tid2,
                Kapasitet = 150,
                Pris = 299
            };

            //Act
            var actionResult = (ViewResult)controller.RegistrerReise(reise);

            //Assert
            Assert.AreEqual(actionResult.ViewName, "");
            Assert.IsNotNull(actionResult.ViewBag.registrert);
            Assert.AreEqual("Feil", actionResult.ViewBag.registrert);
        }

		[TestMethod]
		public void RegistrerReise_FEIL_DATO()
		{
			//Arrange
			var SessionMock = new TestControllerBuilder();
			var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
			SessionMock.InitializeController(controller);
			controller.Session["LoggetInn"] = true;
			DateTime dato1 = new DateTime(2017, 10, 20);
			DateTime dato2 = new DateTime(2017, 10, 19);
			DateTime tid1 = new DateTime(2017, 10, 20, 12, 00, 0);
			DateTime tid2 = new DateTime(2017, 10, 20, 14, 00, 0);
			var reise = new Model.ReiseReg()
			{
				Fra = "Oslo",
				Destinasjon = "Bergen",
				Avreise = dato1,
				AvreiseTid = tid1,
				Ankomst = dato2,
				AnkomstTid = tid2,
				Kapasitet = 150,
				Pris = 299
			};

			//Act
			var actionResult = (ViewResult)controller.RegistrerReise(reise);

			//Assert
			Assert.AreEqual(actionResult.ViewName, "");
			Assert.IsNotNull(actionResult.ViewBag.registrert);
			Assert.AreEqual("Feil", actionResult.ViewBag.registrert);
		}

        [TestMethod]
        public void slettReise_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var resultat = (RedirectToRouteResult)controller.slettReise(1);

            //Assert
            resultat.AssertActionRedirect().ToAction("AdministrerReiser");
        }

        [TestMethod]
        public void slettReise_IKKE_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var resultat = (RedirectToRouteResult)controller.slettReise(0);

            //Assert
            resultat.AssertActionRedirect().ToAction("AdministrerReiser");
            Assert.AreEqual(true, controller.Session["FeilReise"]);
        }

        [TestMethod]
        public void endreReise_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            DateTime dato = new DateTime(2017, 10, 20);
            DateTime tid1 = new DateTime(2017, 10, 20, 12, 00, 0);
            DateTime tid2 = new DateTime(2017, 10, 20, 14, 00, 0);
            Model.ReiseReg reise = new Model.ReiseReg()
            {
                Fra = "Oslo",
                Destinasjon = "Bergen",
                Avreise = dato,
                AvreiseTid = tid1,
                Ankomst = dato,
                AnkomstTid = tid2,
                Kapasitet = 150,
                Pris = 299
            };

            //Act
            var result = (RedirectToRouteResult)controller.endreReise(1, reise);

            //Assert
            result.AssertActionRedirect().ToAction("AdministrerReiser");
        }

        [TestMethod]
        public void endreReise_FEIL_DATO()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            DateTime dato = new DateTime(2017, 10, 20);
            DateTime tid1 = new DateTime(2017, 10, 20, 12, 00, 0);
            DateTime tid2 = new DateTime(2017, 10, 20, 10, 00, 0);
            Model.ReiseReg reise = new Model.ReiseReg()
            {
                Fra = "Oslo",
                Destinasjon = "Bergen",
                Avreise = dato,
                AvreiseTid = tid1,
                Ankomst = dato,
                AnkomstTid = tid2,
                Kapasitet = 150,
                Pris = 299
            };

            //Act
            var result = (ViewResult)controller.endreReise(2, reise);

            //Assert
            Assert.AreEqual(result.ViewName, "");
            Assert.IsNotNull(result.ViewBag.Feil);
            Assert.AreEqual(true, result.ViewBag.Feil);
        }

        [TestMethod]
        public void endreReise_FEIL()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            DateTime dato = new DateTime(2017, 10, 20);
            DateTime tid1 = new DateTime(2017, 10, 20, 12, 00, 0);
            DateTime tid2 = new DateTime(2017, 10, 20, 14, 00, 0);
            Model.ReiseReg reise = new Model.ReiseReg()
            {
                Fra = "Oslo",
                Destinasjon = "Bergen",
                Avreise = dato,
                AvreiseTid = tid1,
                Ankomst = dato,
                AnkomstTid = tid2,
                Kapasitet = 150,
                Pris = 299
            };

            //Act
            var result = (ViewResult)controller.endreReise(2, reise);

            //Assert
            Assert.AreEqual(result.ViewName, "");
            Assert.IsNotNull(result.ViewBag.Feil);
            Assert.AreEqual(true, result.ViewBag.Feil);
        }

        //Kunde
        [TestMethod]
        public void hentEnKunde_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            Model.KundeView kunde = new Model.KundeView()
            {
                kundeId = 1,
                Fornavn = "Ole",
                Etternavn = "Olesen",
                Adresse = "Olegaten 1",
                Epost = "ole@ole.com",
                PostNr = "1234",
                PostSted = "Oslo",
                Telefon = "123456789"
            };

            //Act
            var result = (ViewResult)controller.endreKunde(1);
            var funnetKunde = result.Model as Model.KundeView;

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(kunde.Adresse, funnetKunde.Adresse);
            Assert.AreEqual(kunde.Epost, funnetKunde.Epost);
            Assert.AreEqual(kunde.Etternavn, funnetKunde.Etternavn);
            Assert.AreEqual(kunde.Fornavn, funnetKunde.Fornavn);
            Assert.AreEqual(1, funnetKunde.kundeId);
            Assert.AreEqual(kunde.PostNr, funnetKunde.PostNr);
            Assert.AreEqual(kunde.PostSted, funnetKunde.PostSted);
            Assert.AreEqual(kunde.Telefon, funnetKunde.Telefon);
        }

        [TestMethod]
        public void hentEnKunde_IKKE_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            Model.KundeView kunde = new Model.KundeView()
            {
                kundeId = 1,
                Fornavn = "Ole",
                Etternavn = "Olesen",
                Adresse = "Olegaten 1",
                Epost = "ole@ole.com",
                PostNr = "1234",
                PostSted = "Oslo",
                Telefon = "123456789"
            };

            //Act
            var result = (ViewResult)controller.endreKunde(2);
            var funnetKunde = result.Model as Model.KundeView;

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreNotEqual(kunde.Adresse, funnetKunde.Adresse);
            Assert.AreNotEqual(kunde.Epost, funnetKunde.Epost);
            Assert.AreNotEqual(kunde.Etternavn, funnetKunde.Etternavn);
            Assert.AreNotEqual(kunde.Fornavn, funnetKunde.Fornavn);
            Assert.AreNotEqual(1, funnetKunde.kundeId);
            Assert.AreNotEqual(kunde.PostNr, funnetKunde.PostNr);
            Assert.AreNotEqual(kunde.PostSted, funnetKunde.PostSted);
            Assert.AreNotEqual(kunde.Telefon, funnetKunde.Telefon);
        }

        [TestMethod]
        public void endreKunde_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            Model.KundeView kunde = new Model.KundeView()
            {
                kundeId = 1,
                Fornavn = "Ole",
                Etternavn = "Duck",
                Adresse = "Olegaten 1",
                Epost = "ole@ole.com",
                PostNr = "1234",
                PostSted = "Oslo",
                Telefon = "123456789"
            };

            //Act
            var result = (RedirectToRouteResult)controller.endreKunde(1, kunde);

            //Assert
            result.AssertActionRedirect().ToAction("administrerKunder");
        }

        [TestMethod]
        public void endreKunde_IKKE_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            Model.KundeView kunde = new Model.KundeView()
            {
                kundeId = 2,
                Fornavn = "Ole",
                Etternavn = "Duck",
                Adresse = "Olegaten 1",
                Epost = "ole@ole.com",
                PostNr = "1234",
                PostSted = "Oslo",
                Telefon = "123456789"
            };

            //Act
            var result = (ViewResult)controller.endreKunde(2, kunde);

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(true, result.ViewBag.Feil);
        }

        [TestMethod]
        public void slettKunde_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var result = (RedirectToRouteResult)controller.slettKunde(0);

            //Assert
            result.AssertActionRedirect().ToAction("administrerKunder");
        }

        [TestMethod]
        public void slettKunde_IKKE_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;


            //Act
            var result = (RedirectToRouteResult)controller.slettKunde(1);

            //Assert
            result.AssertActionRedirect().ToAction("administrerKunder");
            Assert.AreEqual(true, controller.Session["FeilKunde"]);
        }

        [TestMethod]
        public void hentAlleKunder_OK()
        {
            //Arrage
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            var kunde = new Model.KundeView()
            {
                kundeId = 1,
                Fornavn = "Ole",
                Etternavn = "Olesen",
                Adresse = "Olegaten 1",
                Epost = "ole@ole.com",
                PostNr = "1234",
                PostSted = "Oslo",
                Telefon = "123456789"
            };

            //Act
            var result = (ViewResult)controller.administrerKunder("", "", "", 0);
            var listResult = (PagedList.IPagedList<Model.KundeView>)result.Model;
            List<Model.KundeView> konvertert = new List<Model.KundeView>();

            //Assert
            Assert.AreEqual("", result.ViewName);
            foreach (var k in listResult)
            {
                Model.KundeView kundeKonv = new Model.KundeView()
                {
                    Fornavn = k.Fornavn,
                    Etternavn = k.Etternavn,
                    kundeId = k.kundeId
                };
                konvertert.Add(kundeKonv);
            }

            foreach(var k in konvertert)
            {
                Assert.AreEqual(kunde.Etternavn, k.Etternavn);
                Assert.AreEqual(kunde.Fornavn, k.Fornavn);
                Assert.AreEqual(kunde.kundeId, k.kundeId);
            }

        }


        //Bestillinger
        [TestMethod]
        public void slettBestilling_OK()
        {
            //Arrage
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var result = (RedirectToRouteResult)controller.slettBestilling(0);

            //Assert
            result.AssertActionRedirect().ToAction("AdministrerBestillinger");
        }

        [TestMethod]
        public void slettBestilling_IKKE_OK ()
        {
            //Arrage
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var result = (RedirectToRouteResult)controller.slettBestilling(1);

            //Assert
            result.AssertActionRedirect().ToAction("AdministrerBestillinger");
            Assert.AreEqual(true, controller.Session["FeilBestilling"]);
        }

        [TestMethod]
        public void endreBestilling_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            var billetter = new Model.BestillingBilletter()
            {
                BillettId = 1,
                Fra = "Gatwick, UK",
                Til = "Gardemoen, Norge",
                Avreise = new DateTime(2017, 10, 18, 16, 10, 0, 0),
                PassasjerFornavn = "Ole",
                PassasjerEtternavn = "Olesen"
            };

            //Act
            var resultat = (ViewResult)controller.endreBestilling(1);

            //Assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void endreBestilling_IKKE_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var resultat = (ViewResult)controller.endreBestilling(1);

            //Assert
            Assert.AreEqual(resultat.ViewName, "");
            Assert.AreEqual(null, resultat.ViewBag.Feil);
        }

        [TestMethod]
        public void slettBillett_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var result = controller.slettBillett(1);

            //Asssert
            result.AssertActionRedirect().ToAction("endreBestilling");
        }

        [TestMethod]
        public void slettBillett_IKKE_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var result = (RedirectToRouteResult)controller.slettBillett(2);

            //Assert
            result.AssertActionRedirect().ToAction("endreBestilling");
            Assert.AreEqual(true, controller.Session["FeilBillett"]);
        }

		

		[TestMethod]
        public void hentAlleBestillinger_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            Model.BestillingView best = new Model.BestillingView()
            {
                AntallBilletter = 2,
                Eier = "Donal Duck",
                HandelId = 1,
                TotalPris = 598
            };

            //Act
            var result = (ViewResult)controller.AdministrerBestillinger("", "", "", 0);
            var listResult = (PagedList.IPagedList<Model.BestillingView>)result.Model;
            List<Model.BestillingView> konvertert = new List<Model.BestillingView>();

            //Assert
            Assert.AreEqual("", result.ViewName);
            foreach (var k in listResult)
            {
                Model.BestillingView bestKonv = new Model.BestillingView()
                {
                    AntallBilletter = k.AntallBilletter,
                    Eier = k.Eier,
                    HandelId = k.HandelId,
                    TotalPris = k.TotalPris
                };
                konvertert.Add(bestKonv);
            }

            foreach (var k in konvertert)
            {
                Assert.AreEqual(best.HandelId, k.HandelId);
                Assert.AreEqual(best.Eier, k.Eier);
                Assert.AreEqual(best.AntallBilletter, k.AntallBilletter);
                Assert.AreEqual(best.TotalPris, k.TotalPris);
            }
        }

        [TestMethod]
        public void hentEnBillett_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            Model.BestillingBilletter funnetBillett = new Model.BestillingBilletter()
            {
                BillettId = 12,
                Fra = "Oslo",
                Til = "Bergen",
                PassasjerEtternavn = "Duck",
                PassasjerFornavn = "Donald",
                Avreise = new DateTime(2017, 10, 20, 14, 0, 0)
            };

            //Act
            var result = (ViewResult)controller.endreBillett(1);
            var hentetBill = result.Model as Model.BestillingBilletter;

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(funnetBillett.Avreise, hentetBill.Avreise);
            Assert.AreEqual(funnetBillett.BillettId, hentetBill.BillettId);
            Assert.AreEqual(funnetBillett.Fra, hentetBill.Fra);
            Assert.AreEqual(funnetBillett.PassasjerFornavn, hentetBill.PassasjerFornavn);
            Assert.AreEqual(funnetBillett.PassasjerEtternavn, hentetBill.PassasjerEtternavn);
            Assert.AreEqual(funnetBillett.Til, hentetBill.Til);
        }

        [TestMethod]
        public void hentEnBillett_FEIL_BILL()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            Model.BestillingBilletter funnetBillett = new Model.BestillingBilletter()
            {
                BillettId = 12,
                Fra = "Oslo",
                Til = "Bergen",
                PassasjerEtternavn = "Duck",
                PassasjerFornavn = "Donald",
                Avreise = new DateTime(2017, 10, 20, 14, 0, 0)
            };

            //Act
            var result = (ViewResult)controller.endreBillett(8);
            var hentetBill = result.Model as Model.BestillingBilletter;

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(funnetBillett.Avreise, hentetBill.Avreise);
            Assert.AreEqual(funnetBillett.BillettId, hentetBill.BillettId);
            Assert.AreEqual(funnetBillett.Fra, hentetBill.Fra);
            Assert.AreEqual(funnetBillett.PassasjerFornavn, hentetBill.PassasjerFornavn);
            Assert.AreEqual(funnetBillett.PassasjerEtternavn, hentetBill.PassasjerEtternavn);
            Assert.AreNotEqual(funnetBillett.Til, hentetBill.Til);
        }

        [TestMethod]
        public void hentEnReise_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            Model.ReiseReg funnetReise = new Model.ReiseReg()
            {
                Ankomst = new DateTime(2017, 12, 20),
                AnkomstTid = new DateTime(2017, 12, 20, 14, 0, 0),
                Avreise = new DateTime(2017, 12, 20),
                AvreiseTid = new DateTime(2017, 12, 20, 14, 0, 0),
                Destinasjon = "Bergen",
                Fra = "Oslo",
                Kapasitet = 150,
                Pris = 299
            };

            //Act
            var result = (ViewResult)controller.endreReise(2);
            var hentetReise = result.Model as Model.ReiseReg;

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(funnetReise.Ankomst, hentetReise.Ankomst);
            Assert.AreEqual(funnetReise.AnkomstTid, hentetReise.AnkomstTid);
            Assert.AreEqual(funnetReise.Avreise, hentetReise.Avreise);
            Assert.AreEqual(funnetReise.AvreiseTid, hentetReise.AvreiseTid);
            Assert.AreEqual(funnetReise.Destinasjon, hentetReise.Destinasjon);
            Assert.AreEqual(funnetReise.Fra, hentetReise.Fra);
            Assert.AreEqual(funnetReise.Pris, funnetReise.Pris);
            Assert.AreEqual(funnetReise.Kapasitet, funnetReise.Kapasitet);
        }

        [TestMethod]
        public void hentEnReise_FANT_FAIL()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            Model.ReiseReg funnetReise = new Model.ReiseReg()
            {
                Ankomst = new DateTime(2017, 12, 20),
                AnkomstTid = new DateTime(2017, 12, 20, 14, 0, 0),
                Avreise = new DateTime(2017, 12, 20),
                AvreiseTid = new DateTime(2017, 12, 20, 14, 0, 0),
                Destinasjon = "New York",
                Fra = "Oslo",
                Kapasitet = 150,
                Pris = 500
            };

            //Act
            var result = (ViewResult)controller.endreReise(6);
            var hentetReise = result.Model as Model.ReiseReg;

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(funnetReise.Ankomst, hentetReise.Ankomst);
            Assert.AreEqual(funnetReise.AnkomstTid, hentetReise.AnkomstTid);
            Assert.AreEqual(funnetReise.Avreise, hentetReise.Avreise);
            Assert.AreEqual(funnetReise.AvreiseTid, hentetReise.AvreiseTid);
            Assert.AreNotEqual(funnetReise.Destinasjon, hentetReise.Destinasjon);
            Assert.AreNotEqual(funnetReise.Fra, hentetReise.Fra);
            Assert.AreNotEqual(funnetReise.Pris, hentetReise.Pris);
            Assert.AreEqual(funnetReise.Kapasitet, hentetReise.Kapasitet);
        }

        [TestMethod]
        public void hentAlleReise()
        {
            //Arrange
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            var nyReise = new Model.ReiseView()
            {
                ReiseId = 1,
                Fra = "Gatwick, UK",
                Destinasjon = "Gardermoen, Norge",
                Avreise = new DateTime(2017, 10, 18, 16, 10, 0, 0),
                Ankomst = new DateTime(2017, 10, 18, 17, 40, 0, 0),
                Varighet = "1 time og 30 minutter",
                Kapasitet = 20,
                Pris = 299
            };

            //Act
            var result = (ViewResult)controller.AdministrerReiser("","","",0);
            var listResult = (PagedList.IPagedList<Model.ReiseView>)result.Model;
            List<Model.ReiseView> konvertert = new List<Model.ReiseView>();

            //Assert
            Assert.AreEqual("", result.ViewName);

            foreach(var r in listResult)
            {
                Assert.AreEqual(r.Destinasjon, nyReise.Destinasjon);
                Assert.AreEqual(r.Avreise, nyReise.Avreise);
                Assert.AreEqual(r.Ankomst, nyReise.Ankomst);
                Assert.AreEqual(r.Fra, nyReise.Fra);
                Assert.AreEqual(r.Kapasitet, nyReise.Kapasitet);
                Assert.AreEqual(r.Pris, nyReise.Pris);
                Assert.AreEqual(r.ReiseId, nyReise.ReiseId);
                Assert.AreEqual(r.Varighet, nyReise.Varighet);
            }
        }

        [TestMethod]
        public void endreBillett_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            Model.BestillingBilletter billett = new Model.BestillingBilletter()
            {
                Avreise = new DateTime(2017, 10, 15, 10, 20, 0),
                Fra = "Oslo",
                Til = "Bergen",
                BillettId = 2,
                PassasjerEtternavn = "Donald",
                PassasjerFornavn = "Duck"
            };

            //Act
            var result = (RedirectToRouteResult)controller.endreBillett(2, billett);

            //Assert
            result.AssertActionRedirect().ToAction("endreBestilling");
        }


        [TestMethod]
        public void endreBillett_IKKE_OK()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController(new AirlineLogikk(new AirlineDALStub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            Model.BestillingBilletter billett = new Model.BestillingBilletter()
            {
                Avreise = new DateTime(2017, 10, 15, 10, 20, 0),
                Fra = "Oslo",
                Til = "Bergen",
                BillettId = 2,
                PassasjerEtternavn = "Donald",
                PassasjerFornavn = "Duck"
            };

            //Act
            var result = (ViewResult)controller.endreBillett(1, billett);

            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(true, controller.ViewBag.Feil);
        }

        //Views
        [TestMethod]
        public void RegistrerReise_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;


            //Act
            var actionResult = (RedirectToRouteResult)controller.RegistrerReise();

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void RegistrerReise_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;


            //Act
            var actionResult = (ViewResult)controller.RegistrerReise();

            //Assert
            Assert.AreEqual("", actionResult.ViewName);
        }

        [TestMethod]
        public void EndreReise_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;


            //Act
            var actionResult = (RedirectToRouteResult)controller.endreReise(1);

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void AdministrerReiser_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;
            
            //Act
            var actionResult = (RedirectToRouteResult)controller.AdministrerReiser("","","",0);

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void endreBillett_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            //Act
            var actionResult = (RedirectToRouteResult)controller.endreBillett(0);

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void endreBestilling_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            //Act
            var actionResult = (RedirectToRouteResult)controller.endreBestilling(0);

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void AdministrerBestillinger_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            //Act
            var actionResult = (RedirectToRouteResult)controller.AdministrerBestillinger("","","",0);

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void AdministrerKunder_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            //Act
            var actionResult = (RedirectToRouteResult)controller.administrerKunder("", "", "", 0);

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void endreKunde_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            //Act
            var actionResult = (RedirectToRouteResult)controller.endreKunde(1);

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void Login_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            //Act
            var actionResult = (ViewResult)controller.Login();

            //Assert
            Assert.AreEqual("", actionResult.ViewName);
        }

        [TestMethod]
        public void Login_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var actionResult = (RedirectToRouteResult)controller.Login();

            //Assert
            Assert.AreEqual("homePage", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void homePage_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var actionResult = (ViewResult)controller.homePage();

            //Assert
            Assert.AreEqual("", actionResult.ViewName);
        }

        [TestMethod]
        public void homePage_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            //Act
            var actionResult = (RedirectToRouteResult)controller.homePage();

            //Assert
            Assert.AreEqual("Login", actionResult.RouteValues.Values.First());
        }

        [TestMethod]
        public void loadNavigation_IKKE_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            //Act
            var actionResult = (PartialViewResult)controller.loadNavigation();

            //Assert
            Assert.AreEqual("tomtView", actionResult.ViewName);
        }

        [TestMethod]
        public void loadNavigation_LOGGET_INN()
        {
            //Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new ClearSkiesController();
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            //Act
            var actionResult = (PartialViewResult)controller.loadNavigation();

            //Assert
            Assert.AreEqual("navigationMenu", actionResult.ViewName);
        }

    }
}
