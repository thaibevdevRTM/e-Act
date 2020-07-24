using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class ActivityProductDetailController : Controller
    {
        public ActionResult productCostDetail(string typeForm, string actId)
        {

            Activity_Model activityModel = TempData["actForm" + actId] == null ? new Activity_Model() : (Activity_Model)TempData["actForm" + actId];
            activityModel.activityFormModel.typeForm = typeForm;
            activityModel.activityFormModel.id = actId;
            TempData.Keep();
            return PartialView(activityModel);
        }

        public JsonResult calDiscountProduct(ProductCostOfGroupByPrice model)
        {
            var result = new AjaxResult();
            try
            {
                calProductDetail(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("calDiscountProduct >>" + ex.Message);
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showDetailGroup(string rowId, string actId)
        {
            Activity_Model activityModel = TempData["actForm" + actId] == null ? new Activity_Model() : (Activity_Model)TempData["actForm" + actId];
            Activity_Model ShowList_Product = new Activity_Model();
            ShowList_Product.productcostdetaillist1 = activityModel.productcostdetaillist1.Where(x => x.productGroupId == rowId).OrderBy(x => x.productName).ToList();

            TempData.Keep();
            return PartialView(ShowList_Product);
        }

        public JsonResult delCostDetail(string rowid, string id)
        {
            var result = new AjaxResult();
            try
            {
                Activity_Model activityModel = TempData["actForm" + id] == null ? new Activity_Model() : (Activity_Model)TempData["actForm" + id];
                if (rowid != null)
                {
                    activityModel.productcostdetaillist1.RemoveAll(r => r.productGroupId == rowid);
                    activityModel.activitydetaillist.RemoveAll(r => r.productGroupId == rowid);
                }
                else
                {
                    activityModel.productcostdetaillist1 = new List<ProductCostOfGroupByPrice>();
                    activityModel.activitydetaillist = new List<CostThemeDetailOfGroupByPrice>();
                }

                TempData["actForm" + activityModel.activityFormModel.id] = activityModel;
                TempData.Keep();
                result.Data = activityModel.productcostdetaillist1.Count;
                result.Success = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("delCostDetail >>" + ex.Message);
                result.Message = ex.Message;
                result.Success = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addItemProduct(
              string productid
            , string brandid
            , string smellId
            , string size
            , string cusid
            , string theme
            , string actId
            , string typeForm
            , string dateActivitySt)
        {
            var result = new AjaxResult();
            try
            {
                Activity_Model activityModel = TempData["actForm" + actId] == null ? new Activity_Model() : (Activity_Model)TempData["actForm" + actId];
                var productlist = new Activity_Model();
                productlist.productcostdetaillist1 = QueryGetProductCostDetail.getProductcostdetail(brandid, smellId, size, cusid, productid, theme, typeForm);
                activityModel.productcostdetaillist1.AddRange(productlist.productcostdetaillist1);


                CostThemeDetailOfGroupByPrice costthememodel = new CostThemeDetailOfGroupByPrice();
                int i = 0;
                foreach (var item in productlist.productcostdetaillist1)
                {
                    costthememodel = new CostThemeDetailOfGroupByPrice();
                    costthememodel.activityId = actId;
                    costthememodel.productGroupId = item.productGroupId;
                    costthememodel.typeTheme = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == theme).FirstOrDefault().activitySales;
                    costthememodel.productId = item.productId;
                    costthememodel.activityTypeId = theme;
                    costthememodel.brandName = item.brandName.Trim() + " " + item.size + "ALL(" + item.detailGroup.Count + ") " + item.pack;
                    costthememodel.productName = item.isShowGroup ? costthememodel.brandName : item.productName + " " + item.pack;
                    costthememodel.size = item.size;
                    costthememodel.smellName = item.smellName;
                    costthememodel.smellId = item.smellId;
                    costthememodel.brandId = item.brandId;
                    costthememodel.unit = item.unit;
                    costthememodel.isShowGroup = item.isShowGroup;
                    costthememodel.detailGroup = item.detailGroup;
                    //if (ActFormAppCode.getDigitGroup(theme) != "")
                    //{

                    //    DateTime getDoc = DateTime.ParseExact(dateActivitySt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    //    string getYear = getDoc.Month > 9 ? getDoc.AddYears(1).ToString("yy") : getDoc.Year.ToString().Substring(2);
                    //    costthememodel.IO = "56S0" + getYear + ActFormAppCode.getDigitGroup(theme) + ActFormAppCode.getDigitRunnigGroup(item.productId);
                    //}
                    activityModel.activitydetaillist.Add(costthememodel);
                    i++;
                }

                //calculate Cost GP
                foreach (var item in productlist.productcostdetaillist1)
                {
                    item.activityId = actId;
                    bool calSuccess = calProductDetail(item);
                }
                TempData["actForm" + actId] = activityModel;
                result.Data = productlist.productcostdetaillist1.Count;
                result.ActivityId = actId;
                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("addItemProduct >> " + ex.Message);
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public bool calProductDetail(ProductCostOfGroupByPrice model)
        {
            bool success = true;
            try
            {
                decimal fixFormula = (decimal)1.07;
                decimal getPackProduct = QueryGetAllProduct.getProductById(model.productId).FirstOrDefault().unit / QueryGetAllProduct.getProductById(model.productId).FirstOrDefault().pack;
                getPackProduct = getPackProduct == 0 ? 1 : getPackProduct;
                decimal specDisc = decimal.Parse(AppCode.checkNullorEmpty(model.specialDisc.ToString()));
                decimal specDiscBath = decimal.Parse(AppCode.checkNullorEmpty(model.specialDiscBaht.ToString()));
                decimal p_wholeSalesPrice = (decimal)model.wholeSalesPrice;
                decimal p_disCount1 = model.disCount1 == 0 ? p_wholeSalesPrice : p_wholeSalesPrice - (decimal)model.disCount1;
                decimal p_disCount2 = model.disCount2 == 0 ? p_disCount1 : p_disCount1 - (decimal)model.disCount2;
                decimal p_disCount3 = model.disCount3 == 0 ? p_disCount2 : p_disCount2 - (decimal)model.disCount3;
                decimal p_PromotionCost = (specDisc == 0 && specDiscBath == 0 || p_disCount3 == 0) ? p_disCount3
                    : (p_disCount3 - (p_disCount3 * (specDisc / 100))) - specDiscBath;

                // % normalGP
                decimal sNormal = (decimal)model.saleNormal;/// getPackProduct;
                decimal p_normalGp = sNormal == 0 ? 0 : ((sNormal - ((p_disCount3 * fixFormula) / getPackProduct)) / sNormal) * 100;
                p_normalGp = p_normalGp < 0 ? p_normalGp * -1 : p_normalGp;

                // % promotionGP
                decimal pPromotion = decimal.Parse(AppCode.checkNullorEmpty(model.saleIn.ToString()));/// getPackProduct;
                decimal p_PromotionGp = pPromotion == 0 ? pPromotion : ((pPromotion - ((p_PromotionCost * fixFormula) / getPackProduct)) / pPromotion) * 100;
                p_PromotionGp = p_PromotionGp > 0 ? p_PromotionGp : p_PromotionGp * -1;

                Activity_Model activityModel = new Activity_Model();
                activityModel = (Activity_Model)TempData["actForm" + model.activityId];
                activityModel.productcostdetaillist1
                    .Where(r => r.productGroupId != null && r.productGroupId.Equals(model.productGroupId))
                    .Select(r =>
                    {
                        r.wholeSalesPrice = p_wholeSalesPrice;
                        r.disCount1 = (decimal)model.disCount1;
                        r.disCount2 = (decimal)model.disCount2;
                        r.disCount3 = (decimal)model.disCount3;
                        r.saleNormal = sNormal;
                        r.saleIn = pPromotion;
                        r.normalGp = Math.Round(p_normalGp, 3);
                        r.promotionGp = Math.Round(p_PromotionGp, 3);
                        r.specialDisc = specDisc;
                        r.specialDiscBaht = specDiscBath;
                        r.normalCost = p_disCount3 == 0 ? model.normalCost : p_disCount3;
                        r.promotionCost = Math.Round(p_PromotionCost, 3);
                        return r;
                    }).ToList();
                TempData["actForm" + model.activityId] = activityModel;
                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("calProductDetail >> " + ex.Message);
                success = false;
            }

            return success;
        }
    }
}