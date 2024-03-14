using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Presenter.MasterData;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using static eForms.Models.MasterData.ImportBudgetControlModel;
using QueryGetAllActivityGroup = eActForm.BusinessLayer.QueryGetAllActivityGroup;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormDetail_InputController : Controller
    {
        public ActionResult textDetailsPay(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult listDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult activityBudgetDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            string yearFrom = "";
            string yearTo = "";
            string nowPhysicalYear = "";

            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formBgTbmId"])
            {
                //เพิ่มปีงบเพื่อ Budget Control dev date 20200708 Peerapop
                //ตั้งไว้ดึงจากปีงบปัจจุบันบวกไปอีก 10ปีหากต้องการเพิ่มหรือจัดการปีทำที่ตาราง TB_Act_master_period_year
                //update 2020-10-01 เพิ่มให้ดึงปีย้อนหลังด้วย 10 ปี peerapop
                nowPhysicalYear = FiscalYearPresenter.getFiscalNow(AppCode.StrCon, ConfigurationManager.AppSettings["typePeriodTBVGroup"]).FirstOrDefault().UseYear;
                yearFrom = (Convert.ToInt32(nowPhysicalYear) - 10).ToString();
                yearTo = (Convert.ToInt32(nowPhysicalYear) + 10).ToString();
            }
            activity_TBMMKT_Model.listFiscalYearModel = FiscalYearPresenter.getFiscalYearByYear(AppCode.StrCon, yearFrom, yearTo).OrderBy(m => m.UseYear).ToList();
            activity_TBMMKT_Model.activityTypeList = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition == "actTbm").ToList();

            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult textDetailsAttachPay(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult textDetailsCreateByV1(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult textDetailsBlankRows(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult showSignatureV1(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult listDetailsPosPremium(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            Decimal? totalCostThisActivity = 0;
            foreach (var item in activity_TBMMKT_Model.activityOfEstimateList)
            {
                #region formPosTbm
                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPosTbmId"]
                    || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formReturnPosTbm"])//ใบเบิกผลิตภัณฑ์,POS/PREMIUM
                {
                    totalCostThisActivity += item.unit;
                }
                else
                {
                    totalCostThisActivity += item.total;
                }
                #endregion
            }
            activity_TBMMKT_Model.totalCostThisActivity = totalCostThisActivity;
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult requestEmp(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            //string cultureLocal = Request.Cookies[ConfigurationManager.AppSettings["nameCookieLanguageEact"]].Value.ToString();
            //string en = ConfigurationManager.AppSettings["cultureEng"];

            activity_TBMMKT_Model.masterRequestEmp = QueryGet_empByComp.getEmpByComp(activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId,
            activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng).ToList();

            if (activity_TBMMKT_Model.requestEmpModel.Count == 0)
            {
                List<RequestEmpModel> RequestEmp = new List<RequestEmpModel>();
                for (int i = 0; i < 4; i++)
                {
                    RequestEmp.Add(new RequestEmpModel() { id = "", empId = "", empName = "", position = "", bu = "" });
                }
                activity_TBMMKT_Model.requestEmpModel = RequestEmp;
            }
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult purposeDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {


            if (activity_TBMMKT_Model.purposeModel.Count == 0)
            {
                activity_TBMMKT_Model.purposeModel = QueryGet_master_purpose.getAllPurpose().ToList();
            }
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult placeDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            if (activity_TBMMKT_Model.placeDetailModel.Count == 0)
            {
                List<PlaceDetailModel> placeDetailModel = new List<PlaceDetailModel>();
                for (int i = 0; i < 4; i++)
                {
                    placeDetailModel.Add(new PlaceDetailModel() { place = "", forProject = "", period = "", departureDate = null, arrivalDate = null });
                }
                activity_TBMMKT_Model.placeDetailModel = placeDetailModel;
            }
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult expensesDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            if (activity_TBMMKT_Model.expensesDetailModel == null || activity_TBMMKT_Model.expensesDetailModel.costDetailLists == null || !activity_TBMMKT_Model.expensesDetailModel.costDetailLists.Any())
            {
                CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT
                {
                    costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
                };
                for (int i = 0; i < 4; i++)
                {
                    model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { productDetail = "", QtyName = "", unitPrice = 0, typeTheme = "", unit = 0, total = 0 });
                }
                activity_TBMMKT_Model.expensesDetailModel = model;
            }
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult expensesDetailRows(CostDetailOfGroupPriceTBMMKT model, bool isNew)
        {
            if (isNew)
            {
                model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
            }
            return PartialView(model);
        }

        public ActionResult benefitDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult remarksDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult exPerryCashDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }


        public ActionResult exPerryEmpInfoDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult exPerryListDetail(Activity_TBMMKT_Model activity_TBMMKT_Model, string actId)
        {
            try
            {
                var estimateList = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormModel.id);
                activity_TBMMKT_Model.activityOfEstimateList2 = estimateList.Where(x => x.activityTypeId.Equals("1")).OrderBy(x => x.rowNo).ToList();
                if (!activity_TBMMKT_Model.activityOfEstimateList2.Any())
                {
                    activity_TBMMKT_Model.activityOfEstimateList2.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
                    activity_TBMMKT_Model.activityFormModel.mode = AppCode.Mode.addNew.ToString();
                }

                activity_TBMMKT_Model.activityOfEstimateList3 = estimateList.Where(x => x.activityTypeId.Equals("2")).OrderBy(x => x.rowNo).ToList();
                if (!activity_TBMMKT_Model.activityOfEstimateList3.Any())
                {
                    activity_TBMMKT_Model.activityOfEstimateList3.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
                    activity_TBMMKT_Model.activityFormModel.mode = AppCode.Mode.addNew.ToString();
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("exPerryListDetail => " + ex.Message);
            }
            return PartialView(activity_TBMMKT_Model);
        }


        public ActionResult exTravelDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            try
            {
                activity_TBMMKT_Model.list_0 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "travelling").OrderBy(x => x.name).OrderByDescending(x => x.name).ToList();
                activity_TBMMKT_Model.tB_Act_CountryList = expensesEntertainAppCode.getCountry();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("exTravelDetail");
            }
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult travellingDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.placeDetailModel.Count == 0)
            {
                List<PlaceDetailModel> placeDetailModel = new List<PlaceDetailModel>();
                for (int i = 0; i < 8; i++)
                {
                    placeDetailModel.Add(new PlaceDetailModel() { place = "", forProject = "", period = "", departureDate = null, arrivalDate = null });
                }
                activity_TBMMKT_Model.placeDetailModel = placeDetailModel;
            }
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult requestEmp2(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.requestEmpModel.Count == 0)
            {
                List<RequestEmpModel> RequestEmp = new List<RequestEmpModel>();
                for (int i = 0; i < 5; i++)
                {
                    RequestEmp.Add(new RequestEmpModel() { empId = "", empName = "", detail = "" });
                }
                activity_TBMMKT_Model.requestEmpModel = RequestEmp;
            }
            return PartialView(activity_TBMMKT_Model);
        }

        public JsonResult getBudgetByEO(string listEO, string companyId, string subjectId, string channelId, string brandId, string activityId, string status)
        {
            var result = new AjaxResult();
            try
            {

                Activity_TBMMKT_Model model = new Activity_TBMMKT_Model();
                List<BudgetTotal> budgetTotalsList = new List<BudgetTotal>();
                List<BudgetTotal> budgetTotalActTypeList = new List<BudgetTotal>();
                var getListEO = JsonConvert.DeserializeObject<List<CostThemeDetailOfGroupByPriceTBMMKT>>(listEO);
                var groupEO = getListEO.Where(x => !string.IsNullOrEmpty(x.EO)).GroupBy(x => new { x.EO, x.UseYearSelect }).Select((group, index) => new BudgetTotal
                {
                    EO = group.First().EO,
                    fiscalYear = group.First().UseYearSelect,
                    total = group.Sum(c => c.total),
                }).ToList();


                var getTxtActGroup = !string.IsNullOrEmpty(subjectId) ? QueryGetSubject.getAllSubject().Where(x => x.id.Equals(subjectId)).FirstOrDefault().description : "";
                var getActTypeId = !string.IsNullOrEmpty(getTxtActGroup) ? BusinessLayer.QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.Equals(getTxtActGroup)).FirstOrDefault().id : "";

                decimal? sumTotal_Input = 0, amountBalanceTotal = 0, useAmountTotal = 0, totalBudgetChannel = 0, sumReturn = 0;


                if (status == "2" || status == "3")
                {
                    //ดึงยอด ก่อนส่่งอนุมัติมาแสดง
                    var getAmount = QueryGetBudgetActivity.getBudgetAmountList(activityId);
                    foreach (var item in getAmount.Where(x => x.typeShowBudget == AppCode.typeShowBudget.subMain.ToString()))
                    {

                        BudgetTotal budgetTotalModel = new BudgetTotal();
                        budgetTotalModel.returnAmountBrand = item.returnAmount;
                        budgetTotalModel.EO = item.EO;
                        budgetTotalModel.useAmount = item.useAmount;
                        budgetTotalModel.totalBudget = item.budgetTotal;
                        budgetTotalModel.amount = item.budgetTotal;
                        budgetTotalModel.amountBalance = item.amountBalance;
                        budgetTotalModel.activityType = item.activityType;
                        var amount = item.budgetTotal > 0 ? item.budgetTotal : 1;
                        budgetTotalModel.amountBalancePercen = (item.useAmount / amount) * 100;
                        budgetTotalModel.brandId = brandId;
                        // budgetTotalModel.amountBalanceTotal = (getAmount.FirstOrDefault().totalBudgetChannel - getAmount.FirstOrDefault().balanceTotal) - item.total;
                        budgetTotalModel.brandName = item.brandName;
                        budgetTotalModel.channelName = !string.IsNullOrEmpty(channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(channelId)).FirstOrDefault().no_tbmmkt : "";
                        budgetTotalModel.yearBG = item.yearBG;
                        budgetTotalModel.typeShowBudget = item.typeShowBudget;
                        budgetTotalsList.Add(budgetTotalModel);

                    }

                    foreach (var item in getAmount.Where(x => x.typeShowBudget == AppCode.typeShowBudget.main.ToString()))
                    {
                        BudgetTotal budgetMainModel = new BudgetTotal();
                        budgetMainModel.totalBudget = item.budgetTotal;
                        budgetMainModel.totalBudgetChannel = item.budgetTotal;
                        budgetMainModel.amountBalanceTotal = item.amountBalance;
                        budgetMainModel.useAmountTotal = item.useAmount;
                        budgetMainModel.returnAmount = item.returnAmount;
                        budgetMainModel.yearBG = item.yearBG;
                        budgetMainModel.brandId = brandId;
                        budgetMainModel.brandName = item.brandName;
                        budgetMainModel.channelName = !string.IsNullOrEmpty(channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(channelId)).FirstOrDefault().no_tbmmkt : "";
                        budgetMainModel.typeShowBudget = AppCode.typeShowBudget.main.ToString();
                        model.budgetMainTotalList.Add(budgetMainModel);
                    }


                }
                else
                {

                    List<BudgetTotal> returnAmountList = new List<BudgetTotal>();
                    if (groupEO.Any())
                    {
                        foreach (var item in groupEO)
                        {
                            BudgetTotal returnAmountModel = new BudgetTotal();
                            var getAmountReturnEOIO = ActFormAppCode.getAmountReturn(item.EO, channelId, brandId, getActTypeId, item.fiscalYear);
                            if (getAmountReturnEOIO.Any())
                            {
                                returnAmountModel.EO = item.EO;
                                returnAmountModel.returnAmountBrand = getAmountReturnEOIO.FirstOrDefault().returnAmountBrand;
                                returnAmountModel.fiscalYear = item.fiscalYear;
                                returnAmountModel.returnAmount = getAmountReturnEOIO.FirstOrDefault().returnAmount;
                                returnAmountList.Add(returnAmountModel);
                            }
                        }
                    }

                    //budget submain
                    foreach (var item in groupEO)
                    {
                        BudgetTotal budgetTotalModel = new BudgetTotal();
                        BudgetTotal budgetMainModel = new BudgetTotal();
                        var getAmount = ActFormAppCode.getBalanceByEO(item.EO, companyId, getActTypeId, channelId, brandId, activityId, item.fiscalYear);
                        string[] stockArray = ConfigurationManager.AppSettings["Instock"].ToLower().Split(',');

                        var getSum = getListEO.Where(x => x.EO == item.EO && x.UseYearSelect == item.fiscalYear && !stockArray.Contains(x.IO.ToLower())
                       ).GroupBy(x => new { x.EO, x.UseYearSelect }).Select((group, index) => new BudgetTotal
                       {
                           total = group.Sum(c => c.total),
                       }).ToList();

                        if (getAmount.Any())
                        {

                            var returnAmount = returnAmountList.Where(a => a.EO == item.EO && a.fiscalYear == item.fiscalYear).ToList();
                            budgetTotalModel.returnAmountBrand = returnAmount.Any() ? returnAmount.FirstOrDefault().returnAmountBrand : 0;
                            if (status == "2" || status == "3")
                            {
                                item.total = 0;
                            }
                            else
                            {
                                item.total = getSum.Any() ? getSum.FirstOrDefault().total : 0;
                            }

                            budgetTotalModel.EO = item.EO;
                            budgetTotalModel.useAmount = getAmount.FirstOrDefault().balance + item.total;
                            budgetTotalModel.amount = getAmount.FirstOrDefault().amount;
                            budgetTotalModel.amountBalance = getAmount.FirstOrDefault().amount - getAmount.FirstOrDefault().balance - item.total + budgetTotalModel.returnAmountBrand;

                            var amount = getAmount.FirstOrDefault().amount > 0 ? getAmount.FirstOrDefault().amount : 1;
                            budgetTotalModel.amountBalancePercen = ((getAmount.FirstOrDefault().balance + item.total) / amount) * 100;
                            budgetTotalModel.brandId = brandId;
                            budgetTotalModel.brandName = QueryGetAllBrand.GetAllBrand().Where(x => x.digit_EO.Contains(item.EO.Substring(0, 4))).FirstOrDefault().brandName;
                            budgetTotalModel.channelName = !string.IsNullOrEmpty(channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(channelId)).FirstOrDefault().no_tbmmkt : "";
                            budgetTotalModel.activityType = !string.IsNullOrEmpty(getTxtActGroup) ? BusinessLayer.QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.Equals(getTxtActGroup)).FirstOrDefault().activitySales : "";
                            budgetTotalModel.fiscalYear = item.fiscalYear;
                            budgetTotalModel.yearBG = getAmount.FirstOrDefault().year;
                            budgetTotalModel.useAmountTotal = item.total;
                            budgetTotalModel.typeShowBudget = AppCode.typeShowBudget.subMain.ToString();
                            budgetTotalsList.Add(budgetTotalModel);

                        }
                    }

                    var groupYear = getListEO.Where(x => !string.IsNullOrEmpty(x.EO)).GroupBy(x => new { x.UseYearSelect }).Select((group, index) => new BudgetTotal
                    {
                        fiscalYear = group.First().UseYearSelect,
                    }).OrderBy(x => x.fiscalYear).ToList();


                    // List Budget Main
                    foreach (var item in groupYear)
                    {
                        //get sum input
                        var budgetTotal = budgetTotalsList.Where(x => x.fiscalYear == item.fiscalYear).ToList();
                        //get budgetTotal
                        var getAmount = ActFormAppCode.getBalanceByEO(groupEO.FirstOrDefault().EO, companyId, getActTypeId, channelId, brandId, activityId, item.fiscalYear).AsEnumerable();
                        // get return
                        if (getAmount.Any())
                        {
                            var returnAmount = returnAmountList.Where(a => a.fiscalYear == item.fiscalYear).FirstOrDefault().returnAmount;
                            var totalBudget = getAmount.FirstOrDefault().amountTotal;
                            var balanceTotal = getAmount.FirstOrDefault().balanceTotal;
                            var sumTotal = budgetTotal.Select(c => c.useAmountTotal).Sum();

                            BudgetTotal budgetMainModel = new BudgetTotal();
                            budgetMainModel.totalBudget = totalBudget;
                            budgetMainModel.totalBudgetChannel = totalBudget;
                            budgetMainModel.amountBalanceTotal = totalBudget - balanceTotal - sumTotal + returnAmount;
                            budgetMainModel.useAmountTotal = balanceTotal + sumTotal;
                            budgetMainModel.returnAmount = returnAmount;
                            budgetMainModel.yearBG = getAmount.FirstOrDefault().year;
                            budgetMainModel.brandId = brandId;
                            budgetMainModel.brandName = !string.IsNullOrEmpty(brandId) ? QueryGetAllBrand.GetAllBrand().Where(x => x.id == brandId).FirstOrDefault().brandName : "";
                            budgetMainModel.channelName = !string.IsNullOrEmpty(channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(channelId)).FirstOrDefault().no_tbmmkt : "";
                            budgetMainModel.typeShowBudget = AppCode.typeShowBudget.main.ToString();
                            model.budgetMainTotalList.Add(budgetMainModel);
                        }
                    }
                }

                model.budgetTotalList = budgetTotalsList;

                TempData["showBudget" + activityId] = model;

                var resultData = new
                {
                    budgetTotalsList = budgetTotalsList,
                    activityTypeId = getActTypeId,

                };
                result.Data = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                ExceptionManager.WriteError("getBudgetByEO => " + activityId + "____" + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showDetailBudgetControl(string activityId)
        {

            Activity_TBMMKT_Model model = TempData["showBudget" + activityId] == null ? new Activity_TBMMKT_Model() : (Activity_TBMMKT_Model)TempData["showBudget" + activityId];


            return PartialView(model);
        }

        public JsonResult callUnitPOS(string productId, string activityNo)
        {
            var result = new AjaxResult();
            try
            {

                var getUnit = ActFormAppCode.callUnitPOSAppCode(productId, activityNo);

                var resultData = new
                {
                    unit = getUnit.FirstOrDefault().unit,
                    unitReturn = getUnit.FirstOrDefault().unitReturn,
                };
                result.Data = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                ExceptionManager.WriteError("callUnitPOS => " + activityNo + "____" + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ckeditor_Input(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult ckeditor4_Input(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            return PartialView(activity_TBMMKT_Model);
        }

    }
}