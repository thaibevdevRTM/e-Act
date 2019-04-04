using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using WebLibrary;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class actFormRepDetailController : Controller
    {
        // GET: actFormRepDetail
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

        public ActionResult searchActForm()
        {
            try
            {
                RepDetailModel.actFormRepDetails model = new RepDetailModel.actFormRepDetails();
                model.actFormRepDetailLists = RepDetailAppCode.getRepDetailReportByCreateDateAndStatusId(AppCode.ApproveStatus.อนุมัติ
                    , Request.Form["startDate"]
                    , Request.Form["endDate"]);
                TempData["ActFormRepDetail"] = model;
            }catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("repListView");
        }

        public ActionResult repListView()
        {
            RepDetailModel.actFormRepDetails model = null;
            try
            {
                model = (RepDetailModel.actFormRepDetails)TempData["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult repReportDetailExcel(string gridHtml)
        {

            AppCode.genPdfFile(gridHtml, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), "pdfRepDetail");
            return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "actFormReportDetail.xls");
        }
    }
}