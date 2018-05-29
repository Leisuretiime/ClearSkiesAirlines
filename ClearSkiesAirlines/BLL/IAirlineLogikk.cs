using Model;
using System.Collections.Generic;

namespace ClearSkiesAirlines.BLL
{
    public interface IAirlineLogikk
    {
        //Login
        bool LoginTest(Login adminBruker);

        //Kunder
        List<KundeView> hentAlleKunder();
        bool slettKunde(int innId);
        KundeView hentEnKunde(int id);
        bool endreKunde(int id, KundeView endreKunde);

        //Reise
        bool RegistrerReise(ReiseReg reise);
        List<ReiseView> HentAlleReiser();
        bool slettReise(int innId);
        ReiseReg hentEnReise(int id);
        bool endreReise(int id, ReiseReg endreReise);

        //Bestilling
        List<BestillingView> hentAlleBestillinger();
        bool slettBestilling(int id);
        List<BestillingBilletter> hentBestillingBilletter(int id);
        BestillingBilletter hentEnBillett(int id);
        bool endreBillett(int id, BestillingBilletter nyBillData);
        bool slettBillett(int slettId);

    }
}