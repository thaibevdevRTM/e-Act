
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using WebLibrary;
using eActForm.BusinessLayer.QueryHandler;

namespace eActForm.Controllers
{
	[LoginExpire]
	public class BudgetReportController : Controller
    {
        // GET: BudgetReport
        public ActionResult RptBudgetActivityIndex()
        {
			SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
			return View(models);
        }

		public ActionResult searchRptBudgetActivity()
		{
			Budget_Report_Model.Report_Budget_Activity model = new Budget_Report_Model.Report_Budget_Activity();
			try
			{
				string act_createdDateStart = null ;
				string act_createdDateEnd = null;
				string act_formType = null;

				//RepDetailModel.actFormRepDetails model = new RepDetailModel.actFormRepDetails();
				//model = RepDetailAppCode.getRepDetailReportByCreateDateAndStatusId(Request.Form["startDate"], Request.Form["endDate"]);

				if (Request.Form["chk_all"] != null && Request.Form["chk_all"] == "true")
				{
					act_createdDateStart = null;
					act_createdDateEnd = null;
				}
				else
				{
					act_createdDateStart = Request.Form["startDate"]; 
					act_createdDateEnd = Request.Form["endDate"];
				}

				if (Request.Form["ddlFormType"] != "" && Request.Form["ddlFormType"] != "Select All")
				{
					act_formType = Request.Form["ddlFormType"];
				}


					//	#region filter
					//	if (Request.Form["ddlFormType"] != "")
					//	{
					//		model = RepDetailAppCode.getFilterRepDetailByStatusId(model, Request.Form["ddlStatus"]);
					//	}
					//	if (Request.Form["ddlCustomer"] != "")
					//	{
					//		model = RepDetailAppCode.getFilterRepDetailByCustomer(model, Request.Form["ddlCustomer"]);
					//	}
					//	if (Request.Form["ddlTheme"] != "")
					//	{
					//		model = RepDetailAppCode.getFilterRepDetailByActivity(model, Request.Form["ddlTheme"]);
					//	}
					//	if (Request.Form["ddlProductType"] != "")
					//	{
					//		model = RepDetailAppCode.getFilterRepDetailByProductType(model, Request.Form["ddlProductType"]);
					//	}
					//	if (Request.Form["ddlProductGrp"] != "")
					//	{
					//		model = RepDetailAppCode.getFilterRepDetailByProductGroup(model, Request.Form["ddlProductGrp"]);
					//	}

					//	#endregion
					//}

					model.Report_Budget_Activity_List = QueryGetBudgetReport.getReportBudgetActivity(null, null, act_formType, act_createdDateStart, act_createdDateEnd);

			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("searchRptBudgetActivity => " + ex.Message);
			}
			return PartialView(model);
		}
	


		[HttpPost]
		[ValidateInput(false)]
		public FileResult viewExportExcel(string gridHtml)
		{
			try
			{
				//RepDetailModel.actFormRepDetails model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
				//gridHtml = gridHtml.Replace("\n", "<br>");
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError(ex.Message);
			}

			string createDate = "";
			createDate = DateTime.Today.ToString("yyyyMMdd");
			return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "BudgetReport_" + createDate + ".xls");
		}




	} //end public class
} // end namespace