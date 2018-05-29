using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClearSkiesAirlines.BLL;
using Model;
using System.Diagnostics;
using PagedList;
using System.Web.Routing;

namespace ClearSkiesAirlines.Controllers
{
    public class ClearSkiesController : Controller
    {
        private IAirlineLogikk _AirlineLogikk;

        public ClearSkiesController()
        {
            _AirlineLogikk = new AirlineLogikk();
        }

        public ClearSkiesController(IAirlineLogikk stub)
        {
            _AirlineLogikk = stub;
        }

        //LOGIN-ADMIN
        public ActionResult Login()
        {
            if (Session["LoggetInn"] == null)
            {
                Session["LoggetInn"] = false;
            }
            else
            {
                if (Session["LoggetInn"] != null)
                {
                    bool loggetInn = (bool)Session["LoggetInn"];
                    if (loggetInn)
                    {
                        return RedirectToAction("homePage");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login loginInfo)
        {
            if (_AirlineLogikk.LoginTest(loginInfo))
            {
                Session["LoggetInn"] = true;
                ViewBag.Innlogget = true;
                return RedirectToAction("homePage");
            }
            else
            {
                Session["LoggetInn"] = false;
                ViewBag.Innlogget = false;
                return View();
            }
        }
        
        public ActionResult logOut()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        
        public ActionResult homePage()
        {
            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }

            //KUNDE-ADMINISTRASJON
		public ActionResult administrerKunder(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    ViewBag.CurrentSort = sortOrder;

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                ViewBag.Surname = String.IsNullOrEmpty(sortOrder) ? "fornavn" : "";
                ViewBag.FamilyName = String.IsNullOrEmpty(sortOrder) ? "etternavn" : "";
                List<KundeView> alleKunder = _AirlineLogikk.hentAlleKunder();

                if (!String.IsNullOrEmpty(searchString))
                {
                    alleKunder = alleKunder.Where(s => s.Etternavn.Contains(searchString)
                                            || s.Fornavn.Contains(searchString)).ToList();
                }

                switch (sortOrder)
                {
                    case "fornavn":
                        alleKunder = alleKunder.OrderBy(s => s.Fornavn).ToList();
                        break;
                    case "etternavn":
                        alleKunder = alleKunder.OrderByDescending(s => s.Etternavn).ToList();
                        break;
                    default:
                        alleKunder = alleKunder.OrderBy(s => s.Etternavn).ToList();
                        break;
                }

                int pageSize = 10;
                int pageNumber = (page ?? 1);

                if (Session["FeilKunde"] != null)
                    ViewBag.Feil = Session["FeilKunde"];

                return View(alleKunder.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login");

        }


        public ActionResult slettKunde(int id)
        {
            bool OK = _AirlineLogikk.slettKunde(id);      
            if (OK)
            {
                Session["FeilKunde"] = false;
                return RedirectToAction("administrerKunder");
            }

            Session["FeilKunde"] = true;
            return RedirectToAction("administrerKunder");
        }

        public ActionResult endreKunde(int id)
        {
            if(Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    KundeView kunde = _AirlineLogikk.hentEnKunde(id);
                    return View(kunde);
                }
            }
            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult endreKunde(int id, KundeView endreKunde)
        {
            if (ModelState.IsValid)
            {
                bool endringOK = _AirlineLogikk.endreKunde(id, endreKunde);
                if (endringOK)
                {
                    return RedirectToAction("administrerKunder");
                }
                ViewBag.Feil = true;
                return View(endreKunde);
            }
            ViewBag.RegEx = false;
            return View(endreKunde);
        }

        //REISE-ADMINISTRASJON
        public ActionResult RegistrerReise()
        {
            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult RegistrerReise(ReiseReg reise)
        {
			if (ModelState.IsValid)
			{
				bool registrert = _AirlineLogikk.RegistrerReise(reise);
				if (registrert)
				{
					return RedirectToAction("AdministrerReiser");
				}
				else
				{
					ViewBag.registrert = "Feil";
					return View();
				}
			}
			else
			{
				ViewBag.registrert = "Valideringsfeil";
				return View();
			}
        }

        public ActionResult AdministrerReiser(string sortOrder, string currentFilter, string searchString, int? page)
        {

            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    ViewBag.currentSort = sortOrder;

                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }

                    ViewBag.fra = String.IsNullOrEmpty(sortOrder) ? "fra" : "";
                    ViewBag.destinasjon = String.IsNullOrEmpty(sortOrder) ? "destinasjon" : "";
                    List<ReiseView> alleReiser = _AirlineLogikk.HentAlleReiser();

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        alleReiser = alleReiser.Where(s => s.Fra.Contains(searchString)
                                                || s.Destinasjon.Contains(searchString)).ToList();
                    }

                    switch (sortOrder)
                    {
                        case "fra":
                            alleReiser = alleReiser.OrderByDescending(r => r.Fra).ToList();
                            break;
                        case "destinasjon":
                            alleReiser = alleReiser.OrderByDescending(r => r.Destinasjon).ToList();
                            break;
                        default:
                            alleReiser = alleReiser.OrderBy(r => r.Fra).ToList();
                            break;
                    }
                    //return View(alleReiser.ToList());
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    if(Session["FeilReise"] != null)
                        ViewBag.feil = (bool)Session["FeilReise"];

                    return View(alleReiser.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login");
        }


        public ActionResult slettReise(int id)
        {
            bool OK = _AirlineLogikk.slettReise(id);
                
            if (OK)
            {
                Session["FeilReise"] = false;
                return RedirectToAction("AdministrerReiser");
            }

            Session["FeilReise"] = true;
            return RedirectToAction("AdministrerReiser");
        }

        public ActionResult endreReise(int id)
        {
            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    ReiseReg reise = _AirlineLogikk.hentEnReise(id);
                    return View(reise);
                }
            }
            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult endreReise(int id, ReiseReg endreReise)
        {
            if (ModelState.IsValid)
            {
                bool endringOK = _AirlineLogikk.endreReise(id, endreReise);
                if (endringOK)
                {
                    return RedirectToAction("AdministrerReiser");
                }
                ViewBag.Feil = true;
                return View(endreReise);
            }
            ViewBag.RegEx = false;
            return View(endreReise);
        }

        //BESTILLING-ADMINISTRASJON
        public ActionResult AdministrerBestillinger(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    ViewBag.currentSort = sortOrder;

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.handelId = String.IsNullOrEmpty(sortOrder) ? "handeld" : "";
                ViewBag.eier = String.IsNullOrEmpty(sortOrder) ? "eier" : "";
                List<BestillingView> alleBestillinger = _AirlineLogikk.hentAlleBestillinger();

                if (!String.IsNullOrEmpty(searchString))
                {
                    alleBestillinger = alleBestillinger.Where(b => b.HandelId.ToString().Contains(searchString)
                                            || b.Eier.Contains(searchString)).ToList();
                }

                switch (sortOrder)
                {
                    case "handelId":
                        alleBestillinger = alleBestillinger.OrderByDescending(b => b.HandelId).ToList();
                        break;
                    case "Eier":
                        alleBestillinger = alleBestillinger.OrderByDescending(b => b.Eier).ToList();
                        break;
                    default:
                        alleBestillinger = alleBestillinger.OrderBy(b => b.HandelId).ToList();
                        break;
                }
                    
                int pageSize = 10;
                int pageNumber = (page ?? 1);

                if (Session["FeilBestilling"] != null)
                    ViewBag.Feil = Session["FeilBestilling"];

                return View(alleBestillinger.ToPagedList(pageNumber, pageSize));
                }
            }
            return RedirectToAction("Login");
        }
            
        public ActionResult slettBestilling(int id)
        {
            bool OK = _AirlineLogikk.slettBestilling(id);
            if (OK)
            {
                Session["FeilBestilling"] = false;
                return RedirectToAction("AdministrerBestillinger");
            }
            Session["FeilBestilling"] = true;
            return RedirectToAction("AdministrerBestillinger");
        }

        public ActionResult endreBestilling(int id)
        {
            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    List<BestillingBilletter> billetter = _AirlineLogikk.hentBestillingBilletter(id);

                    if (Session["FeilBillett"] != null)
                    {
                        ViewBag.Feil = Session["FeilBillett"];
                    }

                        Session["bestillingID"] = id;
                    return View(billetter);
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult endreBillett(int id)
        {
            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    BestillingBilletter billett = _AirlineLogikk.hentEnBillett(id);
                    return View(billett);
                }
            }
            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult endreBillett(int id, BestillingBilletter endreBillett)
        {
            if (ModelState.IsValid)
            {
                bool endringOK = _AirlineLogikk.endreBillett(id, endreBillett);
                if (endringOK)
                {
                    return RedirectToAction("endreBestilling", "ClearSkies", new { @id = Session["bestillingID"] });
                }
                ViewBag.Feil = true;
                return View(endreBillett);
            }
            ViewBag.RegEx = false;
            return View(endreBillett);
        }

        public ActionResult slettBillett(int id)
        {
            bool OK = _AirlineLogikk.slettBillett(id);
            if (OK)
            {
                Session["FeilBillett"] = false;
                return RedirectToAction("endreBestilling", "ClearSkies", new { @id = Session["bestillingID"] });

            }
            Session["FeilBillett"] = true;
            return RedirectToAction("endreBestilling", "ClearSkies", new { @id = Session["bestillingID"] });
        }


        //ANNET
        public PartialViewResult loadNavigation()
            {
                if (Session["LoggetInn"] != null)
                {
                    bool loggetInn = (bool)Session["LoggetInn"];
                    if (loggetInn)
                    {
                        return PartialView("navigationMenu");
                    }
                }
                return PartialView("tomtView");

        }

        public ActionResult navigationMenu()
        {
            return RedirectToAction("Login");
        }

        //Metode for navbar for å gå hjem, Dette istedenfor actionLink fordi Glyphicon så bedre ut med href
        public ActionResult goHome()
        {
            return RedirectToAction("homePage");
        }

    }
}