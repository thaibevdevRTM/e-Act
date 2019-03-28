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

        public ActionResult productCostDetail()
        {
            Activity_Model activityModel = new Activity_Model();
            if (Session["productcostdetaillist1"] != null)
            {
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
            }

            return PartialView(activityModel);
        }

        public ActionResult activityDetail()
        {
            Activity_Model activityModel = new Activity_Model();
            if (Session["activitydetaillist"] != null)
            {
                activityModel.activitydetaillist = ((List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]);
            }

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


        public JsonResult addDetailTheme(string themeId, string txttheme)
        {
            var result = new AjaxResult();
            try
            {
                Activity_Model activityModel = new Activity_Model();
                CostThemeDetailOfGroupByPrice costThemeDetailOfGroupByPriceModel = new CostThemeDetailOfGroupByPrice();
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];

                Productcostdetail productcostdetail = new Productcostdetail();
                costThemeDetailOfGroupByPriceModel.id = Guid.NewGuid().ToString();
                costThemeDetailOfGroupByPriceModel.activityTypeId = themeId;
                costThemeDetailOfGroupByPriceModel.isShowGroup = "false";

                productcostdetail.id = Guid.NewGuid().ToString();
                productcostdetail.typeTheme = txttheme;

                costThemeDetailOfGroupByPriceModel.detailGroup = new List<Productcostdetail>();
                costThemeDetailOfGroupByPriceModel.detailGroup.Add(productcostdetail);
                activityModel.activitydetaillist.Add(costThemeDetailOfGroupByPriceModel);


                Session["activitydetaillist"] = activityModel.activitydetaillist;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult calDetailTheme(string id, string productId, string name, string normalCost, string themeCost, string growth)
        {
            var result = new AjaxResult();

            try
            {
                Activity_Model activityModel = new Activity_Model();
                decimal p_total = 0;
                decimal getPromotionCost = 0;
                decimal getNormalCost = 0;
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];
                if (checkNullorEmpty(themeCost) != "0")
                {
                    getNormalCost = decimal.Parse(checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productId == productId).FirstOrDefault().normalCost.ToString()));
                    getPromotionCost = decimal.Parse(checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productId == productId).FirstOrDefault().promotionCost.ToString()));
                    p_total = (getNormalCost - getPromotionCost) * decimal.Parse(themeCost);
<<<<<<< HEAD
                    get_PerTotal = p_total * 100 / decimal.Parse(themeCost);
