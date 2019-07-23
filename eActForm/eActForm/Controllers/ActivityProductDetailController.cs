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
        public ActionResult productCostDetail()
        {
            Activity_Model activityModel = new Activity_Model();
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
                    costthememodel.productName = item.isShowGroup ? costthememodel.brandName : item.productName + " "+ item.pack;
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
                var normalCost = model.normalCost.ToString().Replace(",", "");
                var wholeSalesPrice = model.wholeSalesPrice.ToString().Replace(",", "");
                var saleNormal = model.saleNormal.ToString().Replace(",", "");
                var saleIn = model.saleIn.ToString().Replace(",", "");
                var normalGP = model.normalGp == null ? "" : model.normalGp.ToString().Replace(",", "");
                var promotionGP = model.promotionGp == null ? "" : model.promotionGp.ToString().Replace(",", "");
                var promotionCost = model.promotionCost == null ? "" : model.promotionCost.ToString().Replace(",", "");

                decimal p_wholeSalesPrice = AppCode.checkNullorEmpty(wholeSalesPrice) == "0" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(wholeSalesPrice));
                decimal p_disCount1 = AppCode.checkNullorEmpty(model.disCount1.ToString()) == "0" ? p_wholeSalesPrice : p_wholeSalesPrice - (decimal.Parse(AppCode.checkNullorEmpty(model.disCount1.ToString())));
                decimal p_disCount2 = AppCode.checkNullorEmpty(model.disCount2.ToString()) == "0" ? p_disCount1 : p_disCount1 - (decimal.Parse(AppCode.checkNullorEmpty(model.disCount2.ToString())));
                decimal p_disCount3 = AppCode.checkNullorEmpty(model.disCount3.ToString()) == "0" ? p_disCount2 : p_disCount2 - (decimal.Parse(AppCode.checkNullorEmpty(model.disCount3.ToString())));
                decimal p_PromotionCost = AppCode.checkNullorEmpty(model.specialDisc.ToString()) == "0" && AppCode.checkNullorEmpty(model.specialDiscBaht.ToString()) == "0" || p_disCount3 == 0 ? p_disCount3 : (p_disCount3 - (p_disCount3 * (decimal.Parse(model.specialDisc.ToString()) / 100))) - decimal.Parse(AppCode.checkNullorEmpty(model.specialDiscBaht.ToString()));

                decimal getPackProduct = QueryGetAllProduct.getProductById(model.productId).FirstOrDefault().unit / QueryGetAllProduct.getProductById(model.productId).FirstOrDefault().pack;
                decimal sNormal = decimal.Parse(AppCode.checkNullorEmpty(saleNormal));/// getPackProduct;
                decimal p_normalGp = AppCode.checkNullorEmpty(saleNormal) == "0" ? 0 
                    : ((sNormal - ((p_disCount3 * decimal.Parse("1.07")) / getPackProduct)) / sNormal) * 100;
                p_normalGp = p_normalGp < 0 ? p_normalGp * -1 : p_normalGp;


                decimal pPromotion = decimal.Parse(AppCode.checkNullorEmpty(saleIn));/// getPackProduct;
                decimal p_PromotionGp = AppCode.checkNullorEmpty(saleIn) == "0" ? 0 
                    : ((pPromotion - ((p_PromotionCost * decimal.Parse("1.07")) / getPackProduct))  / pPromotion) * 100;
                p_PromotionGp = p_PromotionGp > 0 ? p_PromotionGp : p_PromotionGp * -1;


                Activity_Model activityModel = new Activity_Model();
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.productcostdetaillist1
                    .Where(r => r.productGroupId != null && r.productGroupId.Equals(model.productGroupId))
                    .Select(r =>
                    {
                        r.wholeSalesPrice = decimal.Parse(AppCode.checkNullorEmpty(wholeSalesPrice));
                        r.disCount1 = decimal.Parse(AppCode.checkNullorEmpty(model.disCount1.ToString()));
                        r.disCount2 = decimal.Parse(AppCode.checkNullorEmpty(model.disCount2.ToString()));
                        r.disCount3 = decimal.Parse(AppCode.checkNullorEmpty(model.disCount3.ToString()));
                        r.saleNormal = decimal.Parse(AppCode.checkNullorEmpty(saleNormal));
                        r.saleIn = decimal.Parse(AppCode.checkNullorEmpty(saleIn));
                        r.normalGp = Math.Round(p_normalGp, 3);
                        r.promotionGp = Math.Round(p_PromotionGp, 3);
                        r.specialDisc = decimal.Parse(AppCode.checkNullorEmpty(model.specialDisc.ToString()));
                        r.specialDiscBaht = decimal.Parse(AppCode.checkNullorEmpty(model.specialDiscBaht.ToString()));
                        r.normalCost = decimal.Parse(AppCode.checkNullorEmpty(normalCost)) == 0 ? p_disCount3 : decimal.Parse(AppCode.checkNullorEmpty(normalCost));
                        r.promotionCost = AppCode.checkNullorEmpty(promotionCost) == "0" ? Math.Round(p_PromotionCost, 3) : decimal.Parse(AppCode.checkNullorEmpty(promotionCost)) ;
                        return r;
                    }).ToList();
                Session["productcostdetaillist1"] = activityModel.productcostdetaillist1;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }
    }
}