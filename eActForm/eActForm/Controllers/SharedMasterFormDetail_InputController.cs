using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Presenter.MasterData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using static eForms.Models.MasterData.ImportBudgetControlModel;

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
                for (int i = 0; i < 3; i++)
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
                for (int i = 0; i < 6; i++)
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
                activity_TBMMKT_Model.activityOfEstimateList = estimateList.Where(x => x.activityTypeId.Equals("1")).OrderBy(x => x.rowNo).ToList();
                if (!activity_TBMMKT_Model.activityOfEstimateList.Any())
                {
                    activity_TBMMKT_Model.activityOfEstimateList.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
                    activity_TBMMKT_Model.activityFormModel.mode = AppCode.Mode.addNew.ToString();
                }

                activity_TBMMKT_Model.activityOfEstimateList2 = estimateList.Where(x => x.activityTypeId.Equals("2")).OrderBy(x => x.rowNo).ToList();
                if (!activity_TBMMKT_Model.activityOfEstimateList2.Any())
                {
                    activity_TBMMKT_Model.activityOfEstimateList2.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
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

        public JsonResult getBudgetByEO(string listEO, string companyId, string subjectId, string channelId, string brandId, string activityId)
        {
            var result = new AjaxResult();
            try
            {


                List<BudgetTotal> budgetTotalsList = new List<BudgetTotal>();
                var getListEO = JsonConvert.DeserializeObject<List<CostThemeDetailOfGroupByPriceTBMMKT>>(listEO);
                var groupEO = getListEO.Where(x => !string.IsNullOrEmpty(x.EO)).GroupBy(x => x.EO).Select((group, index) => new BudgetTotal
                {
                    EO = group.First().EO,
                    total = group.Sum(c => c.total),
                }).ToList();

                //var getListEOIO = JsonConvert.DeserializeObject<List<CostThemeDetailOfGroupByPriceTBMMKT>>(listEO);
                //var groupEOIO = getListEOIO.Where(x => !string.IsNullOrEmpty(x.EO)).GroupBy(x => new { x.EO, x.IO }).Select((group, index) => new BudgetTotal
                //{
                //    EO = group.First().EO,
                //    IO = group.First().IO,
                //}).ToList();



                result.Success = false;

                var getTxtActGroup = !string.IsNullOrEmpty(subjectId) ? QueryGetSubject.getAllSubject().Where(x => x.id.Equals(subjectId)).FirstOrDefault().description : "";
                var getActTypeId = !string.IsNullOrEmpty(getTxtActGroup) ? BusinessLayer.QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.Equals(getTxtActGroup)).FirstOrDefault().id : "";

                decimal? sumTotal_Input = 0, amountBalanceTotal = 0, useAmountTotal = 0, totalBudgetChannel = 0, sumReturn = 0;



                List<BudgetTotal> returnAmountList = new List<BudgetTotal>();
                if (groupEO.Any())
                {
                    foreach (var item in groupEO)
                    {
                        BudgetTotal returnAmountModel = new BudgetTotal();
                        var getAmountReturnEOIO = ActFormAppCode.getAmountReturn(item.EO, channelId, brandId);
                        if (getAmountReturnEOIO.Any())
                        {
                            returnAmountModel.EO = item.EO;
                            sumReturn = getAmountReturnEOIO.FirstOrDefault().returnAmount;
                            returnAmountModel.returnAmountBrand = getAmountReturnEOIO.FirstOrDefault().returnAmountBrand;
                            returnAmountList.Add(returnAmountModel);
                        }

                    }
                }


                foreach (var item in groupEO)
                {
                    BudgetTotal budgetTotalModel = new BudgetTotal();
                    var getAmount = ActFormAppCode.getBalanceByEO(item.EO, companyId, getActTypeId, channelId, brandId, activityId);
                    if (getAmount.Any())
                    {

                        var returnAmount = returnAmountList.Where(a => a.EO == item.EO).ToList();
                        budgetTotalModel.returnAmountBrand = returnAmount.Any() ? Convert.ToDecimal(returnAmount.FirstOrDefault().returnAmountBrand) : 0;

                        budgetTotalModel.EO = item.EO;
                        budgetTotalModel.useAmount = getAmount.FirstOrDefault().balance + item.total;
                        budgetTotalModel.totalBudget = getAmount.FirstOrDefault().amountTotal;
                        budgetTotalModel.amount = getAmount.FirstOrDefault().amount;
                        budgetTotalModel.amountBalance = (getAmount.FirstOrDefault().amount - getAmount.FirstOrDefault().balance) - item.total + budgetTotalModel.returnAmountBrand;
                        budgetTotalModel.amountBalancePercen = (getAmount.FirstOrDefault().balance + item.total) / getAmount.FirstOrDefault().amount * 100;
                        budgetTotalModel.brandId = brandId;
                        budgetTotalModel.brandName = QueryGetAllBrand.GetAllBrand().Where(x => x.digit_EO.Contains(item.EO.Substring(0, 4))).FirstOrDefault().brandName;
                        budgetTotalModel.channelName = !string.IsNullOrEmpty(channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(channelId)).FirstOrDefault().no_tbmmkt : "";
                        budgetTotalModel.activityType = !string.IsNullOrEmpty(getTxtActGroup) ? BusinessLayer.QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.Equals(getTxtActGroup)).FirstOrDefault().activitySales : "";
                        budgetTotalsList.Add(budgetTotalModel);

                        totalBudgetChannel = getAmount.FirstOrDefault().amountTotal;
                        useAmountTotal = getAmount.FirstOrDefault().balanceTotal;
                        sumTotal_Input += item.total;

                    }


                }
                amountBalanceTotal = totalBudgetChannel - useAmountTotal - sumTotal_Input;
                useAmountTotal = useAmountTotal + sumTotal_Input;



                Activity_TBMMKT_Model model = new Activity_TBMMKT_Model();
                model.budgetTotalList = budgetTotalsList;
                model.budgetTotalModel.totalBudgetChannel = totalBudgetChannel;
                model.budgetTotalModel.useAmountTotal = useAmountTotal;
                model.budgetTotalModel.amountBalanceTotal = amountBalanceTotal + sumReturn;
                model.budgetTotalModel.returnAmount = sumReturn;
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
                ExceptionManager.WriteError("getBudgetByEO => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showDetailBudgetControl(string activityId)
        {

            Activity_TBMMKT_Model model = TempData["showBudget" + activityId] == null ? new Activity_TBMMKT_Model() : (Activity_TBMMKT_Model)TempData["showBudget" + activityId];
            TempData.Keep();
            return PartialView(model);
        }

    }
}