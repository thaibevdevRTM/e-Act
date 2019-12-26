using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.Models;
using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string actId, string typeForm)
        {
            SearchActivityModels models = new SearchActivityModels();
            try
            {
                models = SearchAppCode.getMasterDataForSearchForDetailReport();
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
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Home => Index >> " + ex.Message);
            }
            return View(models);
        }

        public ActionResult approveLists(string actId)
        {
            var result = new AjaxResult();
            ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = ApproveAppCode.getApproveByActFormId(actId);
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Home => approveLists >> " + ex.Message);
            }
            return PartialView(models);
        }



        public ActionResult myDoc(string actId , string typeForm)
        {
            Activity_Model.actForms model = new Activity_Model.actForms();
            try
            {
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
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Home => myDoc >> " + ex.Message);
            }
            return PartialView(model);
        }



        public ActionResult requestDeleteDoc(string actId, string statusId)
        {
            AjaxResult result = new AjaxResult();
            result.Success = false;
            try
            {
                if (statusId == "1" || statusId == "6" || statusId == "5")
                {
                    //Draft || หรือ ถูก reject มาแล้วเป็นบริษัทTBM ให้สามารถยกเลิกเอกสารทิ้งได้ทันที เฟรม dev date 20191224
                    result.Success = ActFormAppCode.deleteActForm(actId, ConfigurationManager.AppSettings["messRequestDeleteActForm"]) > 0 ? true : false;
                }
                else
                {
                    result.Success = ActFormAppCode.updateWaitingCancel(actId, ConfigurationManager.AppSettings["messRequestDeleteActForm"]) > 0 ? true : false;
                }

                TempData["SearchDataModel"] = result.Success ? null : TempData["SearchDataModel"];
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Home => requestDeleteDoc >> " + ex.Message);
            }
            return RedirectToAction("myDoc");
        }

        public ActionResult searchActForm(string activityType)
        {
            try
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
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Home => searchActForm >> " + ex.Message);
            }
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