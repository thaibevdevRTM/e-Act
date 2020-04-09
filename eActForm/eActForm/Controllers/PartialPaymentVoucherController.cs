using eActForm.Models;
using System.Web.Mvc;
using System.Configuration;
using eForms.Presenter.MasterData;
using System.Globalization;
using System;
using System.Linq;
using System.Collections.Generic;

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
                if (activity_TBMMKT_Model.activityFormTBMMKT.listFiscalYearModelSelect == null)
                {
                    activity_TBMMKT_Model.activityFormTBMMKT.listFiscalYearModelSelect = nowPhysicalYear;
                }
            }
            activity_TBMMKT_Model.listFiscalYearModel = FiscalYearPresenter.getFiscalYearByYear(AppCode.StrCon, yearFrom, yearTo).OrderByDescending(m => m.UseYear).ToList();

            if(activity_TBMMKT_Model.listGetDataEO == null)
            {
                List<GetDataEO> getDataEO = new List<GetDataEO>();
                activity_TBMMKT_Model.listGetDataEO = getDataEO;
                //activity_TBMMKT_Model.activityFormTBMMKT.list_1_multi_select = "";
            }
            


            return PartialView(activity_TBMMKT_Model);
        }
    }
}