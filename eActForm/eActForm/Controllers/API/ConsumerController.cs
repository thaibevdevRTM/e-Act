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
        [Route("api/{Controller}")]
        [HttpPost]
        public async Task<IHttpActionResult> Consumer( consumerModel getMdodel)
        {
            ConsumerMassage consumerMassage = new ConsumerMassage();
           

            ConsumerApproverBevAPI response = null;
            response = JsonConvert.DeserializeObject<ConsumerApproverBevAPI>(getMdodel.messagedata);
            consumerMassage.messageResponse = "eact :" + response.data.refId;
            consumerMassage.timeResponse = DateTime.Now.ToString();

            SentKafkaLogModel kafka1 = new SentKafkaLogModel(response.data.approver, response.data.refId, response.eventName, "Consumer", DateTime.Now, "", "", getMdodel.messagedata);
            var resultLog1 = ApproveAppCode.insertLog_Kafka(kafka1);

            if (response != null)
            {
                if (ApproveAppCode.updateApprove(response.data.refId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.message, null, response.data.approver) > 0)
                {
                   
                    Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                    ApproveModel.approveModels approveModels = new ApproveModel.approveModels();
                    activity_TBMMKT_Model = ReportAppCode.mainReport(response.data.refId, response.data.approver);





                    string outputHtml = "";
                    if (!string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                    {
                        approveModels = new ApproveController().getApproveSigList(response.data.refId, activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, response.data.approver);
                        foreach (var item in activity_TBMMKT_Model.master_Type_Form_Detail_Models)
                        {

                            outputHtml += ApproveAppCode.RenderViewToString(item.path_controller, item.path_action, approveModels);


                            if (item.path_action.ToLower().Contains("signature"))
                            {
                                outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", approveModels);
                            }

                        }
                    }
                    else
                    {
                        outputHtml = ApproveAppCode.RenderViewToString("eAct", "previewActMT", ReportAppCode.previewApprove(response.data.refId, response.data.approver));


                        approveModels = new ApproveController().getApproveSigList(response.data.refId, ConfigurationManager.AppSettings["subjectActivityFormId"], response.data.approver);
                        outputHtml += ApproveAppCode.RenderViewToString("Approve", "approvePositionSignatureLists", approveModels);
                    }

                    HostingEnvironment.QueueBackgroundWorkItem(c => new ApiApproveController().doGenFile(outputHtml, response.data.approver, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.refId));
                }



            }

                return Ok(consumerMassage);
        }

    }
}