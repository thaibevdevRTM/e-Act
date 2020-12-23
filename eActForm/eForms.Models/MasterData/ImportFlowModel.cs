using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eForms.Models.MasterData
{
    public class ImportFlowModel
    {

        public class ImportFlowModels : DefaultFieldModel
        {
            public string subject { get; set; }
            public string subjectId { get; set; }
            public string customer { get; set; }
            public string customerId { get; set; }
            public string productCate { get; set; }
            public string productCateId { get; set; }
            public string productType { get; set; }
            public string productTypeId { get; set; }
            public string productBrand { get; set; }
            public string productBrandId { get; set; }
            public string channel { get; set; }
            public string channelId { get; set; }
            public string department { get; set; }
            public string departmentId { get; set; }
            public string limitId { get; set; }
            public string limitBegin { get; set; }
            public string limitTo { get; set; }
            public string limitDisplay { get; set; }
            public string rang { get; set; }
            public string approveGroup { get; set; }
            public string approveGroupId { get; set; }
            public string IsShow { get; set; }
            public string IsApprove { get; set; }
            public string empId { get; set; }
            public string name { get; set; }
            public string empGroup { get; set; }
            public string masterTypeId { get; set; }
            public string companyId { get; set; }
            public string company { get; set; }
            public string flowId { get; set; }

            public List<HttpPostedFileBase> InputFile { get; set; }
            public List<ImportFlowModels> importFlowList { get; set; }

            public ImportFlowModels()
            {
                importFlowList = new List<ImportFlowModels>();
            }
        }

        public List<Master_type_form_Model> masterTypeFormList { get; set; }

    }
}
