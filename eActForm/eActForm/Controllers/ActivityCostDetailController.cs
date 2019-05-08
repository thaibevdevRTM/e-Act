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
    public class ActivityCostDetailController : Controller
    {
        public ActionResult activityCostDetail()
        {
            Activity_Model activityModel = new Activity_Model();
            if (Session["activitydetaillist"] != null)
            {
                activityModel.activitydetaillist = ((List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]);
            }
            else
            {
                activityModel.activitydetaillist = new List<CostThemeDetailOfGroupByPrice>();
                Session["activitydetaillist"] = activityModel.activitydetaillist;
            }

            return PartialView(activityModel);
        }

        public JsonResult delActCostDetail(string rowid, Activity_Model activityModel)
        {
            var result = new AjaxResult();
            try
            {
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];
                if (rowid != null)
                {
                    var list = activityModel.activitydetaillist.Single(r => r.productGroupId == rowid);
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

        public JsonResult addCostDetailTheme(string themeId, string txttheme)
        {
            var result = new AjaxResult();
            try
            {
                Activity_Model activityModel = new Activity_Model();
                CostThemeDetailOfGroupByPrice costThemeDetailOfGroupByPriceModel = new CostThemeDetailOfGroupByPrice();
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];



                ProductCostOfGroupByPrice productcostdetail = new ProductCostOfGroupByPrice();
                costThemeDetailOfGroupByPriceModel.productGroupId = Guid.NewGuid().ToString();
                costThemeDetailOfGroupByPriceModel.activityTypeId = themeId;
                costThemeDetailOfGroupByPriceModel.isShowGroup = false;

                productcostdetail.id = Guid.NewGuid().ToString();
                productcostdetail.typeTheme = txttheme;

                costThemeDetailOfGroupByPriceModel.detailGroup = new List<ProductCostOfGroupByPrice>();
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

        public JsonResult calCostDetailTotal(string productGroupId, string productId, string total, string perTotal, string themeCost)
        {
            var result = new AjaxResult();
            try
            {
                decimal p_total = decimal.Parse(total);
                decimal p_perTotal = 0;
                decimal getPromotionCost = 0;

                Activity_Model activityModel = new Activity_Model();
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];

                if (activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any() && activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any())
                {
                    getPromotionCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).Any() ?
                        activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).FirstOrDefault().promotionCost.ToString() : "0"));
                    getPromotionCost = getPromotionCost == 0 ? 1 : getPromotionCost;
                    p_perTotal = (p_total / (decimal.Parse(themeCost) * getPromotionCost)) * 100; // % ยอดขายโปโมชั่น
                }


                activityModel.activitydetaillist
                           .Where(r => r.productGroupId != null && r.productGroupId.Equals(productGroupId))
                           .Select(r =>
                           {
                               r.total = Math.Round(p_total, 2);
                               r.perTotal = Math.Round(p_perTotal, 2);
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

        public JsonResult calCostPerTotal(string productGroupId, string perTotal)
        {
            var result = new AjaxResult();
            try
            {

                decimal p_perTotal = decimal.Parse(perTotal);

                Activity_Model activityModel = new Activity_Model();
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];

                activityModel.activitydetaillist
                           .Where(r => r.productGroupId != null && r.productGroupId.Equals(productGroupId))
                           .Select(r =>
                           {
                               r.perTotal = Math.Round(p_perTotal, 2);
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

        public JsonResult calCostDetailTheme(string productGroupId, string productId, string name, string normalCost, string themeCost, string growth, string unit ,string compensate ,string LE )
        {
            var result = new AjaxResult();

            try
            {
                Activity_Model activityModel = new Activity_Model();

                decimal getPromotionCost = 0;
                decimal get_PerTotal = 0;
                decimal p_total = 0;
                decimal p_LE = decimal.Parse(AppCode.checkNullorEmpty(LE));

                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];
                if (AppCode.checkNullorEmpty(themeCost) != "0" || AppCode.checkNullorEmpty(unit) != "0" || AppCode.checkNullorEmpty(compensate) != "0" && activityModel.productcostdetaillist1 != null)
                {
                    if (activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any() && activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any())
                    {
                        if (p_LE == 0)
                        {
                            p_total = decimal.Parse(themeCost) * decimal.Parse(unit) * decimal.Parse(compensate);
                        }
                        else
                        {
                            p_total = decimal.Parse(themeCost) * decimal.Parse(unit) * decimal.Parse(compensate) * (p_LE / 100);
                        }


                        getPromotionCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).Any() ?
                        activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).FirstOrDefault().promotionCost.ToString() : "0"));
                        getPromotionCost = getPromotionCost == 0 ? 1 : getPromotionCost;
                        get_PerTotal = p_total == 0 ? 0 : (p_total / (decimal.Parse(themeCost) * getPromotionCost)) * 100; // % ยอดขายโปโมชั่น

                    }

                }

                decimal p_growth = normalCost == "0" ? 0 : (decimal.Parse(themeCost) - decimal.Parse(normalCost)) / decimal.Parse(AppCode.checkNullorEmpty(normalCost) == "0" ? "1" : normalCost) * 100;


                activityModel.activitydetaillist
                        .Where(r => r.productGroupId != null && r.productGroupId.Equals(productGroupId))
                        .Select(r =>
                        {
                            r.detailGroup[0].productName = name;
                            r.normalCost = decimal.Parse(normalCost);
                            r.growth = Math.Round(p_growth, 2);
                            r.themeCost = decimal.Parse(themeCost);
                            r.total = Math.Round(p_total, 2);
                            r.perTotal = Math.Round(get_PerTotal, 2);
                            r.unit = int.Parse(unit);
                            r.compensate = decimal.Parse(compensate);
                            r.LE = decimal.Parse(LE);
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
    }
}