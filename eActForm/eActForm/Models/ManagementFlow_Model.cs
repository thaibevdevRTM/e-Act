using System.Collections.Generic;
using System.Web.UI.WebControls.WebParts;

namespace eActForm.Models
{
    public class ManagementFlow_Model
    {
        public List<TB_Reg_Subject_Model> subjectList { get; set; }
        public List<TB_Act_Customers_Model.Customers_Model> customerList { get; set; }
        public List<TB_Act_ProductCate_Model> cateList { get; set; }
        public List<TB_Act_Chanel_Model.Chanel_Model> chanelList { get; set; }
        public List<TB_Act_ProductBrand_Model> productBrandList { get; set; }
        public List<TB_Act_ProductType_Model> productTypeList { get; set; }
        public List<TB_Act_Other_Model> companyList { get; set; }
        public List<TB_Reg_FlowLimit_Model> getLimitList { get; set; }
        public List<TB_Act_Other_Model> getDDLShowApproveList { get; set; }
        public List<TB_Act_Other_Model> getDDlApproveList { get; set; }
        public List<TB_Reg_ApproveGroup_Model> approveGroupList { get; set; }
        public ApproveFlowModel.approveFlowModel approveFlow { get; set; }
        public List<RequestEmpModel> empList { get; set; }

        public List<string> p_idList { get; set; }
        public List<string> p_rangNoList { get; set; }
        public List<string> p_empIdList { get; set; }
        public List<string> p_appovedGroupList { get; set; }
        public List<string> p_isShowList { get; set; }
        public List<string> p_isApproveList { get; set; }
        public List<string> p_flowId { get; set; }
        public List<string> p_productType { get; set; }
        public List<string> p_empGroup { get; set; }
        public string p_companyId { get; set; }
        public string typeFlow { get; set; }
        public ManagementFlow_Model()
        {
            approveFlow = new ApproveFlowModel.approveFlowModel();
            customerList = new List<TB_Act_Customers_Model.Customers_Model>();
            chanelList = new List<TB_Act_Chanel_Model.Chanel_Model>();
            cateList = new List<TB_Act_ProductCate_Model>();
            productBrandList = new List<TB_Act_ProductBrand_Model>();
            companyList = new List<TB_Act_Other_Model>();
            getLimitList = new List<TB_Reg_FlowLimit_Model>();
            getDDLShowApproveList = new List<TB_Act_Other_Model>();
            getDDlApproveList = new List<TB_Act_Other_Model>();
            approveGroupList = new List<TB_Reg_ApproveGroup_Model>();
            empList = new List<RequestEmpModel>();
        }
    }

    public class getDataList_Model : ActBaseModel
    {
        public string id { get; set; }
        public string subjectId { get; set; }
        public string companyId { get; set; }
        public string customerId { get; set; }
        public string productCatId { get; set; }
        public string productTypeId { get; set; }
        public string flowLimitId { get; set; }
        public string channelId { get; set; }
        public string productBrandId { get; set; }
        public string empId { get; set; }
    }
}