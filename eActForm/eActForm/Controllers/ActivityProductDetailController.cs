using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class ActivityProductDetailController : Controller
    {
        public ActionResult productCostDetail(string typeForm)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.activityFormModel.typeForm = typeForm;
            if (Session["productcostdetaillist1"] != null)
            {
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
            }

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
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showDetailGroup(string rowId)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
            activityModel.productcostdetaillist1 = activityModel.productcostdetaillist1.Where(x => x.productGroupId == rowId).OrderBy(x => x.productName).ToList();

            return PartialView(activityModel);
        }

        public JsonResult delCostDetail(string rowid, Activity_Model activityModel)
        {
            var result = new AjaxResult();
            try
            {
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                if (rowid != null)
                {
                    var list = activityModel.productcostdetaillist1.Single(r => r.productGroupId == rowid);
                    activityModel.productcostdetaillist1.Remove(list);
                }
                else
                {
                    activityModel.productcostdetaillist1 = new List<ProductCostOfGroupByPrice>();
                }

                Session["productcostdetaillist1"] = activityModel.productcostdetaillist1;
                result.Data = activityModel.productcostdetaillist1.Count;
                result.Success = true;
            }
            catch (Exception ex)
            {
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
            , string theme)
        {
            var result = new AjaxResult();
            try
            {
                Activity_Model activityModel = new Activity_Model();
                activityModel.productcostdetaillist1 = Session["productcostdetaillist1"] == null ? new List<ProductCostOfGroupByPrice>()
                    : ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]); ;

                activityModel.activitydetaillist = Session["activitydetaillist"] == null ? new List<CostThemeDetailOfGroupByPrice>()
                    : (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]; ;


                var productlist = new Activity_Model();
                productlist.productcostdetaillist1 = QueryGetProductCostDetail.getProductcostdetail(brandid, smellId, size, cusid, productid, theme);
                activityModel.productcostdetaillist1.AddRange(productlist.productcostdetaillist1);


                CostThemeDetailOfGroupByPrice costthememodel = new CostThemeDetailOfGroupByPrice();
                int i = 0;
                foreach (var item in productlist.productcostdetaillist1)
                {
                    costthememodel = new CostThemeDetailOfGroupByPrice();
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
                    activityModel.activitydetaillist.Add(costthememodel);
                    i++;
                }

                Session["productcostdetaillist1"] = activityModel.productcostdetaillist1;
                //calculate Cost GP
                foreach (var item in productlist.productcostdetaillist1)
                {
                    bool calSuccess = calProductDetail(item);
                }
                Session["activitydetaillist"] = activityModel.activitydetaillist;

                result.Data = productlist.productcostdetaillist1.Count;

            }
            catch (Exception ex)
            {
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
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
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
                //Session["productcostdetaillist1"] = activityModel.productcostdetaillist1;
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