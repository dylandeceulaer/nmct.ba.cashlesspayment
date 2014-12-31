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
    public class AdminLogsController : Controller
    {
        private static List<Claim> GetClaims(Organisation o)
        {
            List<Claim> c = new List<Claim>();
            c.Add(new Claim("dblogin", Cryptography.Encrypt(o.DbLogin)));
            c.Add(new Claim("dbpass", Cryptography.Encrypt(o.DbPassword)));
            c.Add(new Claim("dbname", Cryptography.Encrypt(o.DbName)));
            return c;
        }

        private static DateTime ConvertUnixTimeStamp(int timestamp)
        {
            long timestamplong = Convert.ToInt64(timestamp);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamplong).ToLocalTime();
            return dtDateTime;
        }

        // GET: AdminLogs
        public ActionResult Index(int? id)
        {
            List<Organisation> orgs = OrganisationsDA.GetOrganisations();
            List<SelectListItem> li = new List<SelectListItem>();
            foreach (Organisation org in orgs)
            {
                li.Add(new SelectListItem()
                {
                    Value = org.ID.ToString(),
                    Text = org.OrganisationName
                });
            }
            ViewBag.orgs = li;

            Organisation o = orgs[0];
            if (id.HasValue) o = (from e in orgs where e.ID == (int)id select e).FirstOrDefault();
            if (o == null) o = orgs[0];
            ViewBag.CurrentOrg = o;

            List<Errorlog> le = ErrorlogDA.GetErrorlogsById(o.ID);

            int datemax = 0;
            if (le.Count != 0) datemax = (from e in le select e.Timestamp).Max();

            List<Claim> c = GetClaims(o);

            List<Register> regs = RegistersDA.GetRegisters(c);

            List<Errorlog> newle = ErrorlogDA.GetNewErrorlogs(c, datemax,o.ID);
            if (newle.Count > 0)
            {
                foreach (Errorlog errorlog in newle)
                {
                    ErrorlogDA.InsertErrorLog(errorlog);
                }
            }

            List<LogPM> logpms = new List<LogPM>();

            foreach (Errorlog e in newle)
            {
                logpms.Add(new LogPM()
                {
                    Register = (from r in regs where r.Id == e.RegisterID select r.RegisterName).FirstOrDefault(),
                    IsNew = true,
                    Message = e.Message,
                    StackTrace = e.Stacktrace,
                    Date = ConvertUnixTimeStamp(e.Timestamp)
                });
            }

            foreach (Errorlog e in le)
            {
                logpms.Add(new LogPM()
                {
                    Register = (from r in regs where r.Id == e.RegisterID select r.RegisterName ).FirstOrDefault(),
                    IsNew = false,
                    Message = e.Message,
                    StackTrace = e.Stacktrace,
                    Date = ConvertUnixTimeStamp(e.Timestamp)
                });
            }

            

            return View(logpms);
        }
    }
}