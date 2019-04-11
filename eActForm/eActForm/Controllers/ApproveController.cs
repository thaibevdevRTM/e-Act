using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;

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
                ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
                models.approveStatusLists = ApproveAppCode.getApproveStatus(AppCode.StatusType.app).Where(x => x.id == "3" || x.id == "5").ToList();
                return View(models);
            }
        }

        [HttpPost]
        public JsonResult insertApprove()
        {
            var result = new AjaxResult();
            result.Success = false;
            try
            {
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
                result.Message = ex.Message;
            }
            return Json(result);
        }

        public ActionResult approveLists(ApproveModel.approveModels models)
        {
            return PartialView(models);
        }

        public ActionResult approvePositionLists(string customerId, string productCatId)
        {
            ApproveFlowModel.approveFlowModel model = ApproveFlowAppCode.getFlow(ConfigurationManager.AppSettings["subjectActivityFormId"], customerId, productCatId);
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
            activityModel.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(actId);
            activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(actId);

            return PartialView(activityModel);
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
                    EmailAppCodes.sendReject(activityId);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
                    AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), rootPath);
                    EmailAppCodes.sendApprove(activityId,AppCode.ApproveType.Activity_Form);
                }
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