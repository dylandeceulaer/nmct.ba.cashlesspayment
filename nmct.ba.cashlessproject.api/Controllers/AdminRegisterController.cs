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
    public class AdminRegisterController : Controller
    {
        // GET: AdminRegister
        public ActionResult Index()
        {
            List<Register> registers = RegistersDA.GetRegisters();
            return View(registers);
        }
        public ActionResult Details(int id)
        {
            return View(RegistersDA.GetRegisterById(id));
        }
        public ActionResult Edit(int id)
        {
            return View(RegistersDA.GetRegisterById(id));
        }
        [HttpPost]
        public ActionResult Edit(Register reg)
        {
            RegistersDA.UpdateRegister(reg);
            return RedirectToAction("Index", "AdminRegister");
        }
        public ActionResult Delete(int id)
        {
            RegistersDA.DeleteRegister(id);
            return RedirectToAction("Index", "AdminRegister");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Register reg)
        {
            RegistersDA.InsertRegister(reg);
            return RedirectToAction("Index", "AdminRegister");
        }

    }
}