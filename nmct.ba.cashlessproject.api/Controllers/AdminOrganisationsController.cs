using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.api.Controllers
{
    public class AdminOrganisationsController : Controller
    {
        public ActionResult Index()
        {
            List<Organisation> Organisations = OrganisationsDA.GetOrganisations();
            return View(Organisations);
        }
        public ActionResult Details(int id)
        {

            return View(OrganisationsDA.GetOrganisationById(id));
        }
        public ActionResult Edit(int id)
        {
            return View(OrganisationsDA.GetOrganisationById(id));
        }
        [HttpPost]
        public ActionResult Edit(Organisation Organisation)
        {
            Organisation EncryptedOrganisation = new Organisation()
            {
                Login = Cryptography.Encrypt(Organisation.Login),
                Password = Cryptography.Encrypt(Organisation.Password),
                DbName = Cryptography.Encrypt(Organisation.DbName),
                DbLogin = Cryptography.Encrypt(Organisation.DbLogin),
                DbPassword = Cryptography.Encrypt(Organisation.DbPassword),
                OrganisationName = Organisation.OrganisationName,
                Address = Organisation.Address,
                Email = Organisation.Email,
                Phone = Organisation.Phone
            };
            OrganisationsDA.UpdateOrganisation(EncryptedOrganisation);
            return RedirectToAction("Index", "AdminOrganisations");
        }
        public ActionResult Delete(int id)
        {
            OrganisationsDA.DeleteOrganisation(id);
            return RedirectToAction("Index", "AdminOrganisations");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Organisation Organisation)
        {
            Organisation EncryptedOrganisation = new Organisation()
            {
                Login = Cryptography.Encrypt(Organisation.Login),
                Password = Cryptography.Encrypt(Organisation.Password),
                DbName = Cryptography.Encrypt(Organisation.DbName),
                DbLogin = Cryptography.Encrypt(Organisation.DbLogin),
                DbPassword = Cryptography.Encrypt(Organisation.DbPassword),
                OrganisationName = Organisation.OrganisationName,
                Address = Organisation.Address,
                Email = Organisation.Email,
                Phone = Organisation.Phone
            };
            OrganisationsDA.InsertOrganisation(EncryptedOrganisation);
            OrganisationsDA.CreateDatabase(Organisation);
            return RedirectToAction("Index", "AdminOrganisations");
        }
    }
}