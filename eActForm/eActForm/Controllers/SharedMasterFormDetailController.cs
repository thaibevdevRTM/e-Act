﻿using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using static eForms.Models.MasterData.ImportBudgetControlModel;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormDetailController : Controller
    {
        public ActionResult textDetailsPay(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult listDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
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
        public ActionResult requestToRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult requestEmpRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult purposeDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.purposeModel = activity_TBMMKT_Model.purposeModel.Where(x => x.chk == true).ToList();
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult placeDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult expensesDetailsRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult chargeToRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult benefitDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult remarksDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult lastWordRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult exPerryCashReport(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult showSignatureV2(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult activityBudgetDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult showSignatureDiv(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult travellingDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult confirmDirectorRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.approveModels = ApproveAppCode.getApproveByActFormId(activity_TBMMKT_Model.activityFormTBMMKT.id, "");
            //List<approveDetailModel> approveDetailLists = new List<approveDetailModel>();

            if (activity_TBMMKT_Model.approveModels.approveDetailLists.Count > 0)
            {
                activity_TBMMKT_Model.approveModels.approveDetailLists = activity_TBMMKT_Model.approveModels.approveDetailLists.Where(x => x.approveGroupId == AppCode.ApproveGroup.Director).ToList();
            }
            // int count  =   models.approveFlowDetail.Where(x => x.approveGroupId == AppCode.ApproveGroup.Director).Where(x=>x.statusId == "3").ToList().Count;
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult recordByHcRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            ApproveModel.approveModels models = new ApproveModel.approveModels();
            models = ApproveAppCode.getApproveByActFormId(activity_TBMMKT_Model.activityFormTBMMKT.id, "");
            //List<approveDetailModel> approveDetailLists = new List<approveDetailModel>();
            decimal? calSum1 = 0;
            decimal? calSum2 = 0;
            if (models.approveDetailLists.Count > 0)
            {
                models.approveDetailLists = models.approveDetailLists.Where(x => x.approveGroupId == AppCode.ApproveGroup.Recorder).Where(x => x.statusId == Convert.ToString((int)AppCode.ApproveStatus.อนุมัติ)).ToList();

                if (models.approveDetailLists.Count > 0)
                {//อนุมัติแล้ว
                    CostDetailOfGroupPriceTBMMKT model2 = new CostDetailOfGroupPriceTBMMKT
                    {
                        costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
                    };

                    model2.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, AppCode.GLType.GLSaleSupport);

                    for (int i = 0; i < model2.costDetailLists.Count; i++)
                    {
                        if (model2.costDetailLists[i].listChoiceId == AppCode.Expenses.Allowance)
                        {
                            calSum1 += model2.costDetailLists[i].total;
                        }
                        else
                        {
                            calSum2 += model2.costDetailLists[i].total;
                        }
                    }
                }
            }

            CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT
            {
                costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
            };
            model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { rowNo = 1, total = calSum1 });
            model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { rowNo = 2, total = calSum2 });

            activity_TBMMKT_Model.expensesDetailModel = model;

            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult attachfileDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            List<TB_Act_Image_Model.ImageModel> lists = ImageAppCode.GetImage(activity_TBMMKT_Model.activityFormTBMMKT.id);

            activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannelModel.txt = lists.Count.ToString();
            activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannelModel.val = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id;
            return PartialView(activity_TBMMKT_Model);
            // return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult textGeneral(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult recordByHcMedRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model = ReportAppCode.recordByHcRptAppCode(activity_TBMMKT_Model);
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult showDetailBudgetRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            return PartialView(activity_TBMMKT_Model);
        }
    }
}