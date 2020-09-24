using eActForm.BusinessLayer;
using eActForm.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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

            CostDetailOfGroupPriceTBMMKT modelSub = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };


            AppCode.Expenses expenseEnum = new AppCode.Expenses(activity_TBMMKT_Model.approveFlowDetail.Count > 0 ? activity_TBMMKT_Model.approveFlowDetail[0].empId : activity_TBMMKT_Model.activityFormModel.createdByUserId);
            modelHistory.costDetailLists = QueryGetActivityEstimateByActivityId.getHistoryByActivityId(activity_TBMMKT_Model.activityFormModel.id);
            model2.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, QueryGetGL.getGLTypeByEmpGroupName(expenseEnum.groupName));
            modelSub.costDetailLists = QueryGetActivityEstimateByActivityId.getEstimateSub(activity_TBMMKT_Model.activityFormModel.id, expenseEnum.hotelExpense);

            modelSub.costDetailLists = modelSub.costDetailLists.Where(x => x.unitPrice > 0).ToList();



            decimal? total = 0;
            for (int i = 0; i < 8; i++)
            {
                if (model2.costDetailLists[i].total != 0)
                {
                    if (modelHistory.costDetailLists.Count > 0)
                    {
                        total = modelHistory.costDetailLists.Where(x => x.listChoiceId == model2.costDetailLists[i].listChoiceId).FirstOrDefault().total;
                    }
                    else
                    {
                        total = model2.costDetailLists[i].total;
                    }

                    if (model2.costDetailLists[i].listChoiceId == expenseEnum.hotelExpense && model2.costDetailLists[i].unit != 0 && model2.costDetailLists[i].unitPrice == 0)
                    {
                        string multiPrice = "";

                        foreach (var item in modelSub.costDetailLists)
                        {
                            multiPrice = multiPrice + item.unitPriceDisplayReport + "|";
                        }
                        multiPrice = multiPrice.Substring(0, (multiPrice.Length - 1));
                        //เป็นค่าที่พักหลายราคา
                        modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                        {
                            listChoiceId = model2.costDetailLists[i].listChoiceId,
                            listChoiceName = model2.costDetailLists[i].listChoiceName,
                            productDetail = model2.costDetailLists[i].productDetail,
                            unit = model2.costDetailLists[i].unit,
                            unitPrice = 0,
                            unitPriceDisplay = multiPrice,
                            total = model2.costDetailLists[i].total,
                            displayType = model2.costDetailLists[i].displayType,
                            subDisplayType = model2.costDetailLists[i].subDisplayType,
                            updatedByUserId = model2.costDetailLists[i].updatedByUserId,
                            createdByUserId = model2.costDetailLists[i].createdByUserId,
                            statusEdit = model2.costDetailLists[i].createdByUserId == "" ? "" :
                                       (model2.costDetailLists[i].createdByUserId != model2.costDetailLists[i].updatedByUserId
                                       && model2.costDetailLists[i].total != total ? "*" : ""),
                        });
                    }
                    else
                    {
                        modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                        {
                            listChoiceId = model2.costDetailLists[i].listChoiceId,
                            listChoiceName = model2.costDetailLists[i].listChoiceName,
                            productDetail = model2.costDetailLists[i].productDetail,
                            unit = model2.costDetailLists[i].unit,
                            unitPrice = model2.costDetailLists[i].unitPrice + model2.costDetailLists[i].vat,
                            unitPriceDisplay = "",
                            total = model2.costDetailLists[i].total,
                            displayType = model2.costDetailLists[i].displayType,
                            subDisplayType = model2.costDetailLists[i].subDisplayType,
                            updatedByUserId = model2.costDetailLists[i].updatedByUserId,
                            createdByUserId = model2.costDetailLists[i].createdByUserId,
                            statusEdit = model2.costDetailLists[i].createdByUserId == "" ? "" :
                          (model2.costDetailLists[i].createdByUserId != model2.costDetailLists[i].updatedByUserId
                          && model2.costDetailLists[i].total != total ? "*" : ""),
                        });
                    }

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

            model.costDetailLists = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormModel.id);

            model.costDetailLists[0].hospName = QueryGetAllHospital.getAllHospital().Where(x => x.id.Contains(model.costDetailLists[0].hospId)).FirstOrDefault().hospNameTH;

            activity_TBMMKT_Model.expensesDetailModel = model;

            return PartialView(activity_TBMMKT_Model);
        }

    }


}