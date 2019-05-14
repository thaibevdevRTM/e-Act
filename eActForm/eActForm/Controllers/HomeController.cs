using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult approveLists(string actId)
        {
            var result = new AjaxResult();
            ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
            //models.approveStatusLists = ApproveAppCode.getApproveStatus();
            return PartialView(models);
        }



        public ActionResult myDoc()
        {
            Activity_Model.actForms model;
            if (TempData["SearchDataModel"] != null)
            {
                model = (Activity_Model.actForms)TempData["SearchDataModel"];
            }
            else
            {
                model = new Activity_Model.actForms();
                model.actLists = ActFormAppCode.getActFormByEmpId(UtilsAppCode.Session.User.empId, DateTime.Now.AddDays(-7),DateTime.Now);
            }

            TempData["SearchDataModel"] = null;
            return PartialView(model);
        }


        public ActionResult requestDeleteDoc(string actId, string statusId)
        {
            //return RedirectToAction("index");
            AjaxResult result = new AjaxResult();
            result.Success = false;
            if (statusId == "1")
            {
                //Draft
                if (ActFormAppCode.deleteActForm(actId, "request delete by user") > 0)
                {
                    result.Success = true;
                    TempData["SearchDataModel"] = null;
                }
            }
            else
            {

            }

            return RedirectToAction("myDoc");
        }

        public ActionResult searchActForm()
        {
            string count = Request.Form.AllKeys.Count().ToString();
            Activity_Model.actForms model;
            DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-7) : DateTime.ParseExact(Request.Form["startDate"], "MM/dd/yyyy", null);
            DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "MM/dd/yyyy", null);
            model = new Activity_Model.actForms
            {
                actLists = ActFormAppCode.getActFormByEmpId(UtilsAppCode.Session.User.empId, startDate, endDate)
            };

            if (Request.Form["txtActivityNo"] != "")
            {
                model.actLists = model.actLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
            }

            if (Request.Form["ddlStatus"] != "")
            {
                model.actLists = model.actLists.Where(r => r.statusId == Request.Form["ddlStatus"]).ToList();
            }

            if(Request.Form["ddlCustomer"] != "")
            {
                model.actLists = model.actLists.Where(r => r.customerId == Request.Form["ddlCustomer"]).ToList();
            }

            if( Request.Form["ddlTheme"] != "")
            {
                model.actLists = model.actLists.Where(r => r.theme == Request.Form["ddlTheme"]).ToList();
            }

            if (Request.Form["ddlProductType"] != "")
            {
                model.actLists = model.actLists.Where(r => r.productTypeId == Request.Form["ddlProductType"]).ToList();
            }

            if (Request.Form["ddlProductGrp"] != "")
            {
                model.actLists = model.actLists.Where(r => r.productGroupid == Request.Form["ddlProductGrp"]).ToList();
            }
            
            TempData["SearchDataModel"] = model;
            return RedirectToAction("myDoc");
        }

        public ActionResult logOut()
        {
            UtilsAppCode.Session.User = null;
            return RedirectToAction("index", "home");
        }

        public ActionResult contact()
        {
            return View();
        }
    }
}