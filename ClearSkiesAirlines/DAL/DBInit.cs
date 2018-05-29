using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ClearSkiesAirlines.DAL
{
    public class DBInit : DropCreateDatabaseAlways<AirlineDbContext>
    {
        protected override void Seed(AirlineDbContext context)
        {
            var DalLogikk = new AirlineDAL();
            var innSalt = DalLogikk.lagSalt();
            var adminPassord = DalLogikk.lagHash("Admin" + innSalt);

            var nyAdminBruker = new Admin {
                Brukernavn = "Admin",
                Passord = adminPassord,
                Salt = innSalt
            };

            var nyPost = new PostSted
            {
                PostNr = "1234",
                Sted = "Oslo"
            };
            var nyKunde = new Kunde
            {
                Fornavn = "Donald",
                Etternavn = "Duck",
                Epost = "Apalveien@Oslo.no",
                Adresse = "Osloveien 1",
                Telefon = "45645645",
                PostSted = nyPost
            };

            var nyKunde2 = new Kunde
            {
                Fornavn = "Andy",
                Etternavn = "Kapp",
                Epost = "Skolen@Bergen.no",
                Adresse = "Kongeveien 1",
                Telefon = "89967458",
                PostSted = nyPost
            };

            var nyKunde3 = new Kunde
            {
                Fornavn = "Arne",
                Etternavn = "Arnesen",
                Epost = "Skolen@Stavanger.no",
                Adresse = "Dronningveien 1",
                Telefon = "74856925",
                PostSted = nyPost
            };

            var nyReise1 = new Reise
            {
                Til = "Gardemoen, Norge",
                Fra = "Gatwick, UK",
                Avreise = new DateTime(2017, 10, 18, 16, 10, 0, 0),
                Ankomst = new DateTime(2017, 10, 18, 17, 40, 0, 0),
                Varighet = "1 time og 30 minutter",
                Kapasitet = 20,
                Pris = 299
            };

            var nyRetur1 = new Reise
            {
                Til = "Gatwick, UK",
                Fra = "Gardemoen, Norge",
                Avreise = new DateTime(2017, 10, 19, 16, 10, 0, 0),
                Ankomst = new DateTime(2017, 10, 19, 17, 40, 0, 0),
                Varighet = "1 time og 30 minutter",
                Kapasitet = 20,
                Pris = 299
            };

            var nyReise2 = new Reise
            {
                Til = "Gôteborg, Sverige",
                Fra = "Roma, Italia",
                Avreise = new DateTime(2017, 10, 24, 11, 30, 0, 0),
                Ankomst = new DateTime(2017, 10, 24, 13, 40, 0, 0),
                Varighet = "2 timer og 10 minutter",
                Kapasitet = 35,
                Pris = 299
            };

            var nyRetur2 = new Reise
            {
                Til = "Roma, Italia",
                Fra = "Gôteborg, Sverige",
                Avreise = new DateTime(2017, 10, 25, 11, 30, 0, 0),
                Ankomst = new DateTime(2017, 10, 25, 13, 40, 0, 0),
                Varighet = "2 timer og 10 minutter",
                Kapasitet = 35,
                Pris = 299
            };

            var nyReise3 = new Reise
            {

                Til = "Gardemoen, Norge",
                Fra = "Gatwick, UK",
                Avreise = new DateTime(2017, 10, 18, 20, 10, 0, 0),
                Ankomst = new DateTime(2017, 10, 18, 11, 40, 0, 0),
                Varighet = "1 time og 30 minutter",
                Kapasitet = 20,
                Pris = 299
            };

            var nyRetur3 = new Reise
            {
                Til = "Gatwick, UK",
                Fra = "Gardemoen, Norge",
                Avreise = new DateTime(2017, 10, 19, 22, 10, 0, 0),
                Ankomst = new DateTime(2017, 10, 19, 23, 40, 0, 0),
                Varighet = "1 time og 30 minutter",
                Kapasitet = 20,
                Pris = 299
            };

            var nyReise4 = new Reise
            {

                Til = "Gardemoen, Norge",
                Fra = "Gatwick, UK",
                Avreise = new DateTime(2017, 11, 7, 20, 10, 0, 0),
                Ankomst = new DateTime(2017, 11, 7, 11, 40, 0, 0),
                Varighet = "1 time og 30 minutter",
                Kapasitet = 20,
                Pris = 299
            };

            var nyReise5 = new Reise
            {

                Til = "Tromsø, Norge",
                Fra = "Madrid, Spania",
                Avreise = new DateTime(2017, 11, 18, 20, 10, 0, 0),
                Ankomst = new DateTime(2017, 11, 18, 11, 40, 0, 0),
                Varighet = "1 time og 30 minutter",
                Kapasitet = 20,
                Pris = 299
            };

            var nyReise6 = new Reise
            {

                Til = "Gardemoen, Norge",
                Fra = "Gatwick, UK",
                Avreise = new DateTime(2017, 11, 15, 20, 10, 0, 0),
                Ankomst = new DateTime(2017, 11, 15, 11, 40, 0, 0),
                Varighet = "1 time og 30 minutter",
                Kapasitet = 20,
                Pris = 299
            };

            var nyBillett1 = new Billett
            {
                Reise = nyReise1,
                PassasjerEtternavn = "Olsen",
                PassasjerFornavn = "Knut"
            };
            var nyBillett2 = new Billett
            {
                Reise = nyReise1,
                PassasjerEtternavn = "Arnesen",
                PassasjerFornavn = "Arne"
            };
            var nyBillett3 = new Billett
            {
                Reise = nyReise1,
                PassasjerEtternavn = "Olsen",
                PassasjerFornavn = "Birger"
            };
            var nyBillettRet1 = new Billett
            {
                Reise = nyRetur1,
                PassasjerEtternavn = "Olsen",
                PassasjerFornavn = "Knut"
            };
            var nyBillettRet2 = new Billett
            {
                Reise = nyRetur1,
                PassasjerEtternavn = "Arnesen",
                PassasjerFornavn = "Arne"
            };
            var nyBillettRet3 = new Billett
            {
                Reise = nyRetur1,
                PassasjerEtternavn = "Olsen",
                PassasjerFornavn = "Birger"
            };
            List<Billett> billettTilHandel = new List<Billett>();
            billettTilHandel.Add(nyBillett1);
            billettTilHandel.Add(nyBillett2);
            billettTilHandel.Add(nyBillett3);
            billettTilHandel.Add(nyBillettRet1);
            billettTilHandel.Add(nyBillettRet2);
            billettTilHandel.Add(nyBillettRet3);

            var nyHandel = new Handel
            {
                Kontonummer = "1234567898765",
                Billetter = billettTilHandel,
                Kunde = nyKunde,
                TotalPris = 6 * 299
            };

            context.Handler.Add(nyHandel);
            context.PostSted.Add(nyPost);
            context.Kunder.Add(nyKunde);
            context.Kunder.Add(nyKunde2);
            context.Kunder.Add(nyKunde3);
            context.Admins.Add(nyAdminBruker);
            context.Reiser.Add(nyReise1);
            context.Reiser.Add(nyReise2);
            context.Reiser.Add(nyReise3);
            context.Reiser.Add(nyReise4);
            context.Reiser.Add(nyReise5);
            context.Reiser.Add(nyReise6);
            context.Reiser.Add(nyRetur1);
            context.Reiser.Add(nyRetur2);
            context.Reiser.Add(nyRetur3);
            base.Seed(context);
        }
    }
}