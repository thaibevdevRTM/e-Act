using eActForm.Models;
using System.Web.Mvc;
using System.Configuration;
using eForms.Presenter.MasterData;
using System.Globalization;
using System;
using System.Linq;
using System.Collections.Generic;
using eActForm.BusinessLayer;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class PartialChangeRequestFormController : Controller
    {
        CultureInfo DateThai = new CultureInfo("th-TH");
        CultureInfo DateEnglish = new CultureInfo("en-US");

        public ActionResult crHeader(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

    }
}