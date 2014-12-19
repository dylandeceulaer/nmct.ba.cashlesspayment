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
    public class AdministrationController : Controller
    {
        // GET: Administration
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Organisation organisation)
        {
            Organisation EncryptedOrganisation = new Organisation()
            {
                Login = Cryptography.Encrypt(organisation.Login),
                Password = Cryptography.Encrypt(organisation.Password),
                DbName = Cryptography.Encrypt(organisation.DbName),
                DbLogin = Cryptography.Encrypt(organisation.DbLogin),
                DbPassword = Cryptography.Encrypt(organisation.DbPassword),
                OrganisationName = organisation.OrganisationName,
                Address = organisation.Address,
                Email = organisation.Email,
                Phone = organisation.Phone
            };
            OrganisationsDA.InsertOrganisation(EncryptedOrganisation);
            return View();
        }
    }
}