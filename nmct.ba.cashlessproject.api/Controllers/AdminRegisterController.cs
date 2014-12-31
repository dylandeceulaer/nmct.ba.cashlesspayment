using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.api.PresentationModels;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class AdminRegisterController : Controller
    {
        private static List<Claim> GetClaims(Organisation o)
        {
            List<Claim> c = new List<Claim>();
            c.Add(new Claim("dblogin", Cryptography.Encrypt(o.DbLogin)));
            c.Add(new Claim("dbpass", Cryptography.Encrypt(o.DbPassword)));
            c.Add(new Claim("dbname", Cryptography.Encrypt(o.DbName)));
            return c;
        }
        // GET: AdminRegister
        public ActionResult Index(int? id)
        {
            List<Organisation> OrgNames = new List<Organisation>();
            List<Organistion_Register> OrgRegs = Organistion_RegisterDA.GetOrganistion_Registers();
            List<Register> registers = RegistersDA.GetRegisters();

            if (!id.HasValue)
            {
                
                foreach (Register reg in registers)
                {
                    int org = (from e in OrgRegs where e.RegisterID == reg.Id select e.OrganisationID).FirstOrDefault();
                    OrgNames.Add(OrganisationsDA.GetOrganisationById(org));
                }
                ViewBag.OrganisationName = "Alle kassas";
                ViewBag.IsDetail = false;
                ViewBag.OrganisationNames = OrgNames;
                return View(registers);
            }
            else
            {
                if ((int)id == -1)
                {
                    List<Register> registersZonder = new List<Register>();
                    //registers = (from e in registers where e.Id == (from e in OrgRegs where e.RegisterID != reg.Id select e.OrganisationID))
                    foreach (Register reg in registers)
                    {
                        int org = (from e in OrgRegs where e.RegisterID == reg.Id select e.OrganisationID).FirstOrDefault();
                        if(org == 0){
                            OrgNames.Add(OrganisationsDA.GetOrganisationById(org));
                            registersZonder.Add(reg);
                        }
                    }
                    ViewBag.IsDetail = true;
                    ViewBag.OrganisationNames = OrgNames;
                    ViewBag.OrganisationName = "Niet toegewezen kassas.";
                    return View(registersZonder);
                }
                else
                {
                    List<Register> registersZonder = new List<Register>();
                    //registers = (from e in registers where e.Id == (from e in OrgRegs where e.RegisterID != reg.Id select e.OrganisationID))
                    foreach (Register reg in registers)
                    {
                        int org = (from e in OrgRegs where e.RegisterID == reg.Id select e.OrganisationID).FirstOrDefault();
                        if (org == (int)id)
                        {
                            OrgNames.Add(OrganisationsDA.GetOrganisationById(org));
                            registersZonder.Add(reg);
                        }
                    }
                    ViewBag.IsDetail = true;
                    ViewBag.OrganisationNames = OrgNames;
                    ViewBag.OrganisationName = "Kassas Toegewezen aan " + OrganisationsDA.GetOrganisationById((int)id).OrganisationName;
                    return View(registersZonder);
                }
            }
        }
        public ActionResult Details(int? id)
        {
            if (id.HasValue) return View(RegistersDA.GetRegisterById((int)id));
            return RedirectToAction("Index", "AdminRegister");
        }
        public ActionResult Edit(int? id)
        {
            if (id.HasValue) return View(RegistersDA.GetRegisterById((int)id));
            return RedirectToAction("Index", "AdminRegister");
        }
        [HttpPost]
        public ActionResult Edit(Register reg)
        {
            if (ModelState.IsValid)
            {
                Organistion_Register or = Organistion_RegisterDA.GetOrganistion_RegisterById(reg.Id);
                if (or.OrganisationID != 0)
                {
                    Organisation o = OrganisationsDA.GetOrganisationById(or.OrganisationID);
                    List<Claim> c = GetClaims(o);

                    Register regOld = RegistersDA.GetRegisterById(reg.Id);
                    List<Register> lre = RegistersDA.GetRegisters(c);
                    int re = (from e in lre where e.RegisterName == regOld.RegisterName && e.Device == regOld.Device select e.Id).FirstOrDefault();

                    Register regO = new Register()
                    {
                        Id = re,
                        Device = reg.Device,
                        RegisterName = reg.RegisterName
                    };
                    regO.Id = re;

                    RegistersDA.UpdateRegister(c, regO);
                }
                RegistersDA.UpdateRegister(reg);
                return RedirectToAction("Index", "AdminRegister");
            }
            return View(reg);
        }
        public ActionResult Delete(int? id)
        {
            if (id.HasValue) RegistersDA.DeleteRegister((int)id);
            return RedirectToAction("Index", "AdminRegister");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Register reg)
        {
            if (ModelState.IsValid)
            {
                RegistersDA.InsertRegister(reg);
                return RedirectToAction("Index", "AdminRegister");
            }
            return View(reg);
        }
        public ActionResult AddToOrganisation(int? id)
        {
            if (id.HasValue)
            {
                Register reg = RegistersDA.GetRegisterById((int)id);
                ViewBag.RegisterName = reg.RegisterName;
                List<Organistion_Register> OrgRegs = Organistion_RegisterDA.GetOrganistion_Registers();
                Organistion_Register orgReg = (from e in OrgRegs where e.RegisterID == id select e).FirstOrDefault();

                if (orgReg == null) orgReg = new Organistion_Register()
                {
                    FromDate = DateTime.Now,
                    UntilDate = reg.ExpiresDate,
                    RegisterID = reg.Id
                };
                List<SelectListItem> organisations = new List<SelectListItem>();
                foreach (Organisation org in OrganisationsDA.GetOrganisations())
                {
                    organisations.Add(new SelectListItem()
                    {
                        Value = org.ID.ToString(),

                        Text = org.OrganisationName
                    });
                }
                OrganisationRegisterPM orpm = new OrganisationRegisterPM()
                {
                    OrganisationRegiser = orgReg,
                    Organisations = organisations
                };

                ViewBag.KassaNaam = reg.RegisterName;
                //ViewBag.Organisations = OrganisationsDA.GetOrganisations();
                return View(orpm);
            }
            return RedirectToAction("Index", "AdminRegister");
        }
        [HttpPost]
        public ActionResult AddToOrganisation(OrganisationRegisterPM OrgReg)
        {
            if (OrgReg.OrganisationRegiser.OrganisationID != 0)
            {
                Organisation o = OrganisationsDA.GetOrganisationById(OrgReg.OrganisationRegiser.OrganisationID);
                List<Claim> c = GetClaims(o);

                if (Organistion_RegisterDA.GetOrganistion_RegisterById(OrgReg.OrganisationRegiser.RegisterID).OrganisationID == OrgReg.OrganisationRegiser.OrganisationID)
                {
                    Organistion_RegisterDA.UpdateOrganistion_Register(OrgReg.OrganisationRegiser);
                    RegistersDA.InsertRegister(c, RegistersDA.GetRegisterById(OrgReg.OrganisationRegiser.RegisterID));
                }
                else
                {
                    Organistion_RegisterDA.InsertOrganistion_Register(OrgReg.OrganisationRegiser);
                    RegistersDA.InsertRegister(c, RegistersDA.GetRegisterById(OrgReg.OrganisationRegiser.RegisterID));
                }
                return RedirectToAction("Index", "AdminRegister");
            }
            return View(OrgReg);
        }
        
        public ActionResult RemoveOrganisation(int? id)
        {
            if (id.HasValue)
            {
                Organistion_Register or = Organistion_RegisterDA.GetOrganistion_RegisterById((int)id);
                Organisation o = OrganisationsDA.GetOrganisationById(or.OrganisationID);
                List<Claim> c = GetClaims(o);

                Register ra = RegistersDA.GetRegisterById((int)id);

                List<Register> lre = RegistersDA.GetRegisters(c);
                int re = (from e in lre where e.RegisterName == ra.RegisterName && e.Device == ra.Device select e.Id).FirstOrDefault();

                RegistersDA.DeleteRegister(c,re);

                Organistion_RegisterDA.DeleteOrganistion_Register(or);
            }
            return RedirectToAction("Index", "AdminRegister");
        }
        
    }
}