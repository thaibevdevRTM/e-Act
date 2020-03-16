﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static eActForm.Models.Budget_Activity_Model;

namespace eActForm.Models
{
    public class Budget_Approve_Detail_Model
    {

        //public List<Budget_Activity_Invoice_Att> Budget_Activity_Invoice_list { get; set; }
        public List<Budget_Invoice_history_Att> Budget_Invoce_History_list { get; set; }
        public List<Budget_Approve_Detail_Att> Budget_Approve_detail_list { get; set; }
        public List<TB_Bud_Activity_Model.Budget_Activity_Att> Budget_Activity_list { get; set; }
        public TB_Bud_Activity_Model.Budget_Activity_Att Budget_Activity { get; set; }
        public List<TB_Bud_Image_Model.BudImageModel> Budget_Invoice_list { get; set; }

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
            public List<Budget_Invoice_history_Att> Budget_Invoce_History_list { get; set; }
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
}