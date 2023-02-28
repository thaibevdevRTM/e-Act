using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ActivityController : eActController
    {
        public ActionResult ActivityForm(string activityId, string mode, string typeForm)
        {
            Activity_Model activityModel = new Activity_Model();
            try
            {

                activityModel.activityFormModel = new ActivityForm();
                activityModel.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();

                if (typeForm == Activity_Model.activityType.OMT.ToString())
                {
                    activityModel.customerslist = QueryGetAllCustomers.getCustomersOMT();
                }
                else
                {
                    activityModel.customerslist = QueryGetAllCustomers.getCustomersMT();
                }

                activityModel.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
                activityModel.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                    .Where(x => x.activityCondition.Contains(Activity_Model.activityType.MT.ToString()))
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

                activityModel.activityGroupFilterList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activitySales.Contains("FOC")).ToList();
                activityModel.listPiority = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice("master", "piorityDoc").OrderBy(x => x.orderNum).ToList();
                if (UtilsAppCode.Session.User.regionId != "")
                {
                    activityModel.regionGroupList = QueryGetAllRegion.getRegoinByEmpId(UtilsAppCode.Session.User.empId);
                    activityModel.activityFormModel.regionId = UtilsAppCode.Session.User.regionId;
                }
                else
                {
                    activityModel.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.condition.Equals("OMT")).ToList();
                }

                if (!string.IsNullOrEmpty(activityId))
                {

                    activityModel.activityFormModel = QueryGetActivityById.getActivityById(activityId).FirstOrDefault();
                    activityModel.activityFormModel.mode = mode;
                    activityModel.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(activityId);
                    activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(activityId);
                    activityModel.productSmellLists = QueryGetAllProduct.getProductSmellByGroupId(activityModel.activityFormModel.productGroupId);
                    activityModel.productBrandList = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId == activityModel.activityFormModel.productGroupId).ToList();
                    activityModel.productGroupList = QueryGetAllProductGroup.getAllProductGroup().Where(x => x.cateId == activityModel.activityFormModel.productCateId).ToList();

                    TempData["actForm" + activityId] = activityModel;
                    ViewBag.chkClaim = activityModel.activityFormModel.chkAddIO;
                }
                else
                {
                    string actId = Guid.NewGuid().ToString();
                    activityModel.activityFormModel.id = actId;
                    activityModel.activityFormModel.mode = mode;
                    activityModel.activityFormModel.statusId = 1;
                    TempData["actForm" + actId] = activityModel;
                }

                activityModel.activityFormModel.typeForm = typeForm;
                TempData.Keep();

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ActivityForm => " + ex.Message);
            }
            return View(activityModel);
        }

        public ActionResult ImageList(string activityId)
        {
            try
            {
                TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
                if (!string.IsNullOrEmpty(activityId))
                {
                    getImageModel.tbActImageList = ImageAppCode.GetImage(activityId);
                    return PartialView(getImageModel);
                }
                else
                {
                    return PartialView(getImageModel);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ImageList => " + ex.Message);
            }

            return PartialView();

        }

        public ActionResult PreviewData(string activityId)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel = ReportAppCode.previewApprove(activityId, UtilsAppCode.Session.User.empId);
            activityModel.activityFormModel.callFrom = "app";
            return PartialView(activityModel);
        }

        public JsonResult getPreviewData(ActivityForm activityFormModel, Activity_Model activityModel)
        {
            var result = new AjaxResult();
            try
            {
                activityModel.activityFormModel = activityFormModel;

                var resultData = new
                {
                    activityModel = activityModel,

                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getPreviewData => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult insertDataActivity(ActivityForm activityFormModel)
        {
            var result = new AjaxResult();
            try
            {
                string statusId = "";
                Activity_Model activityModel = TempData["actForm" + activityFormModel.id] == null ? new Activity_Model() : (Activity_Model)TempData["actForm" + activityFormModel.id];
                activityModel.activityFormModel = activityFormModel;
                statusId = ActivityFormCommandHandler.getStatusActivity(activityFormModel.id);
                if (statusId == "1" || statusId == "5" || statusId == "")
                {
                    int countSuccess = ActivityFormCommandHandler.insertAllActivity(activityModel, activityFormModel.id);
                }
                else
                {
                    result.MessageCode = "001";
                }

                TempData.Keep();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("insertDataActivity => " + ex.Message + "actId :" + activityFormModel.id);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateDataActivity(ActivityForm activityFormModel)
        {
            var result = new AjaxResult();
            try
            {
                Activity_Model activityModel = new Activity_Model();
                activityModel.activityFormModel = activityFormModel;

                int countSuccess = ActivityFormCommandHandler.updateActivityForm(activityModel, activityFormModel.id);

                result.ActivityId = activityFormModel.id;
                TempData.Keep();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("updateDataActivity => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult copyAndSaveNewActivityForm(ActivityForm activityFormModel)
        {
            var result = new AjaxResult();
            try
            {
                string actId = Guid.NewGuid().ToString();
                Activity_Model activityModel = new Activity_Model();
                activityModel = (Activity_Model)TempData["actForm" + activityFormModel.id];
                activityModel.activityFormModel = activityFormModel;
                activityModel.activityFormModel.activityNo = "";
                activityModel.activityFormModel.dateDoc = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now, ConfigurationManager.AppSettings["formatDateUse"]);
                int countSuccess = ActivityFormCommandHandler.insertAllActivity(activityModel, actId);
                TempData.Keep();
                result.ActivityId = actId;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("copyAndSaveNewActivityForm => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult copyAndSaveNewActivity_MasterForm(string activityId)
        {
            var result = new AjaxResult();
            try
            {
                string new_actId = Guid.NewGuid().ToString();
                Activity_TBMMKT_Model activityModel = new Activity_TBMMKT_Model();
                activityModel = (Activity_TBMMKT_Model)TempData["actForm" + activityId];
                activityModel.activityFormTBMMKT.activityNo = "";
                activityModel.activityFormTBMMKT.statusId = 1;
                activityModel.activityFormModel.documentDateStr = DocumentsAppCode.convertDateTHToShowCultureDateEN(DateTime.Now, ConfigurationManager.AppSettings["formatDateUse"]);
                activityModel.activityFormModel.activityPeriodStStr = DocumentsAppCode.convertDateTHToShowCultureDateEN(activityModel.activityFormModel.activityPeriodSt, ConfigurationManager.AppSettings["formatDateUse"]);
                activityModel.activityFormModel.activityPeriodEndStr = DocumentsAppCode.convertDateTHToShowCultureDateEN(activityModel.activityFormModel.activityPeriodEnd, ConfigurationManager.AppSettings["formatDateUse"]);
                int countSuccess = ActivityFormTBMMKTCommandHandler.insertAllActivity(activityModel, new_actId);
                TempData.Keep();
                result.ActivityId = new_actId;
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("copyAndSaveNewActivity_MasterForm => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult uploadFilesImage(string actId)
        {
            var result = new AjaxResult();
            try
            {
                byte[] binData = null;
                TB_Act_Image_Model.ImageModel imageFormModel = new TB_Act_Image_Model.ImageModel();
                foreach (string UploadedImage in Request.Files)
                {
                    HttpPostedFileBase httpPostedFile = Request.Files[UploadedImage];

                    string resultFilePath = "";
                    string extension = Path.GetExtension(httpPostedFile.FileName);
                    int indexGetFileName = httpPostedFile.FileName.LastIndexOf('.');
                    var _fileName = Path.GetFileName(httpPostedFile.FileName.Substring(0, indexGetFileName)) + "_" + DateTime.Now.ToString("ddMMyyHHmm") + extension;
                    string UploadDirectory = Server.MapPath(string.Format(System.Configuration.ConfigurationManager.AppSettings["rootUploadfiles"].ToString(), _fileName));
                    resultFilePath = UploadDirectory;
                    BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
                    httpPostedFile.SaveAs(UploadDirectory);

                    imageFormModel.activityId = actId;
                    imageFormModel._image = b.ReadBytes(0);
                    imageFormModel.imageType = "UploadFile";
                    imageFormModel._fileName = _fileName.ToLower();
                    imageFormModel.extension = extension.ToLower();
                    imageFormModel.delFlag = false;
                    imageFormModel.createdByUserId = UtilsAppCode.Session.User.empId;
                    imageFormModel.createdDate = DateTime.Now;
                    imageFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                    imageFormModel.updatedDate = DateTime.Now;
                    int resultImg = ImageAppCode.insertImageForm(imageFormModel);

                }

                result.ActivityId = actId;
                TempData.Keep();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
                ExceptionManager.WriteError("uploadFilesImage => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult deleteImg(string name)
        {
            var result = new AjaxResult();
            int resultImg = ImageAppCode.deleteImg(name, TempData["activityId"].ToString());
            TempData.Keep();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteImgById(string id)
        {
            var result = new AjaxResult();

            int resultImg = ImageAppCode.deleteImgById(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult checkApproveRefNo(string activityId, string typeForm)
        {
            var result = new AjaxResult();
            int setRang = 2;
            result.Success = false;

            //setRang = typeForm == Activity_Model.activityType.OMT.ToString() ? 2 : 3;

            var getApprove = ApproveAppCode.getApproveByActFormId(activityId);
            if (getApprove.approveDetailLists.Where(x => x.rangNo <= setRang && x.statusId == "5").Any())
            {
                result.Success = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<JsonResult> submitPreview(string GridHtml1, string status, string activityId, string statusNote)
        {
            var resultAjax = new AjaxResult();
            int countresult = 0;
            try
            {
                String[] genDoc = ActivityFormCommandHandler.genNumberActivity(activityId);
                countresult = ActivityFormCommandHandler.updateStatusGenDocActivity(status, activityId, genDoc[0]);
                string empId = UtilsAppCode.Session.User.empId;
                if (countresult > 0)
                {
                    List<ActivityFormTBMMKT> model = QueryGetActivityByIdTBMMKT.getActivityById(activityId);
                    if (model.FirstOrDefault().statusId != 3)
                    {
                        if (ApproveAppCode.insertApproveForActivityForm(activityId) > 0)
                        {
                            if (ApproveAppCode.updateApproveWaitingByRangNo(activityId) > 0)
                            {
                                //if (ConfigurationManager.AppSettings["formTransferbudget"].Equals(model.FirstOrDefault().master_type_form_id))
                                //{
                                //    //waiting update budgetControl
                                //    bool resultTransfer = TransferBudgetAppcode.transferBudgetAllApprove(activityId);
                                //}

                                // case form benefit will auto approve
                                if (QueryGetBenefit.getAllowAutoApproveForFormHC(activityId))
                                {
                                    ApproveAppCode.updateApprove(activityId, ((int)AppCode.ApproveStatus.อนุมัติ).ToString(), statusNote, AppCode.ApproveType.Activity_Form.ToString(), UtilsAppCode.Session.User.empId);
                                }

                                GridHtml1 = GridHtml1.Replace("---", genDoc[0]).Replace("<br>", "<br/>");
                                
                                HostingEnvironment.QueueBackgroundWorkItem(c => doGenFile(GridHtml1, empId, "2", activityId, ""));
                            }
                        }
                    }
                    else
                    {
                        HostingEnvironment.QueueBackgroundWorkItem(c => doGenFile(GridHtml1, empId, "2", activityId, ""));
                    }
                }

                ApproveAppCode.setCountWatingApprove(); // เพิ่มให้อัพเดทเอกสารที่ต้องอนุมัติเลย กรณีผู้สร้างเอกสารต้องอนุมัติด้วยหลังจากส่งอนุมัติหนังสือ fream dev date 20200622
                resultAjax.Success = true;
                resultAjax.Message = genDoc[1];
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
                ExceptionManager.WriteError("submitPreview => " + ex.Message);
            }
            return Json(resultAjax, "text/plain");
        }


        public async Task<AjaxResult> doGenFile(string gridHtml, string empId, string statusId, string activityId, string approveFrom)
        {
            var resultAjax = new AjaxResult();
            try
            {

                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {

                    var rootPathMap = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
                    var txtStamp = "เอกสารถูกยกเลิก";
                    bool success = AppCode.stampCancel(Server, rootPathMap, txtStamp);

                    if (approveFrom != "Consumer")
                    {
                        var resultAPI = ApproveAppCode.apiProducerApproveAsync(empId, activityId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == statusId).FirstOrDefault().displayVal);
                    }

                    EmailAppCodes.sendReject(activityId, AppCode.ApproveType.Activity_Form, empId);

                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"] || statusId == ConfigurationManager.AppSettings["waitApprove"])
                {
                    if (statusId == "3" && approveFrom != "Consumer")
                    {
                        var resultAPI = ApproveAppCode.apiProducerApproveAsync(empId, activityId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == statusId).FirstOrDefault().displayVal);
                    }
                    GenPDFAppCode.doGen(gridHtml, activityId, Server);
                    EmailAppCodes.sendApprove(activityId, AppCode.ApproveType.Activity_Form, false, true);

                }
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;

                throw new Exception("Activity>>doGenFile >> " + ex.Message);
            }

            return resultAjax;
        }
        public ActionResult subActivityView(string activityId, string count)
        {
            Activity_Model activityModel = new Activity_Model();
            try
            {
                var getModel = QueryGetActivityById.getActivityById(activityId).FirstOrDefault();
                activityModel.activityModelList = QueryGetActivityById.getSubActivityByActId(activityId);
                activityModel.activityFormModel = getModel;

                if (!string.IsNullOrEmpty(count))
                {
                    activityModel.activityFormModel.countMonth = int.Parse(count);
                }
                else
                {
                    activityModel.activityFormModel.countMonth = activityModel.activityModelList.Any() ? activityModel.activityModelList.Count :
                        (getModel.activityPeriodEnd.Value.AddDays(1).Month + getModel.activityPeriodEnd.Value.AddDays(1).Year * 12) - (getModel.activityPeriodSt.Value.Month + getModel.activityPeriodSt.Value.Year * 12);
                }

                if (activityModel.activityFormModel.countMonth == 0)
                    activityModel.activityFormModel.countMonth = 1;


                activityModel.activityFormModel.delFlag = activityModel.activityModelList.Any() ? true : false;
                activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(activityId);
                activityModel.activityFormModel.sumTotal = activityModel.activitydetaillist.Select(x => x.total).Sum();


                if (activityModel.activityModelList.Any())
                {
                    foreach (var item in activityModel.activityModelList)
                    {
                        switch (item.countAct)
                        {
                            case 1:
                                activityModel.activitydetaillist_0 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 2:
                                activityModel.activitydetaillist_1 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 3:
                                activityModel.activitydetaillist_2 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 4:
                                activityModel.activitydetaillist_3 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 5:
                                activityModel.activitydetaillist_4 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 6:
                                activityModel.activitydetaillist_5 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 7:
                                activityModel.activitydetaillist_6 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 8:
                                activityModel.activitydetaillist_7 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 9:
                                activityModel.activitydetaillist_8 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;
                            case 10:
                                activityModel.activitydetaillist_9 = QueryGetActivityDetailById.getSubActivityDetailById(item.id);
                                break;

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("subActivityView => " + ex.Message);
            }

            return PartialView(activityModel);

        }



        public JsonResult submit_SubActivity(Activity_Model model)
        {
            Activity_Model activityModel = new Activity_Model();
            var result = new AjaxResult();
            try
            {
                int countAct = 1;
                foreach (var item in model.activityModelList)
                {
                    if (string.IsNullOrEmpty(item.statusNote))
                    {
                        item.countAct = countAct++;
                        item.statusId = 3;
                        item.activityPeriodSt = string.IsNullOrEmpty(item.str_activityPeriodSt) ? (DateTime?)null :
                  BaseAppCodes.converStrToDatetimeWithFormat(item.str_activityPeriodSt, ConfigurationManager.AppSettings["formatDateUse"]);
                        item.activityPeriodEnd = string.IsNullOrEmpty(item.str_activityPeriodEnd) ? (DateTime?)null :
                  BaseAppCodes.converStrToDatetimeWithFormat(item.str_activityPeriodEnd, ConfigurationManager.AppSettings["formatDateUse"]);
                        result.Code = ActFormAppCode.insertSubActivity(item);
                    }
                }

                if (model.activitydetaillist_0.Any())

                    foreach (var item in model.activitydetaillist_0)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_1.Any())

                    foreach (var item in model.activitydetaillist_1)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_2.Any())

                    foreach (var item in model.activitydetaillist_2)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_3.Any())
                    foreach (var item in model.activitydetaillist_3)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_4.Any())

                    foreach (var item in model.activitydetaillist_4)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_5.Any())

                    foreach (var item in model.activitydetaillist_5)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_6.Any())
                    foreach (var item in model.activitydetaillist_6)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_7.Any())
                    foreach (var item in model.activitydetaillist_7)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_8.Any())
                    foreach (var item in model.activitydetaillist_8)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


                if (model.activitydetaillist_9.Any())
                    foreach (var item in model.activitydetaillist_9)
                    {
                        if (!string.IsNullOrEmpty(item.promotionCost.ToString()))
                        {
                            result.Code = ActFormAppCode.insertSubActivityDetail(item);
                        }
                    }


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("submit_SubActivity => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }



}






