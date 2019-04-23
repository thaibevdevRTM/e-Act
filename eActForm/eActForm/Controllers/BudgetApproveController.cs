using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;

namespace eActForm.Controllers
{
	[LoginExpire]

	public class BudgetApproveController : Controller
    {
		// GET: BudgetApprove
		public ActionResult Index()
		{
			SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
			return View(models);
		}

		public ActionResult budgetApproveList()
		{
			Activity_Model.actForms model = new Activity_Model.actForms();
			if (TempData["ApproveSearchResult"] == null)
			{
				model = new Activity_Model.actForms();
				model.actLists = ApproveListAppCode.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
				TempData["ApproveFormLists"] = model.actLists;
				model.actLists = ApproveListAppCode.getFilterFormByStatusId(model.actLists, (int)AppCode.ApproveStatus.รออนุมัติ);
			}
			else
			{
				model.actLists = (List<Activity_Model.actForm>)TempData["ApproveSearchResult"];
			}
			return PartialView(model);
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

		public static int insertApproveForBudgetForm(string budgetId)
		{
			try
			{
				if (getApproveByBudgetId(budgetId).approveDetailLists.Count == 0)
				{
					ApproveFlowModel.approveFlowModel flowModel = getFlowIdBudget(ConfigurationManager.AppSettings["subjectBudgetFormId"], budgetId);
					return insertApproveByFlowBudget(flowModel, budgetId);
				}
				else return 999; // alredy approve
			}
			catch (Exception ex)
			{
				throw new Exception("insertApproveBudget >> " + ex.Message);
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
					ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveByActFormId"
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

		public static ApproveFlowModel.approveFlowModel getFlowIdBudget(string subId, string budgetId)
		{
			try
			{
				ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFlowIdByBudgetId"
					, new SqlParameter[] {
						  new SqlParameter("@subId",subId)
						, new SqlParameter("@budgetId",budgetId)
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
							,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
							,new SqlParameter("@updatedDate",DateTime.Now)
							,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
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
					,new SqlParameter("@updateDate", DateTime.Now)
					});
			}
			catch (Exception ex)
			{
				throw new Exception("updateApproveWaitingByRangNo >>" + ex.Message);
			}
		}
		//---------------------------------------------------------------------------------------------------------------





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