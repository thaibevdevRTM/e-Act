﻿using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public partial class ExpenseEstimateInputController
    {
        // GET: partialCostDetail
        public ActionResult partialCostList(string actId)
        {
            Activity_TBMMKT_Model activityModel = TempData["actForm" + actId] == null ? new Activity_TBMMKT_Model() : (Activity_TBMMKT_Model)TempData["actForm" + actId];
            TempData.Keep();
            return PartialView(activityModel);
        }


        public JsonResult addCostDetailTheme(string themeId, string txttheme, string actId, string brandId)
        {
            var result = new AjaxResult();
            try
            {
                Activity_TBMMKT_Model activityModel = TempData["actForm" + actId] == null ? new Activity_TBMMKT_Model() : (Activity_TBMMKT_Model)TempData["actForm" + actId];

                CostThemeDetailOfGroupByPriceTBMMKT costThemeDetailOfGroupByPriceModel = new CostThemeDetailOfGroupByPriceTBMMKT();
                ProductCostOfGroupByPrice productcostdetail = new ProductCostOfGroupByPrice();
                costThemeDetailOfGroupByPriceModel.productGroupId = Guid.NewGuid().ToString();
                costThemeDetailOfGroupByPriceModel.activityTypeId = themeId;
                costThemeDetailOfGroupByPriceModel.isShowGroup = false;

                productcostdetail.id = Guid.NewGuid().ToString();
                productcostdetail.typeTheme = txttheme;
                costThemeDetailOfGroupByPriceModel.detailGroup = new List<ProductCostOfGroupByPrice>();
                costThemeDetailOfGroupByPriceModel.detailGroup.Add(productcostdetail);
                //costThemeDetailOfGroupByPriceModel.IO = "56SO" + DateTime.Now.Year.ToString().Substring(2) + ActFormAppCode.getDigitGroup(themeId);
                activityModel.activityOfEstimateList.Add(costThemeDetailOfGroupByPriceModel);
                TempData["actForm" + actId] = activityModel;
                TempData.Keep();

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ExpenseEstimateInput >> addCostDetailTheme >> " + ex.Message);
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult calActivityDetailCost(string name, string mechanics, string productGroupId, string productId, string normalCase, string promotionCase, string unit, string compensate, string LE, string totalCase, string typeForm, string actId)
        {
            var result = new AjaxResult();

            try
            {
                Activity_TBMMKT_Model activityModel = TempData["actForm" + actId] == null ? new Activity_TBMMKT_Model() : (Activity_TBMMKT_Model)TempData["actForm" + actId];

                decimal getPromotionCost = 0;
                decimal getNormalCost = 0;
                decimal get_PerTotal = 0;
                decimal p_total = 0;
                decimal p_LE = decimal.Parse(AppCode.checkNullorEmpty(LE));
                decimal p_totalCase = decimal.Parse(AppCode.checkNullorEmpty(totalCase));
                decimal p_growth = 0;

                if (activityModel.productcostdetaillist1 != null)
                {
                    if (activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any() && activityModel.productcostdetaillist1.Where(x => x.productId == productId).Any())
                    {
                        // cal normal spending
                        getNormalCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).Any() ?
                            activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).FirstOrDefault().normalCost.ToString() : "0"));

                        getPromotionCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).Any() ?
                            activityModel.productcostdetaillist1.Where(x => x.productGroupId == productGroupId).FirstOrDefault().promotionCost.ToString() : "0"));

                        if (typeForm == Activity_Model.activityType.OMT.ToString())
                        {
                            p_total = (getNormalCost) * decimal.Parse(promotionCase);
                        }
                        else
                        {
                            p_total = (getNormalCost - getPromotionCost) * decimal.Parse(promotionCase);
                        }

                        p_growth = AppCode.checkNullorEmpty(normalCase) == "0" ? 0 : (decimal.Parse(promotionCase) - decimal.Parse(normalCase)) / decimal.Parse(AppCode.checkNullorEmpty(normalCase) == "0" ? "1" : normalCase) * 100;
                    }
                }

                if (decimal.Parse(unit) != 0 && decimal.Parse(compensate) != 0)
                {
                    p_total = decimal.Parse(promotionCase) * decimal.Parse(unit) * decimal.Parse(compensate);
                    p_total = (p_LE > 0) ? p_total * (p_LE / 100) : p_total;
                }

                getPromotionCost = getPromotionCost == 0 ? 1 : getPromotionCost;
                getNormalCost = getNormalCost == 0 ? 1 : getNormalCost;
                get_PerTotal = p_total == 0 ? 0 : (p_total / (decimal.Parse(promotionCase) * getNormalCost)) * 100; // % ยอดขายโปโมชั่น


                if (p_totalCase > 0)
                {
                    p_total = p_totalCase * getNormalCost;
                }

                activityModel.activityOfEstimateList
                        .Where(r => r.productGroupId != null && r.productGroupId.Equals(productGroupId))
                        .Select(r =>
                        {
                            r.productName = name;
                            r.mechanics = mechanics;
                            //r.detailGroup[0].productName = name;
                            r.normalCost = decimal.Parse(normalCase);
                            r.growth = Math.Round(p_growth, 2);
                            r.themeCost = decimal.Parse(promotionCase);
                            r.total = Math.Round(p_total, 2);
                            r.totalCase = Math.Round(p_totalCase, 2);
                            r.perTotal = Math.Round(get_PerTotal, 2);
                            r.unit = int.Parse(unit);
                            r.compensate = decimal.Parse(compensate);
                            r.LE = decimal.Parse(LE);
                            return r;
                        }).ToList();

                TempData["actForm" + actId] = activityModel;
                TempData.Keep();
                result.Success = true;

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ExpenseEstimateInput >> calActivityDetailCost >> " + ex.Message);
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult delActCostDetail(string rowid, string actId)
        {
            var result = new AjaxResult();
            try
            {
                Activity_TBMMKT_Model activityModel = (Activity_TBMMKT_Model)TempData["actForm" + actId];
                if (rowid != null)
                {
                    activityModel.activityOfEstimateList.RemoveAll(r => r.productGroupId == rowid);
                }
                else
                {
                    activityModel.activitydetaillist = new List<CostThemeDetailOfGroupByPrice>();
                    activityModel.productcostdetaillist1 = new List<ProductCostOfGroupByPrice>();
                }
                TempData["actForm" + actId] = activityModel;
                TempData.Keep();

                result.Success = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ExpenseEstimateInput >> delActCostDetail >> " + ex.Message);
                result.Message = ex.Message;
                result.Success = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}