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

		public ActionResult Index()
		{
			SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
			return View(models);
		}

		public ActionResult searchActForm()
		{
			string count = Request.Form.AllKeys.Count().ToString();
			Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
			model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveFormLists"];

			if (Request.Form["txtActivityNo"] != "")
			{
				model.budgetFormLists = model.budgetFormLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
			}
			else if (Request.Form["ddlStatus"] != "")
			{
				model.budgetFormLists = getFilterFormByStatusId(model.budgetFormLists, int.Parse(Request.Form["ddlStatus"]));
			}
			TempData["ApproveSearchResult"] = model.budgetFormLists;
			return RedirectToAction("budgetApproveList");

		}


		//-----------------------------------------------------------------------------------------------------------------
		public ActionResult budgetApproveList()
		{

			Budget_Approve_Detail_Model.budgetForms  model = new Budget_Approve_Detail_Model.budgetForms();
			if (TempData["ApproveSearchResult"] == null)
			{
				model = new Budget_Approve_Detail_Model.budgetForms();
				model.budgetFormLists = QueryGetBudgetApprove.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
				TempData["ApproveFormLists"] = model.budgetFormLists;
				model.budgetFormLists = getFilterFormByStatusId(model.budgetFormLists, (int)AppCode.ApproveStatus.รออนุมัติ);
			}
			else
			{
				model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveSearchResult"];
			}
			return PartialView(model);
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
				var budget_approve_id = "";
				if (BudgetApproveListController.insertApproveForBudgetForm(budgetActivityId) > 0) //usp_insertApproveDetail
				{
					budget_approve_id = getApproveBudgetId(budgetActivityId);
					BudgetApproveListController.updateApproveWaitingByRangNo(budget_approve_id);

					var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id));
					GridHtml = GridHtml.Replace("<br>", "<br/>");

					AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), rootPath);

					EmailAppCodes.sendApproveBudget(budget_approve_id, AppCode.ApproveType.Budget_form,false );
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

				if (BudgetApproveController.getApproveByBudgetApproveId(budget_approve_id).approveDetailLists.Count == 0)
				{
					//ApproveFlowModel.approveFlowModel flowModel = getFlowIdBudget(ConfigurationManager.AppSettings["subjectBudgetFormId"], budgetActivityId);
					ApproveFlowModel.approveFlowModel flowModel = getFlowIdBudgetByBudgetActivityId(ConfigurationManager.AppSettings["subjectBudgetFormId"], budgetActivityId);
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
				 //,new SqlParameter("@createdByUserId", "70016911") //test_emp_id
				,new SqlParameter("@createdByUserId", UtilsAppCode.Session.User.empId) 
				});

				return rtn;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		public static int insertApproveByFlowBudget(ApproveFlowModel.approveFlowModel flowModel, string budgetId)
		{
			try
			{
				int rtn = 0;
				List<ApproveModel.approveModel> list = new List<ApproveModel.approveModel>();
				ApproveModel.approveModel model = new ApproveModel.approveModel();
				model.id = Guid.NewGuid().ToString();
				model.flowId = flowModel.flowMain.id;
				model.actFormId = budgetId;
				model.delFlag = false;
				model.createdDate = DateTime.Now;
				model.createdByUserId = UtilsAppCode.Session.User.empId; 
				//model.createdByUserId = "70016911"; //test_emp_id
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
							,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
							//,new SqlParameter("@createdByUserId","70016911")test_emp_id
							,new SqlParameter("@updatedDate",DateTime.Now)
							,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
							//,new SqlParameter("@updatedByUserId","'70016911'")test_emp_id
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
					,new SqlParameter("@updateBy", UtilsAppCode.Session.User.empId)
					//,new SqlParameter("@updateBy", "70016911") test_emp_id
					,new SqlParameter("@updateDate", DateTime.Now)
					});
			}
			catch (Exception ex)
			{
				throw new Exception("updateApproveWaitingByRangNo >>" + ex.Message);
			}
		}

		public static string getApproveBudgetId(string budgetActivityId)
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
				//throw new Exception("getFlow by actFormId >>" + ex.Message);
				return "0";
			}
		}

		
		public static ApproveFlowModel.approveFlowModel getFlowIdBudgetByBudgetActivityId(string subId, string budgetActivityId)
		{
			try
			{
				ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFlowByBudgetActivtyId"
					, new SqlParameter[] {
						  new SqlParameter("@subId",subId)
						, new SqlParameter("@budgetActivityId",budgetActivityId)
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
				throw new Exception("getFlow Budget By BudgetActivityId >>" + ex.Message);
			}
		}

		public static ApproveFlowModel.approveFlowModel getFlowIdByBudgetApproveId( string budget_approve_id)
		{
			try
			{
				ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFlowIdByBudgetApproveId"
					//DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFlowIdByBudgetActivityId"
					, new SqlParameter[] {
						new SqlParameter("@budgetApproveId",budget_approve_id)
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
	}
}