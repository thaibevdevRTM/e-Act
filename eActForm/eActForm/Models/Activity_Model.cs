using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static eActForm.Models.TB_Act_Customers_Model;
using static eActForm.Models.TB_Act_Product_Cate_Model;
using static eActForm.Models.TB_Act_Product_Model;

namespace eActForm.Models
{
    public class Activity_Model
    {

        public List<Product_Cate_Model> productcatelist { get; set; }
        public List<TB_Act_ProductGroup_Model> productGroupList { get; set; }
        public List<TB_Act_ProductBrand_Model> productBrandList { get; set; }
        public List<TB_Act_ActivityGroup_Model> activityGroupList { get; set; }
        public List<Product_Model> productlist { get; set; }
        public List<ProductSmellModel> productSmellLists { get; set; }
        public List<Customers_Model> customerslist { get; set; }
        public List<TB_Act_Other_Model> otherlist { get; set; }
        public List<Productcostdetail> productcostdetaillist { get; set; }
        public List<CostThemeDetail> costthemedetail { get; set; }
        public List<TB_Act_Image_Model.ImageModel> productImageList { get; set; }
        public ActivityForm activityFormModel { get; set; }
        public List<ProductCostOfGroupByPrice> productcostdetaillist1 { get; set; }
        public List<CostThemeDetailOfGroupByPrice> activitydetaillist { get; set; }


        public Activity_Model()
        {
            productcostdetaillist1 = new List<ProductCostOfGroupByPrice>();
            productcostdetaillist = new List<Productcostdetail>();
            costthemedetail = new List<CostThemeDetail>();
            productlist = new List<Product_Model>();
            productcatelist = new List<Product_Cate_Model>();
            customerslist = new List<Customers_Model>();
            otherlist = new List<TB_Act_Other_Model>();
            productGroupList = new List<TB_Act_ProductGroup_Model>();
            productBrandList = new List<TB_Act_ProductBrand_Model>();
            productImageList = new List<TB_Act_Image_Model.ImageModel>();
            activitydetaillist = new List<CostThemeDetailOfGroupByPrice>();

        }

        public enum modeForm
        {
            insert,
            edit
        }


        public class actForms
        {
            public List<actForm> actLists { get; set; }
        }

        public class actForm
        {
            public string id { get; set; }
            public string statusId { get; set; }
            public string statusName { get; set; }
            public string activityNo { get; set; }
            public DateTime? documentDate { get; set; }
            public string reference { get; set; }
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
            public Boolean delFlag { get; set; }
            public DateTime? createdDate { get; set; }
            public string createdByUserId { get; set; }
            public DateTime? updatedDate { get; set; }
            public string updatedByUserId { get; set; }
            public decimal? normalCost { get; set; }
            public decimal? themeCost { get; set; }
            public decimal? totalCost { get; set; }
        }

    }
    public class SearchActivityModels
    {
        public List<Customers_Model> customerslist { get; set; }
        public List<Product_Cate_Model> productcatelist { get; set; }
        public List<TB_Act_ProductGroup_Model> productGroupList { get; set; }
        public List<TB_Act_ActivityGroup_Model> activityGroupList { get; set; }
        public List<ApproveModel.approveStatus> approveStatusList { get; set; }
    }

    public class ActivityForm
    {
        public string id { get; set; }
        public int statusId { get; set; }
        public string activityNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? documentDate { get; set; }
        public string reference { get; set; }
        public string cusShortName { get; set; }
        public string customerName { get; set; }
        public string customerId { get; set; }
        public string chanel { get; set; }
        public string chanelShort { get; set; }
        public string productCateId { get; set; }
        public string productCateText { get; set; }
        public string productGroupId { get; set; }
        public string productGroupText { get; set; }
        public string groupShort { get; set; }
        public string productTypeId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? activityPeriodSt { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? activityPeriodEnd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? costPeriodSt { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? costPeriodEnd { get; set; }
        public string activityName { get; set; }
        public string theme { get; set; }
        public string txttheme { get; set; }
        public string objective { get; set; }
        public string trade { get; set; }
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

    }

    public class CostThemeDetailOfGroupByPrice : ActBaseModel
    {
        public string id { get; set; }
        public string activityId { get; set; }
        public string typeTheme { get; set; }
        public string activityTypeId { get; set; }
        public string productId { get; set; }
        public string productName { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? normalCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? themeCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? growth { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? total { get; set; }
        public decimal? perTotal { get; set; }
        public string smellId { get; set; }
        public string brandName { get; set; }
        public string smellName { get; set; }
        public string isShowGroup { get; set; }
        public List<Productcostdetail> detailGroup { get; set; }
    }

    public class CostThemeDetail : ActBaseModel
    {
        public string id { get; set; }
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
        public string smellId { get; set; }
        public string brandName { get; set; }
        public string smellName { get; set; }
        public bool isShowGroup { get; set; }
    }

    public class ProductCostOfGroupByPrice : ActBaseModel
    {
        public string id { get; set; }
        public string activityId { get; set; }
        public string productId { get; set; }
        public string pack { get; set; }
        public string productName { get; set; }
        public decimal? wholeSalesPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public decimal? disCount1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public decimal? disCount2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public decimal? disCount3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? normalCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:p0}", ApplyFormatInEditMode = true)]
        public decimal? normalGp { get; set; }
        public string strNormalGP { get; set; }
        public string strPromotionGP { get; set; }
        [DisplayFormat(DataFormatString = "{0:p0}", ApplyFormatInEditMode = true)]
        public decimal? promotionGp { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
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
        public string smellName { get; set; }
        public string isShowGroup { get; set; }
        public List<Productcostdetail> detailGroup { get; set; }
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
        public string pack { get; set; }
        public string typeTheme { get; set; }
        public string activityTypeId { get; set; }
        public decimal? themeCost { get; set; }
        public decimal? growth { get; set; }
        public decimal? total { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }

    }


}