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
        public ActionResult cr_IT_Frm314_page1(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getMasterChooseSystemCRFormIT314(activity_TBMMKT_Model);
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult cr_IT_Frm314_page2(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getMasterChooseSystemCRFormIT314_page2(activity_TBMMKT_Model);
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult inputPageSectionOneTitle_IT314(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult inputPageSectionOne_IT314(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getMasterChooseSystemCRFormIT314(activity_TBMMKT_Model);
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult inputPageSectionFiveTitle_IT314(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getMasterChooseSystemCRFormIT314_page2(activity_TBMMKT_Model);
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult inputPageSectionFive_IT314(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getMasterChooseSystemCRFormIT314(activity_TBMMKT_Model);
            return PartialView(activity_TBMMKT_Model);
        }

    }
}