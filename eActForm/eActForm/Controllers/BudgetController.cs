using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using eActForm.BusinessLayer;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using WebLibrary;


namespace eActForm.Controllers
{
	[LoginExpire]
	public class BudgetController : Controller
	{

		public ActionResult activityIndex(string typeForm)
		{
			return View();
		}

		public ActionResult searchBudgetActivityForm(string typeForm)
		{
			string act_createdDateStart = null;
			string act_createdDateEnd = null;
			string act_budgetStatusIdin = null;

			Budget_Activity_Model budget_activity = new Budget_Activity_Model();

			try { 
				#region filter
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
					typeForm = Request.Form["ddlFormType"];
				}

				if (Request.Form["ddlBudgetStatusId"] != "" && Request.Form["ddlBudgetStatusId"] != "Select All")
				{
					act_budgetStatusIdin = Request.Form["ddlBudgetStatusId"];
				}

				//act_budgetStatusIdin = "(" + act_budgetStatusIdin + ")";
				#endregion

				budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", null, null, null, typeForm, act_createdDateStart, act_createdDateEnd, act_budgetStatusIdin).ToList();
				TempData["searchBudgetActivityForm"] = budget_activity;
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("searchBudgetActivityForm --> " + ex.Message);
			}

			return RedirectToAction("activityList");
		}

