using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using eForms.Models.MasterData;
using eForms.Presenter.MasterData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using static eActForm.Models.ActUserModel;

namespace eActForm.Controllers
{
    public partial class GetDataMainFormController : Controller
    {
        public class objGetDataSubjectByChanelOrBrand
        {
            public string idBrandOrChannel { get; set; }
            public string master_type_form_id { get; set; }
            public string companyId { get; set; }
            public string channelId { get; set; }
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
            public string[] productBrandId { get; set; }
            public string companyId { get; set; }
        }

        public JsonResult GetDataCostCenter(objGetDataCostCenter objGetDataCostCenter)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_master_cost_centerModel> tB_Act_Master_Cost_CenterModels = new List<TB_Act_master_cost_centerModel>();
                if (objGetDataCostCenter.productBrandId != null)
                {
                    foreach (var item in objGetDataCostCenter.productBrandId)
                    {
                        var costlist = QueryGet_TB_Act_master_cost_center.get_TB_Act_master_cost_center(item, objGetDataCostCenter.companyId);
                        tB_Act_Master_Cost_CenterModels.AddRange(costlist);
                    }
                }
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

        public JsonResult GetDataEOPaymentVoucher(ObjGetDataEO objGetDataEO)
        {
            var result = new AjaxResult();
            try
            {
                if (objGetDataEO.channelId == null) { objGetDataEO.channelId = ""; }
                if (objGetDataEO.productBrandId == null) { objGetDataEO.productBrandId = ""; }
                if (objGetDataEO.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]) { objGetDataEO.master_type_form_id = ConfigurationManager.AppSettings["formBgTbmId"]; }

                List<GetDataEO> tbToAjax = new List<GetDataEO>();
                tbToAjax = QueryGetSelectMainForm.GetQueryDataEOPaymentVoucher(objGetDataEO);

                var resultData = new
                {
                    tbToAjax = tbToAjax.ToList(),
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

        public JsonResult GetDataIOPaymentVoucher(ObjGetDataIO objGetDataIO)
        {
            var result = new AjaxResult();
            try
            {

                List<GetDataIO> tbToAjax = new List<GetDataIO>();
                tbToAjax = QueryGetSelectMainForm.GetQueryDataIOPaymentVoucher(objGetDataIO);

                var resultData = new
                {
                    tbToAjax = tbToAjax.ToList(),
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
        public JsonResult GetDataGLPaymentVoucher(ObjGetDataGL objGetDataGL)
        {
            var result = new AjaxResult();
            try
            {

                List<GetDataGL> tbToAjax = new List<GetDataGL>();
                tbToAjax = QueryGetSelectMainForm.GetQueryDataGLPaymentVoucher(objGetDataGL);

                var resultData = new
                {
                    tbToAjax = tbToAjax.ToList(),
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
        public JsonResult GetDataPVPrevious(ObjGetDataPVPrevious objGetDataPVPrevious)
        {
            var result = new AjaxResult();
            try
            {

                List<GetDataPVPrevious> tbToAjax = new List<GetDataPVPrevious>();
                tbToAjax = QueryGetSelectMainForm.GetQueryDataPVPrevious(objGetDataPVPrevious);

                var resultData = new
                {
                    tbToAjax = tbToAjax.ToList(),
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

        public JsonResult GetDepartmentMaster(objGetDepartmentMaster objGetDepartmentMaster)
        {
            var result = new AjaxResult();
            try
            {

                List<departmentMasterModel> tbToAjax = new List<departmentMasterModel>();
                tbToAjax = departmentMasterPresenter.getdepartmentMaster(AppCode.StrCon, objGetDepartmentMaster.companyId);

                var resultData = new
                {
                    tbToAjax = tbToAjax.ToList(),
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

        public JsonResult GetDepartmentMasterBySubjectFlow(objGetDepartmentMaster objGetDepartmentMaster)
        {
            var result = new AjaxResult();
            try
            {

                List<departmentMasterModel> tbToAjax = new List<departmentMasterModel>();
                tbToAjax = departmentMasterPresenter.getdepartmentMasterBySubjectFlow(AppCode.StrCon, objGetDepartmentMaster.master_type_form_id, objGetDepartmentMaster.subjectId);

                var resultData = new
                {
                    tbToAjax = tbToAjax.ToList(),
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

        public JsonResult GetDataCheckFileSize(objGetDataCheckUploadFile objGetDataCheckUploadFile)
        {
            var result = new AjaxResult();
            try
            {
                TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
                getImageModel.tbActImageList = ImageAppCode.GetSizeFiles(objGetDataCheckUploadFile.activityId);

                var resultData = new
                {
                    tbToAjax = getImageModel.tbActImageList,
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

        public JsonResult getIOByEO(string txtEO,string txtIO)
        {
            var result = new AjaxResult();
            List<TransferBudgetModels> transfersList = new List<TransferBudgetModels>();
            try
            {
                transfersList = TransferBudgetAppcode.GetIOByEO(txtEO).Where(x => x.IO.Contains(txtIO) && x.IO != null).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(transfersList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEmpGroupFlowByChannel(objGetDataSubjectByChanelOrBrand objGetDataSubjectBy)
        {
            var result = new AjaxResult();
            try
            {
                List<User> userList = new List<User>();

                var userResult = QueryGet_empByComp.getEmpGroupByChannelId(objGetDataSubjectBy.channelId , objGetDataSubjectBy.master_type_form_id);

              
                var resultData = new
                {
                    userList = userResult.ToList(),
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