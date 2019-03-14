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

        public ActionResult ActivityForm()
        {
            Session["activityId"] = Guid.NewGuid().ToString();
            Activity_Model activityModel = new Activity_Model();
            activityModel.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();
            activityModel.customerslist = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameEN != "").ToList();
            activityModel.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
            activityModel.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                .GroupBy(item => item.activitySales)
                .Select(grp => new TB_Act_ActivityGroup_Model{ id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

            return View(activityModel);
        }

        public ActionResult EditForm(string activityId)
        {

            Session["activityId"] = activityId;
            Activity_Model activityModel = new Activity_Model();

            activityModel.activityFormModel = QueryGetActivityById.getActivityById(activityId).FirstOrDefault();
            activityModel.productcostdetaillist = QueryGetCostDetailById.getcostDetailById(activityId);
            activityModel.costthemedetail = QueryGetActivityDetailById.getActivityDetailById(activityId);

            activityModel.customerslist = QueryGetAllCustomers.getAllCustomers().ToList();
            activityModel.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
            activityModel.productGroupList = QueryGetAllProductGroup.getAllProductGroup();
            activityModel.productSmellLists = QueryGetAllProduct.getProductSmellByGroupId(activityModel.activityFormModel.productGroupId);
            activityModel.productBrandList = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId == activityModel.activityFormModel.productGroupId).ToList();
            activityModel.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup().GroupBy(item => item.activitySales)
               .Select(grp => new TB_Act_ActivityGroup_Model
               {
                   id = grp.First().id,
                   activitySales = grp.First().activitySales
               }).ToList();

            return View(activityModel);
        }

        public ActionResult ImageList(string activityId)
        {
            TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
            getImageModel.tbActImageList = QueryGetImageById.GetImage(activityId);
            return PartialView(getImageModel);
        }

        public ActionResult productCostDetail(Activity_Model activityModel)
        {
            return PartialView(activityModel);
        }

        public ActionResult activityDetail(Activity_Model activityModel)
        {
            return PartialView(activityModel);
        }


        public ActionResult ApproveForm(Activity_Model activityModel)
        {
            return PartialView(activityModel);
        }

        public ActionResult PreviewData(string activityId)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.activityFormModel = QueryGetActivityById.getActivityById(activityId).FirstOrDefault();
            activityModel.productcostdetaillist = QueryGetCostDetailById.getcostDetailById(activityId);
            activityModel.costthemedetail = QueryGetActivityDetailById.getActivityDetailById(activityId);

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


        public JsonResult addDetailTheme(Activity_Model activityModel, string themeId, string txttheme)
        {
            var result = new AjaxResult();
            try
            {
                
                CostThemeDetailOfGroupByPrice costThemeDetailOfGroupByPriceModel = new CostThemeDetailOfGroupByPrice();
                Productcostdetail productcostdetail = new Productcostdetail();
                costThemeDetailOfGroupByPriceModel.id = Guid.NewGuid().ToString();
                costThemeDetailOfGroupByPriceModel.activityTypeId = themeId;
                costThemeDetailOfGroupByPriceModel.isShowGroup = false;
                productcostdetail.id = Guid.NewGuid().ToString();
                productcostdetail.typeTheme = txttheme;

                costThemeDetailOfGroupByPriceModel.detailGroup = new List<Productcostdetail>();
                costThemeDetailOfGroupByPriceModel.detailGroup.Add(productcostdetail);
                activityModel.activitydetaillist.Add(costThemeDetailOfGroupByPriceModel);

                var resultData = new { activityModel };
                result.Data = resultData;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult calDetailTheme(Activity_Model activityModel, string id, string name, string normalCost, string themeCost, string growth)
        {
            var result = new AjaxResult();
            try
            {
                activityModel.costthemedetail
                    .Where(r => r.id != null && r.id.Equals(id))
                    .Select(r =>
                    {
                        r.productName = name;
                        r.normalCost = decimal.Parse(normalCost == "" ? "0" : normalCost);
                        r.growth = decimal.Parse(growth == "" ? "0" : growth);
                        r.themeCost = decimal.Parse(themeCost == "" ? "0" : themeCost);
                        r.total = decimal.Parse(themeCost == "" ? "0" : themeCost)
                        + (decimal.Parse(themeCost == "" ? "0" : themeCost) * (decimal.Parse(growth == "" ? "0" : growth) / 100));
                        return r;
                    }).ToList();

                var resultData = new
                {
                    activityModel,
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

        public JsonResult calDiscountProduct(Activity_Model activityModel
            , string id
            , string normalCost
            , string normalGP
            , string promotionGP
            , string specialDisc)
        {
            var result = new AjaxResult();
            try
            {
                activityModel.productcostdetaillist1
                    .Where(r => r.id != null && r.id.Equals(id))
                    .Select(r =>
                    {
                        r.normalGp = decimal.Parse(normalGP == null || normalGP == "" ? "0" : normalGP);
                        r.promotionGp = decimal.Parse(promotionGP == null || promotionGP == "" ? "0" : promotionGP);
                        r.specialDisc = decimal.Parse(specialDisc == "" ? "0" : specialDisc);
                        r.promotionCost = decimal.Parse(normalCost == "" ? "0" : normalCost)
                        - (decimal.Parse(normalCost == "" ? "0" : normalCost) * (decimal.Parse(specialDisc == "" ? "0" : specialDisc) / 100));
                        return r;
                    }).ToList();

                var resultData = new { activityModel };
                result.Data = resultData;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult addItemProduct(Activity_Model activityModel
            , string productid
            , string brandid
            , string smellId
            , string size
            , string cusid
            , string theme)
        {
            var result = new AjaxResult();
            try
            {
                var productlist = new Activity_Model();
                productlist.productcostdetaillist1 = QueryGetProductCostDetail.getProductcostdetail(brandid, smellId, size, cusid, productid, theme);

                foreach (var item in productlist.productcostdetaillist1)
                {
                    activityModel.productcostdetaillist1.Add(item);
                }

                CostThemeDetailOfGroupByPrice costthememodel = new CostThemeDetailOfGroupByPrice();
                int i = 0;
                foreach (var item in productlist.productcostdetaillist1)
                {
                    costthememodel = new CostThemeDetailOfGroupByPrice();
                    costthememodel.id = Guid.NewGuid().ToString();
                    costthememodel.typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == theme).FirstOrDefault().activitySales;
                    costthememodel.activityTypeId = theme;
                    costthememodel.brandName = item.brandName;
                    costthememodel.smellName = item.smellName;
                    costthememodel.isShowGroup = item.isShowGroup;
                    costthememodel.detailGroup = item.detailGroup;
                    activityModel.activitydetaillist.Add(costthememodel);
                    i++;
                }

                var resultData = new
                {
                    activityModel = activityModel,
                    checkrow = productlist.productcostdetaillist1.Count == 0 ? false : true,
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


        public JsonResult delCostDetail(string rowid, Activity_Model activityModel)
        {
            var result = new AjaxResult();
            if (rowid != null)
            {
                var list = activityModel.productcostdetaillist1.Single(r => r.id == rowid);
                activityModel.productcostdetaillist1.Remove(list);
            }
            else
            {
                activityModel.productcostdetaillist1 = new List<ProductCostOfGroupByPrice>();
            }

            var resultData = new
            {
                activityModel = activityModel,

            };
            result.Data = resultData;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult delActDetail(string rowid, Activity_Model activityModel)
        {
            var result = new AjaxResult();
            if (rowid != null)
            {
                var list = activityModel.activitydetaillist.Single(r => r.id == rowid);
                activityModel.activitydetaillist.Remove(list);
            }
            else
            {
                activityModel.activitydetaillist = new List<CostThemeDetailOfGroupByPrice>();
            }

            var resultData = new
            {
                activityModel = activityModel,

            };
            result.Data = resultData;

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
                    AppCode.genPdfFile(GridHtml, activityId);
                    ApproveAppCode.insertApprove(activityId);
                    EmailAppCodes.sendApproveActForm(activityId,Server);
                }
                //sendEmail(
                //    "tanapong.w@thaibev.com"
                //    , "champ.tanapong@gmail.com"
                //    , "Test Subject eAct"
                //    , "Test Body"
                //    , genPdfFile(GridHtml, activityId)

                //    );


                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax, "text/plain");
        }






    }
}