		public ActionResult activityList(string typeForm)
		{
			Budget_Activity_Model models = new Budget_Activity_Model();

			try
			{
				if (TempData["searchBudgetActivityForm"] != null)
				{
					models = (Budget_Activity_Model)TempData["searchBudgetActivityForm"];
				}
				else
				{
					models.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", null, null, null, typeForm, DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"), null).ToList();
				}
				TempData["searchBudgetActivityForm"] = null;
				return PartialView(models);
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("activityList --> " + ex.Message);
			}
			return PartialView(models);
		}

		public PartialViewResult previewBudgetInvoice(string activityId )
		{
			 Session["activityId"]= activityId;
			string var_actNo = "";
			string var_cusId = "";

			Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();
			Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(activityId,null);
			Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null,null, null, null, null).FirstOrDefault();

			var_actNo = Budget_Model.Budget_Activity.act_activityNo;
			var_cusId = Budget_Model.Budget_Activity.act_customerId;

			Budget_Model.Budget_Invoice_list = ImageAppCodeBudget.getImageBudget(null,null,null, var_actNo,null,null, var_cusId);

			List<TB_Bud_Image_Model.BudImageModel> Result = new List<TB_Bud_Image_Model.BudImageModel>();
			foreach (var inv_his in Budget_Model.Budget_Invoce_History_list) // preview invoice pdf non approved
			{
				if (inv_his.invoiceApproveStatusId == 1 || inv_his.invoiceApproveStatusId == 2 ) // draft or wait
				{
					Result.Add(Budget_Model.Budget_Invoice_list.Find(x => (x.invoiceNo == inv_his.invoiceNo )));
				}
			}
			Result.RemoveAll(item => item == null);
			Budget_Model.Budget_Invoice_list.Clear();
			Budget_Model.Budget_Invoice_list = Result.Distinct().ToList();

			return PartialView(Budget_Model);
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

				if (string.IsNullOrEmpty(budgetInvoiceModel.invoiceNo) == false )
				{
					TB_Bud_Image_Model getBudImageModel = new TB_Bud_Image_Model();
					getBudImageModel.BudImageList = ImageAppCodeBudget.getImageBudget(null, budgetInvoiceModel.invoiceNo, null, null, null, null, budgetInvoiceModel.actCustomerId);
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
		
		public JsonResult delInvoiceDetail(string actId,  string estId, string invId , string delType)
		{
			var result = new AjaxResult();
			try
			{		
				int countSuccess = BudgetFormCommandHandler.deleteInvoiceProduct(actId, estId, invId, delType);
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
				Budget_Activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, null, null, null, null).FirstOrDefault();
				Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault();
				Budget_Activity.Budget_Activity_Invoice = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, invoiceId).FirstOrDefault();
				Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();

				Budget_Activity.Budget_Count_Wait_Approve = QueryGetBudgetActivity.getBudgetActivityWaitApprove(activityId).FirstOrDefault();
				return PartialView(Budget_Activity);
			}
			else
			{// for insert invoice
				Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();
				Budget_Activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, null, null, null, null).FirstOrDefault();
				Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault();
				Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();

				Budget_Activity.Budget_Count_Wait_Approve = QueryGetBudgetActivity.getBudgetActivityWaitApprove(activityId).FirstOrDefault();
				return PartialView(Budget_Activity);
			}
		}

		public PartialViewResult activityProductInvoiceList(string activityId , string activityOfEstimateId )
		{
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
			budget_activity_model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, null, null, null, null).FirstOrDefault();
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
				budget_activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null, null, null, null, null, null).FirstOrDefault(); ;
				budget_activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, null);
				budget_activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
				budget_activity.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault();
			} catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return PartialView(budget_activity);
		}
		
		public ActionResult activityProduct(string activityId)
		{
			Budget_Activity_Model budget_activity = new Budget_Activity_Model();

			if (activityId == null) { activityId = Session["activityId"].ToString(); }

			if (activityId == null) return RedirectToAction("activityList", "Budget");
			else
			{
				try
				{
					budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity("3", activityId, null, null, null,null, null, null).ToList();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			return View(budget_activity);
		}



		//----- invoice file upload --------------------------------------------------------------//
		public JsonResult getImageInvoice(string imgInvoiceNo , string companyEN , string customerId)
		{
			List<TB_Bud_Image_Model.BudImageModel> imgInvoiceList = new List<TB_Bud_Image_Model.BudImageModel>();
			try
			{
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

		public ActionResult manageInvoiceIndex(String companyTH)
		{
			return View();
		}

		public ActionResult manageInvoiceList(String imageId , String imageInvoiceNo, String budgetApproveId, String activityNo, String createdByUserId, String companyTH)
		{
			try
			{
				if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
				{
					createdByUserId = null;
				}
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
					customerList = QueryGetAllCustomers.getCustomersOMT().Where(x => x.regionId == regionId).ToList();

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
				if (UtilsAppCode.Session.User.regionId != "")
				{
					budgetImageModel.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.id == UtilsAppCode.Session.User.regionId).ToList();
				}
				else
				{
					budgetImageModel.regionGroupList = QueryGetAllRegion.getAllRegion();
				}
				
				if (budgetImageModel.BudImage.company == "MT")
				{
					budgetImageModel.CustomerList = QueryGetAllCustomers.getCustomersMT().ToList();
				}
				else
				{
					budgetImageModel.CustomerList = QueryGetAllCustomers.getCustomersOMT().Where(x => x.regionId == budgetImageModel.BudImage.regionId).ToList();
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
					string strDateTime = DateTime.Now.ToString("ddMMyyHHmmssff");
					int tmpFileNameIndex = httpPostedFile.FileName.LastIndexOf('.');
					int indexGetFileName = 10; // limit file name 10 char

					if (tmpFileNameIndex < 10) { indexGetFileName = tmpFileNameIndex; }

					var _fileName = Path.GetFileName(httpPostedFile.FileName.Substring(0, indexGetFileName)) + "_" + strDateTime + extension;
					string UploadDirectory = Server.MapPath(string.Format(System.Configuration.ConfigurationManager.AppSettings["rootUploadfilesBudget"].ToString(), _fileName));

					if (extension == ".pdf")
					{
						resultFilePath = UploadDirectory;
						BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
						binData = b.ReadBytes(0);
						httpPostedFile.SaveAs(resultFilePath);
					}
					else
					{
						resultFilePath = UploadDirectory;
						BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
						binData = b.ReadBytes(0);
						httpPostedFile.SaveAs(resultFilePath);

						// convert image to pdf
						string UploadDirectory_pdf = "";
						UploadDirectory_pdf = UploadDirectory.Replace(extension, ".pdf");
						iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(UploadDirectory);
						using (FileStream fs = new FileStream(UploadDirectory_pdf, FileMode.Create, FileAccess.Write, FileShare.None))
						{
							//using (Document doc = new Document(PageSize.A4, 10, 10, 10, 10))
							using (Document doc = new Document(PageSize.A4, 0, 0, 0, 0))
							{
								using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
								{
									writer.CloseStream = false;
									doc.Open();
									image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
									image.SetAbsolutePosition((PageSize.A4.Width - image.ScaledWidth) / 2, (PageSize.A4.Height - image.ScaledHeight));
									writer.DirectContent.AddImage(image);
									doc.Close();
								}
							}
						}
					}

					imageFormModel._image = binData;
					imageFormModel.imageType = "UploadFile";
					imageFormModel._fileName = _fileName.ToLower().Replace(extension, ".pdf");
					imageFormModel.extension = ".pdf";
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
				getBudImageModel.BudImageList = ImageAppCodeBudget.getImageBudget(null, budgetInvoiceModel.invoiceNo,null , null, null , budgetInvoiceModel.company, customerId);
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