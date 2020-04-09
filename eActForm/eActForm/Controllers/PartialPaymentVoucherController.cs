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

            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])//���觨��� dev date 20200408 Peerapop
            {
                //��������Default���֧2�ա�͹ (�ջѨ�غѹ�Ѻ��͹��ѧ1��) �������Ҥ����á��Default �է�����ҳ�Ѩ�غѹ���
                //���˵ط���ͧ������͡��͹��ѧ��1�� ���Сó� ��͹ �.�. �ͧ�ء���Ҩ���ա�÷Ө��¢ͧ�է�����ҳ��͹˹���� ���ǹ������ռšѺ��ô֧ EO ��������͡㹡�÷���觨���
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