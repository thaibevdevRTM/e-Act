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
		public ActionResult Index(string budgetActivityId)
		{
			if (budgetActivityId == null) return RedirectToAction("index", "Home");
			else
			{
				var budgetApproveId = BudgetApproveListController.getApproveBudgetId(budgetActivityId);
				ApproveModel.approveModels models = getApproveByBudgetApproveId(budgetApproveId);
				models.approveStatusLists = ApproveAppCode.getApproveStatus(AppCode.StatusType.app).Where(x => x.id == "3" || x.id == "5").ToList();
				return View(models);
			}
		}

		public ActionResult approvePositionSignatureLists(string actId)
		{
			var budget_approve_id = BudgetApproveListController.getApproveBudgetId(actId);

			ApproveModel.approveModels models = getApproveByBudgetApproveId(budget_approve_id);
			ApproveFlowModel.approveFlowModel flowModel = BudgetApproveListController.getFlowIdBudget(ConfigurationManager.AppSettings["subjectBudgetFormId"], budget_approve_id);
			models.approveFlowDetail = flowModel.flowDetail;
			return PartialView(models);

		}


		public PartialViewResult previewApproveBudget(string activityId)
		{
			Budget_Activity_Model Budget_Model = new Budget_Activity_Model();
			Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(activityId);
			Budget_Model.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivity(null, activityId, null);
			return PartialView(Budget_Model);
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
												 ImgName = string.Format(ConfigurationManager.AppSettings["rootgetSignaURL"], dr["empId"].ToString()),
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



		[HttpPost]
		public JsonResult insertApprove()
		{
			var result = new AjaxResult();
			result.Success = false;
			try
			{
				if (updateApprove(Request.Form["lblActFormId"], Request.Form["ddlStatus"], Request.Form["txtRemark"], Request.Form["lblApproveType"]) > 0)
				{
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
				var var_budget_approve_id = BudgetApproveListController.getApproveBudgetId(actFormId);

				int rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateApprove"
						, new SqlParameter[] {new SqlParameter("@actFormId",var_budget_approve_id)
					//, new SqlParameter("@empId",UtilsAppCode.Session.User.empId)
					, new SqlParameter("@empId","11025855") //70016911
					,new SqlParameter("@statusId",statusId)
					,new SqlParameter("@remark",remark)
					,new SqlParameter("@updateDate",DateTime.Now)
					//,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
					,new SqlParameter("@updateBy","70016911")
						});


				if (approveType == "ActivityBudget")
				{
					rtn = updateBudgetFormStatus(statusId, var_budget_approve_id); // test
				}
				//else if (approveType == AppCode.ApproveType.Report_Detail.ToString())
				//{
				//	//
				//	rtn = updateActRepDetailStatus(statusId, var_budget_approve_id);
				//}

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
				throw new Exception(ex.Message);
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


		public static void setCountWatingApproveBudget() 
		{
			try
			{
				if (UtilsAppCode.Session.User != null)
				{
					DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetCountWatingApproveByEmpId"
						//, new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
						, new SqlParameter[] { new SqlParameter("@empId", "11025855") });
					//11025855
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