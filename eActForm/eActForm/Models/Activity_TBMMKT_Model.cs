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
        public List< RequestEmpModel> TB_Reg_RequestEmp { get; set; }
        public List<RequestEmpModel> RequestEmp { get; set; }
        public List<PurposeModel> TB_Reg_Purpose { get; set; }
        public List<PlaceDetailModel> PlaceDetailModel { get; set; }
        public List<CostThemeDetailOfGroupByPriceTBMMKT> expensesDetailModel { get; set; }

        public List<ApproveFlowModel.flowApproveDetail> approveFlowDetail { get; set; }
        public List<ChannelMasterType> channelMasterTypeList { get; set; }


        public Activity_TBMMKT_Model()
        {
            channelMasterTypeList = new List<ChannelMasterType>();
            tB_Act_ActivityForm_SelectBrandOrChannel = new List<TB_Act_ActivityForm_SelectBrandOrChannel>();
            costThemeDetailOfGroupByPriceTBMMKT = new List<CostThemeDetailOfGroupByPriceTBMMKT>();
            costThemeDetailOfGroupByPriceTBMMKT = new List<CostThemeDetailOfGroupByPriceTBMMKT>();
            tB_Act_Chanel_Model = new List<TB_Act_Chanel_Model.Chanel_Model>();
            tB_Act_ProductBrand_Model = new List<TB_Act_ProductBrand_Model>();
            list_TB_Act_ActivityLayout = new List<TB_Act_ActivityLayout>();
            tB_Reg_Subject = new List<TB_Reg_Subject>();
            approveFlowDetail = new List<ApproveFlowModel.flowApproveDetail>();
            channelMasterTypeList = new List<ChannelMasterType>();
            tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
            activityFormTBMMKT = new ActivityFormTBMMKT();
            TB_Reg_Purpose = new List<PurposeModel>();
            PlaceDetailModel = new List<PlaceDetailModel>();
            expensesDetailModel = new List<CostThemeDetailOfGroupByPriceTBMMKT>();
            RequestEmp = new List<RequestEmpModel>();
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


    public class CostThemeDetailOfGroupByPriceTBMMKT : CostThemeDetailOfGroupByPrice
    {
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? unitPrice { get; set; }
        public string unitPriceDisplay { get; set; }
        public string unitPriceDisplayReport { get; set; }
        public string QtyName { get; set; }
        public string remark { get; set; }
    }

    public class RequestEmpModel
    {
        public string id { get; set; }
        public string empId { get; set; }
        public string empName { get; set; }
        public string position { get; set; }
        public string level { get; set; }
        public string department { get; set; } 
        public string bu { get; set; }     
    }

    public class PurposeModel
    {
        public string id { get; set; }
        public string detailTh { get; set; }
        public string detailEn { get; set; }
    }

    public class PlaceDetailModel
    {

        public string place { get; set; }
        public string forProject { get; set; }
        public string period { get; set; }
        public string departureDate { get; set; }
        public string arrivalDate { get; set; }

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