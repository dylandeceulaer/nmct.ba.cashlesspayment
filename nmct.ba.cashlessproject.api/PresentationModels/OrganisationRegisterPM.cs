using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.api.PresentationModels
{
    public class OrganisationRegisterPM
    {
        public Organistion_Register  OrganisationRegiser { get; set; }
        public List<SelectListItem> Organisations { get; set; }
    }
}