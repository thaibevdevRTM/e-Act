using System;
using System.Collections.Generic;

namespace eActForm.Models
{
    public class MedSearchModel
    {

    }
    public class ReportTypeModel
    {
        public string id { get; set; }
        public string typeFormId { get; set; }
        public string reportName { get; set; }
    }

    public class SearchReportModels
    {
        public List<Master_type_form_detail_Model> formDetail { get; set; }
        public string typeFormId { get; set; }
        public List<ReportTypeModel> reportTypeList { get; set; }
        public List<CompanyModel> companyList { get; set; }
        public List<eForms.Models.MasterData.departmentMasterModel> departmentList { get; set; }
        public List<RequestEmpModel> empList { get; set; }

    }
    public class MedReportDetail
    {
        public MedReportHeader medReportHeader { get; set; }
        public List<RequestEmpModel> empList { get; set; }
        public List<MedIndividualDetail> medIndividualDetail { get; set; }
        public List<MedAllDetail> medAllDetail { get; set; }
    }

    public class MedReportHeader
    {
        public string reportTypeId { get; set; }
        public string reportTypeName { get; set; }
        public string companyName { get; set; }
        public string period { get; set; }
    }

    public class MedIndividualDetail
    {
        public string activityNo { get; set; }
        public string documentDate { get; set; }
        public int hospPercent { get; set; }
        public decimal? amount { get; set; }
        public decimal? amountLimit { get; set; }
        public decimal? amountCumulative { get; set; }
        public decimal? amountBalance { get; set; }
        public decimal? amountReceived { get; set; }
        public string hospId { get; set; }
        public string typeName { get; set; }
        public string hospNameTH { get; set; }
        public int rowNo { get; set; }
        public string treatmentDate { get; set; }
        public string detail { get; set; }
        public decimal? unitPrice { get; set; }
        public decimal? total { get; set; }
        public decimal? amountByDetail { get; set; }
    }
    public class MedAllDetail
    {
        public string empId { get; set; }
        public string empName { get; set; }
        public string position { get; set; }
        public string level { get; set; }
        public string department { get; set; }
        public string activityNo { get; set; }
        public string documentDate { get; set; }
        public decimal? amount { get; set; }
        public decimal? amountLimit { get; set; }
        public decimal? amountCumulative { get; set; }
        public decimal? amountBalance { get; set; }
        public decimal? amountReceived { get; set; }
    }


}