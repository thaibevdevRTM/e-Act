using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public partial class ExpenseEstimateInputController : Controller
    {
        public ActionResult expensesTrvDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };
            CostDetailOfGroupPriceTBMMKT modelSub = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };

            if (activity_TBMMKT_Model.expensesDetailModel == null || activity_TBMMKT_Model.expensesDetailModel.costDetailLists == null || !activity_TBMMKT_Model.expensesDetailModel.costDetailLists.Any())
            {
                List<TB_Act_master_list_choiceModel> lst = new List<TB_Act_master_list_choiceModel>();

                lst = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, AppCode.GLType.GLSaleSupport).OrderBy(x => x.orderNum).ToList();

                // listChoiceName,listChoiceId
                for (int i = 0; i < lst.Count; i++)
                {
                    model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                    {
                        listChoiceId = lst[i].id,
                        listChoiceName = lst[i].name,
                        productDetail = "",
                        unit = 0,
                        unitPrice = 0,
                        total = 0,
                        vat = 0,
                        displayType = lst[i].displayType,
                        subDisplayType = lst[i].subDisplayType,
                        glCode = "",
                    });
                }

                #region "เพิ่มกรณ๊รายละเอียดของค่าที่พักราคาไม่เท่ากัน ไม่ให้เกิน 7 ราคา"


                if (activity_TBMMKT_Model.expensesDetailSubModel == null || activity_TBMMKT_Model.expensesDetailSubModel.costDetailLists == null || !activity_TBMMKT_Model.expensesDetailSubModel.costDetailLists.Any())
                {

                    for (int i = 0; i < 7; i++)
                    {
                        modelSub.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                        {
                            //------ hotelExpense[0] == Travelling Expense , hotelExpense[1] == ใบเบิกค่าใช้จ่าย พนักงาน
                            listChoiceId = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"] ? AppCode.Expenses.hotelExpense[0] : AppCode.Expenses.hotelExpense[1],
                            rowNo = i + 1,
                            unit = 0,
                            unitPrice = 0,
                            vat = 0,
                            total = 0,

                        });
                    }
                }
                #endregion
            }
            else
            {
                var gethotel = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"] ? AppCode.Expenses.hotelExpense[0] : AppCode.Expenses.hotelExpense[1];



                //edit
                model.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, AppCode.GLType.GLSaleSupport);
                modelSub.costDetailLists = QueryGetActivityEstimateByActivityId.getEstimateSub(activity_TBMMKT_Model.activityFormModel.id, gethotel);

            }

            activity_TBMMKT_Model.expensesDetailModel = model;
            activity_TBMMKT_Model.expensesDetailSubModel = modelSub;

            if (activity_TBMMKT_Model.list_0 == null || activity_TBMMKT_Model.list_0.Count == 0)
            {

                activity_TBMMKT_Model.list_0 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice("master", "trainClass").OrderBy(x => x.orderNum).ToList();

            }
            if (activity_TBMMKT_Model.list_1 == null || activity_TBMMKT_Model.list_1.Count == 0)
            {

                activity_TBMMKT_Model.list_1 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice("master", "airplaneClass").OrderBy(x => x.orderNum).ToList();

            }

            return PartialView(activity_TBMMKT_Model);
        }


        public ActionResult expensesMedDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };
            if (activity_TBMMKT_Model.expensesDetailModel == null || activity_TBMMKT_Model.expensesDetailModel.costDetailLists == null || !activity_TBMMKT_Model.expensesDetailModel.costDetailLists.Any())
            {
                for (int i = 0; i < 5; i++)
                {
                    model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                    {
                        date = null,
                        detail = "",
                        hospId = "",
                        glCode = "",
                        unit = 0,
                        unitPrice = 0,
                        total = 0,

                    });
                }
            }
            else
            {
                //edit
                model.costDetailLists = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormModel.id);

                model.costDetailLists[0].hospName = QueryGetAllHospital.getAllHospital().Where(x => x.id.Contains(model.costDetailLists[0].hospId)).FirstOrDefault().hospNameTH;

            }
            activity_TBMMKT_Model.expensesDetailModel = model;

            if (activity_TBMMKT_Model.list_0 == null || activity_TBMMKT_Model.list_0.Count == 0)
            {

                activity_TBMMKT_Model.list_0 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice("master", "trainClass").OrderBy(x => x.orderNum).ToList();

            }
            if (activity_TBMMKT_Model.list_1 == null || activity_TBMMKT_Model.list_1.Count == 0)
            {

                activity_TBMMKT_Model.list_1 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice("master", "airplaneClass").OrderBy(x => x.orderNum).ToList();

            }


            return PartialView(activity_TBMMKT_Model);
        }


        public ActionResult allowanceDetail(string st_date,string end_date ,string statusId, Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            TB_Act_Allowance_Model allowanceList = new TB_Act_Allowance_Model();
            DateTime? st_Date, end_Date;

            if (!activity_TBMMKT_Model.tB_Act_AllowanceList.Any())
            {
                st_Date = BaseAppCodes.converStrToDatetimeWithFormat(st_date, ConfigurationManager.AppSettings["formatDateUse"]);
                end_Date = BaseAppCodes.converStrToDatetimeWithFormat(end_date, ConfigurationManager.AppSettings["formatDateUse"]);

                if (st_Date != null && end_Date != null)
                {
                    var countDays = (end_Date - st_Date).Value.TotalDays + 1;
                   
                        for (int i = 0; i < countDays; i++)
                        {
                            TB_Act_Allowance_Model allowanceModel = new TB_Act_Allowance_Model();
                            var getDate = st_Date.Value.AddDays(i);
                            allowanceModel.date = getDate;

                            activity_TBMMKT_Model.tB_Act_AllowanceList.Add(allowanceModel);
                        }
                 
                }
            }

            return PartialView(activity_TBMMKT_Model);
        }

        public JsonResult getAllowanceOverDays(string countryId, string lvl ,string typeDay)
        {
            var result = new AjaxResult();
            try
            {
                var getCountryDetail = expensesEntertainAppCode.callGetAllowance(countryId, lvl, typeDay);
                
                result.Data = getCountryDetail;
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