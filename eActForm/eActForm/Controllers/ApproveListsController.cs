using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class ApproveListsController : Controller
    {
        // GET: ApproveLists
        public ActionResult Index()
        {
            SearchActivityModels models = new SearchActivityModels();
            models.approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app);
            models.productGroupList = QueryGetAllProductGroup.getAllProductGroup();
            models.customerslist = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameEN != "").ToList();
            models.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
            models.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                .GroupBy(item => item.activitySales)
                .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

            return View(models);
        }

        public ActionResult ListView()
        {
            Activity_Model.actForms model = new Activity_Model.actForms();
            if (TempData["ApproveSearchResult"] == null)
            {
                model = new Activity_Model.actForms();
                model.actLists = ApproveListAppCode.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
                TempData["ApproveFormLists"] = model.actLists;
                model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists,(int)AppCode.ApproveStatus.รออนุมัติ);
            }
            else
            {
                model.actLists = (List<Activity_Model.actForm>)TempData["ApproveSearchResult"];
            }
            return PartialView(model);
        }
        

        public ActionResult searchActForm()
        {
            string count = Request.Form.AllKeys.Count().ToString();
            Activity_Model.actForms model = new Activity_Model.actForms();
            model.actLists = (List<Activity_Model.actForm>)TempData["ApproveFormLists"];

            if (Request.Form["txtActivityNo"] != "")
            {
                model.actLists = model.actLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
            }

            if (Request.Form["ddlStatus"] != "")
            {
                model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists, int.Parse(Request.Form["ddlStatus"]));
            }
            TempData["ApproveSearchResult"] = model.actLists;
            return RedirectToAction("ListView");
        }
    }
}