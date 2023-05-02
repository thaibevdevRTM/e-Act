using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eActForm.BusinessLayer;

using static eActForm.Models.TB_Act_Customers_Model;
using static eActForm.Models.TB_Bud_Activity_Model;
//using static eActForm.Models.Budget_Approve_Detail_Model;

namespace eActForm.Models //update 21-04-2020
{

    public class SearchBudgetActivityPosEVAModels
    {
        public List<Customers_Model> customerslist { get; set; }

        public List<TB_Act_ActivityGroup_Model> activityGroupList { get; set; }

        public List<TB_Act_ProductBrand_Model> productBrandList { get; set; }

        public List<Budget_Activity_Model.Budget_Activity_Status_Att> budgetStstuslist { get; set; }

    }

    public class SearchBudgetActivityModels
    {
        public List<Customers_Model> customerslist { get; set; }
        public List<TB_Act_ProductType_Model> productTypelist { get; set; }
        public List<TB_Act_ProductGroup_Model> productGroupList { get; set; }
        public List<TB_Act_ActivityGroup_Model> activityGroupList { get; set; }
        public List<ApproveModel.approveStatus> approveStatusList { get; set; }
        public List<Budget_Activity_Model.Budget_Activity_Status_Att> budgetStstuslist { get; set; }
        public string typeForm { get; set; }

        public List<Budget_Activity_Model.Budget_Activity_Year_Att> activityYearlist { get; set; }

    }

    public class Budget_Activity_Model
    {

        public List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel> Budget_InvoiceList { get; set; }


        public List<Budget_Activity_Att> Budget_Activity_list { get; set; }
        public List<Budget_Activity_Status_Att> Budget_Activity_Ststus_list { get; set; }
        public List<Budget_Activity_Product_Att> Budget_Activity_Product_list { get; set; }
        public List<Budget_Activity_Invoice_Att> Budget_Activity_Invoice_list { get; set; }
        public List<Budget_Invoice_history_Att> Budget_Invoce_History_list { get; set; }
        public List<Budget_Approve_Detail_Model> Budget_Approve_Detail_list { get; set; }

        public Budget_Activity_Att Budget_Activity { get; set; }
        public Budget_Activity_Product_Att Budget_Activity_Product { get; set; }
        public Budget_Activity_Invoice_Att Budget_Activity_Invoice { get; set; }

        public Budget_Count_Wait_Approve_Att Budget_Count_Wait_Approve { get; set; }
        public Budget_Activity_Last_Approve_Att Budget_Activity_Last_Approve { get; set; }

        public List<Budget_Activity_Year_Att> activityYearlist { get; set; }
        public class Budget_Activity_Year_Att
        {
            public string activityYear { get; set; }
        }

        public class Budget_Invoice_history_Att
        {
            public string budgetId { get; set; }
            public string budgetActivityId { get; set; }
            public string activityId { get; set; }
            public string activityNo { get; set; }
            public string activityName { get; set; }
            public string activitEstimateId { get; set; }
            public string activityTypeTheme { get; set; }

