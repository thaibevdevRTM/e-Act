﻿using eActForm.BusinessLayer;
using eActForm.Models;
using eForms.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ApproveListsController : Controller
    {
        // GET: ApproveLists
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            models.approveStatusList.Add(new ApproveModel.approveStatus()
            {
                id = "7",
                nameTH = "ทั้งหมด",
                nameEN = "All",
            });
            return View(models);
        }

        public ActionResult ListView(string fromPage, string StatusApprove)
        {
            Activity_Model.actForms model = new Activity_Model.actForms();
            ActSignatureModel.SignModels signModels = SignatureAppCode.currentSignatureByEmpId(UtilsAppCode.Session.User.empId);
            if (signModels.lists == null || signModels.lists.Count == 0)
            {
                ViewBag.messCannotFindSignature = true;
            }
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
            return PartialView(model);
        }


        public ActionResult searchActForm()
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
            //============เดิมไม่ได้ใช้ เพิ่มการกรอกง createDate กรอง เฟรมเพิ่ม ให้ทำงานได้ 20200527=============
            if (!string.IsNullOrEmpty(Request.Form["startDate"]) && !string.IsNullOrEmpty(Request.Form["endDate"]))
            {
                model.actLists = model.actLists.Where(r => r.documentDate >= DateTime.ParseExact(Request.Form["startDate"], "dd/MM/yyyy", null) && r.documentDate <= DateTime.ParseExact(Request.Form["endDate"], "dd/MM/yyyy", null)).ToList();
            }
            //===END=========เดิมไม่ได้ใช้ เพิ่มการกรอกง createDate กรอง เฟรมเพิ่ม ให้ทำงานได้ 20200527=============
            TempData["ApproveSearchResult"] = model.actLists;
            return RedirectToAction("ListView");
        }


        public ActionResult getRejectApproveByEmpId()
        {
            Activity_Model.actForms model = new Activity_Model.actForms();
            model = new Activity_Model.actForms();
            model.actLists = ApproveListAppCode.getRejectApproveListsByEmpId(UtilsAppCode.Session.User.empId);
            TempData["ApproveSearchResult"] = model.actLists;
            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult insertApproveList(string actId, string status, string approveType)
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