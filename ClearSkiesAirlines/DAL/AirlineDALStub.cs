using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Security.Cryptography;

namespace ClearSkiesAirlines.DAL
{
    public class AirlineDALStub : DAL.IAirlineDAL
    {
        public bool LoginTest(Login adminBruker)
        {
            if (adminBruker.Brukernavn == "" && adminBruker.Passord == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Byte[] lagHash(string adminPassord)
        {
            Byte[] hash = new Byte[0];
            return hash;
        }

        public Byte[] lagSalt()
        {
            Byte[] salt = new Byte[0];
            return salt;
        }

        //Kunde
        public List<KundeView> hentAlleKunder()
        {
            var kundeListe = new List<KundeView>();
            var kunde = new KundeView()
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
            kundeListe.Add(kunde);
            kundeListe.Add(kunde);
            kundeListe.Add(kunde);
            return kundeListe;
        }

        public bool slettKunde(int slettID)
        {
            if (slettID == 0)
            {
                return true;
            }
            else
            {
               return false;
            }
        }

        public KundeView hentEnKunde(int id)
        {
            if (id == 1)
            {
                KundeView kunde = new KundeView()
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
                return kunde;
            }
            else
            {
                KundeView kunde = new KundeView()
                {
                    kundeId = 2,
                    Fornavn = "Duck",
                    Etternavn = "Rogers",
                    Adresse = "Olegaten 2",
                    Epost = "Duck@ole.com",
                    PostNr = "1274",
                    PostSted = "Andeby",
                    Telefon = "123458889"
                };
                return kunde;
            }
        }

        public bool endreKunde(int id, KundeView nyKundeData)
        {
            //Dummy metode for at InterFace implementasjon skal være OK,
            KundeView kunde = new KundeView()
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

            if(id == 1)
            {
                kunde.Etternavn = nyKundeData.Etternavn;
            }

            if (kunde.Etternavn == nyKundeData.Etternavn)
            {
                return true;
            } else
            {
                return false;
            }
        }

        //Reise
        public bool RegistrerReise(ReiseReg innReise)
        {
            DateTime dato = new DateTime(2017, 10, 20);
            DateTime tid1 = new DateTime(2017, 10, 20, 12, 00, 0);
            DateTime tid2 = new DateTime(2017, 10, 20, 14, 00, 0);
            if (innReise.Fra == "Oslo"
                && innReise.Destinasjon == "Bergen"
                && innReise.Avreise == dato
                && innReise.AvreiseTid == tid1
                && innReise.Ankomst == dato
                && innReise.AnkomstTid == tid2
                && innReise.Kapasitet == 150
                && innReise.Pris == 299)
            {
                return true;
            }

            return false;
        }

        public List<ReiseView> HentAlleReiser()
        {
            List<ReiseView> reiseListe = new List<ReiseView>();
            var nyReise = new ReiseView()
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
            reiseListe.Add(nyReise);
            reiseListe.Add(nyReise);
            reiseListe.Add(nyReise);
            return reiseListe;
        }

        public bool slettReise(int slettId)
        {
            if (slettId == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public ReiseReg hentEnReise(int id)
        {
            if(id == 2)
            {
                ReiseReg funnetReise = new ReiseReg()
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
                return funnetReise;
            } else
            {
                ReiseReg funnetReise = new ReiseReg()
                {
                    Ankomst = new DateTime(2017, 12, 20),
                    AnkomstTid = new DateTime(2017, 12, 20, 14, 0, 0),
                    Avreise = new DateTime(2017, 12, 20),
                    AvreiseTid = new DateTime(2017, 12, 20, 14, 0, 0),
                    Destinasjon = "Bergen",
                    Fra = "Stavanger",
                    Kapasitet = 150,
                    Pris = 299
                };
                return funnetReise;
            }
        }

		public bool endreReise(int id, ReiseReg nyReiseData)
		{
			DateTime AvreiseInn = nyReiseData.Avreise.Add(nyReiseData.AvreiseTid.TimeOfDay);
			DateTime AnkomstInn = nyReiseData.Ankomst.Add(nyReiseData.AnkomstTid.TimeOfDay);
			if (AvreiseInn > AnkomstInn)
			{
				return false;
			}
			else if (id == 1 && nyReiseData.Kapasitet == 150)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//Bestillinger og billetter
		public List<BestillingView> hentAlleBestillinger()
        {
            //Dummy metode for at InterFace implementasjon skal være OK,
            //Må endre før testing.
            List<BestillingView> liste = new List<BestillingView>();
            BestillingView best = new BestillingView()
            {
                AntallBilletter = 2,
                Eier = "Donal Duck",
                HandelId = 1,
                TotalPris = 598
            };

            liste.Add(best);
            liste.Add(best);
            liste.Add(best);

            return liste;
        }

        public bool slettBestilling(int slettId)
        {
            if(slettId == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public List<BestillingBilletter> hentBestillingBilletter(int id)
        {
            //Dummy metode for at InterFace implementasjon skal være OK,
            //Må endre før testing.
            List<BestillingBilletter> funnetBilletter = new List<BestillingBilletter>();
            return funnetBilletter;
        }
        public BestillingBilletter hentEnBillett(int id)
        {
            if (id == 1) {
                BestillingBilletter funnetBillett = new BestillingBilletter()
                {
                    BillettId = 12,
                    Fra = "Oslo",
                    Til = "Bergen",
                    PassasjerEtternavn = "Duck",
                    PassasjerFornavn = "Donald",
                    Avreise = new DateTime(2017, 10, 20, 14, 0, 0)
                };
                return funnetBillett;
            } else
            {
                BestillingBilletter funnetBillett = new BestillingBilletter()
                {
                    BillettId = 12,
                    Fra = "Oslo",
                    Til = "Stavanger",
                    PassasjerEtternavn = "Duck",
                    PassasjerFornavn = "Donald",
                    Avreise = new DateTime(2017, 10, 20, 14, 0, 0)
                };
                return funnetBillett;
            }
        }
        public bool endreBillett(int id, BestillingBilletter nyBillData)
        {
            if(id == 2 && nyBillData.BillettId == 2)
            {
                return true;
            } else
            {
                return false;
            }
        }
        public bool slettBillett(int slettId)
        {
            if (slettId == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}