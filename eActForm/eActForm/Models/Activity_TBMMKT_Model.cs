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
    public class Activity_TBMMKT_Model : Activity_Model
    {        
        public TB_Act_ActivityForm_DetailOther TB_Act_ActivityForm_DetailOther { get; set; }
        public List<TB_Act_Chanel_Model.Chanel_Model> TB_Act_Chanel_Model { get; set; }
        public List<TB_Act_ProductBrand_Model> TB_Act_ProductBrand_Model { get; set; }
        public List<TB_Act_ActivityForm_SelectBrandOrChannel> TB_Act_ActivityForm_SelectBrandOrChannel { get; set; }
        public List<CostThemeDetailOfGroupByPriceTBMMKT> CostThemeDetailOfGroupByPriceTBMMKT { get; set; }
        public List<TB_Act_ActivityLayout> List_TB_Act_ActivityLayout { get; set; }
    }

    public class ActivityFormTBMMKT : ActivityForm
    {

    
    }

    public class TB_Act_ActivityForm_DetailOther
    {
        public string Id { get; set; }
        public string activityId { get; set; }
        public string activityProduct { get; set; }
        public string activityTel { get; set; }
        public string EO { get; set; }
        public Boolean delFlag { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedByUserId { get; set; }
    }


    public class TB_Act_ActivityLayout
    {
        public string id { get; set; }
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
    }


}