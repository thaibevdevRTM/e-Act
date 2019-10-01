using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using WebLibrary;

namespace eActForm.Controllers
{
	[LoginExpire]
	public class BudgetController : Controller
	{

		public PartialViewResult previewBudgetInvoice(string activityId )
		{
			 Session["activityId"]= activityId;

			Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();
			Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(activityId,null);
			Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null,null).FirstOrDefault();
			return PartialView(Budget_Model);

			//budgetApproveId
			//activityId

		}


		public JsonResult submitInvoice(Budget_Activity_Model.Budget_Activity_Invoice_Att budgetInvoiceModel)
		{
			var resultAjax = new AjaxResult();
			//int count_invo = 0;
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


				

				if (budgetInvoiceModel.budgetImageId != null)
				{
					TB_Bud_Image_Model getBudImageModel = new TB_Bud_Image_Model();
					getBudImageModel.BudImageList = ImageAppCodeBudget.getImageBudget(budgetInvoiceModel.budgetImageId, null, null, null, null, null,null);
					if (getBudImageModel.BudImageList.Any()) // True, the list is not empty
					{
						if (getBudImageModel.BudImageList.ElementAtOrDefault(0).count_activityNo > 1)
						{
							resultAjax.Code = 2;
							resultAjax.Message = getBudImageModel.BudImageList.ElementAtOrDefault(0).invoiceNo;
						}
					}
				}
				else
				{
					resultAjax.Code = 0;
					resultAjax.Message = null;
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
		public PartialViewResult activityProductInvoiceEdit(string activityId, string activityOfEstimateId, string invoiceId , string company)
		{
			if (!string.IsNullOrEmpty(invoiceId))
			{// for edit invoice 
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, null).FirstOrDefault();
				Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault();
				Budget_Activity.Budget_Activity_Invoice = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, invoiceId).FirstOrDefault();
				Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();

				Budget_Activity.Budget_Count_Wait_Approve = QueryGetBudgetActivity.getBudgetActivityWaitApprove(activityId).FirstOrDefault();

				Budget_Activity.Budget_ImageList = ImageAppCodeBudget.getImageBudget(null, null, null, null, null, company,null);

				return PartialView(Budget_Activity);
			}
			else
			{// for insert invoice
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, null).FirstOrDefault();
				Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault();
				Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();

				Budget_Activity.Budget_Count_Wait_Approve = QueryGetBudgetActivity.getBudgetActivityWaitApprove(activityId).FirstOrDefault();

				Budget_Activity.Budget_ImageList = ImageAppCodeBudget.getImageBudget(null, null, null, null, null, company, null);
				return PartialView(Budget_Activity);
			}
		}

		public PartialViewResult activityProductInvoiceList(string activityId , string activityOfEstimateId )
		{
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
			budget_activity_model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, null).FirstOrDefault();
			budget_activity_model.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, null);
			budget_activity_model.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault(); 

			return PartialView(budget_activity_model);
		}

		public ActionResult activityProductList(string activityId)
		{
			Session["activityId"] = activityId;
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			try
			{
				budget_activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, null).FirstOrDefault(); ;
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
					budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", activityId, null, null,null).ToList();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			return View(budget_activity);
		}

		public ActionResult activityList(string typeForm)
		{
			//Session["activityId"] = Guid.NewGuid().ToString();
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();
			budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", null,null,null, typeForm).ToList();
			return View(budget_activity);
		}

		//----- invoice file upload --------------------------------------------------------------//
		public JsonResult getImageInvoice(string imgInvoiceNo , string companyEN , string customerId)
		{
			List<TB_Bud_Image_Model.BudImageModel> imgInvoiceList = new List<TB_Bud_Image_Model.BudImageModel>();
			try
			{
				//var test = companyEN;
				//var Key_company = Session["budget_Key_company"].ToString();
				imgInvoiceList = ImageAppCodeBudget.getImageBudget(null, null, null, null, null, companyEN, customerId)
					.Where(x => x.invoiceNo.Contains(imgInvoiceNo) )
					.ToList();
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getImageInvoice => " + ex.Message);
			}

			return Json(imgInvoiceList, JsonRequestBehavior.AllowGet);
		}

		

		public JsonResult getRegionInvoice(string nameEN)
		{
			List<TB_Act_Region_Model> regionList = new List<TB_Act_Region_Model>();
			try
			{
				regionList = QueryGetAllRegion.getAllRegion().Where(x => x.name.Contains(nameEN)).ToList();
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getRegionInvoice => " + ex.Message);
			}

			return Json(regionList, JsonRequestBehavior.AllowGet);
		}

		//public JsonResult getCustomerInvoice(string nameEN,string companyEN)
		//{
		//	List<TB_Act_Customers_Model> customerList = new List<TB_Act_Customers_Model>();
		//	try
		//	{
		//		customerList = QueryGetAllCustomers.getAllCustomersRegion()
		//	}
		//	catch (Exception ex)
		//	{
		//		ExceptionManager.WriteError("getCustomerInvoice => " + ex.Message);
		//	}

		//	return Json(customerList, JsonRequestBehavior.AllowGet);
		//}


		public ActionResult manageInvoiceIndex(String companyTH)
		{
			return View();
		}

		public ActionResult manageInvoiceList(String imageId , String imageInvoiceNo, String budgetApproveId, String activityNo, String createdByUserId, String companyTH)
		{
			try
			{
				TB_Bud_Image_Model budgetImageModel = new TB_Bud_Image_Model();
				budgetImageModel.BudImageList = ImageAppCodeBudget.getImageBudget(imageId, imageInvoiceNo, budgetApproveId, activityNo, createdByUserId, companyTH,null);
				return PartialView(budgetImageModel);
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("BudgetImageList => " + ex.Message);
			}

			return PartialView();
		}

		public JsonResult getCustomerInvoice(string customerTH, string companyEN, string regionId)
		{
			var result = new AjaxResult();
			List<TB_Act_Customers_Model.Customers_Model> customerList = new List<TB_Act_Customers_Model.Customers_Model>();
			try
			{
				if (companyEN == "MT")
				{
					customerList = QueryGetAllCustomers.getCustomersMT().Where(x => x.cusNameTH.Contains(customerTH)).ToList();
				}
				else
				{
					customerList = QueryGetAllCustomers.getAllCustomersRegion().Where(x => x.regionId == regionId).ToList();

					var resultData = new
					{
						getCustomerName = customerList.Select(x => new
						{
							Value = x.id,
							Text = x.cusNameTH
						}).ToList(),
					};
					result.Data = resultData;

				}
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getInvoiceCustomer => " + ex.Message);
			}

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public PartialViewResult manageInvoiceView(string imageId)
		{
			TB_Bud_Image_Model budgetImageModel = new TB_Bud_Image_Model();
			try
			{
				budgetImageModel.BudImage = ImageAppCodeBudget.getImageBudget(imageId, null, null, null, null, null,null).FirstOrDefault();
				budgetImageModel.RegionList = QueryGetAllRegion.getAllRegion().ToList();
				if (budgetImageModel.BudImage.company == "MT")
				{
					budgetImageModel.CustomerList = QueryGetAllCustomers.getCustomersMT().ToList();
				}
				else
				{
					budgetImageModel.CustomerList = QueryGetAllCustomers.getAllCustomersRegion().Where(x => x.regionId == budgetImageModel.BudImage.regionId).ToList();
				}

			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("manageInvoiceView => " + ex.Message);
			}
			return PartialView(budgetImageModel);
		}


		[HttpPost]
		public ActionResult manageInvoiceUpload(String company)
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

					imageFormModel.company = "";
					if (company == "MT") { imageFormModel.company = "5600"; };
					if (company == "OMT") { imageFormModel.company = "5601"; };
					
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
			var empId = UtilsAppCode.Session.User.empId;

			int resultImg = ImageAppCodeBudget.deleteImageBudgetById(id, empId);

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public PartialViewResult manageInvoiceEdit(string imageId)
		{
			TB_Bud_Image_Model.BudImageModel getBudImageModel = new TB_Bud_Image_Model.BudImageModel();
			getBudImageModel = ImageAppCodeBudget.getImageBudget(imageId,null, null, null, null,null,null).FirstOrDefault();

			return PartialView(getBudImageModel);
		}

		public JsonResult manageInvoiceEditSubmit(string id,string invoiceNo , string remark , string companyEN , string regionId , string customerId)
		{
			var resultAjax = new AjaxResult();
			try
			{
				TB_Bud_Image_Model.BudImageModel budgetInvoiceModel = new TB_Bud_Image_Model.BudImageModel();
				budgetInvoiceModel.id = id;
				budgetInvoiceModel.invoiceNo = invoiceNo;
				budgetInvoiceModel.remark = remark;
				budgetInvoiceModel.company = companyEN;

				budgetInvoiceModel.regionId = regionId;
				budgetInvoiceModel.customerId = customerId;

				budgetInvoiceModel.updatedByUserId = UtilsAppCode.Session.User.empId;
				budgetInvoiceModel.updatedDate = DateTime.Now;

				TB_Bud_Image_Model getBudImageModel = new TB_Bud_Image_Model();
				getBudImageModel.BudImageList = ImageAppCodeBudget.getImageBudget(null, budgetInvoiceModel.invoiceNo,null , null, null, budgetInvoiceModel.company,null);
				if (getBudImageModel.BudImageList.Any())
				{
					resultAjax.Code = 2;
					resultAjax.Message = getBudImageModel.BudImageList.ElementAtOrDefault(0).invoiceNo;
				}
				else
				{
					resultAjax.Code = 0;
					resultAjax.Message = null;
				}

				//update image invoice
				int countSuccess = ImageAppCodeBudget.updateImageBudget(budgetInvoiceModel);

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

	}
}