            public string productId { get; set; }
            public string productDetail { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal normalCost { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal themeCost { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal totalCost { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal productStandBath { get; set; }

            public string invoiceId { get; set; }
            public string invoiceNo { get; set; }
            public string invoiceCustomerId { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal invoiceTotalBath { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal productBalanceBath { get; set; }

            public Int32 productBudgetStatusId { get; set; }
            public string productBudgetStatusNameTH { get; set; }

            //[DataType(DataType.Date)]
            //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? invoiceActionDate { get; set; }
            //public string invoiceActionDate { get; set; }

            public Int32 invoiceBudgetStatusId { get; set; }
            public string invoiceBudgetStatusNameTH { get; set; }
            public Int32 invoiceSeq { get; set; }
            public Int32 productCountInvoice { get; set; }

            public Int32 invoiceApproveStatusId { get; set; }
            public string invoiceApproveStatusName { get; set; }

            public string invoiceRemark { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal productSumInvoiceBath { get; set; }
            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal sum_cost_product_inv { get; set; }
            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal sum_total_invoice { get; set; }
            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal sum_balance_product_inv { get; set; }

        }

        public enum budgetType
        {
            OMT,
            MT
        }

        public class Budget_Activity_Invoice_Att
        {
            public string typeForm { get; set; }

            public string invoiceId { get; set; }
            public string activityId { get; set; }
            public string activityNo { get; set; }
            public string activityOfEstimateId { get; set; }
            public string activityTypeTheme { get; set; }

            public string productId { get; set; }
            public string productDetail { get; set; }
            public decimal normalCost { get; set; }
            public decimal themeCost { get; set; }
            public decimal totalCost { get; set; }
            public decimal productStandBath { get; set; }

            public string paymentNo { get; set; }

            public string budgetImageId { get; set; }
            public string actCustomerId { get; set; }
            public string invoiceNo { get; set; }
            public decimal saleActCase { get; set; }
            public decimal saleActBath { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal invoiceTotalBath { get; set; }

            public decimal productBalanceBath { get; set; }
            public Int32 productBudgetStatusId { get; set; }
            public string productBudgetStatusNameTH { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? dateInvoiceAction { get; set; }
            public String invoiceActionDate { get; set; }
            //public string invoiceActionDate { get; set; }
            public Int32 invoiceBudgetStatusId { get; set; }
            public string invoiceBudgetStatusNameTH { get; set; }
            public Int32 invoiceSeq { get; set; }

            public Int32 invoiceApproveStatusId { get; set; }
            public string invoiceApproveStatusName { get; set; }

            public string invoiceRemark { get; set; }
            public string approveInvoiceId { get; set; }
            public string budgetApproveId { get; set; }

            public Boolean delFlag { get; set; }
            public DateTime createdDate { get; set; }
            public string createdByUserId { get; set; }
            public DateTime updatedDate { get; set; }
            public string updatedByUserId { get; set; }
        }

        public class Budget_Activity_Product_Att
        {
            //usp_getBudgetActivityProduct for Query Biz
            public string act_activityId { get; set; }
            public string act_activityNo { get; set; }
            public string prd_productId { get; set; }
            public string activityOfEstimateId { get; set; }
            public string act_typeTheme { get; set; }
            public string prd_productDetail { get; set; }
            public decimal normalCost { get; set; }
            public decimal themeCost { get; set; }
            public decimal totalCost { get; set; }
            public decimal invoiceTotalBath { get; set; }       /*จำนวนเงินจ่าย*/
            public decimal productBalanceBath { get; set; } /*ผลต่าง*/
            public string budgetStatusId { get; set; }
            public string budgetStatusNameTH { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? invActionDate { get; set; } /*วันที่ทำรายการ*/
        }

        public class Budget_Activity_Status_Att
        {
            public string id { get; set; }
            public string nameEN { get; set; }
            public string nameTH { get; set; }
            public string description { get; set; }
            public Boolean delFlag { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? createdDate { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? updatedDate { get; set; }

            public string createdByUserId { get; set; }
            public string updatedByUserId { get; set; }
        }

        public class Budget_Product_Status_Att
        {
            public string id { get; set; }
            public string nameEN { get; set; }
            public string nameTH { get; set; }
            public string description { get; set; }
        }

        public class Budget_Count_Wait_Approve_Att
        {
            public string activityId { get; set; }
            public Int32 count_wait_approve { get; set; }
        }

        public class Budget_Activity_Last_Approve_Att
        {
            public string budgetActivityId { get; set; }
            public string budgetApproveId { get; set; }
        }

    }

    public class Budget_Approve_Detail_Model  //update 21-04-2020
    {

        //public List<Budget_Activity_Invoice_Att> Budget_Activity_Invoice_list { get; set; }
        public List<Budget_Activity_Model.Budget_Invoice_history_Att> Budget_Invoce_History_list { get; set; }
        public List<Budget_Approve_Detail_Att> Budget_Approve_detail_list { get; set; }
        public List<TB_Bud_Activity_Model.Budget_Activity_Att> Budget_Activity_list { get; set; }
        public TB_Bud_Activity_Model.Budget_Activity_Att Budget_Activity { get; set; }
        public List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel> Budget_Invoice_list { get; set; }
        public Budget_Activity_Model.Budget_Activity_Last_Approve_Att Budget_Activity_Last_Approve { get; set; }

        public class Budget_Approve_Detail_Att
        {
            public string id { get; set; }
            public string budgetActivityId { get; set; }
            public string budgetApproveId { get; set; }
            //public string budgetActivityInvoiceId { get; set; }

            public Boolean delFlag { get; set; }

            public DateTime createdDate { get; set; }
            public string createdByUserId { get; set; }
            public DateTime updatedDate { get; set; }
            public string updatedByUserId { get; set; }
        }


        public class budgetForms
        {
            public List<budgetForm> budgetFormLists { get; set; }
            public List<Budget_Activity_Model.Budget_Invoice_history_Att> Budget_Invoce_History_list { get; set; }
            public List<Budget_Activity_Model> Budget_Activity_list { get; set; }
            public List<Budget_Approve_Detail_Att> Budget_Approve_detail_list { get; set; }
        }

        public class budgetForm : ActBaseModel
        {
            public string activityId { get; set; }
            public string statusId { get; set; }
            public string statusName { get; set; }
            public string activityNo { get; set; }

            public string regApproveId { get; set; }
            public string regApproveFlowId { get; set; }
            //public string budgetApproveId { get; set; }
            public DateTime? documentDate { get; set; }

            public string reference { get; set; }
            public string productCateId { get; set; }
            public string productGroupid { get; set; }
            public string customerId { get; set; }
            public string channelName { get; set; }
            public string productTypeId { get; set; }
            public string productTypeNameEN { get; set; }
            public string cusShortName { get; set; }
            public string cusNameTH { get; set; }

            public string productCategory { get; set; }
            public string productGroup { get; set; }
            public string productGroupName { get; set; }
            public DateTime? activityPeriodSt { get; set; }
            public DateTime? activityPeriodEnd { get; set; }
            public DateTime? costPeriodSt { get; set; }
            public DateTime? costPeriodEnd { get; set; }
            public string activityName { get; set; }
            public string themeId { get; set; }
            public string theme { get; set; }
            public string objective { get; set; }
            public string trade { get; set; }
            public string activityDetail { get; set; }

            public string budgetActivityId { get; set; }
            public string budgetApproveId { get; set; }
            public string approveId { get; set; }
            public string approveDetailId { get; set; }

            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? normalCost { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? themeCost { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? totalCost { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? totalInvoiceApproveBath { get; set; }
        }

    }

    public class TB_Bud_Activity_Model
    {
        public class Budget_Activity_Att
        {

            public string budget_id { get; set; }
            public string act_form_id { get; set; }
            public int act_approveStatusId { get; set; }
            public string act_activityNo { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
            public DateTime? act_documentDate { get; set; }
            public string act_reference { get; set; }
            public string act_customerId { get; set; }

            public string act_companyEN { get; set; }

            public string cus_cusShortName { get; set; }
            public string cus_cusNameEN { get; set; }
            public string cus_cusNameTH { get; set; }

            public string ch_chanelCust { get; set; }
            public string ch_chanelGroup { get; set; }
            public string ch_chanelTradingPartner { get; set; }

            public string prd_groupName { get; set; }
            public string prd_groupNameTH { get; set; }
            public string prd_groupShort { get; set; }

            public string act_brandNameTH { get; set; }
            public string act_brandName { get; set; }
            public string act_shortBrand { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? act_activityPeriodSt { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? act_activityPeriodEnd { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? act_costPeriodSt { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? act_costPeriodEnd { get; set; }

            public string act_activityName { get; set; }
            public string act_theme { get; set; }
            //public string act_themeName { get; set; }
            public string act_objective { get; set; }
            public string act_trade { get; set; }
            public string act_activityDetail { get; set; }
            public string act_delFlag { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? act_createdDate { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? act_updatedDate { get; set; }

            public string act_createdByUserId { get; set; }
            public string act_updatedByUserId { get; set; }
            public decimal act_normalCost { get; set; }
            public decimal act_themeCost { get; set; }
            public decimal act_totalCost { get; set; }
            public decimal act_balance { get; set; } /*ผลต่าง*/
            public decimal act_total_invoive { get; set; }/*ยอดยกมา*/

            public string act_claimNo { get; set; }
            public string act_claimShare { get; set; }
            public string act_claimStatus { get; set; }

            public string bud_ActivityStatusId { get; set; }
            public string bud_ActivityStatus { get; set; }

        }
    }

    public class TB_Bud_Invoice_Document_Model
    {

        public List<BudgetInvoiceModel> BudgetInvoiceList { get; set; }
        public List<TB_Act_Region_Model> RegionList { get; set; }
        public BudgetInvoiceModel BudgetInvoice { get; set; }
        public List<TB_Act_Customers_Model.Customers_Model> CustomerList { get; set; }

        public List<TB_Act_Region_Model> regionGroupList { get; set; }

        public class BudgetInvoiceModel : ActBaseModel
        {
            public BudgetInvoiceModel()
            {
                _image = new byte[0];
                extension = ".pdf";
                delFlag = false;
                createdByUserId = UtilsAppCode.Session.User.empId;
                createdDate = DateTime.Now;
                updatedByUserId = UtilsAppCode.Session.User.empId;
                updatedDate = DateTime.Now;
            }
            public string id { get; set; }
            public string imageType { get; set; }
            public byte[] _image { get; set; }
            public string _fileName { get; set; }
            public string extension { get; set; }
            public string remark { get; set; }
            public string typeFiles { get; set; }

            public string companyId { get; set; }
            public string regionId { get; set; }
            public string customerId { get; set; }

            public string company { get; set; }
            public string regionName { get; set; }
            public string customerName { get; set; }

            public string budgetApproveId { get; set; }
            public string budgetActivityId { get; set; }
            public string activityNo { get; set; }

            public int count_budgetApproveId { get; set; }
            public int count_budgetActivityId { get; set; }
            public int count_activityNo { get; set; }

            public string invoiceNo { get; set; }
        }
    }

    public class Budget_Report_Model
    {
        public class Report_Budget_Activity
        {
            public List<Report_Budget_Activity_Att> Report_Budget_Activity_List { get; set; }
        }

        public class Report_Budget_Activity_Att
        {
            public string company { get; set; }
            public string channelName { get; set; }

            public string reportMMYY { get; set; }
            public string claim_actStatus { get; set; }
            public string claim_shareStatus { get; set; }
            public string claim_actValue { get; set; }
            public string claim_actIO { get; set; }
            public string product_IO { get; set; }

            public string act_activityNo { get; set; }
            public string sub_code { get; set; }
            public string act_activityName { get; set; }
            public string act_reference { get; set; }
            public string brandName { get; set; }
            public string themeId { get; set; }
            public string Theme { get; set; }

            public string cus_id { get; set; }
            public string cus_regionId { get; set; }
            public string cus_regionName { get; set; }
            public string cus_regionDesc { get; set; }
            public string cus_cusNameTH { get; set; }
            public string cus_cusNameEN { get; set; }

            public string prd_typeId { get; set; }
            public string prd_groupId { get; set; }

            public string prd_productDetail { get; set; }
            public string prd_productDetail50 { get; set; }
            public Int32 prd_productDetailCount { get; set; }
            public string activity_Period { get; set; }
            public string activity_costPeriod { get; set; }
            public string actCreatedDate { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal activityTotalBath { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal activityInvoiceTotalBath { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal activityBalanceBath { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal activityCostRemainBath { get; set; }
            public string productBudgetStatusGroupId { get; set; }
            public string ProductBudgetStatusId { get; set; }
            public string productBudgetStatusNameTH { get; set; }
            public string invoiceCreatedDate { get; set; }
            public string act_status { get; set; }
            public string actForm_CreatedByUserId { get; set; }
            public string actForm_CreatedByName { get; set; }
            public string budget_CurrentApproveName { get; set; }
            public string wait_activityInvoiceTotalBath { get; set; }
        }
    }

}