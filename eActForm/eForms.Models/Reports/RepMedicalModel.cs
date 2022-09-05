using System;
using System.Collections.Generic;

namespace eForms.Models.Reports
{

    public class RepMedicalModels
    {
        public List<RepMedicalModel> repMedicalists { get; set; }
        //  public List<RepPostEvaModel> repPostEvaGroupBrand { get; set; }
    }

    //public class RepPostEvaGroup
    //{
    //    public string name { get; set; }
    //    public string value { get; set; }
    //    public string sumActSalesParti { get; set; }
    //    public double? sumNormalCase { get; set; }
    //    public double? sumPromotionCase { get; set; }
    //    public double? sumSalesInCase { get; set; }
    //    public string countGroup { get; set; }
    //}
    public class RepMedicalModel
    {
        public string id { get; set; }
        //public string empId { get; set; }
        //public string empName { get; set; }
        //public string positionName { get; set; }
        //public string level { get; set; }
        //public string department { get; set; }
        //ใช้ Model เดิมที่มี
        public DateTime? documentDart { get; set; }
        public DateTime? treatmentDate { get; set; }

        public string hospTypeName { get; set; }
        public string hospName { get; set; }

        public string detail { get; set; }


        public int hospPercent { get; set; }
        public decimal? amount { get; set; }
        public decimal? amountLimit { get; set; }
        public decimal? amountCumulative { get; set; }
        public decimal? amountBalance { get; set; }
        public decimal? amountReceived { get; set; }
        public string departmentIdFlow { get; set; }


    }
}
