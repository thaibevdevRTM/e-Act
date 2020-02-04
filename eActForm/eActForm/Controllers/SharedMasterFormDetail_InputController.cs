﻿using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult requestEmp(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            List<RequestEmpModel> RequestEmp = new List<RequestEmpModel>();
            for (int i = 0; i < 5; i++)
            {
                RequestEmp.Add(new RequestEmpModel() { id = "", empId = "", empName = "", position = "", bu = "" });
            }
            activity_TBMMKT_Model.requestEmpModel = RequestEmp;
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult purposeDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult placeDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            List<PlaceDetailModel> placeDetailModel = new List<PlaceDetailModel>();
            for (int i = 0; i < 3; i++)
            {
                placeDetailModel.Add(new PlaceDetailModel() { place = "", forProject = "", period = "", departureDate = null, arrivalDate = null });
            }
            activity_TBMMKT_Model.placeDetailModel = placeDetailModel;

            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult expensesDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if(activity_TBMMKT_Model.expensesDetailModel == null || activity_TBMMKT_Model.expensesDetailModel.costDetailLists == null)
            {
                CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT {
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
        
    }
}