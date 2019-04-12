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
                normalCost = normalCost.Replace(",", "");
                wholeSalesPrice = wholeSalesPrice.Replace(",", "");
                saleOut = saleOut.Replace(",", "");
                saleIn = saleIn.Replace(",", "");
                normalGP = normalGP == null ? "" : normalGP.Replace(",", "");
                promotionGP = promotionGP == null ? "" : promotionGP.Replace(",", "");
                decimal p_wholeSalesPrice = AppCode.checkNullorEmpty(wholeSalesPrice) == "0" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(wholeSalesPrice));
                decimal p_disCount1 = AppCode.checkNullorEmpty(disCount1) == "0" ? p_wholeSalesPrice : p_wholeSalesPrice - ((decimal.Parse(AppCode.checkNullorEmpty(disCount1)) / 100) * p_wholeSalesPrice);
                decimal p_disCount2 = AppCode.checkNullorEmpty(disCount2) == "0" ? p_disCount1 : p_disCount1 - ((decimal.Parse(AppCode.checkNullorEmpty(disCount2)) / 100) * p_disCount1);
                decimal p_disCount3 = AppCode.checkNullorEmpty(disCount3) == "0" ? p_disCount2 : p_disCount2 - ((decimal.Parse(AppCode.checkNullorEmpty(disCount3)) / 100) * p_disCount2);

                decimal getPackProduct = QueryGetAllProduct.getProductById(productId).FirstOrDefault().pack;

                decimal p_normalGp = AppCode.checkNullorEmpty(saleOut) == "0" ? 0 : ((decimal.Parse(saleOut) - (p_disCount3 * decimal.Parse("1.07")))
                    / getPackProduct / decimal.Parse(saleOut)) * 100;

                decimal p_PromotionCost = AppCode.checkNullorEmpty(specialDisc) == "0" && AppCode.checkNullorEmpty(specialDiscBaht) == "0" || p_disCount3 == 0 ? p_disCount3 : (p_disCount3 - (p_disCount3 * (decimal.Parse(specialDisc) / 100))) - decimal.Parse(AppCode.checkNullorEmpty(specialDiscBaht));

                decimal p_PromotionGp = AppCode.checkNullorEmpty(saleIn) == "0" ? 0 : ((decimal.Parse(saleIn) - (p_PromotionCost * decimal.Parse("1.07")))
                  / getPackProduct / decimal.Parse(AppCode.checkNullorEmpty(saleIn))) * 100;


                Activity_Model activityModel = new Activity_Model();
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.productcostdetaillist1
                    .Where(r => r.id != null && r.id.Equals(id))
                    .Select(r =>
                    {
                        r.wholeSalesPrice = decimal.Parse(AppCode.checkNullorEmpty(wholeSalesPrice));
                        r.disCount1 = decimal.Parse(AppCode.checkNullorEmpty(disCount1));
                        r.disCount2 = decimal.Parse(AppCode.checkNullorEmpty(disCount2));
                        r.disCount3 = decimal.Parse(AppCode.checkNullorEmpty(disCount3));
                        r.saleOut = decimal.Parse(AppCode.checkNullorEmpty(saleOut));
                        r.saleIn = decimal.Parse(AppCode.checkNullorEmpty(saleIn));
                        r.normalGp = p_normalGp;
                        r.promotionGp = p_PromotionGp;
                        r.specialDisc = decimal.Parse(AppCode.checkNullorEmpty(specialDisc));
                        r.specialDiscBaht = decimal.Parse(AppCode.checkNullorEmpty(specialDiscBaht));
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

        public ActionResult showDetailGroup(string rowId)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
            activityModel.productcostdetaillist1 = activityModel.productcostdetaillist1.Where(x => x.id == rowId).ToList();

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
                    var list = activityModel.productcostdetaillist1.Single(r => r.id == rowid);
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
                activityModel.productcostdetaillist1.AddRange(productlist.productcostdetaillist1);
              
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
                    costthememodel.size = item.size;
                    costthememodel.smellName = item.smellName;
                    costthememodel.smellId = item.smellId;
                    costthememodel.brandId = item.brandId;
                    costthememodel.isShowGroup = item.isShowGroup;
                    costthememodel.detailGroup = item.detailGroup;
                    activityModel.activitydetaillist.Add(costthememodel);
                    i++;
                }

                Session["productcostdetaillist1"] = activityModel.productcostdetaillist1;
                Session["activitydetaillist"] = activityModel.activitydetaillist;
                //var resultData = new
                //{
                //    checkrow = productlist.productcostdetaillist1.Count,
                //};
                result.Data = productlist.productcostdetaillist1.Count;

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