using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eActForm.Models
{



    public class Claim_Report_Model
    {
        public List<Claim_Activity_Att> Claim_Activity_List { get; set; }

        public class Claim_Activity_Att
        {

            public string act_formId { get; set; }
            public string act_activityNo { get; set; }
            public string act_subCode { get; set; }
            public string est_rowno { get; set; }
            public string s40_status { get; set; }
            public string s40_Assignment { get; set; }
            public string s40_GL { get; set; }
            public string prd_productDetailShort { get; set; }
            public string s40_Order { get; set; }
            public string cus_cusNameEN { get; set; }
            public string prd_brandName { get; set; }
            public string s40_PstngDate { get; set; }
            public string s40_Reference { get; set; }
            public string s40_DocumentNo { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal s40_Amount { get; set; }

            public string s20_Assignment { get; set; }
            public string s20_DocumentDate { get; set; }
            public string s20_DocumentNo { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal s20_Amount { get; set; }

            public string claim_actStatus { get; set; }
            public string claim_shareStatus { get; set; }
            public string claim_actValue { get; set; }
            public string claim_actIO { get; set; }
            public string product_IO { get; set; }
            //public string posting_activityNo
            //public string posting_date
            //public string posting_month
            //public string posting_year
            public string act_activityName { get; set; }
            public string prd_Theme { get; set; }
            public string cus_cusNameTH { get; set; }
            public string prd_productDetail { get; set; }
            public string act_activityPeriodSt { get; set; }
            public string act_activityPeriodEnd { get; set; }
            public string act_Period { get; set; }
            public string act_costPeriod { get; set; }
            public string act_createdDate { get; set; }
            public string invoiceSeq { get; set; }
            public string invoiceNo { get; set; }

            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal est_totalBath { get; set; }
            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal inv_totalBath { get; set; }
            [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
            public decimal est_balanceBath { get; set; }

            public string est_budgetStatusNameTH { get; set; }
            public string inv_createdDate { get; set; }
            public string act_companyId { get; set; }
            public string companyName { get; set; }
            public string act_createdByUserId { get; set; }
            public string act_createdByName { get; set; }

            public string prd_themeId { get; set; }
            public string cus_cusId { get; set; }

        }
    }
}