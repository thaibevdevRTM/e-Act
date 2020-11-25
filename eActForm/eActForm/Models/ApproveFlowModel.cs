using eActForm.BusinessLayer;
using System.Collections.Generic;
using System.Web;

namespace eActForm.Models
{
    public class ApproveFlowModel
    {
        public class approveFlowModel
        {
            public flowApprove flowMain { get; set; }
            public List<flowApproveDetail> flowDetail { get; set; }

            public approveFlowModel()
            {
                flowDetail = new List<flowApproveDetail>();
            }
        }
        public class flowApprove : ActBaseModel
        {
            public string id { get; set; }
            public string flowNameTH { get; set; }
            public string cusNameTH { get; set; }
            public string cusNameEN { get; set; }
            public string nameTH { get; set; }
        }
        public class flowApproveDetail : ActBaseModel
        {
            public flowApproveDetail(string empId)
            {
                if (empId != "")
                {
                    List<RequestEmpModel> model = HttpContext.Current.Session[empId] == null ? QueryGet_empDetailById.getEmpDetailById(empId) : (List<RequestEmpModel>)HttpContext.Current.Session[empId];
                    if (model.Count > 0)
                    {
                        this.empPositionTitleTH = model.Count > 0 ? model[0].position : "";
                        this.empPositionTitleEN = model.Count > 0 ? model[0].positionEN : "";
                        this.empFNameTH = model.Count > 0 ? model[0].empName : "";
                        this.empFNameEN = model.Count > 0 ? model[0].empNameEN : "";
                        HttpContext.Current.Session[empId] = model;
                    }
                    else
                    {
                        this.empPositionTitleTH = "";
                        this.empPositionTitleEN = "";
                        this.empFNameTH="";
                        this.empFNameEN="";
                    }
                }
            }
            public string id { get; set; }
            public string companyId { get; set; }
            public string flowId { get; set; }
            public int? rangNo { get; set; }
            public string empId { get; set; }
            public string empEmail { get; set; }
            public string empFNameTH { get; set; }
            public string empLNameTH { get; set; }
            public string empPositionTitleTH { get; set; }
            public string approveGroupId { get; set; }
            public string approveGroupName { get; set; }
            public string approveGroupNameEN { get; set; }
            public bool isShowInDoc { get; set; }
            public string description { get; set; }
            public string statusId { get; set; }
            public string remark;
            public string imgSignature { get; set; }

            public string urlImg { get; set; }
            public bool? isApproved { get; set; }
            public string bu { get; set; }
            public string buEN { get; set; }
            public string empFNameEN { get; set; }
            public string empLNameEN { get; set; }
            public string empPositionTitleEN { get; set; }
            public byte[] signature { get; set; }
            public string empGroup { get; set; }
            public string typeFlow { get; set; }
            public string activityGroup { get; set; }
        }
    }
}