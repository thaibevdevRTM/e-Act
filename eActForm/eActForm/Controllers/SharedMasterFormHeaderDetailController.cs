using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormHeaderDetailController : Controller
    {
        public ActionResult headerDetailsDate(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
        public ActionResult headerDetails(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
    }
}