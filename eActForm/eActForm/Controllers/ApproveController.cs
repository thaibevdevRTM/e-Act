﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;
using eActForm.Models;
using System.Configuration;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class ApproveController : Controller
    {
        // GET: Approve
        public ActionResult Index(string actId)
        {
            if (actId == null) return RedirectToAction("index", "Home");
            else
            {
                ApproveAppCode.insertApprove(actId);
                ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
                models.approveStatusLists = ApproveAppCode.getApproveStatus();
                return View(models);
            }
        }

        [HttpPost]
        public JsonResult insertApprove()
        {
            var result = new AjaxResult();
            if (ApproveAppCode.updateApprove(Request.Form["lblActFormId"], Request.Form["approveStatusLists"], Request.Form["txtRemark"]) > 0)
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
            }

            return Json(result);
        }

        public ActionResult approveLists(ApproveModel.approveModels models)
        {
            return PartialView(models);
        }

        public ActionResult approvePositionLists(string customerId, string productTypeId)
        {
            ApproveFlowModel.approveFlowModel model = ApproveFlowAppCode.getFlow(ConfigurationManager.AppSettings["subjectActivityFormId"], customerId, productTypeId);
            return PartialView(model);
        }

        public ActionResult approvePositionSignatureLists(string actId)
        {
            ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
            ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowId(ConfigurationManager.AppSettings["subjectActivityFormId"], actId);
            models.approveFlowDetail = flowModel.flowDetail;
            return PartialView(models);
        }


        public ActionResult previewApprove(string actId)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.activityFormModel = QueryGetActivityById.getActivityById(actId).FirstOrDefault();
            activityModel.productcostdetaillist = QueryGetCostDetailById.getcostDetailById(actId);
            activityModel.costthemedetail = QueryGetActivityDetailById.getActivityDetailById(actId);

            return PartialView(activityModel);
        }

        
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult genPdfApprove(string GridHtml, string activityId)
        {
            var resultAjax = new AjaxResult();
            try
            {
                eActController.sendEmail(
                    "tanapong.w@thaibev.com"
                    , "champ.tanapong@gmail.com"
                    , "Test Subject eAct"
                    , "Test Body"
                    , eActController.genPdfFile(GridHtml, activityId)

                    );
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax, "text/plain");
        }



    }
}