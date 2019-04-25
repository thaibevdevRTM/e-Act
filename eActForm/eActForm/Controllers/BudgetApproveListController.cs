using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using System.IO;
using System.Configuration;
using System.Web.Mvc;
using WebLibrary;


namespace eActForm.Controllers
{
	[LoginExpire]

	public class BudgetApproveListController : Controller
    {
		// GET: BudgetApprove
		public ActionResult Index()
		{
			SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
			return View(models);
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
			else if (Request.Form["ddlStatus"] != "")
			{
				model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists, int.Parse(Request.Form["ddlStatus"]));
			}
			TempData["ApproveSearchResult"] = model.actLists;
			return RedirectToAction("ListView");
		}




		//-----------------------------------------------------------------------------------------------------------------
		public ActionResult budgetApproveList()
		{

			Budget_Approve_Detail_Model.budgetForms  model = new Budget_Approve_Detail_Model.budgetForms();
			if (TempData["ApproveSearchResult"] == null)
			{
				model = new Budget_Approve_Detail_Model.budgetForms();

				UtilsAppCode.Session.User.empId = "11025855"; // empid for test approve modele
				model.budgetFormLists = getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
				TempData["ApproveFormLists"] = model.budgetFormLists;
				model.budgetFormLists = getFilterFormByStatusId(model.budgetFormLists, (int)AppCode.ApproveStatus.รออนุมัติ);
			}
			else
			{
				model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveSearchResult"];
			}
			return PartialView(model);
		}


