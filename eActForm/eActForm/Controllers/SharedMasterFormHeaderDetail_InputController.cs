using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormHeaderDetail_InputController : Controller
    {
        public ActionResult dropdownCondtionTbmmkt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetailsDate(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            ////=================บังคับรูปแบบให้ใช้เป็น คศ. หน้า Input==============Peerapop dev date 20200310=======
            //DocumentsAppCode.setCulture(ConfigurationManager.AppSettings["cultureEng"]);
            ////===END==============บังคับรูปแบบให้ใช้เป็น คศ. หน้า Input==============Peerapop dev date 20200310====

            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            ////=================บังคับรูปแบบให้ใช้เป็น คศ. หน้า Input==============Peerapop dev date 20200310=======
            //DocumentsAppCode.setCulture(ConfigurationManager.AppSettings["cultureEng"]);
            ////===END==============บังคับรูปแบบให้ใช้เป็น คศ. หน้า Input==============Peerapop dev date 20200310====

            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetails_Pos_Premium(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPosTbmId"])//ใบเบิกผลิตภัณฑ์,POS/PREMIUM
            {
                activity_TBMMKT_Model.list_0 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "in_or_out_stock").OrderBy(x => x.name).ToList();
                activity_TBMMKT_Model.list_1 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "product_pos_premium");
                activity_TBMMKT_Model.list_2 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "for");
                activity_TBMMKT_Model.list_3 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "channel_place").OrderBy(x => x.name).ToList();
                activity_TBMMKT_Model.tB_Act_ProductBrand_Model_2 = QueryGetAllBrandByForm.GetAllBrand().Where(x => x.no_tbmmkt != "").ToList();
                activity_TBMMKT_Model.activityFormModel.documentDate = DateTime.Now;
            }
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerDetailsDate_dmy(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr) && (activity_TBMMKT_Model.activityFormModel.documentDate!= null))
            {
                activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr = activity_TBMMKT_Model.activityFormModel.documentDate?.ToString("dd-MM-yyyy");
            }

            if (activity_TBMMKT_Model.list_0 == null|| activity_TBMMKT_Model.list_0.Count == 0 )
            {

               // if (activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPosTbmId"])//ใบเบิกผลิตภัณฑ์,POS/PREMIUM
            
                activity_TBMMKT_Model.list_0 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "travelling").OrderBy(x => x.name).ToList();
            }
            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult headerViewManual(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr) && (activity_TBMMKT_Model.activityFormModel.documentDate != null))
            {
                activity_TBMMKT_Model.activityFormTBMMKT.documentDateStr = activity_TBMMKT_Model.activityFormModel.documentDate?.ToString("dd-MM-yyyy");
            }
            return PartialView(activity_TBMMKT_Model);
        }
        
    }
}