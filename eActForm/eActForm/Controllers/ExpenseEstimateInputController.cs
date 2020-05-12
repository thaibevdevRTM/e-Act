using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ExpenseEstimateInputController : Controller
    {
        public ActionResult expensesTrvDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };
            if (activity_TBMMKT_Model.expensesDetailModel == null || activity_TBMMKT_Model.expensesDetailModel.costDetailLists == null || !activity_TBMMKT_Model.expensesDetailModel.costDetailLists.Any())
            {
                List<TB_Act_master_list_choiceModel> lst = new List<TB_Act_master_list_choiceModel>();
                lst = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "expensesTrv").OrderBy(x => x.orderNum).ToList();

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
                        displayType = lst[i].displayType,
                        subDisplayType = lst[i].subDisplayType,
                        glCode = "",
                    });
                }


            }
            else
            {
                //edit
                model.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, "expensesTrv");
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
     

    }
}