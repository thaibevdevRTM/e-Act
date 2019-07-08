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
            Activity_Model activityModel = new Activity_Model
            {
                activitydetaillist = Session["activitydetaillist"] != null
                ? ((List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"])
                : new List<CostThemeDetailOfGroupByPrice>()
            };
            Session["activitydetaillist"] = activityModel.activitydetaillist;


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
                    activityModel.activitydetaillist.RemoveAll(r => r.productGroupId == rowid);
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
                Activity_Model activityModel = new Activity_Model
                {
                    activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]
                };
                CostThemeDetailOfGroupByPrice costThemeDetailOfGroupByPriceModel = new CostThemeDetailOfGroupByPrice();
                



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

        /// <summary>
        /// case Spending Change
        /// </summary>
        /// <param name="productGroupId"></param>
        /// <param name="productId"></param>
        /// <param name="total"></param>
        /// <param name="themeCost"></param>
        /// <returns></returns>
        public JsonResult calPercentSpendingOfSale(string productGroupId, string productId, string total, string themeCost)
        {
            var result = new AjaxResult();
            try
            {
                decimal p_total = decimal.Parse(total);
                decimal p_perTotal = 0;
                decimal getPromotionCost = 0;

                Activity_Model activityModel = new Activity_Model
                {
                    productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]),
                    activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]
                };

                if (activityModel.productcostdetaillist1 != null)
                {
                    if (activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any() && activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any())
                    {
                        getPromotionCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).Any() ?
                            activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).FirstOrDefault().promotionCost.ToString() : "0"));
                        getPromotionCost = getPromotionCost == 0 ? 1 : getPromotionCost;
                        try
                        {
                            p_perTotal = (p_total / (decimal.Parse(themeCost) * getPromotionCost)) * 100; // % ยอดขายโปโมชั่น
                        }catch{ }
                    }
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

        /// <summary>
        /// case %Spending of sale Change
        /// </summary>
        /// <param name="productGroupId"></param>
        /// <param name="perTotal"></param>
        /// <returns></returns>
        public JsonResult calSpendingOfSaleChange(string productGroupId, string perTotal)
        {
            var result = new AjaxResult();
            try
            {
                decimal p_perTotal = decimal.Parse(perTotal);
                Activity_Model activityModel = new Activity_Model
                {
                    activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]
                };
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

        /// <summary>
        /// case all input change
        /// </summary>
        /// <param name="name"></param>
        /// <param name="productGroupId"></param>
        /// <param name="productId"></param>
        /// <param name="normalCase"></param>
        /// <param name="promotionCase"></param>
        /// <param name="unit"></param>
        /// <param name="compensate"></param>
        /// <param name="LE"></param>
        /// <returns></returns>
        public JsonResult calActivityDetailCost(string name, string productGroupId, string productId, string normalCase, string promotionCase, string unit, string compensate, string LE)
        {
            var result = new AjaxResult();

            try
            {
                Activity_Model activityModel = new Activity_Model
                {
                    productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]),
                    activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"]
                };

                decimal getPromotionCost = 0; 
                decimal get_PerTotal = 0;
                decimal p_total = 0;
                decimal p_LE = decimal.Parse(AppCode.checkNullorEmpty(LE));

                if (activityModel.productcostdetaillist1 != null)
                {
                    if (activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any() && activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any())
                    {
                        // cal normal spendinf
                        decimal getNormalCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).Any() ?
                            activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).FirstOrDefault().normalCost.ToString() : "0"));

                        getPromotionCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).Any() ?
                            activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).FirstOrDefault().promotionCost.ToString() : "0"));

                        p_total = (getNormalCost - getPromotionCost) * decimal.Parse(promotionCase);
                    }
                }

                if (AppCode.checkNullorEmpty(unit) != "0"
                    && AppCode.checkNullorEmpty(compensate) != "0")
                {
                    p_total = decimal.Parse(promotionCase) * decimal.Parse(unit) * decimal.Parse(compensate);
                    p_total = (p_LE > 0) ? p_total * (p_LE / 100) : p_total;
                }
                decimal p_growth = normalCase == "0" ? 0 : (decimal.Parse(promotionCase) - decimal.Parse(normalCase)) / decimal.Parse(AppCode.checkNullorEmpty(normalCase) == "0" ? "1" : normalCase) * 100;
                getPromotionCost = getPromotionCost == 0 ? 1 : getPromotionCost;
                get_PerTotal = p_total == 0 ? 0 : (p_total / (decimal.Parse(promotionCase) * getPromotionCost)) * 100; // % ยอดขายโปโมชั่น
                


                activityModel.activitydetaillist
                        .Where(r => r.productGroupId != null && r.productGroupId.Equals(productGroupId))
                        .Select(r =>
                        {
                            r.productName = name;
                            //r.detailGroup[0].productName = name;
                            r.normalCost = decimal.Parse(normalCase);
                            r.growth = Math.Round(p_growth, 2);
                            r.themeCost = decimal.Parse(promotionCase);
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