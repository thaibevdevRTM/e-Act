using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using WebLibrary;

namespace eActForm.Controllers
{
	[LoginExpire]
	public class BudgetController : Controller
	{

		public PartialViewResult previewBudgetInvoice(string activityId)
		{
			 Session["activityId"]= activityId;

			Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();
			Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(activityId,null);
			Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null).FirstOrDefault();
			return PartialView(Budget_Model);

			//budgetApproveId
			//activityId

		}


		public JsonResult submitInvoice(Budget_Activity_Model.Budget_Activity_Invoice_Att budgetInvoiceModel)
		{
			var resultAjax = new AjaxResult();
			try
			{

				if (budgetInvoiceModel.invoiceId == null)
				{
					// insert invoice
					int countSuccess = BudgetFormCommandHandler.insertInvoiceProduct(budgetInvoiceModel);
				}
				else
				{
					//update invoice
					int countSuccess = BudgetFormCommandHandler.updateInvoiceProduct(budgetInvoiceModel);
				}

				//resultAjax.ActivityId = Session["activityId"].ToString();
				resultAjax.Success = true;
			}
			catch (Exception ex)
			{
				resultAjax.Success = false;
				resultAjax.Message = ex.Message;
			}
			return Json(resultAjax, "text/plain");
		}
		
		public JsonResult delInvoiceDetail(string actId,  string estId, string invId)
		{
			var result = new AjaxResult();
			try
			{		
				int countSuccess = BudgetFormCommandHandler.deleteInvoiceProduct(actId, estId, invId);
				result.Success = true;
			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
				result.Success = false;
			}
			return Json(result, JsonRequestBehavior.AllowGet);
		}


		//---------------------------------------------------------------------------------------
		public PartialViewResult activityProductInvoiceEdit(string activityId, string activityOfEstimateId, string invoiceId)
		{
			if (!string.IsNullOrEmpty(invoiceId))
			{// for edit invoice 
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault(); 
				Budget_Activity.Budget_Activity_Invoice = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, invoiceId).FirstOrDefault(); 

				//Budget_Activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId);
				//Budget_Activity.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, invoiceId);
				Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
				return PartialView("activityProductInvoiceEdit", Budget_Activity);
			}
			else
			{// for insert invoice
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault();
				//Budget_Activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId);
				Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
				return PartialView("activityProductInvoiceEdit", Budget_Activity);
			}
		}

		public PartialViewResult activityProductInvoiceList(string activityId , string activityOfEstimateId)
		{
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
			budget_activity_model.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId,  activityOfEstimateId);
			budget_activity_model.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, null);
			budget_activity_model.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
			return PartialView(budget_activity_model);
		}

		public ActionResult activityProductList(string activityId)
		{
			Session["activityId"] = activityId;
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			try
			{
				budget_activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, null);
				budget_activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();	
			} catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return PartialView(budget_activity);
		}
		//----------------------------------------------------------------------------------------


		public ActionResult activityProduct(string activityId)
		{
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();

			if (activityId == null) { activityId = Session["activityId"].ToString(); }

			if (activityId == null) return RedirectToAction("activityList", "Budget");
			else
			{
				try
				{
					budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", activityId, null, null).ToList();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			return View(budget_activity);
		}

		public ActionResult activityList()
		{
			//Session["activityId"] = Guid.NewGuid().ToString();
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", null,null,null).ToList();
			return View(budget_activity);
		}

		//----- invoice file upload --------------------------------------------------------------//
		public ActionResult manageInvoiceIndex(String budgetApproveId)
		{
			return View();
		}

		public ActionResult manageInvoiceList(String budgetApproveId, String activityNo, String createdByUserId)
		{
			try
			{
				TB_Bud_Image_Model.BudImageModels getBudImageModel = new TB_Bud_Image_Model.BudImageModels();
				getBudImageModel.tbBudImageList = ImageAppCodeBudget.getImageBudget(budgetApproveId, activityNo, createdByUserId);
				return PartialView(getBudImageModel);

				//if (!string.IsNullOrEmpty(budgetApproveId))
				//{
				//	//getBudImageModel.tbBudImageList = ImageAppCodeBudget.getImageBudget(budgetApproveId, activityNo, createdByUserId);
				//	//return PartialView(getBudImageModel);
				//}
				//else
				//{
				//	//getBudImageModel.tbBudImageList = ImageAppCodeBudget.getImageBudget(null);
				//	//return PartialView(getBudImageModel);
				//}

			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("BudgetImageList => " + ex.Message);
			}

			return PartialView();
		}

		public PartialViewResult manageInvoiceView()
		{
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", null, null, null).ToList();
			return PartialView(budget_activity);
		}


		[HttpPost]
		public ActionResult manageInvoiceUpload()
		{
			var result = new AjaxResult();
			try
			{
				byte[] binData = null;
				TB_Bud_Image_Model.BudImageModel imageFormModel = new TB_Bud_Image_Model.BudImageModel();
				foreach (string UploadedImage in Request.Files)
				{
					HttpPostedFileBase httpPostedFile = Request.Files[UploadedImage];

					string resultFilePath = "";
					string extension = Path.GetExtension(httpPostedFile.FileName);
					int indexGetFileName = httpPostedFile.FileName.LastIndexOf('.');
					var _fileName = Path.GetFileName(httpPostedFile.FileName.Substring(0, indexGetFileName)) + "_" + DateTime.Now.ToString("ddMMyyHHmm") + extension;
					string UploadDirectory = Server.MapPath(string.Format(System.Configuration.ConfigurationManager.AppSettings["rootUploadfilesBudget"].ToString(), _fileName));
					resultFilePath = UploadDirectory;
					BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
					binData = b.ReadBytes(0);
					httpPostedFile.SaveAs(resultFilePath);

					imageFormModel._image = binData;
					imageFormModel.imageType = "UploadFile";
					imageFormModel._fileName = _fileName.ToLower();
					imageFormModel.extension = extension.ToLower();
					imageFormModel.remark = "";
					imageFormModel.delFlag = false;
					imageFormModel.createdByUserId = UtilsAppCode.Session.User.empId;
					imageFormModel.createdDate = DateTime.Now;
					imageFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
					imageFormModel.updatedDate = DateTime.Now;


					int resultImg = ImageAppCodeBudget.insertImageBudget(imageFormModel);

				}

				//manageInvoiceList(null);
				//result.ActivityId = Session["activityId"].ToString();
				result.Success = true;
			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
				result.Success = false;
				ExceptionManager.WriteError("manageInvoiceUpload => " + ex.Message);
			}

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public JsonResult manageInvoiceDelete(string id)
		{
			var result = new AjaxResult();

			int resultImg = ImageAppCodeBudget.deleteImageBudgetById(id);

			return Json(result, JsonRequestBehavior.AllowGet);
		}


	}
}