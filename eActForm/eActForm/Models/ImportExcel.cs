using System.ComponentModel.DataAnnotations;
using System.Web;
using eActForm.BusinessLayer.Appcodes;

namespace eActForm.Models
{
    public class ImportExcel
    {
        [Required(ErrorMessage = "Please select file")]
        [FileExt(Allow = ".xls,.xlsx", ErrorMessage = "Only excel file")]
        public HttpPostedFileBase file { get; set; }
    }

}