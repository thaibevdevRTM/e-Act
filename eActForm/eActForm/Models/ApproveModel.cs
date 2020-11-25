using eActForm.BusinessLayer;
using System.Collections.Generic;
using System.Web;
namespace eActForm.Models
{
    public class ApproveModel
    {
        public class approveWaitingModels
        {
            public List<approveWaitingModel> waitingLists { get; set; }
        }
        public class approveWaitingModel
        {
            public string empId { get; set; }
            public string waitingCount { get; set; }
            public string empPrefix { get; set; }
            public string empFNameTH { get; set; }
            public string empLNameTH { get; set; }
            public string empEmail { get; set; }
        }
        public class approveModels
        {
            public bool isApproveDetailListsShowDocHaveNull { get; set; }
            public string masterTypeFormId { get; set; }
            public approveModel approveModel { get; set; }
            public List<approveDetailModel> approveDetailLists { get; set; }
            public List<approveStatus> approveStatusLists { get; set; }
            public List<ApproveFlowModel.flowApproveDetail> approveFlowDetail { get; set; }
            public Activity_TBMMKT_Model activity_TBMMKT_Model { get; set; }//fream dev date 20200114
            public List<CostThemeDetailOfGroupByPriceTBMMKT> costThemeDetailOfGroupByPriceTBMMKT { get; set; }

            public approveModels()
            {
                approveModel = new approveModel();
                approveDetailLists = new List<approveDetailModel>();
            }

        }
        public class approveModel : ActBaseModel
        {
            public string id { get; set; }
            public string flowId { get; set; }
            public string actFormId { get; set; }
            public string actNo { get; set; }
            public bool isPermisionApprove { get; set; } // current login can be approve
            public string statusId { get; set; } // approve status of current user
        }
        public class approveDetailModel : ActBaseModel
        {

            public approveDetailModel(string empId)
            {
                if (empId != "")
                {
                    List<RequestEmpModel> model = HttpContext.Current.Session[empId] == null ? QueryGet_empDetailById.getEmpDetailById(empId) : (List<RequestEmpModel>)HttpContext.Current.Session[empId];
                    if (model.Count > 0)
                    {
                        this.empPositionTitleTH = model.Count > 0 ? model[0].position : "";
                        this.empPositionTitleEN = model.Count > 0 ? model[0].positionEN : "";
                        HttpContext.Current.Session[empId] = model;
                    }
                    else
                    {
                        this.empPositionTitleTH = "";
                        this.empPositionTitleEN = "";
                    }
                }
            }

            public string id { get; set; }
            public string approveId { get; set; }
            public int? rangNo { get; set; }
            public string ImgName { get; set; }
            public string empId { get; set; }
            public string empPrefix { get; set; }
            public string empEmail { get; set; }
            public string empName { get; set; }
            public string empName_EN { get; set; }
            public string statusId { get; set; }
            public string statusName { get; set; }
            public string statusNameEN { get; set; }
            public bool? isSendEmail { get; set; }
            public string remark { get; set; }
            public byte[] signature { get; set; }
            public string activityNo { get; set; }
            public bool? isApprove { get; set; }
            public string companyName { get; set; }
            public string companyNameEN { get; set; }
            public string approveGroupId { get; set; }
            public string approveGroupNameTH { get; set; }
            public string approveGroupnameEN { get; set; }
            public string empPositionTitleTH { get; set; }
            public string empPositionTitleEN { get; set; }
            public bool? isShowInDoc { get; set; }
        }
        public class approveStatus : ActBaseModel
        {
            public string id { get; set; }
            public string nameTH { get; set; }
            public string nameEN { get; set; }
            public string type { get; set; }
            public string description { get; set; }
        }

        public class approveEmailDetailModel : ActBaseModel
        {
            public approveEmailDetailModel(string empId,string emailDB)
            {
                if( empId != "")
                {
                    List<RequestEmpModel> model = QueryGet_empDetailById.getEmpDetailById(empId);
                    if(model.Count > 0)
                    {
                        empEmail = model[0].email;
                    }
                    else
                    {
                        empEmail = emailDB;
                    }
                }
            }
            public string id { get; set; }
            public string activityName { get; set; }
            public string activitySales { get; set; }
            public string activityNo { get; set; }
            public decimal sumTotal { get; set; }
            public string customerName { get; set; }
            public string productTypeName { get; set; }
            public string empPrefix { get; set; }
            public string empEmail { get; set; }
            public string empName { get; set; }
            public string empName_EN { get; set; }
            public string createBy { get; set; }
            public string createBy_EN { get; set; }
            public string approveGroupId { get; set; }
            public string approveGroupTH { get; set; }
            public string approveGroupEN { get; set; }
            public string statusId { get; set; }
        }
    }
}