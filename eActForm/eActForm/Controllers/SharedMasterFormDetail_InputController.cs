using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

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
            activity_TBMMKT_Model.masterRequestEmp = QueryGet_empByComp.getEmpByComp(activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId).ToList();
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
            if (activity_TBMMKT_Model.expensesDetailModel == null || activity_TBMMKT_Model.expensesDetailModel.costDetailLists == null)
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
            activity_TBMMKT_Model.companyList = QueryGetAllCompany.getAllCompany();
            activity_TBMMKT_Model.objExpenseCashList = QueryOtherMaster.getOhterMaster("expenseCash", "");
            return PartialView(activity_TBMMKT_Model);
        }


        public ActionResult exPerryEmpInfoDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult exPerryListDetail(Activity_TBMMKT_Model activity_TBMMKT_Model,string actId)
        {
            try
            {
                activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT = QueryGetActivityEstimateByActivityId.getByActivityId(actId);
                if (!activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT.Any())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT.Add(new CostThemeDetailOfGroupByPriceTBMMKT());
                    }
                    activity_TBMMKT_Model.activityFormModel.mode = AppCode.Mode.addNew.ToString();
                }

                activity_TBMMKT_Model.exPerryCashList = exPerryCashAppCode.getCashPosition(UtilsAppCode.Session.User.empId);
                activity_TBMMKT_Model.exPerryCashModel.rulesCash = activity_TBMMKT_Model.exPerryCashList.Any() ? activity_TBMMKT_Model.exPerryCashList.Where(x => x.cashLimitId.Equals("87757B5B-C946-4001-A74B-AB6C9003AD25")).FirstOrDefault().cash : 0;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("exPerryListDetail => " + ex.Message);
            }
            return PartialView(activity_TBMMKT_Model);
        }





    }
}