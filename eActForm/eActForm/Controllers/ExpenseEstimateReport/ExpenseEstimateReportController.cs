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
    public partial class ExpenseEstimateReportController : Controller
    {

        public ActionResult expensesTrvDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            CostDetailOfGroupPriceTBMMKT modelResult = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };

            CostDetailOfGroupPriceTBMMKT model2 = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };

            CostDetailOfGroupPriceTBMMKT modelHistory = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };

            modelHistory.costDetailLists = QueryGetActivityEstimateByActivityId.getHistoryByActivityId(activity_TBMMKT_Model.activityFormModel.id);
            model2.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, "expensesTrv");

            decimal? unitPrice = 0;
            for (int i = 0; i < 8; i++)
            {
                if (model2.costDetailLists[i].total != 0)
                {
                    if (modelHistory.costDetailLists.Count > 0)
                    {
                        unitPrice= modelHistory.costDetailLists.Where(x => x.listChoiceId == model2.costDetailLists[i].listChoiceId).FirstOrDefault().unitPrice;
                    }
                    else
                    {
                        unitPrice = model2.costDetailLists[i].unitPrice;
                    }

                    modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                    {
                        listChoiceId = model2.costDetailLists[i].listChoiceId,
                        listChoiceName = model2.costDetailLists[i].listChoiceName,
                        productDetail = model2.costDetailLists[i].productDetail,
                        unit = model2.costDetailLists[i].unit,
                        unitPrice = model2.costDetailLists[i].unitPrice+ model2.costDetailLists[i].vat,
                        total = model2.costDetailLists[i].total,
                        displayType = model2.costDetailLists[i].displayType,
                        subDisplayType = model2.costDetailLists[i].subDisplayType,
                        updatedByUserId = model2.costDetailLists[i].updatedByUserId,
                        createdByUserId = model2.costDetailLists[i].createdByUserId,
                        statusEdit = model2.costDetailLists[i].createdByUserId == "" ? "" :
                        (model2.costDetailLists[i].createdByUserId != model2.costDetailLists[i].updatedByUserId
                        && model2.costDetailLists[i].unitPrice != unitPrice ? "*" : ""),
                    }); ;
                }
            }

            int rowAdd = 8 - modelResult.costDetailLists.Count;
            for (int i = 0; i < rowAdd; i++)
            {

                modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                {
                    listChoiceId = "",
                    listChoiceName = "",
                    productDetail = "",
                    unit = 0,
                    unitPrice = 0,
                    total = 0,
                    displayType = "",
                    subDisplayType = "",
                    statusEdit = ""
                });

            }


            modelResult.costDetailLists = modelResult.costDetailLists.ToList();


            activity_TBMMKT_Model.expensesDetailModel = modelResult;


            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult expensesMedDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };
            //if (activity_TBMMKT_Model.expensesDetailModel == null || activity_TBMMKT_Model.expensesDetailModel.costDetailLists == null || !activity_TBMMKT_Model.expensesDetailModel.costDetailLists.Any())
            //{
                //List<TB_Act_master_list_choiceModel> lst = new List<TB_Act_master_list_choiceModel>();
                //lst = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "expensesTrv").OrderBy(x => x.orderNum).ToList();

                // listChoiceName,listChoiceId
            //    for (int i = 0; i < 5; i++)
            //    {
            //        model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
            //        {
            //            date = null,
            //            detail = "",
            //            hospId = "",
            //            glCode = "",
            //            unit = 0,
            //            unitPrice = 0,
            //            total = 0,

            //        });
            //    }
            //}
            //else
            //{ }
                //edit
                model.costDetailLists = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormModel.id);

                model.costDetailLists[0].hospName = QueryGetAllHospital.getAllHospital().Where(x => x.id.Contains(model.costDetailLists[0].hospId)).FirstOrDefault().hospNameTH;

           



            //int rowAdd = 5 - modelResult.costDetailLists.Count;
            //for (int i = 0; i < rowAdd; i++)
            //{
            //    modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
            //    {
            //        listChoiceId = "",
            //        listChoiceName = "",
            //        productDetail = "",
            //        unit = 0,
            //        unitPrice = 0,
            //        total = 0,
            //        displayType = "",
            //        subDisplayType = "",
            //        statusEdit = ""
            //    });

           // }
           // modelResult.costDetailLists = modelResult.costDetailLists.ToList();modelResult

            activity_TBMMKT_Model.expensesDetailModel = model;

            return PartialView(activity_TBMMKT_Model);
        }
        //public ActionResult testExpensesDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        //{
        //    return PartialView(activity_TBMMKT_Model);
        //}
    }


}