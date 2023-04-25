using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebLibrary;


namespace eActForm.Controllers
{
    [LoginExpire]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string actId, string typeForm)
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport(typeForm);

            if (typeForm == Activity_Model.activityType.MT.ToString() || typeForm == Activity_Model.activityType.SetPrice.ToString())
            {
                models.customerslist = QueryGetAllCustomers.getCustomersMT();
            }
            else
            {
                models.customerslist = QueryGetAllCustomers.getCustomersOMT();
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

        public ActionResult myDoc(string actId, string typeForm)
        {

            Activity_Model.actForms model;
            if (TempData["SearchDataModel"] != null)
            {
                model = (Activity_Model.actForms)TempData["SearchDataModel"];
            }
            else
            {
                DateTime getDateStart = DateTime.Now.AddDays(-7);
                if (string.IsNullOrEmpty(typeForm))
                {
                    //support click link from email
                    var getCompany = QueryGetActivityById.getActivityById(actId).FirstOrDefault().companyId;
                    typeForm = BaseAppCodes.getactivityTypeByCompanyId(getCompany);
                    getDateStart = DateTime.Now.AddDays(-100);
                }
                model = new Activity_Model.actForms();
                model.actLists = ActFormAppCode.getActFormByEmpId(getDateStart, DateTime.Now, typeForm);
                model.typeForm = typeForm;

                if (actId != null && actId != "")
                {
                    model.actLists = model.actLists.Where(x => x.id.Contains(actId)).ToList();
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



        public ActionResult requestDeleteDoc(string actId, string statusId, string statusNote, string typeForm)
        {
            AjaxResult result = new AjaxResult();
            try
            {

                result.Success = false;
                if (statusId == "1" || statusId == "6" || (statusId == "5" && ActFormAppCode.isOtherCompanyMTOfDocByActId(actId)))
                {
                    // case delete
                    result.Success = ActFormAppCode.deleteActForm(actId, ConfigurationManager.AppSettings["messRequestDeleteActForm"], statusNote) > 0 ? true : false;
                    ApproveAppCode.updateBudgetControl_Balance(actId);
                    if (statusId == "6" && result.Success && !ActFormAppCode.isOtherCompanyMTOfDocByActId(actId))
                    {
                        EmailAppCodes.sendRequestCancelToAdmin(actId);
                    }
                }
                else if (statusId == "5" || statusId == "3")
                {
                    // waiting delete
                    result.Success = ActFormAppCode.updateWaitingCancel(actId, ConfigurationManager.AppSettings["messRequestDeleteActForm"], statusNote) > 0 ? true : false;
                    if (result.Success)
                    {
                        var rootPathMap = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actId));
                        var txtStamp = "เอกสารถูกยกเลิก";
                        bool success = AppCode.stampCancel(Server, rootPathMap, txtStamp);

                        EmailAppCodes.sendRequestCancelToAdmin(actId);
                    }
                }

                ApproveAppCode.setCountWatingApprove();

                // TempData["SearchDataModel"] = result.Success ? null : TempData["SearchDataModel"];
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("requestDeleteDoc => " + ex.Message);
            }

            return RedirectToAction("myDoc", new { typeForm = typeForm });
        }

        public ActionResult searchActForm(string activityType)
        {
            string count = Request.Form.AllKeys.Count().ToString();

            Activity_Model.actForms model;
            DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-7) : DateTime.ParseExact(Request.Form["startDate"], "dd/MM/yyyy", null);
            DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "dd/MM/yyyy", null);
            model = new Activity_Model.actForms
            {
                actLists = ActFormAppCode.getActFormByEmpId(startDate, endDate, activityType)
            };

            if (!string.IsNullOrEmpty(Request.Form["ddlMasterType"]))
            {
                var masterList = Request.Form["ddlMasterType"].Split(',').ToList();
                if (masterList.Any())
                {
                    model.actLists = model.actLists.Where(r => masterList.Contains(r.master_type_form_id)).ToList();
                }
            }
            if (!string.IsNullOrEmpty(Request.Form["txtActivityNo"]))
            {
                model.actLists = model.actLists.Where(r => r.activityNo.Contains(Request.Form["txtActivityNo"])).ToList();
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

            if (!string.IsNullOrEmpty(Request.Form["ddlDepartment"]))
            {
                if (Request.Form["ddlDepartment"] == "Channel")
                {
                    model.actLists = model.actLists.Where(r => r.channelId != "").ToList();
                }
                else
                {
                    model.actLists = model.actLists.Where(r => r.brandId != "").ToList();
                }

            }

            if (!string.IsNullOrEmpty(Request.Form["ddlbrand"]))
            {
                model.actLists = model.actLists.Where(r => r.brandId == Request.Form["ddlbrand"]).ToList();
            }
            if (!string.IsNullOrEmpty(Request.Form["ddlChannel"]))
            {
                model.actLists = model.actLists.Where(r => r.channelId == Request.Form["ddlChannel"]).ToList();
            }




            model.typeForm = activityType;
            TempData["SearchDataModel"] = model;
            return RedirectToAction("myDoc");
        }



        public ActionResult searchForIconReject()
        {
            Activity_Model.actForms model;
            model = new Activity_Model.actForms
            {
                actLists = ActFormAppCode.getActFormRejectByEmpId()
            };

            model.typeForm = BaseAppCodes.getCompanyTypeForm().ToString();
            TempData["SearchDataModel"] = model;
            return RedirectToAction("Index", new { typeForm = model.typeForm });
        }

        public ActionResult logOut()
        {
            UtilsAppCode.Session.User = null;

            HttpCookie delCookie = new HttpCookie("CL");
            delCookie.Expires = DateTime.Now.AddDays(-1D);
            Response.Cookies.Add(delCookie);

            return RedirectToAction("index", "home");
        }

        public ActionResult contact()
        {
            return View();
        }

        public JsonResult getStatusNote(string actId)
        {
            AjaxResult result = new AjaxResult();

            result.Message = ActFormAppCode.getStatusNote(actId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult checkCompany(string actId)
        {
            AjaxResult result = new AjaxResult();

            result.Success = ActFormAppCode.isOtherCompanyMTOfDocByActId(actId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}