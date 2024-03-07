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
using System.Web.Http.Results;
using System.Web.Mvc;
using static eActForm.Controllers.API.ConsumerController;
using static iTextSharp.text.pdf.AcroFields;


namespace eActForm.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConsumerController : ApiController
    {
        public static string typeConsumer = "Consumer";
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


            SentKafkaLogModel log_kafka;
            try
            {

                response = JsonConvert.DeserializeObject<ConsumerApproverBevAPI>(getMdodel.messagedata);
                consumerMassage.messageResponse = "eact :" + response.data.refId;
                consumerMassage.timeResponse = DateTime.Now.ToString();
                response.data.typeApprove = !string.IsNullOrEmpty(response.data.typeApprove) ? response.data.typeApprove : typeConsumer;


                log_kafka = new SentKafkaLogModel(response.data.approver, response.data.refId, response.eventName, response.data.typeApprove, DateTime.Now, "", "", getMdodel.messagedata);
                var resultLog1 = ApproveAppCode.insertLog_Kafka(log_kafka);

                // ApproveAppCode.apiProducerApproveAsync(response.data.approver, response.data.refId, response.eventName);

                if (response != null)
                {
                    new getHTML().genHTML(response, getMdodel);

                }

                return Ok(consumerMassage);
            }
            catch (Exception ex)
            {
                consumerMassage.messageResponse = "eact :" + ex.Message;
                consumerMassage.timeResponse = DateTime.Now.ToString();

                if (response != null)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(c => new ActivityController().doGenFile("<div>Please regen pdf</div>", "", response.data.approver, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.refId, response.data.typeApprove));
                }
                else
                {
                    SentKafkaLogModel kafka1 = new SentKafkaLogModel("", "", "", response.data.typeApprove, DateTime.Now, "Error", "", getMdodel.messagedata + " >> " + ex.Message);
                    ApproveAppCode.insertLog_Kafka(kafka1);
                }


                return Ok(consumerMassage);
            }
        }




    }

    public class getHTML : ApiController
    {
        public void genHTML(ConsumerApproverBevAPI response, consumerModel getMdodel)
        {
            SentKafkaLogModel log_kafka;
            bool checkStatusApprove;
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {

                if (response != null)
                {
                    activity_TBMMKT_Model = ReportAppCode.mainReport(response.data.refId, response.data.approver);

                    //--- check make sure status cuurent must be = 2
                    var getApproveModel = ApproveAppCode.getApproveByActFormId(response.data.refId, response.data.approver);
                   
                    if (response.data.typeApprove == typeConsumer)
                    {
                        checkStatusApprove = getApproveModel.approveDetailLists.Where(x => x.rangNo.ToString() == response.data.orderRank
                                                                                                     && x.empId == response.data.approver).FirstOrDefault().statusId == "2";
                    }
                    else
                    {
                        checkStatusApprove = true;
                    }

                    //check status approve rang -1 
                    var getEmp = ApproveAppCode.checkStatusBeforeCallKafka(response.data.approver, response.data.refId);
                    if (!string.IsNullOrEmpty(getEmp))
                    {
                        ApproveAppCode.apiProducerApproveAsync(getEmp, response.data.refId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == "3").FirstOrDefault().displayVal);
                    }
                    if (activity_TBMMKT_Model.activityFormTBMMKT.statusId == 2 && checkStatusApprove)
                    {
                        if (ApproveAppCode.updateApprove(response.data.refId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.message, null, response.data.approver) > 0)
                        {
                            string padding = "", classFont = "";
                            ApproveModel.approveModels approveModels = new ApproveModel.approveModels();

                            var getHeader = GenPDFAppCode.getHeader(activity_TBMMKT_Model);
                            string outputHtml = "";
                            if (!string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                            {
                                approveModels = new ApproveController().getApproveSigList(response.data.refId, activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, response.data.approver);

                                outputHtml += "<div id=\"divForm\" style=\"width:100%;\">";
                                outputHtml += ApproveAppCode.RenderViewToString("MainReport", "styleView", null);
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
                                        if (item.path_action.ToLower().Equals("reportpettycashnum"))
                                        {
                                            activity_TBMMKT_Model = ReportAppCode.reportPettyCashNumAppCode(activity_TBMMKT_Model);
                                            outputHtml += ApproveAppCode.RenderViewToString(item.path_controller, item.path_action, activity_TBMMKT_Model);
                                        }
                                        else
                                        {
                                            outputHtml += ApproveAppCode.RenderViewToString(item.path_controller, item.path_action, activity_TBMMKT_Model);
                                        }

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
                                outputHtml += "</table>";
                                //outputHtml += "</div>";
                                outputHtml += "</div>";
                            }
                            else
                            {
                                outputHtml = ApproveAppCode.RenderViewToString("eAct", "previewActMT", ReportAppCode.previewApprove(response.data.refId, response.data.approver));


                                approveModels = new ApproveController().getApproveSigList(response.data.refId, ConfigurationManager.AppSettings["subjectActivityFormId"], response.data.approver);
                                outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", approveModels);
                            }
                            

                            HostingEnvironment.QueueBackgroundWorkItem(c => new ActivityController().doGenFile(outputHtml, getHeader, response.data.approver, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.refId, response.data.typeApprove));
                        }
                    }
                    else if (activity_TBMMKT_Model.activityFormTBMMKT.statusId == 3)
                    {
                        log_kafka = new SentKafkaLogModel(response.data.approver, response.data.refId, response.eventName, response.data.typeApprove, DateTime.Now, "", "", getMdodel.messagedata + "Document status is approved");
                        ApproveAppCode.insertLog_Kafka(log_kafka);
                    }

                }
            }
            catch (Exception ex)
            {
                log_kafka = new SentKafkaLogModel(response.data.approver, response.data.refId, response.eventName, response.data.typeApprove, DateTime.Now, "", "", getMdodel.messagedata + "Document status is approved");
                ApproveAppCode.insertLog_Kafka(log_kafka);
            }

        }
    }
}