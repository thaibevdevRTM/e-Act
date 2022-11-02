using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.MasterData;
using eForms.Presenter.AppCode;
using iTextSharp.tool.xml.html;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using WebLibrary;
using static iTextSharp.text.pdf.AcroFields;

namespace eActForm.Controllers
{
    public class ApiApproveController : Controller
    {
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult doApprove(string gridHtml, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();

            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();

            //TBM
            //activity_TBMMKT_Model = ReportAppCode.mainReport(activityId, "70008316");
            //string output = ApproveAppCode.RenderViewToString("eAct", "Index", activity_TBMMKT_Model);

            //MT
            //string outputHtml = ApproveAppCode.RenderViewToString("eAct", "previewActMT", ReportAppCode.previewApprove(activityId, "70008316"));
            //ApproveModel.approveModels models = new ApproveModel.approveModels();
            //models = new ApproveController().getApproveSigList("4002da07-59c8-4310-9341-daedbe8f5a12", ConfigurationManager.AppSettings["subjectActivityFormId"], "70008316");
            //outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", models);


            string empId = UtilsAppCode.Session.User.empId;

            ApproveAppCode.setCountWatingApprove();
            HostingEnvironment.QueueBackgroundWorkItem(c => doGenFile(gridHtml, empId, statusId, activityId));

            return Json(resultAjax, "text/plain");
        }

        /// <summary>
        /// ******* BackGround Service can't  use session ************
        /// </summary>
        /// <param name="gridHtml"></param>
        /// <param name="empId"></param>
        /// <param name="statusId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult doApprovel_Consumer(string messageKey, string eventName, string messageVersion, string messageDate, ApproverModel data)
        {
            var resultAjax = new AjaxResult();
            try
            {
                if (data != null)
                {
                    if (ApproveAppCode.updateApprove(data.refId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == eventName).FirstOrDefault().val1, data.message, null, data.approver) > 0)
                    {
                        resultAjax.Success = true;
                    }

                    if (resultAjax.Success)
                    {
                        Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                        ApproveModel.approveModels approveModels = new ApproveModel.approveModels();
                        activity_TBMMKT_Model = ReportAppCode.mainReport(data.refId, data.approver);
                       

                        string outputHtml = "";
                        if (!string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                        {
                            approveModels = new ApproveController().getApproveSigList(data.refId, activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, data.approver);
                            foreach (var item in activity_TBMMKT_Model.master_Type_Form_Detail_Models)
                            {

                                var estimateList = activity_TBMMKT_Model.activityOfEstimateList;
                                activity_TBMMKT_Model.activityOfEstimateList = estimateList.Where(x => x.activityTypeId == "1").ToList();
                                activity_TBMMKT_Model.activityOfEstimateList2 = estimateList.Where(x => x.activityTypeId == "2").ToList();
                                activity_TBMMKT_Model.masterRequestEmp = QueryGet_empDetailById.getEmpDetailById(activity_TBMMKT_Model.activityFormTBMMKT.empId);

                                outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", approveModels);


                            }

                            //outputHtml = ApproveAppCode.RenderViewToString("eAct", "Index", activity_TBMMKT_Model);
                        }
                        else
                        {
                            outputHtml = ApproveAppCode.RenderViewToString("eAct", "previewActMT", ReportAppCode.previewApprove(data.refId, data.approver));


                            approveModels = new ApproveController().getApproveSigList(data.refId, ConfigurationManager.AppSettings["subjectActivityFormId"], data.approver);
                            outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", approveModels);
                        }

                        HostingEnvironment.QueueBackgroundWorkItem(c => doGenFile(outputHtml, data.approver, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == eventName).FirstOrDefault().val1, data.refId));
                    }

                }

                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;

                throw new Exception("apiApprove doApprovel_Consumer >> " + ex.Message);
            }

            return Json(resultAjax, "text/plain");
        }


        private async Task<AjaxResult> doGenFile(string gridHtml, string empId, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();
            try
            {

                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    var rootPathMap = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
                    var txtStamp = "เอกสารถูกยกเลิก";
                    bool success = AppCode.stampCancel(Server, rootPathMap, txtStamp);

                    ApproveAppCode.apiProducerApproveAsync(empId, activityId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == statusId).FirstOrDefault().displayVal);
                    EmailAppCodes.sendReject(activityId, AppCode.ApproveType.Activity_Form, empId);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    if (statusId == "3")
                    {
                        var resultAPI = ApproveAppCode.apiProducerApproveAsync(empId, activityId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == statusId).FirstOrDefault().displayVal);
                    }

                    GenPDFAppCode.doGen(gridHtml, activityId, Server);
                    EmailAppCodes.sendApprove(activityId, AppCode.ApproveType.Activity_Form, false);
                }
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;

                throw new Exception("apiApprove genPdfApprove >> " + ex.Message);
            }

            return resultAjax;
        }


        // GET: ApiApprove
        public ActionResult Index()
        {
            return View();
        }
    }
}