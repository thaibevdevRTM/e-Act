using eActForm.BusinessLayer;
using eActForm.Models;
using System.Linq;
using System.Web.Mvc;

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
            var estimateList = activity_TBMMKT_Model.activityOfEstimateList;
            activity_TBMMKT_Model.activityOfEstimateList = estimateList.Where(x => x.activityTypeId == "1").ToList();
            activity_TBMMKT_Model.activityOfEstimateList2 = estimateList.Where(x => x.activityTypeId == "2").ToList();
            activity_TBMMKT_Model.masterRequestEmp = QueryGet_empDetailById.getEmpDetailById(activity_TBMMKT_Model.activityFormTBMMKT.empId);


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

            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult recordByHcRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            return PartialView(activity_TBMMKT_Model);
        }
        public ActionResult attachfileDetailRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            return PartialView(activity_TBMMKT_Model);
        }
    }
}