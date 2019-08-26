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
	public class BudgetApproveController : Controller
	{

		// GET: Approve
		public ActionResult Index(string budgetApproveId)
		{
			if (budgetApproveId == null) return RedirectToAction("index", "BudgetApproveList");
			else
			{
				//var budgetApproveId = BudgetApproveListController.getApproveBudgetId(budgetActivityId);
				ApproveModel.approveModels models = getApproveByBudgetApproveId(budgetApproveId);
				models.approveStatusLists = ApproveAppCode.getApproveStatus(AppCode.StatusType.app).Where(x => x.id == "3" || x.id == "5").ToList();
				return View(models);
			}
		}

		public static ApproveModel.approveModels getApproveByBudgetApproveId(string budgetApproveId)
		{
			try
			{
				//var budget_approve_id = BudgetApproveListController.getApproveBudgetId(budgetApproveId); //2019-04-26 : 1:57

				ApproveModel.approveModels models = new ApproveModel.approveModels();
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetApproveDetailByBudgetId"
					, new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId) });
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
												 ImgName = string.Format(ConfigurationManager.AppSettings["rootgetSignaURL"], dr["empId"].ToString()), //rootgetSignaURL , rootSignaURL
												 delFlag = (bool)dr["delFlag"],
												 createdDate = (DateTime?)dr["createdDate"],
												 createdByUserId = dr["createdByUserId"].ToString(),
												 updatedDate = (DateTime?)dr["updatedDate"],
												 updatedByUserId = dr["updatedByUserId"].ToString(),

											 }).ToList();


				if (models.approveDetailLists.Count > 0)
				{
					ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetApproveByBudgetApproveId"
				   , new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId) });

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


		public ActionResult approvePositionSignatureLists(string actId)
		{
			ApproveModel.approveModels models = getApproveByBudgetApproveId(actId);
			ApproveFlowModel.approveFlowModel flowModel = BudgetApproveListController.getFlowIdBudgetByBudgetActivityId(ConfigurationManager.AppSettings["subjectBudgetFormId"], actId);
			models.approveFlowDetail = flowModel.flowDetail;
			return PartialView(models);

		}

		public ActionResult approvePositionSignatureListsByBudgetApproveId(string budgetApproveId)
		{
			 ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = ApproveAppCode.getApproveByActFormId(budgetApproveId);
                ApproveFlowModel.approveFlowModel flowModel = BudgetApproveListController.getFlowIdByBudgetApproveId(budgetApproveId);
                models.approveFlowDetail = flowModel.flowDetail;
            }
            catch(Exception ex)
            {
                TempData["approvePositionSignatureError"] = AppCode.StrMessFail + ex.Message;
            }
            return PartialView(models);
		}


		public PartialViewResult previewApproveBudget(string budgetApproveId , string test)
		{
			Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();
			Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(null,budgetApproveId);
			//Budget_Model.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity(null, null, null, budgetApproveId);
			Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, null, null, budgetApproveId).FirstOrDefault();
			Budget_Model.Budget_Approve_detail_list = QueryGetBudgetApprove.getBudgetApproveId(budgetApproveId);
			return PartialView(Budget_Model);

			//budgetApproveId
			//activityId

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

		public ActionResult approveLists(ApproveModel.approveModels models)
		{
			return PartialView(models);
		}

		[HttpPost]
		[ValidateInput(false)]
		public JsonResult genPdfApproveBudget(string GridHtml, string statusId,string budgetApproveId)
		{
			var resultAjax = new AjaxResult();
			try
			{

				var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId));
				GridHtml = GridHtml.Replace("<br>", "<br/>");
				AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), rootPath);

				if (statusId == ConfigurationManager.AppSettings["statusReject"])
				{
					EmailAppCodes.sendRejectBudget(budgetApproveId, AppCode.ApproveType.Budget_form);
				}
				else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
				{
					EmailAppCodes.sendApproveBudget(budgetApproveId, AppCode.ApproveType.Budget_form,false );
				}


				resultAjax.Success = true;
			}
			catch (Exception ex)
			{
				resultAjax.Success = false;
				resultAjax.Message = ex.Message;
				ExceptionManager.WriteError("genPdfApproveBudget => " + ex.Message);
			}
			return Json(resultAjax, "text/plain");
		}



		[HttpPost]
		public JsonResult insertApprove()
		{
			var result = new AjaxResult();
			result.Success = false;
			try
			{
				if (updateApprove(Request.Form["lblActFormId"], Request.Form["ddlStatus"], Request.Form["txtRemark"], Request.Form["lblApproveType"]) > 0)
				{
					setCountWatingApproveBudget();
					result.Success = true;
				}
				else
				{
					result.Message = AppCode.StrMessFail;
				}
			}
			catch (Exception ex)
			{
				result.Message = ex.Message;
			}
			return Json(result);
		}

		public static int updateApprove(string actFormId, string statusId, string remark, string approveType)
		{
			try
			{
				// update approve detail
				var var_budget_approve_id = actFormId;
					 int rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateBudgetApprove"
						, new SqlParameter[] {new SqlParameter("@actFormId",var_budget_approve_id)
					, new SqlParameter("@empId",UtilsAppCode.Session.User.empId)
					,new SqlParameter("@statusId",statusId)
					,new SqlParameter("@remark",remark)
					,new SqlParameter("@updateDate",DateTime.Now)
					,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
						});


				if (approveType == "ActivityBudget")
				{
					rtn = updateBudgetFormStatus(statusId, var_budget_approve_id);
				}
				return rtn;
			}
			catch (Exception ex)
			{
				throw new Exception("updateApprove >> " + ex.Message);
			}
		}

		private static int updateBudgetFormStatus(string statusId, string budgetApproveId)
		{
			try
			{
				int rtn = 0;
				// update activity form
				if (statusId == ConfigurationManager.AppSettings["statusReject"])
				{
					// update reject
					rtn += updateBudgetFormWithApproveReject(budgetApproveId);
				}
				else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
				{
					// update approve
					rtn += updateBudgetFormWithApproveDetail(budgetApproveId);

				}
				return rtn;
			}
			catch (Exception ex)
			{
				throw new Exception("updateBudgetFormStatus >> " + ex.Message);
			}
		}

		public static int updateBudgetFormWithApproveReject(string budgetApproveId)
		{
			try
			{
				return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusBudgetFormByApproveReject"
					, new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId)
					,new SqlParameter("@updateDate",DateTime.Now)
					,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
			}
			catch (Exception ex)
			{
				throw new Exception("updateBudgetFormWithApproveReject >> " + ex.Message);
			}
		}

		public static int updateBudgetFormWithApproveDetail(string budgetApproveId)
		{
			try
			{
				return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusBudgetFormByApproveDetail"
					, new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId)
					,new SqlParameter("@updateDate",DateTime.Now)
					,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
			}
			catch (Exception ex)
			{
				throw new Exception("updateBudgetFormWithApproveDetail >> " + ex.Message);
			}
		}

		public static List<ApproveModel.approveDetailModel> getUserCreateBudgetForm(string budgetApproveId)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserCreateBudgetForm"
					, new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId) });
				var lists = (from DataRow dr in ds.Tables[0].Rows
							 select new ApproveModel.approveDetailModel()
							 {
								 empId = dr["empId"].ToString(),
								 empName = dr["empName"].ToString(),
								 empEmail = dr["empEmail"].ToString(),
								 activityNo = dr["activityNo"].ToString(),
								 delFlag = (bool)dr["delFlag"],
								 createdDate = (DateTime?)dr["createdDate"],
								 createdByUserId = dr["createdByUserId"].ToString(),
								 updatedDate = (DateTime?)dr["updatedDate"],
								 updatedByUserId = dr["updatedByUserId"].ToString()
							 }).ToList();
				return lists;
			}
			catch (Exception ex)
			{
				throw new Exception("getUserCreateBudgetForm >>" + ex.Message);
			}
		}

		public static void setCountWatingApproveBudget() 
		{
			try
			{
				if (UtilsAppCode.Session.User != null)
				{
					DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetCountWatingApproveByEmpId"
						, new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });

					UtilsAppCode.Session.User.countWatingBudgetForm = "";
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
	}
}