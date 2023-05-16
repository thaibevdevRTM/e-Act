using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.MasterData;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using static iTextSharp.text.pdf.AcroFields;


namespace eActForm.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConsumerController : ApiController
    {

        public class consumerModel
        {
            public string messagedata { get; set; }
        }

        // POST api/<controller>
        [System.Web.Http.Route("api/{Controller}")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<IHttpActionResult> Consumer(consumerModel getMdodel)
        {
            ConsumerApproverBevAPI response = null;
            ConsumerMassage consumerMassage = new ConsumerMassage();
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {
               
                response = JsonConvert.DeserializeObject<ConsumerApproverBevAPI>(getMdodel.messagedata);
                consumerMassage.messageResponse = "eact :" + response.data.refId;
                consumerMassage.timeResponse = DateTime.Now.ToString();

                SentKafkaLogModel kafka1 = new SentKafkaLogModel(response.data.approver, response.data.refId, response.eventName, "Consumer", DateTime.Now, "", "", getMdodel.messagedata);
                var resultLog1 = ApproveAppCode.insertLog_Kafka(kafka1);

                // ApproveAppCode.apiProducerApproveAsync(response.data.approver, response.data.refId, response.eventName);

                if (response != null)
                {
                    //check status approve rang -1 
                    var getEmp = ApproveAppCode.checkStatusBeforeCallKafka(response.data.approver, response.data.refId);
                    if (!string.IsNullOrEmpty(getEmp))
                    {
                        ApproveAppCode.apiProducerApproveAsync(getEmp, response.data.refId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == "3").FirstOrDefault().displayVal);
                    }

                    if (ApproveAppCode.updateApprove(response.data.refId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.message, null, response.data.approver) > 0)
                    {
                        string padding = "", classFont = "";
                        
                        ApproveModel.approveModels approveModels = new ApproveModel.approveModels();
                        activity_TBMMKT_Model = ReportAppCode.mainReport(response.data.refId, response.data.approver);
                        var getHeader = GenPDFAppCode.getHeader(activity_TBMMKT_Model);
                        string outputHtml = "";
                        if (!string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                        {
                            approveModels = new ApproveController().getApproveSigList(response.data.refId, activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, response.data.approver);

                            outputHtml += "<div id=\"divForm\" style=\"width:97%;\">";
                            outputHtml += ApproveAppCode.RenderViewToString("MainReport", "styleView", approveModels);
                            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvTbmId"]
                                || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHcmId"]
                                || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"]
                                )
                            {
                                classFont = "fontDocSmall";
                                padding = "paddingFormV2";
                                outputHtml += "<table style=\"width:100%;\" id=\"tabel_report\" class=\"fontDocSmall\">";
                            }
                            else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formBgTbmId"])//แบบฟอร์มอนุมัติจัดกิจกรรม dev date 20200313
                            {
                                classFont = "formBorderStyle1";
                                padding = "paddingFormV3";
                                outputHtml += "<table style=\"width:100%;\" id=\"tabel_report\" class=\"formBorderStyle1\">";
                            }
                            else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]
                                || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPurchaseTbm"])//ใบสั่งจ่ายTBM dev date 20200420 peerapop
                            {
                                classFont = "formBorderStyle2";
                                padding = "paddingFormV3";
                                //outputHtml += "<table style=\"width: 100%; border: 1px solid black; font-family: 'TH SarabunPSK'; font-size: 18px; color: black; border-collapse: collapse;\" id=\"tabel_report\">";
                                outputHtml += "<table style=\"width:100%;\" id=\"tabel_report\" class=\"formBorderStyle2\">";
                            }
                            else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpMedNumId"])
                            {
                                classFont = "fontDocSmall";
                                padding = "paddingFormV2";
                                outputHtml += "<table style=\"width:100%;\" id=\"tabel_report\" class=\"fontDocSmall\">";
                            }
                            else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formCR_IT_FRM_314"])
                            {
                                classFont = "formBorderStyle3";
                                padding = "paddingFormV3";
                                outputHtml += "<table style=\"width:100%;\" id=\"tabel_report\" class=\"formBorderStyle3\">";
                            }
                            else
                            {
                                classFont = "fontDocV1";
                                padding = "paddingFormV1";
                                outputHtml += "<table style=\"width:100%;\" id=\"tabel_report\" class=\"fontDocV1\">";
                            }


                            // outputHtml += "<div class=\"" + padding + "\">";
                            // outputHtml += "<table style=\"width: 100 %;\" id=\"tabel_report\" class=\"" + classFont + "\">";

                            foreach (var item in activity_TBMMKT_Model.master_Type_Form_Detail_Models)
                            {
                                outputHtml += "<tr><td>";


                                if (!item.path_action.ToLower().Contains("signature"))
                                {
                                    outputHtml += ApproveAppCode.RenderViewToString(item.path_controller, item.path_action, activity_TBMMKT_Model);
                                }

                                if (item.path_action == "showSignatureV1")
                                {
                                    outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", approveModels);
                                }
                                else if (item.path_action == "showSignatureV2")
                                {
                                    outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureListsV2", approveModels);
                                }
                                else if (item.path_action == "showSignatureDiv")
                                {
                                    outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", approveModels);
                                }


                                outputHtml += "</td></tr>";
                            }
                        }
                        else
                        {
                            outputHtml = ApproveAppCode.RenderViewToString("eAct", "previewActMT", ReportAppCode.previewApprove(response.data.refId, response.data.approver));


                            approveModels = new ApproveController().getApproveSigList(response.data.refId, ConfigurationManager.AppSettings["subjectActivityFormId"], response.data.approver);
                            outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", approveModels);
                        }
                        outputHtml += "</table>";
                        //outputHtml += "</div>";
                        outputHtml += "</div>";


                        HostingEnvironment.QueueBackgroundWorkItem(c => new ActivityController().doGenFile(outputHtml, getHeader, response.data.approver, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.refId, "Consumer", activity_TBMMKT_Model));
                    }

                }
                return Ok(consumerMassage);
            }
            catch (Exception ex)
            {
                consumerMassage.messageResponse = "eact :" + ex.Message;
                consumerMassage.timeResponse = DateTime.Now.ToString();

                if (response != null)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(c => new ActivityController().doGenFile("<div>Please regen pdf</div>","", response.data.approver, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.refId, "Consumer", activity_TBMMKT_Model));
                }
                else
                {
                    SentKafkaLogModel kafka1 = new SentKafkaLogModel("", "", "", "Consumer", DateTime.Now, "Error", "", getMdodel.messagedata + " >> " + ex.Message);
                    ApproveAppCode.insertLog_Kafka(kafka1);
                }
               

                return Ok(consumerMassage);
            }

        }

    }
}