		public static List<Budget_Approve_Detail_Model.budgetForm> getApproveListsByEmpId(string empId)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveBudgetByEmpId"
					, new SqlParameter[] { new SqlParameter("@empId", empId) });
				var lists = (from DataRow dr in ds.Tables[0].Rows
							 select new Budget_Approve_Detail_Model.budgetForm()
							 {
								 activityId = dr["ActivityFormId"].ToString(),
								 statusId = dr["statusId"].ToString(),
								 statusName = dr["statusName"].ToString(),
								 activityNo = dr["activityNo"].ToString(),
								 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],
								 reference = dr["reference"].ToString(),
								 customerId = dr["customerId"].ToString(),
								 channelName = dr["channelName"].ToString(),
								 productTypeId = dr["productTypeId"].ToString(),
								 productTypeNameEN = dr["productTypeNameEN"].ToString(),

								 cusShortName = dr["cusShortName"].ToString(),
								 productCategory = dr["productCateText"].ToString(),
								 productGroup = dr["productGroupId"].ToString(),
								 productGroupName = dr["productGroupName"].ToString(),

								 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
								 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
								 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
								 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
								 activityName = dr["activityName"].ToString(),
								 theme = dr["theme"].ToString(),
								 objective = dr["objective"].ToString(),
								 trade = dr["trade"].ToString(),
								 activityDetail = dr["activityDetail"].ToString(),

								 budgetActivityId = dr["budgetActivityId"].ToString(),
								 approveId = dr["approveId"].ToString(),
								 approveDetailId = dr["approveDetailId"].ToString(),

								 //delFlag = (bool)dr["delFlag"],
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
				//throw new Exception("getApproveListsByStatusId >> " + ex.Message);
				ExceptionManager.WriteError("getApproveListsByStatusId >> " + ex.Message);
				return new List<Budget_Approve_Detail_Model.budgetForm>();
			}
		}

		public static List<Budget_Approve_Detail_Model.budgetForm> getFilterFormByStatusId(List<Budget_Approve_Detail_Model.budgetForm> lists, int statusId)
		{
			try
			{
				return lists.Where(r => r.statusId == statusId.ToString()).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("getFilterFormByStatusId >> " + ex.Message);
			}
		}
		//-----------------------------------------------------------------------------------------------------------------


		[HttpPost]
		[ValidateInput(false)]
		public JsonResult submitPreviewBudget(string GridHtml, string budgetActivityId)
		{
			var resultAjax = new AjaxResult();
			try
			{

				var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetActivityId));
				AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), rootPath);
				if (BudgetApproveListController.insertApproveForBudgetForm(budgetActivityId) > 0)
				{
					var budget_approve_id = getApproveBudgetId(budgetActivityId);
					BudgetApproveListController.updateApproveWaitingByRangNo(budget_approve_id);

					//  ยังไม่ได้แก้ไปทำตอน approve ก่อน *****
					//EmailAppCodes.sendApprove(budget_approve_id, AppCode.ApproveType.Activity_Form);
				}
				resultAjax.Success = true;
			}
			catch (Exception ex)
			{
				resultAjax.Success = false;
				resultAjax.Message = ex.Message;
				ExceptionManager.WriteError(ex.Message);
			}
			return Json(resultAjax, "text/plain");
		}

		public static int insertApproveForBudgetForm(string budgetActivityId)
		{
			try
			{
				insertApproveBudgetDetail(budgetActivityId);
				var budget_approve_id  = getApproveBudgetId(budgetActivityId);

				if (getApproveByBudgetId(budget_approve_id).approveDetailLists.Count == 0)
				{
					ApproveFlowModel.approveFlowModel flowModel = getFlowIdBudget(ConfigurationManager.AppSettings["subjectBudgetFormId"], budget_approve_id);
					return insertApproveByFlowBudget(flowModel, budget_approve_id);
				}
				else return 999; // alredy approve
			}
			catch (Exception ex)
			{
				throw new Exception("insertApproveBudget >> " + ex.Message);
			}
		}

		public static int insertApproveBudgetDetail(string budgetActivityId)
		{
			try
			{
				int rtn = 0;

				SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertBudgetApproveDetail"
				, new SqlParameter[] 
				{
				 new SqlParameter("@budgetActivityId", budgetActivityId)
				 ,new SqlParameter("@createdByUserId", "70016911")
				//,new SqlParameter("@createdByUserId", UtilsAppCode.Session.User.empId)
				});

				return rtn;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		public static string getApproveBudgetId( string budgetActivityId)
		{
			try
			{
				Budget_Approve_Detail_Model models = new Budget_Approve_Detail_Model();
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetApproveId"
					, new SqlParameter[] {
						new SqlParameter("@budgetActivityId",budgetActivityId)
					});
				models.Budget_Approve_detail_list = (from DataRow dr in ds.Tables[0].Rows
											 select new Budget_Approve_Detail_Model.Budget_Approve_Detail_Att()
											 {
												 budgetActivityId = dr["id"].ToString()
											 }).ToList();
				return models.Budget_Approve_detail_list.ElementAt(0).budgetActivityId.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("getFlow by actFormId >>" + ex.Message);
			}
		}
	
		public static int insertApproveByFlowBudget(ApproveFlowModel.approveFlowModel flowModel, string budgetIdId)
		{
			try
			{
				int rtn = 0;
				List<ApproveModel.approveModel> list = new List<ApproveModel.approveModel>();
				ApproveModel.approveModel model = new ApproveModel.approveModel();
				model.id = Guid.NewGuid().ToString();
				model.flowId = flowModel.flowMain.id;
				model.actFormId = budgetIdId;
				model.delFlag = false;
				model.createdDate = DateTime.Now;
				model.createdByUserId = UtilsAppCode.Session.User.empId;
				model.updatedDate = DateTime.Now;
				model.updatedByUserId = UtilsAppCode.Session.User.empId;
				list.Add(model);
				DataTable dt = AppCode.ToDataTable(list);
				foreach (DataRow dr in dt.Rows)
				{
					rtn += SqlHelper.ExecuteNonQueryTypedParams(AppCode.StrCon, "usp_insertApprove", dr);
				}

				// insert approve detail
				foreach (ApproveFlowModel.flowApproveDetail m in flowModel.flowDetail)
				{
					rtn += SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertApproveDetail"
						, new SqlParameter[] {new SqlParameter("@id",Guid.NewGuid().ToString())
							,new SqlParameter("@approveId",model.id)
							,new SqlParameter("@rangNo",m.rangNo)
							,new SqlParameter("@empId",m.empId)
							,new SqlParameter("@statusId","")
							,new SqlParameter("@isSendEmail",false)
							,new SqlParameter("@remark","")
							,new SqlParameter("@delFlag",false)
							,new SqlParameter("@createdDate",DateTime.Now)
							//,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
							,new SqlParameter("@createdByUserId","70016911")
							,new SqlParameter("@updatedDate",DateTime.Now)
							//,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
							,new SqlParameter("@updatedByUserId","'70016911'")
						});
				}

				return rtn;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static int updateApproveWaitingByRangNo(string budgetId) 
		{
			try
			{

				return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateWaitingApproveByRangNo"
					, new SqlParameter[] {new SqlParameter("@rangNo",1)
					,new SqlParameter("@actId", budgetId)
					//,new SqlParameter("@updateBy", UtilsAppCode.Session.User.empId)
					,new SqlParameter("@updateBy", "70016911")
					,new SqlParameter("@updateDate", DateTime.Now)
					});
			}
			catch (Exception ex)
			{
				throw new Exception("updateApproveWaitingByRangNo >>" + ex.Message);
			}
		}


		//------------- ยังไม่ได้แก้  -----------------------------------------------------------------------------------------
		public static void setCountWatingApproveBudget() //ยังไม่ได้แก้ *******
		{
			try
			{
				if (UtilsAppCode.Session.User != null)
				{
					DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountWatingApproveByEmpId"
						, new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
					if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
					{
						UtilsAppCode.Session.User.countWatingBudgetForm = ds.Tables[0].Rows[0]["actFormId"].ToString();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("setCountWatingApproveBudget >>" + ex.Message);
			}
		}

		public static bool getPremisionApproveByEmpid(List<ApproveModel.approveDetailModel> lists, string empId)
		{
			try
			{
				bool rtn = false;
				if (lists != null)
				{
					var model = (from x in lists where x.empId.Equals(empId) select x).ToList();
					rtn = model.Count > 0 ? true : false;
				}

				return rtn;
			}
			catch (Exception ex)
			{
				throw new Exception("fillterApproveByEmpid >>" + ex.Message);
			}
		}

		public static ApproveModel.approveModels getApproveByBudgetId(string budgetId)
		{
			try
			{
				ApproveModel.approveModels models = new ApproveModel.approveModels();
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetApproveDetailByBudgetId"
					, new SqlParameter[] { new SqlParameter("@budgetId", budgetId) });
				models.approveDetailLists = (from DataRow dr in ds.Tables[0].Rows
											 select new ApproveModel.approveDetailModel()
											 {
												 id = dr["id"].ToString(),
												 approveId = dr["approveId"].ToString(),
												 rangNo = (int)dr["rangNo"],
												 empId = dr["empId"].ToString(),
												 empName = dr["empName"].ToString(),
												 empEmail = dr["empEmail"].ToString(),
												 statusId = dr["statusId"].ToString(),
												 statusName = dr["statusName"].ToString(),
												 isSendEmail = (bool)dr["isSendEmail"],
												 remark = dr["remark"].ToString(),
												 signature = (dr["signature"] == null || dr["signature"] is DBNull) ? new byte[0] : (byte[])dr["signature"],
												 ImgName = string.Format(ConfigurationManager.AppSettings["rootgetSignaURL"], dr["empId"].ToString()),
												 delFlag = (bool)dr["delFlag"],
												 createdDate = (DateTime?)dr["createdDate"],
												 createdByUserId = dr["createdByUserId"].ToString(),
												 updatedDate = (DateTime?)dr["updatedDate"],
												 updatedByUserId = dr["updatedByUserId"].ToString(),

											 }).ToList();


				if (models.approveDetailLists.Count > 0)
				{
					ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveByBudgetId"
				   , new SqlParameter[] { new SqlParameter("@budgetId", budgetId) });

					var empDetail = models.approveDetailLists.Where(r => r.empId == UtilsAppCode.Session.User.empId).ToList(); //
					var lists = (from DataRow dr in ds.Tables[0].Rows
								 select new ApproveModel.approveModel()
								 {
									 id = dr["id"].ToString(),
									 flowId = dr["flowId"].ToString(),
									 actFormId = dr["actFormId"].ToString(),
									 actNo = dr["activityNo"].ToString(),
									 statusId = (empDetail.Count > 0) ? empDetail.FirstOrDefault().statusId : "",
									 delFlag = (bool)dr["delFlag"],
									 createdDate = (DateTime?)dr["createdDate"],
									 createdByUserId = dr["createdByUserId"].ToString(),
									 updatedDate = (DateTime?)dr["updatedDate"],
									 updatedByUserId = dr["updatedByUserId"].ToString(),
									 isPermisionApprove = getPremisionApproveByEmpid(models.approveDetailLists, UtilsAppCode.Session.User.empId)
								 }).ToList();

					models.approveModel = lists[0];

				}

				return models;
			}
			catch (Exception ex)
			{
				throw new Exception("getCountApproveByActFormId >>" + ex.Message);
			}
		}

		
		
		//------------- OK -----------------------------------------------------------------------------------------

		public static ApproveFlowModel.approveFlowModel getFlowIdBudget(string subId, string budget_approve_id)
		{
			try
			{
				ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFlowIdByBudgetId"
					, new SqlParameter[] {
						  new SqlParameter("@subId",subId)
						, new SqlParameter("@budgetApproveId",budget_approve_id)
					});
				var lists = (from DataRow dr in ds.Tables[0].Rows
							 select new ApproveFlowModel.flowApprove()
							 {
								 id = dr["id"].ToString(),
							 }).ToList();
				if (lists.Count > 0)
				{
					model.flowMain = lists[0];
					model.flowDetail = getFlowDetailBudget(model.flowMain.id);
				}
				return model;
			}
			catch (Exception ex)
			{
				throw new Exception("getFlow by actFormId >>" + ex.Message);
			}
		}

		public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailBudget(string flowId)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveDetail"
					, new SqlParameter[] { new SqlParameter("@flowId", flowId) });
				var lists = (from DataRow dr in ds.Tables[0].Rows
							 select new ApproveFlowModel.flowApproveDetail()
							 {
								 id = dr["id"].ToString(),
								 rangNo = (int)dr["rangNo"],
								 empId = dr["empId"].ToString(),
								 empEmail = dr["empEmail"].ToString(),
								 empFNameTH = dr["empFNameTH"].ToString(),
								 empLNameTH = dr["empLNameTH"].ToString(),
								 empPositionTitleTH = dr["empPositionTitleTH"].ToString(),
								 approveGroupName = dr["approveGroupName"].ToString(),
								 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
								 isShowInDoc = (bool)dr["showInDoc"],
								 description = dr["description"].ToString(),
							 }).ToList();
				return lists;
			}
			catch (Exception ex)
			{
				throw new Exception("getFlowDetail >>" + ex.Message);
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public JsonResult genPdfApprove(string GridHtml, string budgetId)
		{
			var resultAjax = new AjaxResult();
			try
			{
				var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetId));
				AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), rootPath);
				//EmailAppCodes.sendApproveActForm(activityId);

				resultAjax.Success = true;
			}
			catch (Exception ex)
			{
				resultAjax.Success = false;
				resultAjax.Message = ex.Message;
			}
			return Json(resultAjax, "text/plain");
		}

		public ActionResult getPDF(string budgetActivityId)
		{
			var fileStream = new FileStream(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetActivityId)),
											 FileMode.Open,
											 FileAccess.Read
										   );
			var fsResult = new FileStreamResult(fileStream, "application/pdf");
			return fsResult;
		}

		//public ActionResult budgetApproveDetail(string activityId)
		//{
		//	//Session["activityId"] = activityId;
		//	Budget_Activity_Model budget_activity = new Budget_Activity_Model();
		//	try
		//	{
		//		budget_activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, null);
		//		budget_activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine(ex.Message);
		//	}
		//	return PartialView(budget_activity);
		//}

		//public PartialViewResult budgetApproveList(string isSubmitApprove, string activityId)
		//{
		//	//Session["activityId"] = activityId;
		//	Budget_Approve_Model budget_approve = new Budget_Approve_Model();
		//	try
		//	{
		//		budget_approve.Budget_Approve_list = QueryGetBudgetApprove.getBudgetActivityApprove( activityId);
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine(ex.Message);
		//	}
		//	return PartialView(budget_approve);
		//}

		//public ActionResult budgetApprove(string activityId)
		//{
		//	Budget_Approve_Model budget_approve = new Budget_Approve_Model();
		//	try
		//	{
		//		//budget_activity.Budget_Activity_list = QueryBudgetBiz.getBudgetActivity("3", activityId, null).ToList();
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine(ex.Message);
		//	}
		//	return View(budget_approve);
		//}

	}
}