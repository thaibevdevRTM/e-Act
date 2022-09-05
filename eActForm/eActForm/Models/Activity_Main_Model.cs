using eActForm.BusinessLayer;
using eForms.Models.MasterData;
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
        public List<TB_Act_ActivityForm_DetailOtherList> tB_Act_ActivityForm_DetailOtherList { get; set; }
        public List<TB_Act_Chanel_Model.Chanel_Model> tB_Act_Chanel_Model { get; set; }
        //public List<TB_Act_Chanel_Model.Chanel_Model> tBChanelHCModel { get; set; }
        public List<TB_Act_ProductBrand_Model> tB_Act_ProductBrand_Model { get; set; }
        public List<TB_Act_master_cost_centerModel> TB_Act_master_cost_centerModel_List { get; set; }
        public List<TB_Act_ActivityForm_SelectBrandOrChannel> tB_Act_ActivityForm_SelectBrandOrChannel { get; set; }
        public List<CostThemeDetailOfGroupByPriceTBMMKT> activityOfEstimateList { get; set; }
        public List<CostThemeDetailOfGroupByPriceTBMMKT> activityOfEstimateList2 { get; set; }
        public List<CostThemeDetailOfGroupByPriceTBMMKT> activityOfEstimateSubList { get; set; }
        public List<TB_Reg_Subject> tB_Reg_Subject { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalCostThisActivity { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public List<RequestEmpModel> masterRequestEmp { get; set; }
        public List<RequestEmpModel> requestEmpModel { get; set; }
        public List<PurposeModel> purposeModel { get; set; }
        public List<PlaceDetailModel> placeDetailModel { get; set; }
        public CostDetailOfGroupPriceTBMMKT expensesDetailModel { get; set; }
        public CostDetailOfGroupPriceTBMMKT expensesDetailSubModel { get; set; }
        public List<string> chkPurpose { get; set; }

        public List<ApproveFlowModel.flowApproveDetail> approveFlowDetail { get; set; }
        public List<ChannelMasterType> channelMasterTypeList { get; set; }
        public List<CompanyModel> companyList { get; set; }
        public List<TB_Act_Other_Model> objExpenseCashList { get; set; }
        public List<exPerryCashModel> exPerryCashList { get; set; }
        public exPerryCashModel exPerryCashModel { get; set; }
        public List<TB_Act_master_list_choiceModel> list_chooseRequest { get; set; }
        public List<TB_Act_master_list_choiceModel> list_0 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_1 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_2 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_3 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_4 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_5 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_6 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_7 { get; set; }
        public List<TB_Act_master_list_choiceModel> list_8 { get; set; }
        public List<TB_Act_ActivityChoiceSelectModel> tB_Act_ActivityChoiceSelectModel { get; set; }
        public List<TB_Act_ProductBrand_Model> tB_Act_ProductBrand_Model_2 { get; set; }
        public List<TB_Act_master_list_choiceModel> listPiority { get; set; }
        public RequestEmpModel empInfoModel { get; set; }
        public List<RegionalModel> regionalModel { get; set; }
        public List<TB_Act_ActivityGroup_Model> activityGroupList2 { get; set; }
        public List<TB_Act_ActivityGroup_Model> activityTypeList { get; set; }
        public List<BudgetTotal> budgetTotalList { get; set; }
        public List<BudgetTotal> budgetMainTotalList { get; set; }
        public BudgetTotal budgetTotalModel { get; set; }
        public List<TB_Act_Region_Model> regionGroupList { get; set; }
        public List<TB_Act_AmountBudget> amountBudgetList { get; set; }
        public List<TB_Act_Other_Model> otherList_1 { get; set; }
        public List<TB_Act_Other_Model> otherList_2 { get; set; }
        public List<TB_Act_Other_Model> otherList_3 { get; set; }
        public List<TB_Act_Other_Model> otherList_4 { get; set; }
        public List<TB_Act_Other_Model> otherList_5 { get; set; }
        public ApproveModel.approveModels approveModels { get; set; }
        public List<eForms.Models.MasterData.FiscalYearModel> listFiscalYearModel { get; set; }
        public List<GetDataEO> listGetDataEO { get; set; }
        public List<eForms.Models.MasterData.APModel> listAPModel { get; set; }
        public List<GetDataIO> listGetDataIO { get; set; }
        public List<GetDataPVPrevious> listGetDataPVPrevious { get; set; }
        public List<GetDataDetailPaymentAll> listGetDataDetailPaymentAll { get; set; }
        public List<departmentMasterModel> listGetDepartmentMaster { get; set; }
        public List<DataRequesterToShow> dataRequesterToShows { get; set; }
        public List<string> listEoInDoc { get; set; }
        public List<detailEO> eoList { get; set; }

        public List<ObjGetDataLayoutDoc> list_ObjGetDataLayoutDoc { get; set; }


        public Activity_TBMMKT_Model()
        {
            activityFormTBMMKT = new ActivityFormTBMMKT();
            channelMasterTypeList = new List<ChannelMasterType>();
            masterRequestEmp = new List<RequestEmpModel>();
            requestEmpModel = new List<RequestEmpModel>();
            purposeModel = new List<PurposeModel>();
            placeDetailModel = new List<PlaceDetailModel>();
            expensesDetailModel = new CostDetailOfGroupPriceTBMMKT();
            expensesDetailSubModel = new CostDetailOfGroupPriceTBMMKT();
            approveFlowDetail = new List<ApproveFlowModel.flowApproveDetail>();
            exPerryCashList = new List<exPerryCashModel>();
            exPerryCashModel = new exPerryCashModel("");
            empInfoModel = new RequestEmpModel();
            regionalModel = new List<RegionalModel>();
            budgetTotalModel = new BudgetTotal();
            budgetTotalList = new List<BudgetTotal>();
            tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
            TB_Act_master_cost_centerModel_List = new List<TB_Act_master_cost_centerModel>();
            amountBudgetList = new List<TB_Act_AmountBudget>();
            tB_Reg_Subject = new List<TB_Reg_Subject>();
            tB_Act_ActivityForm_DetailOtherList = new List<TB_Act_ActivityForm_DetailOtherList>();
            activityGroupList = new List<TB_Act_ActivityGroup_Model>();
            otherList_1 = new List<TB_Act_Other_Model>();
            otherList_2 = new List<TB_Act_Other_Model>();
            otherList_3 = new List<TB_Act_Other_Model>();
            otherList_4 = new List<TB_Act_Other_Model>();
            otherList_5 = new List<TB_Act_Other_Model>();
            objExpenseCashList = new List<TB_Act_Other_Model>();
            listGetDataEO = new List<GetDataEO>();
            budgetMainTotalList = new List<BudgetTotal>();
        }

    }

    public class ActivityFormTBMMKT : ActivityForm
    {
        public string selectedBrandOrChannel { get; set; }
        public string channelId { get; set; }
        public string BrandlId { get; set; }
        public string createdByName { get; set; }
        public string formName { get; set; }
        public string companyName { get; set; }
        public string list_0_select { get; set; }
        public string list_0_select_value { get; set; }
        public string[] list_0_multi_select { get; set; }
        public string[] list_1_multi_select { get; set; }
        public string[] list_2_multi_select { get; set; }
        public string[] list_3_multi_select { get; set; }
        public string[] list_4_multi_select { get; set; }
        public string[] list_5_multi_select { get; set; }
        public string[] list_6_multi_select { get; set; }
        public string[] list_7_multi_select { get; set; }
        public string[] list_8_multi_select { get; set; }
        public string[] brand_multi_select { get; set; }
        public string[] costCenter_multi_select { get; set; }


        public string list_1_select { get; set; }
        public string list_2_select { get; set; }
        public string list_3_select { get; set; }
        public string brand_select { get; set; }
        public string labelRequire { get; set; }
        public string labelInOrOutStock { get; set; }
        public string labelBrandOrChannel { get; set; }
        public string labelFor { get; set; }
        public string labelBrand { get; set; }
        public string labelChannelRegion { get; set; }
        public string formNameEn { get; set; }
        public bool chkUseEng { get; set; }
        public string createdByNameEN { get; set; }
        public string[] list_chooseRequest_multi_select { get; set; }
        public Boolean chkTemp { get; set; }
        public string remarkApprove { get; set; }

    }

    public class detailEO
    {
        public string EO { get; set; }
        public string brandName { get; set; }
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

        public TB_Act_ActivityForm_DetailOther()
        {
            this.SubjectId = "";
            this.productBrandId = "";
            this.channelId = "";
            this.BudgetNumber = "";
            this.costCenter = "";
            this.channelRegionName = "";
            this.glNo = "";
            this.glName = "";
        }
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
        public decimal? totalnormalCostEstimate { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalvat { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalnormalCostEstimateWithVat { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalallPayByIO { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalallPayNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalTransfer { get; set; }
        public decimal? totalallPayByIOBalance { get; set; }
        public string fiscalYear { get; set; }
        public string APCode { get; set; }
        public string payNo { get; set; }
        public string activityIdNoSub { get; set; }
        public string orderOf { get; set; }
        public string regionalId { get; set; }
        public string departmentId { get; set; }
        public string other1 { get; set; }
        public string other2 { get; set; }
        public int hospPercent { get; set; }
        public decimal? amount { get; set; }
        public decimal? amountLimit { get; set; }
        public decimal? amountCumulative { get; set; }
        public decimal? amountBalance { get; set; }
        public decimal? amountReceived { get; set; }
        public string departmentIdFlow { get; set; }
    }

    public class TB_Act_ActivityForm_DetailOtherList : ActBaseModel
    {
        public string id { get; set; }
        public string activityId { get; set; }
        public string typeKeep { get; set; }
        public int rowNo { get; set; }
        public string activityIdEO { get; set; }
        public string IO { get; set; }
        public string GL { get; set; }
        public string select_list_choice_id_ChReg { get; set; }
        public string productBrandId { get; set; }
        public string EO { get; set; }
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

        public CostDetailOfGroupPriceTBMMKT()
        {
            costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>();
        }
    }

    public class CostThemeDetailOfGroupByPriceTBMMKT : CostThemeDetailOfGroupByPrice
    {
        [DisplayFormat(DataFormatString = "{0:n3}", ApplyFormatInEditMode = true)]
        public decimal? unitPrice { get; set; }
        public string unitPriceDisplay { get; set; }
        public string unitPriceDisplayReport { get; set; }
        public string QtyName { get; set; }
        public string remark { get; set; }
        public decimal? amountBalance { get; set; }
        public decimal? amountTotal { get; set; }


    }

    public class RequestEmpModel : ActBaseModel
    {

        public RequestEmpModel()
        {

        }
        public RequestEmpModel(string empId, bool langEn, bool chkFormHc)
        {
            if (empId != "")
            {
                List<RequestEmpModel> model = QueryGet_empDetailById.getEmpDetailById(empId);
                if (model.Count > 0)
                {
                    this.empId = model[0].empId;
                    empName = !langEn ? (chkFormHc ? "" : "คุณ") + model[0].empName : model[0].empNameEN;
                    position = model[0].position;
                    level = model[0].level;
                    department = model[0].department;
                    bu = model[0].bu;
                    companyName = model[0].companyName;
                    empNameEN = model[0].empNameEN;
                    positionEN = model[0].positionEN;
                    departmentEN = model[0].departmentEN;
                    buEN = model[0].buEN;
                    companyNameEN = model[0].companyNameEN;
                    compId = model[0].compId;
                    email = model[0].email;
                    hireDate = model[0].hireDate;
                    empTel = model[0].empTel;
                }
            }
        }
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
        public string empTel { get; set; }
        public string compId { get; set; }
        public string email { get; set; }
        public string detail { get; set; }
        public string hireDate { get; set; }
        public string empGroupName { get; set; }
        public string empGroupNameTH { get; set; }
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
        public string depart { get; set; }
        public string arrived { get; set; }

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

    public class CompanyModel
    {
        public string id { get; set; }
        public string companyId { get; set; }
        public string companyNameEN { get; set; }
        public string companyNameTH { get; set; }
    }

    public class ObjGetDataEO
    {
        public string fiscalYear { get; set; }
        public string master_type_form_id { get; set; }
        public string productBrandId { get; set; }
        public string channelId { get; set; }
    }

    public class GetDataEO
    {
        public string EO { get; set; }
        public string activityId { get; set; }
        public string activityIdAndEO { get; set; }
    }

    public class ObjGetDataIO
    {
        public string ActivityByEOSelect { get; set; }
        public string EOSelect { get; set; }
    }

    public class GetDataIO
    {
        public string IO { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalPayByIO { get; set; }
        public decimal? amountTransfer { get; set; }
    }

    public class ObjGetDataGL
    {
        public string IOCode { get; set; }
        public string SubGroupCode { get; set; }
    }

    public class GetDataGL
    {
        public string GL { get; set; }
        public string GLSale { get; set; }
        public string id { get; set; }
        public string groupGL { get; set; }
    }


    public class ObjGetDataPVPrevious
    {
        public string master_type_form_id { get; set; }
        public string payNo { get; set; }
    }
    public class GetDataPVPrevious
    {
        public string activityNo { get; set; }
        public string activityId { get; set; }
        public string payNo { get; set; }
        public string statusId { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalallPayByIO { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalallPayNo { get; set; }
    }

    public class ObjGetDataDetailPaymentAll
    {
        public string activityId { get; set; }
        public string payNo { get; set; }
    }
    public class GetDataDetailPaymentAll
    {
        public string payNo { get; set; }
        public int rowNo { get; set; }
        public string IO { get; set; }
        public string productDetail { get; set; }
        public string vat { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? normalCost { get; set; }
        public DateTime? documentDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? totalnormalCostEstimate { get; set; }
        public string activityNo { get; set; }
        public string activityId { get; set; }
        public string activityIdNoSub { get; set; }
    }


    public class CashEmpModel
    {
        public string empId { get; set; }
        public string choiceID { get; set; }
        public string choiceName { get; set; }
        public decimal cashPerDay { get; set; }
        public string empLevel { get; set; }

    }

    public class objGetDataSubjectByFormOnly
    {
        public string master_type_form_id { get; set; }
    }

    public class objGetDepartmentMaster
    {
        public string companyId { get; set; }
        public string master_type_form_id { get; set; }
        public string subjectId { get; set; }
    }

    public class objGetDataCheckUploadFile
    {
        public string activityId { get; set; }
    }

    public class DataRequesterToShow
    {
        public DataRequesterToShow(string empId)
        {
            if (empId != "")
            {
                List<RequestEmpModel> model = QueryGet_empDetailById.getEmpDetailById(empId);
                if (model.Count > 0)
                {
                    this.empName = model.Count > 0 ? model[0].empName : "";
                    this.empDepartment = model.Count > 0 ? model[0].department : "";
                    this.empPhone = model.Count > 0 ? model[0].empTel : "";
                    this.empCompany = model.Count > 0 ? model[0].companyName : "";
                    this.empEmail = model.Count > 0 ? model[0].email : "";

                    //HttpContext.Current.Session[empId] = model;
                }
                else
                {
                    this.empName = "";
                    this.empDepartment = "";
                    this.empPhone = "";
                    this.empCompany = "";
                    this.empEmail = "";
                }
            }
        }

        public string empName { get; set; }
        public string empId { get; set; }
        public string empDepartment { get; set; }
        public string empPhone { get; set; }
        public string empCompany { get; set; }
        public string empEmail { get; set; }
        public string languageDoc { get; set; }

    }

    public class ObjGetDataLayoutDoc
    {
        public string id { get; set; }
        public string typeKeys { get; set; }
        public string valuesUse { get; set; }
        public string activityId { get; set; }
    }


}