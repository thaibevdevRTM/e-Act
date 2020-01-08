using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormHeaderCompanyController : Controller
    {
        public ActionResult headerCompany(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
    }
}