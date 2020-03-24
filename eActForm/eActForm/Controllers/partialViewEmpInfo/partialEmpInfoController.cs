using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class partialEmpInfoController : Controller
    {
        // GET: partialEmpInfo
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}