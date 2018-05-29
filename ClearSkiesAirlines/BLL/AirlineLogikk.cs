using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearSkiesAirlines.DAL;
using Model;

namespace ClearSkiesAirlines.BLL
{
    public class AirlineLogikk : BLL.IAirlineLogikk
    {

        private IAirlineDAL _AirlineDAL;

        public AirlineLogikk()
        {
            _AirlineDAL = new AirlineDAL();
        }

        public AirlineLogikk(IAirlineDAL stub)
        {
            _AirlineDAL = stub;
        }

        //LOGIN
        public bool LoginTest(Login adminBruker)
        {
            return _AirlineDAL.LoginTest(adminBruker);
        }

        //KUNDE
        public List<KundeView> hentAlleKunder()
        {
            List<KundeView> alleKunder = _AirlineDAL.hentAlleKunder();
            return alleKunder;
        }

        public bool slettKunde(int innId)
        {
            return _AirlineDAL.slettKunde(innId);
        }

        public KundeView hentEnKunde(int id)
        {
            return _AirlineDAL.hentEnKunde(id);
        }

        public bool endreKunde(int id, KundeView endreKunde)
        {
            return _AirlineDAL.endreKunde(id, endreKunde);
        }

        //REISE
        public bool RegistrerReise(ReiseReg reise)
		{
			return _AirlineDAL.RegistrerReise(reise);
		}

        public List<ReiseView> HentAlleReiser()
        {
            List<ReiseView> alleReiser = _AirlineDAL.HentAlleReiser();
            return alleReiser;
        }

        public bool slettReise(int innId)
        {
            return _AirlineDAL.slettReise(innId);
        }

        public ReiseReg hentEnReise(int id)
        {
            return _AirlineDAL.hentEnReise(id);
        }

        public bool endreReise(int id, ReiseReg endreReise)
        {
            return _AirlineDAL.endreReise(id, endreReise);
        }

        //BESTILLING
        public List<BestillingView> hentAlleBestillinger()
        {
            List<BestillingView> alleBestillinger = _AirlineDAL.hentAlleBestillinger();
            return alleBestillinger;
        }

        public bool slettBestilling(int id)
        {
            return _AirlineDAL.slettBestilling(id); 
        }

        public List<BestillingBilletter> hentBestillingBilletter(int id)
        {
            return _AirlineDAL.hentBestillingBilletter(id);
        }
        public BestillingBilletter hentEnBillett(int id)
        {
            return _AirlineDAL.hentEnBillett(id);
        }
        public bool endreBillett(int id, BestillingBilletter nyBillData)
        {
            return _AirlineDAL.endreBillett(id, nyBillData);
        }

        public bool slettBillett(int id)
        {
            return _AirlineDAL.slettBillett(id);
        }

    }
}