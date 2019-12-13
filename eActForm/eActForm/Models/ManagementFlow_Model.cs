using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class ManagementFlow_Model
    {
        public List<TB_Reg_Subject_Model> subjectList { get; set; }
        public List<TB_Act_Customers_Model.Customers_Model> customerList { get; set; }
        public List<TB_Act_ProductCate_Model> cateList { get; set; }
        public List<TB_Act_Chanel_Model.Chanel_Model> chanelList { get; set; }
        public List<TB_Act_ProductBrand_Model> productBrandList { get; set; }
        public List<TB_Act_Other_Model> companyList { get; set; }
        public List<TB_Act_Other_Model> getLimitList { get; set; }

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
    }
}