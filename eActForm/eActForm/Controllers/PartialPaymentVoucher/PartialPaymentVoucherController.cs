using eActForm.BusinessLayer;
using eActForm.Models;
using eForms.Presenter.MasterData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class PartialPaymentVoucherController : Controller
    {
        CultureInfo DateThai = new CultureInfo("th-TH");
        CultureInfo DateEnglish = new CultureInfo("en-US");

        public ActionResult headerPv(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerPvDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult detailSectionOneToFive(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult detailSectionSix(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            ObjGetDataDetailPaymentAll objGetDataDetailPaymentAll = new ObjGetDataDetailPaymentAll();
            objGetDataDetailPaymentAll.activityId = activity_TBMMKT_Model.activityFormModel.id;
            objGetDataDetailPaymentAll.payNo = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.payNo;
            activity_TBMMKT_Model.listGetDataDetailPaymentAll = QueryGetSelectMainForm.GetDetailPaymentAll(objGetDataDetailPaymentAll);
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult inputPageHeaderDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            string yearFrom = "";
            string yearTo = "";
            string nowPhysicalYear = "";

            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])//ใบสั่งจ่าย dev date 20200408 Peerapop
            {
                //ฟอร์มนี้Defaultไว้ดึง2ปีก่อน (ปีปัจจุบันกับย้อนหลัง1ปี) ถ้าเข้ามาครั้งแรกจะDefault ปีงบประมาณปัจจุบันให้
                //สาเหตุที่ต้องให้เลือกย้อนหลังได้1ปี เพราะกรณี เดือน พ.ย. ของทุกปีอาจจะมีการทำจ่ายของปีงบประมาณก่อนหน้าได้ ในส่วนนี้จะไปมีผลกับการดึง EO มาให้เลือกในการทำสั่งจ่าย
                nowPhysicalYear = FiscalYearPresenter.getFiscalNow(AppCode.StrCon, ConfigurationManager.AppSettings["typePeriodTBVGroup"]).FirstOrDefault().UseYear;
                yearFrom = (Convert.ToInt32(nowPhysicalYear) - 1).ToString();
                yearTo = nowPhysicalYear;
                if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.fiscalYear == null)
                {
                    activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.fiscalYear = nowPhysicalYear;
                }
            }
            activity_TBMMKT_Model.listFiscalYearModel = FiscalYearPresenter.getFiscalYearByYear(AppCode.StrCon, yearFrom, yearTo).OrderByDescending(m => m.UseYear).ToList();

            if (activity_TBMMKT_Model.listGetDataEO == null)
            {
                List<GetDataEO> getDataEO = new List<GetDataEO>();
                activity_TBMMKT_Model.listGetDataEO = getDataEO;
            }



            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult inputPageSectionOne(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.listAPModel = APPresenter.getDataAP(AppCode.StrCon, "watchAllActive");
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult inputPageSectionTwo(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])//ใบสั่งจ่าย
            {
                activity_TBMMKT_Model.list_0 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "attachPV").OrderBy(x => x.orderNum).ToList();
            }
            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult inputPageSectionThreeToFive(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])//ใบสั่งจ่าย
            {
                activity_TBMMKT_Model.list_1 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(ConfigurationManager.AppSettings["formPosTbmId"], "channel_place").OrderBy(x => x.name).ToList();
                activity_TBMMKT_Model.tB_Act_ProductBrand_Model_2 = QueryGetAllBrandByForm.GetAllBrand().Where(x => x.no_tbmmkt != "").ToList();
            }
            if (activity_TBMMKT_Model.listGetDataIO == null)
            {
                List<GetDataIO> getDataIO = new List<GetDataIO>();
                activity_TBMMKT_Model.listGetDataIO = getDataIO;
            }

            return PartialView(activity_TBMMKT_Model);
        }

        public ActionResult inputPageSectionSix(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])//ใบสั่งจ่าย
            {
                activity_TBMMKT_Model.list_1 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "haveVat").OrderBy(x => x.orderNum).ToList();
            }
            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalnormalCostEstimate == null)
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalnormalCostEstimate = decimal.Parse("0.00");
            }
            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalvat == null)
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalvat = decimal.Parse("0.00");
            }
            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalnormalCostEstimateWithVat == null)
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalnormalCostEstimateWithVat = decimal.Parse("0.00");
            }
            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalallPayByIO == null)
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalallPayByIO = decimal.Parse("0.00");
            }
            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalallPayNo == null)
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalallPayNo = decimal.Parse("0.00");
            }
            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalallPayByIOBalance == null)
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.totalallPayByIOBalance = decimal.Parse("0.00");
            }
            if (activity_TBMMKT_Model.listGetDataPVPrevious == null)
            {
                List<GetDataPVPrevious> getDataPVPrevious = new List<GetDataPVPrevious>();
                if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.payNo != null)
                {
                    if (int.Parse(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.payNo) >= 2)
                    {
                        ObjGetDataPVPrevious objGetDataPVPrevious = new ObjGetDataPVPrevious();
                        objGetDataPVPrevious.master_type_form_id = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id;
                        objGetDataPVPrevious.payNo = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.payNo;
                        getDataPVPrevious = QueryGetSelectMainForm.GetQueryDataPVPrevious(objGetDataPVPrevious); ;
                    }
                }
                activity_TBMMKT_Model.listGetDataPVPrevious = getDataPVPrevious;
            }
            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.activityIdNoSub == null)
            {
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.activityIdNoSub = "";
            }

            return PartialView(activity_TBMMKT_Model);
        }

    }
}