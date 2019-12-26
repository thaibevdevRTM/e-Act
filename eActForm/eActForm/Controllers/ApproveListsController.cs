using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ApproveListsController : Controller
    {
        // GET: ApproveLists
        public ActionResult Index()
        {
            SearchActivityModels models = new SearchActivityModels();
            try
            {
                models = SearchAppCode.getMasterDataForSearch();
                models.approveStatusList.Add(new ApproveModel.approveStatus()
                {
                    id = "7",
                    nameTH = "ทั้งหมด",
                    nameEN = "All",
                });
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveList >> Index >>" + ex.Message);
            }
            return View(models);
        }

        public ActionResult ListView(string fromPage, string StatusApprove)
        {
            Activity_Model.actForms model = new Activity_Model.actForms();
            try
            {
                if (TempData["ApproveSearchResult"] == null)
                {
                    model = new Activity_Model.actForms();
                    model.actLists = ApproveListAppCode.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
                    TempData["ApproveFormLists"] = model.actLists;

                    if (fromPage != null && StatusApprove != null)
                    {
                        if (fromPage == "DashboardPage")
                        {
                            if (StatusApprove == "2")
                            {
                                model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists, (int)AppCode.ApproveStatus.รออนุมัติ);
                            }
                            else if (StatusApprove == "3")
                            {
                                model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists, (int)AppCode.ApproveStatus.อนุมัติ);
                            }
                        }
                    }
                    else
                    {
                        model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists, (int)AppCode.ApproveStatus.รออนุมัติ);
                    }
                }
                else
                {
                    model.actLists = (List<Activity_Model.actForm>)TempData["ApproveSearchResult"];
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveList >> ListView >>" + ex.Message);
            }
            return PartialView(model);
        }


        public ActionResult searchActForm()
        {
            try
            {
                string count = Request.Form.AllKeys.Count().ToString();
                Activity_Model.actForms model = new Activity_Model.actForms();
                model.actLists = (List<Activity_Model.actForm>)TempData["ApproveFormLists"];

                if (!string.IsNullOrEmpty(Request.Form["ddlStatus"]) && Request.Form["ddlStatus"] != "7")
                {
                    model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists, int.Parse(Request.Form["ddlStatus"]));
                }
                if (!string.IsNullOrEmpty(Request.Form["txtActivityNo"]))
                {
                    model.actLists = model.actLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
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
                TempData["ApproveSearchResult"] = model.actLists;
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ApproveList >> searchActForm >>" + ex.Message);
            }
            return RedirectToAction("ListView");
        }

        [HttpPost]
        public JsonResult insertApproveList(string actId,string status,string approveType)
        {
            var result = new AjaxResult();
            result.Success = false;
            try
            {
                if (ApproveAppCode.updateApprove(actId, status, "", approveType) > 0)
                {
                    result.Success = true;
                }
                else
                {
                    result.Message = AppCode.StrMessFail;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("insertApprove >>" + ex.Message);
                result.Message = ex.Message;
            }
            return Json(result);
        }
    }
}