using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace ClearSkiesAirlines.DAL
{
    public class AirlineDAL : DAL.IAirlineDAL
    {
        //LOGIN
        public bool LoginTest(Login adminBruker)
        {
            using (var db = new AirlineDbContext())
            {
                Byte[] hentSalt;
                try
                {
                    hentSalt = db.Admins.FirstOrDefault(aBr => aBr.Brukernavn == adminBruker.Brukernavn).Salt;
                }
                catch (Exception feil)
                {
                    return false;
                }
                byte[] passordDb = lagHash(adminBruker.Passord + hentSalt);
                Admin funnetBruker = db.Admins.FirstOrDefault(aBr => aBr.Passord == passordDb && aBr.Brukernavn == adminBruker.Brukernavn);
                if (funnetBruker == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public Byte[] lagHash(string adminPassord)
        {
            byte[] innData, utData;
            var algoritme = System.Security.Cryptography.SHA256.Create();
            innData = System.Text.Encoding.ASCII.GetBytes(adminPassord);
            utData = algoritme.ComputeHash(innData);
            return utData;
        }

        public Byte[] lagSalt()
        {
            var salt = new Byte[32];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }


        //KUNDE
        public List<KundeView> hentAlleKunder()
        {
            try
            {
                using (var db = new AirlineDbContext())
                {
                    List<Kunde> alleKunder = db.Kunder.ToList();
                    List<KundeView> kundeListe = new List<KundeView>();
                    foreach (var i in alleKunder)
                    {
                        KundeView kunde = new KundeView();
                        kunde.kundeId = i.KundeId;
                        kunde.Fornavn = i.Fornavn;
                        kunde.Etternavn = i.Etternavn;
                        kunde.Adresse = i.Adresse;
                        kunde.Epost = i.Epost;
                        kunde.PostNr = i.PostSted.PostNr;
                        kunde.PostSted = i.PostSted.Sted;
                        kunde.Telefon = i.Telefon;
                        kundeListe.Add(kunde);
                    }
                    return kundeListe;
                }
            }
            catch (Exception error)
            {

                string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";
                File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);

                List<KundeView> kundeListe = new List<KundeView>();
                return kundeListe;
            }
        }

        public bool slettKunde(int slettID)
        {
            using (var db = new AirlineDbContext())
            {
                try
                {
                    Kunde slettKunde = db.Kunder.Find(slettID);
                    List<Handel> kundensHandler = db.Handler.Where(k=> k.Kunde.KundeId == slettKunde.KundeId).ToList();
                    foreach(var kh in kundensHandler)
                    {
                        List<Billett> billetterListe = db.Billetter.Where(b => b.Handel.HandelId == kh.HandelId).ToList();
                        foreach (var b in billetterListe)
                        {
                            db.Billetter.Remove(b);
                        }
                        db.Handler.Remove(kh);
                    }
                    db.Kunder.Remove(slettKunde);
                    //Logge endringer gjort i DB
                    db.SaveChanges();

                    string innTilFil = DateTime.Now.ToString() + "Kunde er slettet. ID: " + slettID.ToString() + "\n\r";
                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), innTilFil);

                    return true;
                }
                catch (Exception error)
                {
                    string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";

                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);
                    return false;
                }
            }
        }

        public KundeView hentEnKunde(int id)
        {
            using (var db = new AirlineDbContext())
            {
                KundeView kundeUt = new KundeView();
                Kunde dataKunde = db.Kunder.Find(id);
                kundeUt.Adresse = dataKunde.Adresse;
                kundeUt.Epost = dataKunde.Epost;
                kundeUt.Etternavn = dataKunde.Etternavn;
                kundeUt.Fornavn = dataKunde.Fornavn;
                kundeUt.kundeId = dataKunde.KundeId;
                kundeUt.PostNr = dataKunde.PostSted.PostNr;
                kundeUt.PostSted = dataKunde.PostSted.Sted;
                kundeUt.Telefon = dataKunde.Telefon;

                return kundeUt;
            }
        }

        public bool endreKunde(int id, KundeView nyKundeData)
        {
            using (var db = new AirlineDbContext())
            {
                try
                {
                    Kunde eksisterendeKunde = db.Kunder.Find(id);
                    eksisterendeKunde.Adresse = nyKundeData.Adresse;
                    eksisterendeKunde.Epost = nyKundeData.Epost;
                    eksisterendeKunde.Etternavn = nyKundeData.Etternavn;
                    eksisterendeKunde.Fornavn = nyKundeData.Fornavn;
                    eksisterendeKunde.Telefon = nyKundeData.Telefon;

                    var postSted = db.PostSted.Find(nyKundeData.PostNr);
                    if (postSted == null)
                    {
                        PostSted nyttPoststed = new PostSted();
                        nyttPoststed.PostNr = nyKundeData.PostNr;
                        nyttPoststed.Sted = nyKundeData.PostSted;

                        db.PostSted.Add(nyttPoststed);
                        eksisterendeKunde.PostSted = nyttPoststed;
                    }
                    else
                    {
                        eksisterendeKunde.PostSted = postSted;
                    }

                    db.SaveChanges();

                    string innTilFil = DateTime.Now.ToString() + "Kunde er endret i DB: " + eksisterendeKunde.KundeId + " " + eksisterendeKunde.Etternavn + "\n\r";
                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), innTilFil);

                    return true;
                }
                catch (Exception error)
                {
                    string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";

                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);
                    return false;
                }
            }
        }

		//REISE
		public bool RegistrerReise(ReiseReg innReise)
		{
			DateTime AvreiseInn = innReise.Avreise.Add(innReise.AvreiseTid.TimeOfDay);
			DateTime AnkomstInn = innReise.Ankomst.Add(innReise.AnkomstTid.TimeOfDay);
			String innReisetid = "";

			if (AvreiseInn > AnkomstInn)
			{
				return false;
			}

			double Reisetid = AnkomstInn.Subtract(AvreiseInn).TotalMinutes;
			if (Reisetid < 60)
			{
				innReisetid = Reisetid.ToString() + " minutter";
			}
			else
			{
				double minutter = Reisetid % 60;
				int timer = System.Convert.ToInt32(System.Math.Floor(Reisetid / 60));

				if (minutter > 0)
				{
					innReisetid = timer.ToString() + " time(r) og " + minutter.ToString() + " minutter";
				}
				else
				{
					innReisetid = timer.ToString() + " time(r)";
				}
			}

			using (var db = new AirlineDbContext())
			{
				try
				{
					var nyReise = new Reise
					{
						Fra = innReise.Fra,
						Til = innReise.Destinasjon,
						Avreise = AvreiseInn,
						Ankomst = AnkomstInn,
						Varighet = innReisetid,
						Kapasitet = innReise.Kapasitet,
						Pris = innReise.Pris
					};

					string innTilFil = DateTime.Now.ToString() + "Ny reise er registrert i DB: " + nyReise.Fra + " " + nyReise.Til + " " + nyReise.Avreise + "\n\r";
					File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), innTilFil);
					db.Reiser.Add(nyReise);
					db.SaveChanges();
					return true;
				}
				catch (Exception error)
				{
					string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";

					File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);
					return false;
				}
			}
		}

		public List<ReiseView> HentAlleReiser()
        {
            try
            {
                using (var db = new AirlineDbContext())
                {
                    List<Reise> alleReiser = db.Reiser.ToList();
                    List<ReiseView> reiseListe = new List<ReiseView>();
                    foreach (var reise in alleReiser)
                    {
                        ReiseView nyReise = new ReiseView
                        {
                            ReiseId = reise.ReiseId,
                            Fra = reise.Fra,
                            Destinasjon = reise.Til,
                            Kapasitet = reise.Kapasitet,
                            Pris = reise.Pris,
                            Avreise = reise.Avreise,
                            Ankomst = reise.Ankomst,
                            Varighet = reise.Varighet
                        };
                        reiseListe.Add(nyReise);
                    }
                    return reiseListe;
                }
            }
            catch (Exception error)
            {

                string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";
                File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);

                List<ReiseView> reiseListe = new List<ReiseView>();
                return reiseListe;
            }
        }

        public bool slettReise(int slettId)
        {
            using (var db = new AirlineDbContext())
            {
                try
                {
                    Reise slettReise = db.Reiser.Find(slettId);
                    List<Billett> billetter = db.Billetter.Where(b => b.Reise.ReiseId == slettReise.ReiseId).ToList();
                    List<Handel> handler = new List<Handel>();
                    foreach(var b in billetter)
                    {
                        Handel handel = db.Handler.Find(b.Handel.HandelId);
                        handler.Add(handel);
                    }

                    List<Handel> rensetHandler = handler.GroupBy(i => i.HandelId).Select(g => g.First()).ToList();

                    foreach (var rh in rensetHandler)
                    {
                        List<Billett> rBillett = db.Billetter.Where(b => b.Handel.HandelId == rh.HandelId).ToList();
                        foreach (var rB in rBillett)
                        {
                            db.Billetter.Remove(rB);
                        }
                        db.Handler.Remove(rh);
                    }


                    db.Reiser.Remove(slettReise);
                    //Logge endringer gjort i DB
                    db.SaveChanges();

                    string innTilFil = DateTime.Now.ToString() + "Reise er slettet. ID: " + slettId.ToString() + "\n\r";
                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), innTilFil);

                    return true;
                }
                catch (Exception error)
                {
                    string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";

                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);
                    return false;
                }
            }
        }

        public ReiseReg hentEnReise(int id)
        {
            using (var db = new AirlineDbContext())
            {
                Reise dataReise = db.Reiser.Find(id);
                ReiseReg reiseUt = new ReiseReg();

                reiseUt.Ankomst = dataReise.Ankomst.Date;
                reiseUt.AnkomstTid = dataReise.Ankomst;
                reiseUt.Avreise = dataReise.Avreise.Date;
                reiseUt.AvreiseTid = dataReise.Avreise;
                reiseUt.Destinasjon = dataReise.Til;
                reiseUt.Fra = dataReise.Fra;
                reiseUt.Kapasitet = dataReise.Kapasitet;
                reiseUt.Pris = dataReise.Pris;

                return reiseUt;
            }
        }

        public bool endreReise(int id, ReiseReg nyReiseData)
        {
            using (var db = new AirlineDbContext())
            {
                DateTime AvreiseInn = nyReiseData.Avreise.Add(nyReiseData.AvreiseTid.TimeOfDay);
                DateTime AnkomstInn = nyReiseData.Ankomst.Add(nyReiseData.AnkomstTid.TimeOfDay);
                String innReisetid = "";

			    if (AvreiseInn > AnkomstInn)
			    {
				    return false;
			    }

                double Reisetid = AnkomstInn.Subtract(AvreiseInn).TotalMinutes;
                if (Reisetid < 60)
                {
                    innReisetid = Reisetid.ToString() + " minutter";
                }
                else
                {
                    double minutter = Reisetid % 60;
                    int timer = System.Convert.ToInt32(System.Math.Floor(Reisetid / 60));

                    if (minutter > 0)
                    {
                        innReisetid = timer.ToString() + " time(r) og " + minutter.ToString() + " minutter";
                    }
                    else
                    {
                        innReisetid = timer.ToString() + " time(r)";
                    }
                }
                try
                {
                    Reise eksisterendeReise = db.Reiser.Find(id);
                    eksisterendeReise.Ankomst = AnkomstInn;
                    eksisterendeReise.Avreise = AvreiseInn;
                    eksisterendeReise.Fra = nyReiseData.Fra;
                    eksisterendeReise.Til = nyReiseData.Destinasjon;
                    eksisterendeReise.Pris = nyReiseData.Pris;
                    eksisterendeReise.Kapasitet = nyReiseData.Kapasitet;
				    eksisterendeReise.Varighet = innReisetid;

                    db.SaveChanges();

                    string innTilFil = DateTime.Now.ToString() + "Kunde er endret i DB: " + eksisterendeReise.ReiseId + " " + eksisterendeReise.Fra + " til " + eksisterendeReise.Til + "\n\r";
                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), innTilFil);

                    return true;
                }
                catch (Exception error)
                {
                    string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";

                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);
                    return false;
                }
            }
        }

        //BESTILLINGER
        public List<BestillingView> hentAlleBestillinger()
        {
            try
            {
                using (var db = new AirlineDbContext())
                {
                    List<Handel> alleBestillinger = db.Handler.ToList();
                    List<BestillingView> bestillingsListe = new List<BestillingView>();
                    foreach (var bestilling in alleBestillinger)
                    {
                        BestillingView nyBestilling = new BestillingView
                        {
                            HandelId = bestilling.HandelId,
                            Eier = bestilling.Kunde.Fornavn + " " + bestilling.Kunde.Etternavn,
                            AntallBilletter = bestilling.Billetter.Count,
                            TotalPris = bestilling.TotalPris

                        };
                        bestillingsListe.Add(nyBestilling);
                    }
                    return bestillingsListe;
                }
            }
            catch (Exception error)
            {

                string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";
                File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);

                List<BestillingView> bestillingsListe = new List<BestillingView>();
                return bestillingsListe;
            }
        }

        public bool slettBestilling(int slettId)
        {
            using (var db = new AirlineDbContext())
            {
                try
                {
                Handel slettHandel = db.Handler.Find(slettId);
                    List<Billett> slettBilletter = db.Handler.Find(slettId).Billetter.ToList();
                    foreach (var billett in slettBilletter)
                    {
                        db.Billetter.Remove(billett);
                    }
                    db.Handler.Remove(slettHandel);
                    db.SaveChanges();

                    string innTilFil = DateTime.Now.ToString() + "Handel er slettet. ID: " + slettId.ToString() + "\n\r";
                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), innTilFil);

                    return true;
                }
                catch (Exception error)
                {
                    string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";

                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);
                    return false;
                }
            }
        }
            
        public List<BestillingBilletter> hentBestillingBilletter(int id)
        {
            using (var db = new AirlineDbContext())
            {
            var billetter = db.Handler.Find(id).Billetter.ToList();
            var billetterUt = new List<BestillingBilletter>();
            foreach (var billett in billetter)
            {
                billetterUt.Add(new BestillingBilletter
                {
                    BillettId = billett.BillettId,
                    Fra = billett.Reise.Fra,
                    Til = billett.Reise.Til,
                    Avreise = billett.Reise.Avreise,
                    PassasjerFornavn = billett.PassasjerFornavn,
                    PassasjerEtternavn = billett.PassasjerEtternavn
                });
            }
                return billetterUt;
            }  
        }

        public BestillingBilletter hentEnBillett(int id)
        {
        using (var db = new AirlineDbContext())
            {
                Billett dataBillett = db.Billetter.Find(id);
                BestillingBilletter billettUt = new BestillingBilletter()
                {
                    BillettId = dataBillett.BillettId,
                    PassasjerFornavn = dataBillett.PassasjerFornavn,
                    PassasjerEtternavn = dataBillett.PassasjerEtternavn
                };

                return billettUt;
            }
        }

        public bool endreBillett(int id, BestillingBilletter nyBillData)
        {
            using (var db = new AirlineDbContext())
            {
                try
                {
                    Billett eksisterendeBillett = db.Billetter.Find(id);
                    eksisterendeBillett.PassasjerFornavn = nyBillData.PassasjerFornavn;
                    eksisterendeBillett.PassasjerEtternavn = nyBillData.PassasjerEtternavn;

                    db.SaveChanges();

                    string innTilFil = DateTime.Now.ToString() + "Bestilling er endret i DB til: " + eksisterendeBillett.BillettId + " " + eksisterendeBillett.PassasjerFornavn + " til " + eksisterendeBillett.PassasjerEtternavn + "\n\r";
                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), innTilFil);

                    return true;
                }
                catch (Exception error)
                {
                    string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";

                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);
                    return false;
                }
            }
        }

        public bool slettBillett(int slettId)
        {
            using (var db = new AirlineDbContext())
            {
                try
                {
                        
                    Billett slettBillett = db.Billetter.Find(slettId);
                    Handel bestilling = slettBillett.Handel;

                    bestilling.TotalPris -= slettBillett.Reise.Pris;

                    db.Billetter.Remove(slettBillett);
                    db.SaveChanges();

                    string innTilFil = DateTime.Now.ToString() + "Billett er slettet. ID: " + slettId.ToString() + "\n\r";
                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), innTilFil);

                    return true;
                }
                catch (Exception error)
                {
                    string feilmelding = DateTime.Now.ToString() + " " + error.ToString() + "\n\r";

                    File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logger/Registreringer.txt"), feilmelding);
                    return false;
                }
            }
        }
    }
}