using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public partial class GetDataMainFormController : Controller
    {
        public class objGetDataSubjectByChanelOrBrand
        {
            public string idBrandOrChannel { get; set; }
            public string master_type_form_id { get; set; }
            public string companyId { get; set; }
        }
        public JsonResult GetDataSubjectByChanelOrBrand(objGetDataSubjectByChanelOrBrand objGetDataSubjectBy)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Reg_Subject> tB_Reg_Subject = new List<TB_Reg_Subject>();
                tB_Reg_Subject = QueryGetSelectAllTB_Reg_Subject.GetQueryGetSelectAllTB_Reg_Subject_ByFormAndFlow(objGetDataSubjectBy);

                var resultData = new
                {
                    tB_Reg_Subject = tB_Reg_Subject.ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}