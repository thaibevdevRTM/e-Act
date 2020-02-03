using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using WebLibrary;

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

        public class objGetDataCostCenter
        {
            public string productBrandId { get; set; }
            public string companyId { get; set; }
        }

        public JsonResult GetDataCostCenter(objGetDataCostCenter objGetDataCostCenter)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_master_cost_centerModel> tB_Act_Master_Cost_CenterModels = new List<TB_Act_master_cost_centerModel>();
                tB_Act_Master_Cost_CenterModels = QueryGet_TB_Act_master_cost_center.get_TB_Act_master_cost_center(objGetDataCostCenter.productBrandId, objGetDataCostCenter.companyId);

                var resultData = new
                {
                    tB_Act_Master_Cost_CenterModels = tB_Act_Master_Cost_CenterModels.ToList(),
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

        public class objGetDataListChoiceById
        {
            public string id { get; set; }
            public string master_type_form_id { get; set; }
            public string type { get; set; }
        }

        public JsonResult GetDataListChoiceById(objGetDataListChoiceById objGetDataListChoiceById)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_master_list_choiceModel> tB_Act_Master_List_ChoiceModels = new List<TB_Act_master_list_choiceModel>();
                tB_Act_Master_List_ChoiceModels = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(objGetDataListChoiceById.master_type_form_id, objGetDataListChoiceById.type).Where(x => x.id == objGetDataListChoiceById.id).ToList();

                var resultData = new
                {
                    tB_Act_Master_List_ChoiceModels = tB_Act_Master_List_ChoiceModels.ToList(),
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

        public JsonResult GetDataListChoiceByType(objGetDataListChoiceById objGetDataListChoiceById)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_master_list_choiceModel> tB_Act_Master_List_ChoiceModels = new List<TB_Act_master_list_choiceModel>();
                tB_Act_Master_List_ChoiceModels = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(objGetDataListChoiceById.master_type_form_id, objGetDataListChoiceById.type);

                var resultData = new
                {
                    tB_Act_Master_List_ChoiceModels = tB_Act_Master_List_ChoiceModels.ToList(),
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


        public JsonResult GET_master_material_autoComplete(ObjGetData_master_material_Model objGetDataListChoiceById)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_master_material_Model> tB_Act_Master_Material_Models = new List<TB_Act_master_material_Model>();
                tB_Act_Master_Material_Models = QueryGet_TB_Act_master_material.get_TB_Act_master_material_autoComplete(objGetDataListChoiceById);

                var resultData = new
                {
                    tB_Act_Master_Material_Models = tB_Act_Master_Material_Models.ToList(),
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