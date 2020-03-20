using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eActForm.Models
{
    public class Activity_TBMMKT_Model : Activity_Model
    {
        public List<Master_type_form_detail_Model> master_Type_Form_Detail_Models { get; set; }
        public ActivityFormTBMMKT activityFormTBMMKT { get; set; }
        public TB_Act_ActivityForm_DetailOther tB_Act_ActivityForm_DetailOther { get; set; }
        public List<TB_Act_Chanel_Model.Chanel_Model> tB_Act_Chanel_Model { get; set; }
        //public List<TB_Act_Chanel_Model.Chanel_Model> tBChanelHCModel { get; set; }
        public List<TB_Act_ProductBrand_Model> tB_Act_ProductBrand_Model { get; set; }
        public List<TB_Act_ActivityForm_SelectBrandOrChannel> tB_Act_ActivityForm_SelectBrandOrChannel { get; set; }
        public List<CostThemeDetailOfGroupByPriceTBMMKT> costThemeDetailOfGroupByPriceTBMMKT { get; set; }
        public List<TB_Act_ActivityLayout> list_TB_Act_ActivityLayout { get; set; }
        public List<TB_Reg_Subject> tB_Reg_Subject { get; set; }
        public string createdByName { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalCostThisActivity { get; set; }

        public List<RequestEmpModel> masterRequestEmp { get; set; }
        public List<RequestEmpModel> requestEmpModel { get; set; }
        public List<PurposeModel> purposeModel { get; set; }
        public List<PlaceDetailModel> placeDetailModel { get; set; }
        public CostDetailOfGroupPriceTBMMKT expensesDetailModel { get; set; }
        public List<string> chkPurpose { get; set; }

        public List<ApproveFlowModel.flowApproveDetail> approveFlowDetail { get; set; }
        public List<ChannelMasterType> channelMasterTypeList { get; set; }
        public List<TB_Act_master_list_choiceModel> list_0 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_1 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_2 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_3 { get; set; }
        public List<TB_Act_ActivityChoiceSelectModel> tB_Act_ActivityChoiceSelectModel { get; set; }
        public List<TB_Act_ProductBrand_Model> tB_Act_ProductBrand_Model_2 { get; set; }
        public List<TB_Act_master_list_choiceModel> listPiority { get; set; }
        public Activity_TBMMKT_Model()
        {
            channelMasterTypeList = new List<ChannelMasterType>();
            activityFormTBMMKT = new ActivityFormTBMMKT();

            masterRequestEmp = new List<RequestEmpModel>();
            requestEmpModel = new List<RequestEmpModel>();
            purposeModel = new List<PurposeModel>();
            placeDetailModel = new List<PlaceDetailModel>();
            expensesDetailModel = new CostDetailOfGroupPriceTBMMKT();
            approveFlowDetail = new List<ApproveFlowModel.flowApproveDetail>();

        }

    }

    public class ActivityFormTBMMKT : ActivityForm
    {
        public string selectedBrandOrChannel { get; set; }
        public string channelId { get; set; }
        public string BrandlId { get; set; }
        public string SubjectId { get; set; }
        public string createdByName { get; set; }
        public string formName { get; set; }
        public string companyName { get; set; }
        public string list_0_select { get; set; }
        public string list_0_select_value { get; set; }
        public string[] list_1_multi_select { get; set; }
        public string list_2_select { get; set; }
        public string list_3_select { get; set; }
        public string brand_select { get; set; }
        public string labelRequire { get; set; }
        public string labelInOrOutStock { get; set; }
        public string labelBrandOrChannel { get; set; }
        public string labelFor { get; set; }
        public string labelBrand { get; set; }
        public string labelChannelRegion { get; set; }
        public string formCompanyId { get; set; }
        public string formNameEn { get; set; }
        public bool chkUseEng { get; set; }
        public string createdByNameEN { get; set; }
        public string piorityDoc { get; set; }
    }

    public class TB_Reg_Subject
    {
        public string id { get; set; }
        public string companyId { get; set; }
        public string nameTH { get; set; }
        public string nameEN { get; set; }
        public string description { get; set; }
        public string master_type_form_id { get; set; } //insert dev date 20200109 fream
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }
    }

    public class TB_Act_ActivityForm_DetailOther
    {
        public string Id { get; set; }
        public string activityId { get; set; }
        public string channelId { get; set; }
        public string productBrandId { get; set; }
        public string SubjectId { get; set; }
        public string activityProduct { get; set; }
        public string activityTel { get; set; }
        public string EO { get; set; }
        public string IO { get; set; }
        public string descAttach { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }
        public string BudgetNumber { get; set; }
        public string groupName { get; set; }
        public string costCenter { get; set; }
        public string channelRegionName { get; set; }
        public string glNo { get; set; }
        public string glName { get; set; }
        public string toName { get; set; }
        public string toAddress { get; set; }
        public string toContact { get; set; }
        public string detailContact { get; set; }
        public string brand_select { get; set; }
    }


    public class TB_Act_ActivityLayout
    {
        public string id { get; set; }
        public string activityId { get; set; }
        public string no { get; set; }
        public string io { get; set; }
        public string activity { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? amount { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }
    }

    public class TB_Act_ActivityForm_SelectBrandOrChannel
    {
        public string txt { get; set; }
        public string val { get; set; }
    }

    public class CostDetailOfGroupPriceTBMMKT
    {
        public List<CostThemeDetailOfGroupByPriceTBMMKT> costDetailLists { get; set; }
    }

    public class CostThemeDetailOfGroupByPriceTBMMKT : CostThemeDetailOfGroupByPrice
    {
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? unitPrice { get; set; }
        public string unitPriceDisplay { get; set; }
        public string unitPriceDisplayReport { get; set; }
        public string QtyName { get; set; }
        public string remark { get; set; }
    }

    public class RequestEmpModel : ActBaseModel
    {

        public string id { get; set; }
        public string activityId { get; set; }
        public int rowNo { get; set; }
        public string empId { get; set; }
        public string empName { get; set; }
        public string position { get; set; }
        public string level { get; set; }
        public string department { get; set; }
        public string bu { get; set; }
        public string companyName { get; set; }
        public string companyNameEN { get; set; }
        public string empNameEN { get; set; }
        public string positionEN { get; set; }
        public string departmentEN { get; set; }
        public string buEN { get; set; }
    }

    public class PurposeModel : ActBaseModel
    {
        public string id { get; set; }
        public string activityId { get; set; }
        // public int rowNo { get; set; }
        public string detailTh { get; set; }
        public string detailEn { get; set; }
        public bool chk { get; set; }

    }
    //public class CheckboxModels
    //    {
    //        public List<string> chkPurpose { get; set; }
    //    }
    public class PlaceDetailModel : ActBaseModel
    {
        public string id { get; set; }
        public string activityId { get; set; }
        public int rowNo { get; set; }
        public string place { get; set; }
        public string forProject { get; set; }
        public string period { get; set; }
        public DateTime? departureDate { get; set; }
        public DateTime? arrivalDate { get; set; }
        public string departureDateStr { get; set; }
        public string arrivalDateStr { get; set; }

    }


    //public class CostThemeDetailOfGroupByPriceTBMMKT : CostThemeDetailOfGroupByPrice
    //{
    //    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    //    public decimal? unitPrice { get; set; }
    //    public string unitPriceDisplay { get; set; }
    //    public string unitPriceDisplayReport { get; set; }
    //    public string QtyName { get; set; }
    //    public string remark { get; set; }
    //}
    public class ChannelMasterType
    {
        public string id { get; set; }
        public string groupName { get; set; }
        public string subTypeId { get; set; }
        public string subTypeName { get; set; }
        public string subName { get; set; }
    }

}