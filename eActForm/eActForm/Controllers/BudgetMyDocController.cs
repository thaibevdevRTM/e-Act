using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eActForm.BusinessLayer;
using eActForm.Models;
using System.Web.Mvc;
using WebLibrary;
using System.Configuration;
using iTextSharp.text;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using WebLibrary;
//using eActForm.BusinessLayer;
//using eActForm.Models;
//using System.Configuration;


namespace eActForm.Controllers
{
	[LoginExpire]
	public class BudgetMyDocController : Controller
    {
        // GET: BudgetMyDoc
        
		public ActionResult Index()
		{
			SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
			return View(models);
		}

		public ActionResult myDocBudgetList(string actId)
		{
			var result = new AjaxResult();
			ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
			return PartialView(models);
		}

		public ActionResult searchBudgetForm()
		{
			string count = Request.Form.AllKeys.Count().ToString();
			Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
			model = new Budget_Approve_Detail_Model.budgetForms();

			if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
			{
				model.budgetFormLists = getBudgetListsByEmpId(null, null);
			}
			else
			{
				string companyEN = "MT";
				if (UtilsAppCode.Session.User.empCompanyId == "5601") { companyEN = "OMT"; } ;

				model.budgetFormLists = getBudgetListsByEmpId(UtilsAppCode.Session.User.empId, companyEN);
			}

			if (Request.Form["txtActivityNo"] != "")
			{
				model.budgetFormLists = model.budgetFormLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
			}

			if (Request.Form["ddlStatus"] != "" )
			{
				model.budgetFormLists = model.budgetFormLists.Where(r => r.statusId == Request.Form["ddlStatus"]).ToList();
			}

			if (Request.Form["ddlCustomer"] != "")
			{
				model.budgetFormLists = model.budgetFormLists.Where(r => r.customerId == Request.Form["ddlCustomer"]).ToList();
			}

			if (Request.Form["ddlTheme"] != "")
			{
				model.budgetFormLists = model.budgetFormLists.Where(r => r.themeId == Request.Form["ddlTheme"]).ToList();
			}


			TempData["SearchDataModelBudget"] = model.budgetFormLists;
			return RedirectToAction("myDocBudget");
		}

		
		public ActionResult myDocBudget(string companyEN) 
		{
			Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
			model = new Budget_Approve_Detail_Model.budgetForms();

			if (TempData["SearchDataModelBudget"] != null)
			{
				model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["SearchDataModelBudget"];
			}
			else
			{
				if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
				{
					model.budgetFormLists = getBudgetListsByEmpId(null, companyEN);
				} else {
					model.budgetFormLists = getBudgetListsByEmpId(UtilsAppCode.Session.User.empId, companyEN);
				}
			}
			return PartialView(model);
		}

		public static List<Budget_Approve_Detail_Model.budgetForm> getBudgetListsByEmpId(string empId , string companyEN)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFormByEmpId"
					, new SqlParameter[] {
					new SqlParameter("@empId", empId),
					new SqlParameter("@companyEN", companyEN)
					});
				var lists = (from DataRow dr in ds.Tables[0].Rows
							 select new Budget_Approve_Detail_Model.budgetForm()
							 {
								 statusId = dr["statusId"].ToString(),
								 statusName = dr["statusName"].ToString(),
								 activityNo = dr["activityNo"].ToString(),

								 budgetApproveId = dr["budgetApproveId"].ToString(),
								 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],

								 reference = dr["reference"].ToString(),
								 customerId = dr["customerId"].ToString(),
								 channelName = dr["channelName"].ToString(),
								 productTypeId = dr["productTypeId"].ToString(),
								 productTypeNameEN = dr["productTypeNameEN"].ToString(),

								 cusShortName = dr["cusShortName"].ToString(),
								 cusNameTH = dr["cusNameTH"].ToString(),
								 productCategory = dr["productCateText"].ToString(),
								 productGroup = dr["productGroupId"].ToString(),
								 productGroupName = dr["productGroupName"].ToString(),

								 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
								 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
								 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
								 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
								 activityName = dr["activityName"].ToString(),

								 themeId = dr["themeId"].ToString(),
								 theme = dr["theme"].ToString(),
								 objective = dr["objective"].ToString(),
								 trade = dr["trade"].ToString(),
								 activityDetail = dr["activityDetail"].ToString(),

								 budgetActivityId = dr["budgetActivityId"].ToString(),
								 approveId = dr["approveId"].ToString(),

								 createdDate = (DateTime?)dr["createdDate"],
								 createdByUserId = dr["createdByUserId"].ToString(),
								 updatedDate = (DateTime?)dr["updatedDate"],
								 updatedByUserId = dr["updatedByUserId"].ToString(),

								 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
								 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
								 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
								 totalInvoiceApproveBath = dr["totalInvoiceApproveBath"] is DBNull ? 0 : (decimal?)dr["totalInvoiceApproveBath"]

							 }).ToList();
				return lists;
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getApproveListsByStatusId >> " + ex.Message);
				return new List<Budget_Approve_Detail_Model.budgetForm>();
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public JsonResult genPdfApprove(string GridHtml, string statusId, string budgetApproveId)
		{
			var resultAjax = new AjaxResult();
			try
			{

				var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId);
				GridHtml = GridHtml.Replace("<br>", "<br/>");
				AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

				//TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
				//getImageModel.tbActImageList = ImageAppCode.GetImage(budgetApproveId).Where(x => x.extension == ".pdf").ToList();
				//string[] pathFile = new string[getImageModel.tbActImageList.Count + 1];
				//pathFile[0] = Server.MapPath(rootPathInsert);


				//if (getImageModel.tbActImageList.Any())
				//{
				//	int i = 1;
				//	foreach (var item in getImageModel.tbActImageList)
				//	{
				//		pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
				//		i++;
				//	}
				//}

				//var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId));
				//var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile);
				resultAjax.Success = true;
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("genPdfApprove >> " + ex.Message);
				resultAjax.Success = false;
				resultAjax.Message = ex.Message;
			}
			return Json(resultAjax, "text/plain");
		}

	}
}