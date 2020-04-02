using eForms.Models.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static eActForm.Models.TB_Act_Customers_Model;
using static eActForm.Models.TB_Act_Product_Model;

namespace eActForm.Models
{
    public class Activity_Model
    {

        public List<TB_Act_ProductCate_Model> productcatelist { get; set; }
        public List<TB_Act_ProductGroup_Model> productGroupList { get; set; }
        public List<TB_Act_ProductBrand_Model> productBrandList { get; set; }
        public List<TB_Act_ActivityGroup_Model> activityGroupList { get; set; }
        public List<Product_Model> productlist { get; set; }
        public List<ProductSmellModel> productSmellLists { get; set; }
        public List<Customers_Model> customerslist { get; set; }
        public List<TB_Act_Other_Model> otherlist { get; set; }
        //public List<Productcostdetail> productcostdetaillist { get; set; }
        public List<CostThemeDetail> costthemedetail { get; set; }
        public List<TB_Act_Image_Model.ImageModel> productImageList { get; set; }
        public ActivityForm activityFormModel { get; set; }
        public List<ProductCostOfGroupByPrice> productcostdetaillist1 { get; set; }
        public List<CostThemeDetailOfGroupByPrice> activitydetaillist { get; set; }
        public List<TB_Act_Region_Model> regionGroupList { get; set; }

        public Activity_Model()
        {
            productcostdetaillist1 = new List<ProductCostOfGroupByPrice>();
            // productcostdetaillist = new List<Productcostdetail>();
            costthemedetail = new List<CostThemeDetail>();
            productlist = new List<Product_Model>();
            productcatelist = new List<TB_Act_ProductCate_Model>();
            customerslist = new List<Customers_Model>();
            otherlist = new List<TB_Act_Other_Model>();
            productGroupList = new List<TB_Act_ProductGroup_Model>();
            productBrandList = new List<TB_Act_ProductBrand_Model>();
            productImageList = new List<TB_Act_Image_Model.ImageModel>();
            activitydetaillist = new List<CostThemeDetailOfGroupByPrice>();
            activityFormModel = new ActivityForm();
        }

        public enum modeForm
        {
            insert,
            edit
        }

        public enum activityType
        {
            OMT,
            MT,
            TBM,
            EXPENSE,
            HCM,
            NUM,
            OtherCompany
        }


        public class actForms
        {
            public List<actForm> actLists { get; set; }
            public string typeForm { get; set; }
        }

        public class actForm : ActBaseModel
        {
            public string id { get; set; }
            public string statusId { get; set; }
            public string statusName { get; set; }
            public string statusNameEN { get; set; }
            public string languageDoc { get; set; }
            public string activityNo { get; set; }
            public DateTime? documentDate { get; set; }
            public string reference { get; set; }
            public string productCateId { get; set; }
            public string productGroupid { get; set; }
            public string customerId { get; set; }
            public string channelName { get; set; }
            public string productTypeId { get; set; }
            public string productTypeNameEN { get; set; }
            public string cusShortName { get; set; }
            public string productCategory { get; set; }
            public string productGroup { get; set; }
            public string groupName { get; set; }
            public DateTime? activityPeriodSt { get; set; }
            public DateTime? activityPeriodEnd { get; set; }
            public DateTime? costPeriodSt { get; set; }
            public DateTime? costPeriodEnd { get; set; }
            public string activityName { get; set; }
            public string theme { get; set; }
            public string objective { get; set; }
            public string trade { get; set; }
            public string activityDetail { get; set; }
            public decimal? normalCost { get; set; }
            public decimal? themeCost { get; set; }
            [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
            public decimal? totalCost { get; set; }

            public decimal? perTotal { get; set; }
            public string createByUserName { get; set; }
            public string regionId { get; set; }
            public string brandId { get; set; }
            public string master_type_form_id { get; set; }
            public DateTime? dateSentApprove { get; set; }
        }

    }
    public class SearchActivityModels
    {
        public searchParameterFilterModel showUIModel { get; set; }
        public List<Customers_Model> customerslist { get; set; }
        public List<TB_Act_ProductType_Model> productTypelist { get; set; }
        public List<TB_Act_ProductGroup_Model> productGroupList { get; set; }
        public List<TB_Act_ActivityGroup_Model> activityGroupList { get; set; }
        public List<ApproveModel.approveStatus> approveStatusList { get; set; }
        public string typeForm { get; set; }
    }

