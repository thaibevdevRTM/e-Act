﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.MasterData
{
    public class ManagentFlowModel
    {
        public List<flowSubject> flowSubjectsList { get; set; }
        public class flowSubject
        {
            public string flowApproveId { get; set; }
            public string subjectName { get; set; }
            public string subjectId { get; set; }
            public string empId { get; set; }
            public string chanelGroup { get; set; }
            public string channelId { get; set; }
            public string brandName { get; set; }
            public string productBrandId { get; set; }
            public string departmentId { get; set; }
            public string cateName { get; set; }
            public string productCatId { get; set; }
            public string companyId { get; set; }
            public string companyName { get; set; }
            public string rangNo { get; set; }
            public string cusNameTH { get; set; }
            public Boolean selectRow { get; set; }
            public List<string> flowApproveIdList { get; set; }


        }


    }
}