using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using iTextSharp.text;
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
                //if (model.activity_TBMMKT_Model.activityOfEstimateList.Any())
                //{
                //    ApproveAppCode.manageApproveEmpExpense(model, Request.Form["lblActFormId"]);
                //}


                if (ApproveAppCode.updateApprove(Request.Form["lblActFormId"], Request.Form["ddlStatus"], Request.Form["txtRemark"], Request.Form["lblApproveType"]) > 0)
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


        public ActionResult approveLists(ApproveModel.approveModels models)
        {
            return PartialView(models);
        }

        public ActionResult approvePositionSignatureLists(string actId, string subId)
        {
            ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = ApproveAppCode.getApproveByActFormId(actId);
                ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowId(subId, actId);
                models.approveFlowDetail = flowModel.flowDetail;
                //=============dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========
                Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                models.activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actId);
                //=======END======dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("approvePositionSignatureLists >>" + ex.Message);
                TempData["approvePositionSignatureError"] = AppCode.StrMessFail + ex.Message;
            }
            return PartialView(models);
        }

        public ActionResult previewApprove(string actId)
        {
            Activity_Model activityModel = new Activity_Model();
            try
            {
                activityModel.activityFormModel = QueryGetActivityById.getActivityById(actId).FirstOrDefault();
                activityModel.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(actId);
                activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(actId);
                activityModel.productImageList = ImageAppCode.GetImage(actId).Where(x => x.extension != ".pdf").ToList();
                activityModel.activityFormModel.typeForm = BaseAppCodes.getactivityTypeByCompanyId(activityModel.activityFormModel.companyId);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("previewApprove >>" + ex.Message);
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


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult genPdfApprove(string GridHtml, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();
            try
            {
                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    EmailAppCodes.sendReject(activityId, AppCode.ApproveType.Activity_Form, UtilsAppCode.Session.User.empId);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId + "_");
                    GridHtml = GridHtml.Replace("<br>", "<br/>");
                    GridHtml = GridHtml.Replace("undefined", "");
                    AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

                    TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
                    getImageModel.tbActImageList = ImageAppCode.GetImage(activityId).Where(x => x.extension == ".pdf").ToList();
                    string[] pathFile = new string[getImageModel.tbActImageList.Count + 1];
                    pathFile[0] = Server.MapPath(rootPathInsert);
                    if (getImageModel.tbActImageList.Any())
                    {
                        int i = 1;
                        foreach (var item in getImageModel.tbActImageList)
                        {
                            pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                            i++;
                        }
                    }
                    var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
                    var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile);

                    EmailAppCodes.sendApprove(activityId, AppCode.ApproveType.Activity_Form, false);
                    ApproveAppCode.setCountWatingApprove();
                }
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("genPdfApprove >> " + ex.Message);
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax, "text/plain");
        }


        public ActionResult approvePositionSignatureListsV2(string actId, string subId)
        {
            ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = ApproveAppCode.getApproveByActFormId(actId);
                ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowId(subId, actId);
                models.approveFlowDetail = flowModel.flowDetail;
                //=============dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========
                Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                models.activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actId);
                //=======END======dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("approvePositionSignatureLists >>" + ex.Message);
                TempData["approvePositionSignatureError"] = AppCode.StrMessFail + ex.Message;
            }
            return PartialView(models);
        }
    }
}