=======
>>>>>>> parent of 962f5fb... Merge branch 'developer' of https://github.com/thaibevdevRTM/e-Act into developer
                }

                decimal p_growth = normalCost == "0" ? 0 : (decimal.Parse(themeCost) - decimal.Parse(normalCost)) / decimal.Parse(normalCost);
                activityModel.activitydetaillist
                        .Where(r => r.id != null && r.id.Equals(id))
                        .Select(r =>
                        {
                            r.productName = name;
                            r.normalCost = decimal.Parse(normalCost);
                            r.growth = p_growth;
                            r.themeCost = decimal.Parse(themeCost);
                            r.total = p_total;
<<<<<<< HEAD
                            r.perTotal = p_growth;
=======
>>>>>>> parent of 962f5fb... Merge branch 'developer' of https://github.com/thaibevdevRTM/e-Act into developer
                            return r;
                        }).ToList();

                Session["activitydetaillist"] = activityModel.activitydetaillist;
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult calDiscountProduct(
              string id
            , string productId
            , string wholeSalesPrice
            , string saleOut
            , string saleIn
            , string disCount1
            , string disCount2
            , string disCount3
            , string normalCost
            , string normalGP
            , string promotionGP
            , string specialDisc
            , string specialDiscBaht)
        {
            var result = new AjaxResult();
            try
            {
                decimal p_wholeSalesPrice = checkNullorEmpty(wholeSalesPrice) == "0" ? 0 : decimal.Parse(checkNullorEmpty(wholeSalesPrice));
                decimal p_disCount1 = checkNullorEmpty(disCount1) == "0" ? p_wholeSalesPrice : p_wholeSalesPrice - ((decimal.Parse(checkNullorEmpty(disCount1)) / 100) * p_wholeSalesPrice);
                decimal p_disCount2 = checkNullorEmpty(disCount2) == "0" ? p_disCount1 : p_disCount1 - ((decimal.Parse(checkNullorEmpty(disCount2)) / 100) * p_disCount1);
                decimal p_disCount3 = checkNullorEmpty(disCount3) == "0" ? p_disCount2 : p_disCount2 - ((decimal.Parse(checkNullorEmpty(disCount3)) / 100) * p_disCount2);

                decimal getPackProduct = QueryGetAllProduct.getProductById(productId).FirstOrDefault().pack;

                decimal p_normalGp = checkNullorEmpty(saleOut) == "0" || getPackProduct == 0 ? 0 : (decimal.Parse(saleOut) - (p_wholeSalesPrice * decimal.Parse("1.07"))
                    / getPackProduct) / decimal.Parse(saleOut);
              
                decimal p_PromotionCost = checkNullorEmpty(specialDisc) == "0" && checkNullorEmpty(specialDiscBaht) == "0" || p_disCount3 == 0 ? p_disCount3 : (p_disCount3 - (p_disCount3 * (decimal.Parse(specialDisc) / 100))) - decimal.Parse(checkNullorEmpty(specialDiscBaht));

                decimal p_PromotionGp = checkNullorEmpty(saleIn) == "0" ? 0 : (decimal.Parse(saleIn) - (p_PromotionCost * decimal.Parse("1.07"))
                  / getPackProduct) / decimal.Parse(checkNullorEmpty(saleIn));


                Activity_Model activityModel = new Activity_Model();
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.productcostdetaillist1
                    .Where(r => r.id != null && r.id.Equals(id))
                    .Select(r =>
                    {
                        r.wholeSalesPrice = decimal.Parse(checkNullorEmpty(wholeSalesPrice));
                        r.disCount1 = decimal.Parse(checkNullorEmpty(disCount1));
                        r.disCount2 = decimal.Parse(checkNullorEmpty(disCount2));
                        r.disCount3 = decimal.Parse(checkNullorEmpty(disCount3));
                        r.saleOut = decimal.Parse(checkNullorEmpty(saleOut));
                        r.saleIn = decimal.Parse(checkNullorEmpty(saleIn));
                        r.normalGp = p_normalGp;
                        r.promotionGp = p_PromotionGp;
                        r.specialDisc = decimal.Parse(checkNullorEmpty(specialDisc));
                        r.specialDiscBaht = decimal.Parse(checkNullorEmpty(specialDiscBaht));
                        r.normalCost = p_disCount3 == 0 ? p_wholeSalesPrice : p_disCount3;
                        r.promotionCost = p_PromotionCost;
                        return r;
                    }).ToList();
                Session["productcostdetaillist1"] = activityModel.productcostdetaillist1;


                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult addItemProduct(
              string productid
            , string brandid
            , string smellId
            , string size
            , string cusid
            , string theme)
        {
            var result = new AjaxResult();
            try
            {
                Activity_Model activityModel = new Activity_Model();


                if (Session["productcostdetaillist1"] == null)
                {
                    activityModel.productcostdetaillist1 = new List<ProductCostOfGroupByPrice>();
                    activityModel.activitydetaillist = new List<CostThemeDetailOfGroupByPrice>();
                }
                else
                {
                    activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                    activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];
                }


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
                    costthememodel.productId = item.productId;
                    costthememodel.activityTypeId = theme;
                    costthememodel.brandName = item.brandName;
                    costthememodel.smellName = item.smellName;
                    costthememodel.isShowGroup = item.isShowGroup;
                    costthememodel.detailGroup = item.detailGroup;
                    activityModel.activitydetaillist.Add(costthememodel);
                    i++;
                }

                Session["productcostdetaillist1"] = activityModel.productcostdetaillist1;
                Session["activitydetaillist"] = activityModel.activitydetaillist;
                var resultData = new
                {
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
            try
            {
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                if (rowid != null)
                {
                    var list = activityModel.productcostdetaillist1.Single(r => r.id == rowid);
                    activityModel.productcostdetaillist1.Remove(list);
                }
                else
                {
                    activityModel.productcostdetaillist1 = new List<ProductCostOfGroupByPrice>();
                }

                Session["productcostdetaillist1"] = activityModel.productcostdetaillist1;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult delActDetail(string rowid, Activity_Model activityModel)
        {
            var result = new AjaxResult();
            try
            {
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];
                if (rowid != null)
                {
                    var list = activityModel.activitydetaillist.Single(r => r.id == rowid);
                    activityModel.activitydetaillist.Remove(list);
                }
                else
                {
                    activityModel.activitydetaillist = new List<CostThemeDetailOfGroupByPrice>();
                }

                Session["activitydetaillist"] = activityModel.activitydetaillist;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }

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
            return Json(resultAjax, "text/plain");
        }



        public string checkNullorEmpty(string p)
        {
            return p == "" || p == null || p == "0" || p == "0.00" ? "0" : p;
        }


    }
}