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
            try
            {
               
                response = JsonConvert.DeserializeObject<ConsumerApproverBevAPI>(getMdodel.messagedata);
                consumerMassage.messageResponse = "eact :" + response.data.refId;
                consumerMassage.timeResponse = DateTime.Now.ToString();

                SentKafkaLogModel kafka1 = new SentKafkaLogModel(response.data.approver, response.data.refId, response.eventName, "Consumer", DateTime.Now, "", "", getMdodel.messagedata);
                var resultLog1 = ApproveAppCode.insertLog_Kafka(kafka1);

                // ApproveAppCode.apiProducerApproveAsync(response.data.approver, response.data.refId, response.eventName);

                

                        HostingEnvironment.QueueBackgroundWorkItem(c => new ActivityController().doGenFile("", response.data.approver, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.refId, "Consumer"));
                    

                
                return Ok(consumerMassage);
            }
            catch (Exception ex)
            {
                consumerMassage.messageResponse = "eact :" + ex.Message;
                consumerMassage.timeResponse = DateTime.Now.ToString();

                if (response != null)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(c => new ActivityController().doGenFile("<div>Please regen pdf</div>", response.data.approver, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.displayVal == response.eventName).FirstOrDefault().val1, response.data.refId, "Consumer"));
                }

                SentKafkaLogModel kafka1 = new SentKafkaLogModel("", "", "", "Consumer", DateTime.Now, "Error", "", getMdodel.messagedata +" >> " + ex.Message);
                var resultLog1 = ApproveAppCode.insertLog_Kafka(kafka1);

                return Ok(consumerMassage);
            }

        }

    }
}