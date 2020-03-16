﻿using eActForm.BusinessLayer;
using System;
using System.Collections.Generic;

namespace eActForm.Models
{
    public class TB_Bud_Image_Model
    {
        //public class BudImageModels
        //{


        //}

        public List<BudImageModel> BudImageList { get; set; }
        public List<TB_Act_Region_Model> RegionList { get; set; }
        public BudImageModel BudImage { get; set; }
        public List<TB_Act_Customers_Model.Customers_Model> CustomerList { get; set; }

        public List<TB_Act_Region_Model> regionGroupList { get; set; }

        public class BudImageModel : ActBaseModel
        {
            public BudImageModel()
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
}