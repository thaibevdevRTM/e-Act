using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.Models;
using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string actId, string typeForm)
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport();
            if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
            {
                if (typeForm == Activity_Model.activityType.MT.ToString())
                {
                    models.customerslist = QueryGetAllCustomers.getCustomersMT();
                }
                else
                {
                    models.customerslist = QueryGetAllCustomers.getCustomersOMT();
                }
            }

            models.typeForm = typeForm;
            return View(models);
        }

        public ActionResult approveLists(string actId)
        {
            var result = new AjaxResult();
            ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
            //models.approveStatusLists = ApproveAppCode.getApproveStatus();
            return PartialView(models);
        }



        public ActionResult myDoc(string actId , string typeForm)
        {
            Activity_Model.actForms model;
            if (TempData["SearchDataModel"] != null)
            {
                model = (Activity_Model.actForms)TempData["SearchDataModel"];
            }
            else
            {
                model = new Activity_Model.actForms();
                model.actLists = ActFormAppCode.getActFormByEmpId(DateTime.Now.AddDays(-7), DateTime.Now, typeForm);
                model.typeForm = typeForm;

                if (actId != null && actId != "")
                {
                    model.actLists = model.actLists.Where(r => r.id.Equals(actId)).ToList();
                }
            }

            TempData["SearchDataModel"] = null;
            return PartialView(model);
        }


        public JsonResult checkActInvoice(string actId)
        {
            AjaxResult result = new AjaxResult();

            result.Success = ActFormAppCode.checkActInvoice(actId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult requestDeleteDoc(string actId, string statusId)
        {
            //return RedirectToAction("index");
            AjaxResult result = new AjaxResult();
            result.Success = false;
            if ((statusId == "1")|| (statusId == "3") || (statusId == "6" && (UtilsAppCode.Session.User.isAdminOMT || UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)) || (statusId == "5" && (UtilsAppCode.Session.User.empCompanyId == "3030"||UtilsAppCode.Session.User.isAdminOMT || UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isAdminTBM || UtilsAppCode.Session.User.isSuperAdmin)))
            {
                //Draft || หรือ ถูก reject มาแล้วเป็นบริษัทTBM ให้สามารถยกเลิกเอกสารทิ้งได้ทันที เฟรม dev date 20191224
                result.Success = ActFormAppCode.deleteActForm(actId, ConfigurationManager.AppSettings["messRequestDeleteActForm"]) > 0 ? true : false;
            }
            else
            {
                result.Success = ActFormAppCode.updateWaitingCancel(actId, ConfigurationManager.AppSettings["messRequestDeleteActForm"]) > 0 ? true : false;
            }

            TempData["SearchDataModel"] = result.Success ? null : TempData["SearchDataModel"];
            return RedirectToAction("myDoc");
        }

        public ActionResult searchActForm(string activityType)
        {
            string count = Request.Form.AllKeys.Count().ToString();

            Activity_Model.actForms model;
            DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-15) : DateTime.ParseExact(Request.Form["startDate"], "MM/dd/yyyy", null);
            DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "MM/dd/yyyy", null);
            model = new Activity_Model.actForms
            {
                actLists = ActFormAppCode.getActFormByEmpId(startDate, endDate, activityType)
            };


            if (!string.IsNullOrEmpty(Request.Form["txtActivityNo"]))
            {
                model.actLists = model.actLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlStatus"]))
            {
                model.actLists = model.actLists.Where(r => r.statusId == Request.Form["ddlStatus"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlCustomer"]))
            {
                model.actLists = model.actLists.Where(r => r.customerId == Request.Form["ddlCustomer"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlTheme"]))
            {
                model.actLists = model.actLists.Where(r => r.theme == Request.Form["ddlTheme"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlProductType"]))
            {
                model.actLists = model.actLists.Where(r => r.productTypeId == Request.Form["ddlProductType"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlProductGrp"]))
            {
                model.actLists = model.actLists.Where(r => r.productGroupid == Request.Form["ddlProductGrp"]).ToList();
            }

            model.typeForm = activityType;
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