﻿using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ApproveController : Controller
    {
        // GET: Approve
        public ActionResult Index(string actId)
        {

            try
            {
                if (actId == null) return RedirectToAction("index", "Home");
                else
                {
                    ActSignatureModel.SignModels signModels = SignatureAppCode.currentSignatureByEmpId(UtilsAppCode.Session.User.empId);
                    if (signModels.lists == null || signModels.lists.Count == 0)
                    {
                        ViewBag.messCannotFindSignature = true;
                    }
                    ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
                    models.approveStatusLists = ApproveAppCode.getApproveStatus(AppCode.StatusType.app).Where(x => x.id == "3" || x.id == "5").ToList();

                    List<ActivityForm> getActList = QueryGetActivityById.getActivityById(actId);
                    if (getActList.Any() && getActList.FirstOrDefault().master_type_form_id != null)
                    {
                        models.masterTypeFormId = getActList.FirstOrDefault().master_type_form_id.ToString();
                    }

                    return View(models);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Approve >> Index >>" + ex.Message);
                return null;
            }
        }

        [HttpPost] //post method
        [ValidateInput(false)]
        public JsonResult insertApprove(ApproveModel.approveModels model)
        {
            var result = new AjaxResult();
            result.Success = false;
            try
            {

                //check status approve rang -1 
                var getEmp = ApproveAppCode.checkStatusBeforeCallKafka(UtilsAppCode.Session.User.empId, Request.Form["lblActFormId"]);
                if (!string.IsNullOrEmpty(getEmp))
                {
                    ApproveAppCode.apiProducerApproveAsync(getEmp, Request.Form["lblActFormId"], QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == "3").FirstOrDefault().displayVal);
                }

                if (ApproveAppCode.updateApprove(Request.Form["lblActFormId"], Request.Form["ddlStatus"], Request.Form["txtRemark"], Request.Form["lblApproveType"], UtilsAppCode.Session.User.empId) > 0)
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

        [HttpPost]
        public JsonResult selectApprove(string actId, string status, string approveType)
        {
            var result = new AjaxResult();
            result.Success = false;
            try
            {
                if (ApproveAppCode.updateApprove(actId, status, "", approveType, UtilsAppCode.Session.User.empId) > 0)
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


        public ActionResult approveLists(ApproveModel.approveModels models)
        {
            return PartialView(models);
        }

        public ActionResult previewApprove(string actId)
        {
            Activity_Model activityModel = new Activity_Model();
            try
            {
                activityModel = ReportAppCode.previewApprove(actId, UtilsAppCode.Session.User.empId);
                activityModel.activityFormModel.callFrom = "app";

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("previewApprove >>" + actId + "___" + ex.Message);
            }
            return PartialView(activityModel);
        }

        public ActionResult previewActBudget(string activityId)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            if (!string.IsNullOrEmpty(activityId))
            {
                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);
            }

            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult getApproveComment(string actId, string actTypeName)
        {
            ApproveModel.approveModels model = new ApproveModel.approveModels();
            try
            {
                //add Condition Admin For Regen PDF
                if (actTypeName == "FOC" && ConfigurationManager.AppSettings["empIdShowAtCommentApproved"].Contains(UtilsAppCode.Session.User.empId)
                    || UtilsAppCode.Session.User.isSuperAdmin || UtilsAppCode.Session.User.isAdmin)
                {
                    model.approveDetailLists = ApproveAppCode.getRemarkApprovedByEmpId(actId, ConfigurationManager.AppSettings["empIdShowAtCommentApproved"]);
                    ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowId(ConfigurationManager.AppSettings["subjectActivityFormId"], actId);
                    model.approveFlowDetail = flowModel.flowDetail.Where(x => x.empId == ConfigurationManager.AppSettings["empIdShowAtCommentApproved"]).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getApproveComment >>" + ex.Message);
            }
            return PartialView(model);
        }


        public ActionResult approvePositionSignatureLists(string actId, string subId)
        {
            ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = getApproveSigList(actId, subId, UtilsAppCode.Session.User.empId);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("approvePositionSignatureLists >>" + ex.Message);
            }
            return PartialView(models);
        }
        public ActionResult approvePositionSignatureListsV2(string actId, string subId)
        {
            ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = getApproveSigList(actId, subId, UtilsAppCode.Session.User.empId);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("approvePositionSignatureLists >>" + ex.Message);
            }
            return PartialView(models);

        }


        public ApproveModel.approveModels getApproveSigList(string actId, string subId, string empId)
        {
            ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = ApproveAppCode.getApproveByActFormId(actId, empId);
                models.approveFlowDetail = ApproveFlowAppCode.getFlowId(subId, actId).flowDetail;
                //เพิ่มตัดตำแหน่ง
                newlinePosition(models);
                //=============dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========
                Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                models.activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actId);
                //=======END======dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getApproveSigList >>" + ex.Message);
                TempData["approvePositionSignatureError"] = AppCode.StrMessFail + ex.Message;
            }
            return models;
        }


        public void newlinePosition(ApproveModel.approveModels model)
        {

            List<positionModel> positionList = new List<positionModel>();
            positionList = QueryGetPosition.getNewlinePosition().ToList();
            if (positionList.Count > 0)
            {
                int index = 0;
                int indexDetailLists = 0;


                if (model.approveDetailLists.Any())
                {
                    foreach (var item in model.approveDetailLists)
                    {

                        var positionEN = positionList.Where(x => x.positionName == item.empPositionTitleEN).FirstOrDefault()?.newPositionName;
                        if (positionEN != null)
                        {
                            model.approveDetailLists[indexDetailLists].empPositionTitleEN = positionEN;
                        }

                        var positionTH = positionList.Where(x => x.positionName == item.empPositionTitleTH).FirstOrDefault()?.newPositionName;
                        if (positionTH != null)
                        {
                            model.approveDetailLists[indexDetailLists].empPositionTitleTH = positionTH;
                        }

                        indexDetailLists++;
                    }
                }
                else
                {
                    foreach (var item in model.approveFlowDetail)
                    {

                        var positionEN = positionList.Where(x => x.positionName == item.empPositionTitleEN).FirstOrDefault()?.newPositionName;
                        if (positionEN != null)
                        {
                            model.approveFlowDetail[index].empPositionTitleEN = positionEN;
                        }

                        var positionTH = positionList.Where(x => x.positionName == item.empPositionTitleTH).FirstOrDefault()?.newPositionName;
                        if (positionTH != null)
                        {
                            model.approveFlowDetail[index].empPositionTitleTH = positionTH;
                        }

                        index++;
                    }
                }

            }
        }

        public ApproveModel.approveModels setSrcSignature(ApproveModel.approveModels models, string actId)
        {
            List<ActivityForm> getActList = QueryGetActivityById.getActivityById(actId);
            if (getActList.Any() && getActList.FirstOrDefault().master_type_form_id != null)
            {
                if (AppCode.hcForm.Contains(getActList.FirstOrDefault().master_type_form_id.ToString()))
                {
                    int index = 0;
                    foreach (var item in models.approveDetailLists)
                    {
                        models.approveDetailLists[index].ImgName = models.approveDetailLists[index].ImgName.Replace(ConfigurationManager.AppSettings["renderHost"], ConfigurationManager.AppSettings["renderHostPublicIP"]);
                        index++;
                    }
                }
            }
            return models;
        }


        [HttpPost]
        public JsonResult checkApproveByActId(string actId)
        {
            var result = new AjaxResult();
            ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);

            result.Success = models.approveModel.statusId == "3";
            return Json(result);
        }

    }
}