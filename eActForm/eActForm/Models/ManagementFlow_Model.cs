using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class ManagementFlow_Model
    {
        public List<TB_Reg_Subject_Model> subjectList { get; set; }
        public List<TB_Act_Customers_Model> customerList { get; set; }
        public List<TB_Act_ProductCate_Model> cateList { get; set; }
        public List<TB_Act_Chanel_Model> chanelList { get; set; }
        public List<TB_Act_ProductBrand_Model> productBrandList { get; set; }
        public List<TB_Act_Other_Model> companyList { get; set; }

    }
}