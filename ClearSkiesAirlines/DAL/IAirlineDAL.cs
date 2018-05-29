using Model;
using System;
using System.Collections.Generic;

namespace ClearSkiesAirlines.DAL
{
    public interface IAirlineDAL
    {
        //login
        bool LoginTest(Login adminBruker);
        Byte[] lagHash(string adminPassord);
        Byte[] lagSalt();

        //kunder
        List<KundeView> hentAlleKunder();
        bool slettKunde(int slettID);
        KundeView hentEnKunde(int id);
        bool endreKunde(int id, KundeView nyKundeData);

        //REISE
        bool RegistrerReise(ReiseReg innReise);
        List<ReiseView> HentAlleReiser();
        bool slettReise(int slettId);
        ReiseReg hentEnReise(int id);
        bool endreReise(int id, ReiseReg nyReiseData);

        //Bestilling
        List<BestillingView> hentAlleBestillinger();
        bool slettBestilling(int slettId);
        List<BestillingBilletter> hentBestillingBilletter(int id);
        BestillingBilletter hentEnBillett(int id);
        bool endreBillett(int id, BestillingBilletter nyBillData);
        bool slettBillett(int slettId);
    }
}