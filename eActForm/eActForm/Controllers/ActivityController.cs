using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ActivityController : eActController
    {
        // GET: Activity
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ActivityForm(string activityId, string mode)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.activityFormModel = new ActivityForm();
            activityModel.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();
            activityModel.customerslist = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameEN != "").ToList();
            activityModel.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
            activityModel.productGroupList = QueryGetAllProductGroup.getAllProductGroup();
            activityModel.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                .GroupBy(item => item.activitySales)
                .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

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

            }
            else
            {
                Session["activityId"] = Guid.NewGuid().ToString();
                activityModel.activityFormModel.mode = mode;
            }

            return View(activityModel);
        }



        public ActionResult ImageList(string activityId)
        {
            TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
            if (!string.IsNullOrEmpty(activityId))
            {
                getImageModel.tbActImageList = QueryGetImageById.GetImage(activityId);
                return PartialView(getImageModel);
            }
            else
            {
                return PartialView(getImageModel);
            }
        }

        public ActionResult PreviewData(string activityId)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.activityFormModel = QueryGetActivityById.getActivityById(activityId).FirstOrDefault();
            activityModel.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(activityId);
            activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(activityId);

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


        public JsonResult insertDataActivity(ActivityForm activityFormModel, Activity_Model activityModel)
        {
            var result = new AjaxResult();
            try
            {
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
                    string folderKeepFile = "ActivityForm";
                    string UploadDirectory = Server.MapPath("~") + "\\Uploadfiles\\" + folderKeepFile + "\\";
                    string resultFilePath = "";
                    AppCode.CheckFolder_CreateNotHave_Direct(UploadDirectory);

                    string genUniqueName = httpPostedFile.FileName.ToString();
                    string extension = Path.GetExtension(httpPostedFile.FileName);
                    int indexGetFileName = httpPostedFile.FileName.LastIndexOf('.');
                    var _fileName = Path.GetFileName(httpPostedFile.FileName.Substring(0, indexGetFileName)) + "_" + Session["activityId"].ToString();
                    string resultFileUrl = UploadDirectory + _fileName + extension;
                    resultFilePath = resultFileUrl;
                    BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
                    binData = b.ReadBytes(httpPostedFile.ContentLength);
                    httpPostedFile.SaveAs(resultFilePath);

                    imageFormModel.activityId = Session["activityId"].ToString();
                    imageFormModel._image = binData;
                    imageFormModel.imageType = "ActivityForm";
                    imageFormModel._fileName = genUniqueName;
                    imageFormModel.delFlag = false;
                    imageFormModel.createdByUserId = UtilsAppCode.Session.User.empId;
                    imageFormModel.createdDate = DateTime.Now;
                    imageFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                    imageFormModel.updatedDate = DateTime.Now;

                    int resultImg = ActivityFormCommandHandler.insertImageForm(imageFormModel);

                }


                result.ActivityId = Session["activityId"].ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult deleteImg(string name)
        {
            var result = new AjaxResult();

            int resultImg = ActivityFormCommandHandler.deleteImg(name, Session["activityId"].ToString());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteImgById(string id)
        {
            var result = new AjaxResult();

            int resultImg = ActivityFormCommandHandler.deleteImgById(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult submitPreview(string GridHtml, string status, string activityId)
        {
            var resultAjax = new AjaxResult();
            int countresult = 0;
            try
            {
                string genDoc = ActivityFormCommandHandler.genNumberActivity(activityId);
                countresult = ActivityFormCommandHandler.updateStatusGenDocActivity(status, activityId, genDoc);
                if (countresult > 0)
                {
                    GridHtml = GridHtml.Replace("---", genDoc);
                    AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), activityId);
                    if (ApproveAppCode.insertApprove(activityId) > 0)
                    {
                        ApproveAppCode.updateApproveWaitingByRangNo(activityId);
                        EmailAppCodes.sendApproveActForm(activityId);
                    }
                }
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }
            return  Json(resultAjax, "text/plain");
        }


    }
}