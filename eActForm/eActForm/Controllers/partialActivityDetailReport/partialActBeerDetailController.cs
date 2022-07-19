using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public partial class partialActivityDetailReportController
    {
        // GET: partialActBeerDetail
        public ActionResult partialActBeerDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.activityFormTBMMKT.txtPay = QueryOtherMaster.getOhterMaster("pay", "").Where(x => x.val1.Equals(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.payNo)).FirstOrDefault().displayVal;
            activity_TBMMKT_Model.activityFormTBMMKT.txtGame = QueryOtherMaster.getOhterMaster("game", "").Where(x => x.val1.Equals(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.glNo)).FirstOrDefault().displayVal;
            activity_TBMMKT_Model.activityFormTBMMKT.txtArea = QueryOtherMaster.getOhterMaster("area", "").Where(x => x.val1.Equals(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.toAddress)).FirstOrDefault().displayVal;
            activity_TBMMKT_Model.activityFormTBMMKT.chanel = QueryGetAllChanel.getAllChanel().Where(x => x.no_tbmmkt.Equals(ConfigurationManager.AppSettings["conditionActBeer"])).Where(x => x.id.Equals(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId)).FirstOrDefault().chanelGroup;

            return PartialView(activity_TBMMKT_Model);
        }
    }
}