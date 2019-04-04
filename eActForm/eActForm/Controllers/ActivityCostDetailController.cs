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

        public JsonResult addCostDetailTheme(string themeId, string txttheme)
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

        public JsonResult calCostDetailTheme(string id, string productId, string name, string normalCost, string themeCost, string growth)
        {
            var result = new AjaxResult();

            try
            {
                Activity_Model activityModel = new Activity_Model();
                decimal p_total = 0;
                decimal getPromotionCost = 0;
                decimal getNormalCost = 0;
                decimal get_PerTotal = 0;
                activityModel.productcostdetaillist1 = ((List<ProductCostOfGroupByPrice>)Session["productcostdetaillist1"]);
                activityModel.activitydetaillist = (List<CostThemeDetailOfGroupByPrice>)Session["activitydetaillist"];
                if (AppCode.checkNullorEmpty(themeCost) != "0")
                {
                    getNormalCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productId == productId).FirstOrDefault().normalCost.ToString()));
                    getPromotionCost = decimal.Parse(AppCode.checkNullorEmpty(activityModel.productcostdetaillist1.Where(x => x.productId == productId).FirstOrDefault().promotionCost.ToString()));
                    p_total = (getNormalCost - getPromotionCost) * decimal.Parse(themeCost);

                    //get_PerTotal = p_total * 100 / (decimal.Parse(normalCost) * getPromotionCost); //ยอดขายปกติ
                    get_PerTotal = p_total * 100 / (decimal.Parse(themeCost) * getPromotionCost);// % ยอดขายโปโมชั่น
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
                            r.perTotal = get_PerTotal;
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