    public class ActivityForm
    {
        public string id { get; set; }
        public int statusId { get; set; }
        public string activityNo { get; set; }
        public DateTime? documentDate { get; set; }
        public string documentDateStr { get; set; }
        public string reference { get; set; }
        public string cusShortName { get; set; }
        public string customerName { get; set; }
        public string customerId { get; set; }
        public string chanel { get; set; }
        public string chanelShort { get; set; }
        public string brandName { get; set; }
        public string shortBrand { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productBrandId { get; set; }
        public string size { get; set; }
        public string smell { get; set; }
        public string smell_Id { get; set; }
        public string unit { get; set; }
        public string pack { get; set; }
        public string chanel_Id { get; set; }
        public string productCateId { get; set; }
        public string productCateText { get; set; }
        public string productGroupId { get; set; }
        public string productGroupText { get; set; }
        public string groupShort { get; set; }
        public string productTypeId { get; set; }
        public DateTime? activityPeriodSt { get; set; }
        public string activityPeriodStStr { get; set; }
        public DateTime? activityPeriodEnd { get; set; }
        public string activityPeriodEndStr { get; set; }
        public DateTime? costPeriodSt { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? costPeriodEnd { get; set; }
        public string activityName { get; set; }
        public string theme { get; set; }
        public string txttheme { get; set; }
        public string objective { get; set; }
        public string trade { get; set; }
        [DataType(DataType.MultilineText)]
        public string activityDetail { get; set; }
        public byte UploadedImage { get; set; }
        public string getUploadedImage { get; set; }
        public string refId { get; set; }
        public string mode { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }
        public string dateDoc { get; set; }
        public string str_costPeriodSt { get; set; }
        public string str_costPeriodEnd { get; set; }
        public string str_activityPeriodSt { get; set; }
        public string str_activityPeriodEnd { get; set; }
        public string regionId { get; set; }
        public string regionName { get; set; }
        public string regionShort { get; set; }
        public string typeForm { get; set; }
        public string remark { get; set; }
        public string companyId { get; set; }
        public Boolean chkAddIO { get; set; }
        public string actIO { get; set; }
        public string actClaim { get; set; }
        public string master_type_form_id { get; set; }
        public string benefit { get; set; }
        public string companyNameEN { get; set; }
        public string companyNameTH { get; set; }
        public string SubjectId { get; set; }
        public string empId { get; set; }
        public string formCompanyId { get; set; }
        public string languageDoc { get; set; }
        public string digit_IO { get; set; }
        public string statusNote { get; set; }
        public string piorityDoc { get; set; }
    }

    public class CostThemeDetailOfGroupByPrice : ActBaseModel
    {
        public string id { get; set; }
        public string productGroupId { get; set; }
        public string activityId { get; set; }
        public string typeTheme { get; set; }
        public string activityTypeId { get; set; }

        public string productDetail { get; set; }
        public string productId { get; set; }
        public string productName { get; set; }
        public decimal? wholeSalesPrice { get; set; }
        public decimal? LE { get; set; }
        public decimal? compensate { get; set; }
        public int unit { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? normalCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? themeCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? growth { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? total { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? perTotal { get; set; }
        public string smellId { get; set; }
        public string brandName { get; set; }
        public string brandId { get; set; }
        public int size { get; set; }
        public string pack { get; set; }
        public string smellName { get; set; }
        public Boolean isShowGroup { get; set; }
        public string IO { get; set; }
        public int rowNo { get; set; }
        public string detail { get; set; }
        public DateTime? date { get; set; }
        public string dateInput { get; set; }
        public string mechanics { get; set; }
        public bool chkBox { get; set; }

        public List<ProductCostOfGroupByPrice> detailGroup { get; set; }

        public CostThemeDetailOfGroupByPrice()
        {
            detailGroup = new List<ProductCostOfGroupByPrice>();
        }
        public string listChoiceId { get; set; }
        public string listChoiceName { get; set; }

        public string displayType { get; set; }
        public string subDisplayType { get; set; }
    }

    public class CostThemeDetail : ActBaseModel
    {
        public string id { get; set; }
        public string productGroupId { get; set; }
        public string activityId { get; set; }
        public string typeTheme { get; set; }
        public string activityTypeId { get; set; }
        public string productId { get; set; }
        public string productName { get; set; }
        public decimal? normalCost { get; set; }
        public decimal? themeCost { get; set; }
        public decimal? growth { get; set; }
        public decimal? total { get; set; }
        public decimal? perTotal { get; set; }
        public int unit { get; set; }
        public decimal? compensate { get; set; }
        public decimal? LE { get; set; }
        public decimal? wholeSalesPrice { get; set; }
        public string smellId { get; set; }
        public string brandName { get; set; }
        public string brandId { get; set; }
        public string smellName { get; set; }
        public string productDetail { get; set; }
        public Boolean isShowGroup { get; set; }
        public string IO { get; set; }
        public int rowNo { get; set; }
        public string mechanics { get; set; }
    }

    public class ProductCostOfGroupByPrice : ActBaseModel
    {
        public string id { get; set; }
        public string productGroupId { get; set; }
        public string activityId { get; set; }
        public string activityTypeId { get; set; }
        public string productId { get; set; }
        public string pack { get; set; }
        public string productName { get; set; }
        public string productDetail { get; set; }
        public string typeTheme { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? wholeSalesPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? disCount1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? disCount2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? disCount3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]

        public decimal? saleNormal { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? normalCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? normalGp { get; set; }
        public string strNormalGP { get; set; }
        public string strPromotionGP { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? promotionGp { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? specialDisc { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? specialDiscBaht { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? promotionCost { get; set; }
        public decimal? saleIn { get; set; }
        public decimal? saleOut { get; set; }
        public string brandId { get; set; }
        public string smellId { get; set; }
        public string brandName { get; set; }
        public int size { get; set; }
        public string smellName { get; set; }
        public decimal? themeCost { get; set; }
        public decimal? growth { get; set; }
        public decimal? total { get; set; }
        public decimal? perTotal { get; set; }
        public int unit { get; set; }
        public decimal? compensate { get; set; }
        public decimal? LE { get; set; }
        public Boolean isShowGroup { get; set; }
        public int rowNo { get; set; }
        public string digitGroup { get; set; }
        public string digitSubGroup { get; set; }
        public string EO { get; set; }
        public bool chkBox { get; set; }
        public string DateInput { get; set; }
        public string place { get; set; }
        public string detail { get; set; }
        public string customer { get; set; }
        public List<ProductCostOfGroupByPrice> detailGroup { get; set; }

        public ProductCostOfGroupByPrice()
        {
            detailGroup = new List<ProductCostOfGroupByPrice>();
        }

    }

    public class Productcostdetail
    {
        public string id { get; set; }
        public string activityId { get; set; }
        public string productId { get; set; }
        public string productName { get; set; }
        public decimal? wholeSalesPrice { get; set; }
        public decimal? disCount1 { get; set; }
        public decimal? disCount2 { get; set; }
        public decimal? disCount3 { get; set; }
        public decimal? normalCost { get; set; }
        public decimal? normalGp { get; set; }
        public decimal? promotionGp { get; set; }
        public decimal? specialDisc { get; set; }
        public decimal? specialDisBaht { get; set; }
        public decimal? promotionCost { get; set; }
        public decimal? saleIn { get; set; }
        public decimal? saleOut { get; set; }
        public string brandId { get; set; }
        public string smellId { get; set; }
        public string brandName { get; set; }
        public string smellName { get; set; }
        public string size { get; set; }
        public string pack { get; set; }
        public string typeTheme { get; set; }
        public string activityTypeId { get; set; }
        public decimal? themeCost { get; set; }
        public decimal? growth { get; set; }
        public decimal? total { get; set; }
        public Boolean isShowGroup { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }

    }




}