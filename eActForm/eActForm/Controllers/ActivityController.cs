using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
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
            activityModel.activityFormModel = new ActivityForm();
            activityModel.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();
            activityModel.customerslist = QueryGetAllCustomers.getCustomersByEmpId();
            activityModel.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
            activityModel.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                .GroupBy(item => item.activitySales)
                .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();
            if (UtilsAppCode.Session.User.regionId != "")
            {
                activityModel.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.id == UtilsAppCode.Session.User.regionId).ToList();
                activityModel.activityFormModel.regionId = UtilsAppCode.Session.User.regionId;
            }
            else
            {
                activityModel.regionGroupList = QueryGetAllRegion.getAllRegion();
            }

            Session.Remove("productcostdetaillist1");
            Session.Remove("activitydetaillist");

            if (!string.IsNullOrEmpty(activityId))
            {
                Session["activityId"] = activityId;
                activityModel.activityFormModel = QueryGetActivityById.getActivityById(activityId).FirstOrDefault();
                activityModel.activityFormModel.mode = mode;
                Session["productcostdetaillist1"] = QueryGetCostDetailById.getcostDetailById(activityId);
                Session["activitydetaillist"] = QueryGetActivityDetailById.getActivityDetailById(activityId);
                activityModel.productSmellLists = QueryGetAllProduct.getProductSmellByGroupId(activityModel.activityFormModel.productGroupId);
                activityModel.productBrandList = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId == activityModel.activityFormModel.productGroupId).ToList();
                activityModel.productGroupList = QueryGetAllProductGroup.getAllProductGroup().Where(x => x.cateId == activityModel.activityFormModel.productCateId).ToList();
            }
            else
            {
                string actId = Guid.NewGuid().ToString();
                Session["activityId"] = actId;
                activityModel.activityFormModel.id = actId;
                activityModel.activityFormModel.mode = mode;
                activityModel.activityFormModel.statusId = 1;
            }
            activityModel.activityFormModel.typeForm = typeForm;
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

        public ActionResult PreviewData(string activityId,string typeForm)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.activityFormModel = QueryGetActivityById.getActivityById(activityId).FirstOrDefault();
            activityModel.activityFormModel.typeForm = typeForm;
            activityModel.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(activityId);
            activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(activityId);
            activityModel.productImageList = ImageAppCode.GetImage(activityId).Where(x => x.extension != ".pdf").ToList();

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
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult insertDataActivity(ActivityForm activityFormModel)
        {
            var result = new AjaxResult();
            try
            {
                Activity_Model activityModel = new Activity_Model();
                activityModel.activityFormModel = activityFormModel;
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.activitydetaillist = ((List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]);
                int countSuccess = ActivityFormCommandHandler.insertAllActivity(activityModel, Session["activityId"].ToString());

                result.ActivityId = Session["activityId"].ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
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

                int countSuccess = ActivityFormCommandHandler.updateActivityForm(activityModel, Session["activityId"].ToString());

                result.ActivityId = Session["activityId"].ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
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
                activityModel.activityFormModel = activityFormModel;
                activityModel.activityFormModel.activityNo = "";
                activityModel.activityFormModel.dateDoc = DateTime.Now.ToString("dd-MM-yyyy");
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.activitydetaillist = ((List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]);
                int countSuccess = ActivityFormCommandHandler.insertAllActivity(activityModel, actId);

                result.ActivityId = actId;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult uploadFilesImage()
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
                    binData = b.ReadBytes(0);
                    httpPostedFile.SaveAs(resultFilePath);

                    imageFormModel.activityId = Session["activityId"].ToString();
                    imageFormModel._image = binData;
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

                result.ActivityId = Session["activityId"].ToString();
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
            int resultImg = ImageAppCode.deleteImg(name, Session["activityId"].ToString());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteImgById(string id)
        {
            var result = new AjaxResult();

            int resultImg = ImageAppCode.deleteImgById(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult submitPreview(string GridHtml1, string status, string activityId)
        {
            var resultAjax = new AjaxResult();
            int countresult = 0;
            try
            {

                String[] genDoc = ActivityFormCommandHandler.genNumberActivity(activityId);
                countresult = ActivityFormCommandHandler.updateStatusGenDocActivity(status, activityId, genDoc[0]);
                if (countresult > 0)
                {
                    var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId + "_");
                    GridHtml1 = GridHtml1.Replace("---", genDoc[0]).Replace("<br>", "<br/>");
                    AppCode.genPdfFile(GridHtml1, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

                    TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
                    getImageModel.tbActImageList = ImageAppCode.GetImage(activityId).Where(x => x.extension == ".pdf").ToList();
                    string[] pathFile = new string[getImageModel.tbActImageList.Count + 1];
                    pathFile[0] = Server.MapPath(rootPathInsert);
                    if (getImageModel.tbActImageList.Any())
                    {
                        int i = 1;
                        foreach (var item in getImageModel.tbActImageList)
                        {
                            pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                            i++;
                        }
                    }
                    var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
                    var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile);

                    if (ApproveAppCode.insertApproveForActivityForm(activityId) > 0)
                    {
                        ApproveAppCode.updateApproveWaitingByRangNo(activityId);
                        EmailAppCodes.sendApprove(activityId, AppCode.ApproveType.Activity_Form, false);
                    }
                }
                resultAjax.Success = true;
                resultAjax.Message = genDoc[1];
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }
            return Json(resultAjax, "text/plain");
        }

      


        
    }
}


