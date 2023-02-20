using eActForm.BusinessLayer;
using eActForm.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using static eActForm.Controllers.ApiApproveController;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace eActForm.Controllers.API
{
    [RoutePrefix("api")]
    public class eactAPIController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        public class consumerModel
        {
            public string messagedata { get; set; }
        }
        // POST api/<controller>
        [HttpPost]
        [Route("/api/projectApi/categories")]
        public IHttpActionResult Post([FromBody] consumerModel getMdodel)
        {
            SentKafkaLogModel kafka1 = new SentKafkaLogModel("test", "refTest", "name", "consumer", DateTime.Now, "", "", getMdodel.messagedata);
            var resultLog1 = ApproveAppCode.insertLog_Kafka(kafka1);
            return Ok(resultLog1);
        }

        // PUT api/<controller>/5
       
        public void Put(int id, [FromBody] consumerModel getMdodel)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}