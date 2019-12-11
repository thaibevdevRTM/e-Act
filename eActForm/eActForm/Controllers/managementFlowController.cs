using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class ManagementFlowController : Controller
    {
        // GET: managementFlow
        public ActionResult Index()
        {
            ManagementFlow_Model model = new ManagementFlow_Model();
            model.companyList = managementFlowAppCode.getCompany();

            return View(model);
        }
    }
}