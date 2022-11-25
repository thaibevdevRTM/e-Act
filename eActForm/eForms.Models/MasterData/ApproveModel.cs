
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls.WebParts;

namespace eForms.Models.MasterData
{

    public class ProducerApproverBevAPI
    {
        public string messageKey { get; set; }
        public string eventName { get; set; }
        public string messageVersion { get; set; }
        public string messageDate { get; set; }
        public ApproverModel data { get; set; }

        public ProducerApproverBevAPI(string p_messageKey, string p_eventName, string p_messageVersion, string p_messageDate, ApproverModel p_data)
        {
            messageKey = p_messageKey;
            eventName = p_eventName;
            messageVersion = p_messageVersion;
            messageDate = p_messageDate;
            data = p_data;
        }
    }

    public class ApproverModel
    {
        public string appId { get; set; }
        public string appName { get; set; }
        public string docNo { get; set; }
        public string refId { get; set; }
        public string orderRank { get; set; }
        public string subject { get; set; }
        public string requester { get; set; }
        public string requesterNameTh { get; set; }
        public string requesterNameEn { get; set; }
        public string requestDate { get; set; }
        public string totalAmount { get; set; }
        public string currency { get; set; }
        public string approver { get; set; }
        public string companyName { get; set; }
        public ProducerDetailModel requestDetail { get; set; }

        public ApproverModel()
        {
            requestDetail = new ProducerDetailModel();
        }
    }

    public class ApproverDetailModel
    {
        public string appId { get; set; }
        public string appName { get; set; }
        public string docNo { get; set; }
        public string refId { get; set; }
        public string orderRank { get; set; }
        public string subject { get; set; }
        public string requester { get; set; }
        public string requesterNameTh { get; set; }
        public string requesterNameEn { get; set; }
        public string requestDate { get; set; }
        public string totalAmount { get; set; }
        public string currency { get; set; }
        public string approver { get; set; }
        public string companyName { get; set; }
        public string organizationUnitName { get; set; }
        public string detail { get; set; }

    }

    public class ApproverConsumerModel
    {
        public string appId { get; set; }
        public string appName { get; set; }
        public string docNo { get; set; }
        public string refId { get; set; }
        public string orderRank { get; set; }
        public string subject { get; set; }
        public string requester { get; set; }
        public string requesterNameTh { get; set; }
        public string requesterNameEn { get; set; }
        public string requestDate { get; set; }
        public string totalAmount { get; set; }
        public string currency { get; set; }
        public string approver { get; set; }
        public string message { get; set; }
        public string companyName { get; set; }
        public ProducerDetailModel requestDetail { get; set; }

        public ApproverConsumerModel()
        {
            requestDetail = new ProducerDetailModel();
        }
    }

    public class ProducerDetailModel
    {
        public string companyName { get; set; }
        public string organizationUnitName { get; set; }
        public string detail { get; set; }
        public string attachedFileName { get; set; }
        public string attachedUrl { get; set; }

    }


    public class ConsumerApproverBevAPI
    {
        public string messageKey { get; set; }
        public string eventName { get; set; }
        public string messageVersion { get; set; }
        public string messageDate { get; set; }
        public string messageResponse { get; set; }
        public string timeResponse { get; set; }
        public ApproverConsumerModel data { get; set; }

    }

    public class ConsumerMassage
    {
        public string timeResponse { get; set; }
        public string messageResponse { get; set; }
    }
        

}