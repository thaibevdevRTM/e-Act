using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using System.IO;

namespace eActForm.Controllers //update 21-04-2020
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
                ApproveModel.approveModels models = getApproveByBudgetApproveId(budgetApproveId);
                models.approveStatusLists = ApproveAppCode.getApproveStatus(AppCode.StatusType.app).Where(x => x.id == "3" || x.id == "5").ToList();
                return View(models);
            }
        }

        public ActionResult approveLists(ApproveModel.approveModels models)
        {
            return PartialView(models);
        }

        public ActionResult approvePositionSignatureLists(string actId, string companyEN)
        {
            ApproveModel.approveModels models = getApproveByBudgetApproveId(actId);
            if (companyEN == "MT")
            {
                ApproveFlowModel.approveFlowModel flowModel = BudgetApproveController.getFlowIdBudgetByBudgetActivityId(ConfigurationManager.AppSettings["subjectBudgetFormId"], actId);
                models.approveFlowDetail = flowModel.flowDetail;

                //ApproveFlowModel.approveFlowModel flowModel = BudgetApproveController.getFlowIdBudgetByBudgetActivityId(ConfigurationManager.AppSettings["subjectBudgetFormId"], actId);
                //var modelApproveDetail = models.approveDetailLists;
                //if (modelApproveDetail.Any())
                //{
                //    bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], actId)));
                //    if (!folderExists)
                //        Directory.CreateDirectory(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], actId)));

                //    foreach (var item in modelApproveDetail)
                //    {
                //        UtilsAppCode.Session.writeFileHistory(System.Web.HttpContext.Current.Server
                //            , item.signature
                //            , string.Format(ConfigurationManager.AppSettings["rootSignaByActURL"], actId, item.empId));
                //    }

                //}
            }
            else
            {
                ApproveFlowModel.approveFlowModel flowModel = BudgetApproveController.getFlowIdBudgetByBudgetActivityIdOMT(ConfigurationManager.AppSettings["subjectBudgetFormId"], actId);
                models.approveFlowDetail = flowModel.flowDetail;


            }
            return PartialView(models);
        }

        public ActionResult approvePositionSignatureListsByBudgetApproveId(string budgetApproveId)
        {
            ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = ApproveAppCode.getApproveByActFormId(budgetApproveId);
                ApproveFlowModel.approveFlowModel flowModel = BudgetApproveController.getFlowIdByBudgetApproveId(budgetApproveId);
                models.approveFlowDetail = flowModel.flowDetail;

                var modelApproveDetail = models.approveDetailLists.ToList();

                if (modelApproveDetail.Any())
                {
                    bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)));
                    if (!folderExists)
                        Directory.CreateDirectory(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)));

                    foreach (var item in modelApproveDetail)
                    {
                        UtilsAppCode.Session.writeFileHistory(System.Web.HttpContext.Current.Server
                            , item.signature
                            , string.Format(ConfigurationManager.AppSettings["rootSignaByActURL"], budgetApproveId, item.empId));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["approvePositionSignatureError"] = AppCode.StrMessFail + ex.Message;
            }
            return PartialView(models);
        }

        public PartialViewResult previewApproveBudget(string budgetApproveId)
        {
            Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();
            Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(null, budgetApproveId);
            Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, null, null, budgetApproveId, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null).FirstOrDefault();
            Budget_Model.Budget_Approve_detail_list = QueryGetBudgetApprove.getBudgetApproveId(budgetApproveId);
            return PartialView(Budget_Model);
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
                return "0";
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

        public static ApproveModel.approveModels getApproveByBudgetApproveId(string budgetApproveId)
        {
            try
            {
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

        public static ApproveFlowModel.approveFlowModel getFlowIdBudgetByBudgetActivityIdOMT(string subId, string budgetActivityId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFlowByBudgetActivtyIdOMT"
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

        public static ApproveFlowModel.approveFlowModel getFlowIdByBudgetApproveId(string budget_approve_id)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFlowIdByBudgetApproveId"
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
                                 empGroup = dr["empGroup"].ToString(),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetail >>" + ex.Message);
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

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult genPdfApproveBudget(string GridHtml, string statusId, string budgetApproveId)
        {
            var resultAjax = new AjaxResult();
            try
            {

                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId + "_");
                GridHtml = GridHtml.Replace("<br>", "<br/>");
                AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

                //del signature file
                bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)));
                if (folderExists)
                    Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)),true);

                TB_Bud_Image_Model getBudgetImageModel = new TB_Bud_Image_Model();
                getBudgetImageModel.BudImageList = ImageAppCodeBudget.getImageBudgetByApproveId(budgetApproveId);

                string[] pathFile = new string[getBudgetImageModel.BudImageList.Count + 1];
                pathFile[0] = Server.MapPath(rootPathInsert);

                if (getBudgetImageModel.BudImageList.Any())
                {
                    int i = 1;
                    foreach (var item in getBudgetImageModel.BudImageList)
                    {
                        if (System.IO.File.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName))))
                        {
                            pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName));
                        }
                        else
                        {
                            pathFile = pathFile.Where((val, idx) => idx != i).ToArray();
                        }
                        i++;
                    }
                }

                var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId));
                var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile);



                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    EmailAppCodes.sendRejectBudget(budgetApproveId, AppCode.ApproveType.Budget_form);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    EmailAppCodes.sendApproveBudget(budgetApproveId, AppCode.ApproveType.Budget_form, false);
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

        public static int insertApproveBudgetDetail(string budgetActivityId)
        {
            try
            {
                int rtn = 0;

                SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertBudgetApproveDetail"
                , new SqlParameter[]
                {
                 new SqlParameter("@budgetActivityId", budgetActivityId)
                ,new SqlParameter("@createdByUserId", UtilsAppCode.Session.User.empId)
                });

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static int insertApproveByFlowBudget(ApproveFlowModel.approveFlowModel flowModel, string budgetId, Int32 count_req_app)
        {
            try
            {
                string statusId = "";
                int rtn = 0;

                if (count_req_app == 0) { statusId = "3"; }

                List<ApproveModel.approveModel> list = new List<ApproveModel.approveModel>();
                ApproveModel.approveModel model = new ApproveModel.approveModel();
                model.id = Guid.NewGuid().ToString();
                model.flowId = flowModel.flowMain.id;
                model.actFormId = budgetId;
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
                            ,new SqlParameter("@statusId",statusId)
                            ,new SqlParameter("@isSendEmail",false)
                            ,new SqlParameter("@remark","")

                            ,new SqlParameter("@isApproved",true)

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

        public static int insertApproveForBudgetForm(string budgetActivityId, string companyEN, Int32 count_req_app)
        {
            try
            {
                insertApproveBudgetDetail(budgetActivityId);
                var budget_approve_id = getApproveBudgetId(budgetActivityId);

                if (BudgetApproveController.getApproveByBudgetApproveId(budget_approve_id).approveDetailLists.Count == 0)
                {
                    if (companyEN == "MT")
                    {
                        ApproveFlowModel.approveFlowModel flowModel = getFlowIdBudgetByBudgetActivityId(ConfigurationManager.AppSettings["subjectBudgetFormId"], budgetActivityId);
                        return insertApproveByFlowBudget(flowModel, budget_approve_id, count_req_app);
                    }
                    else
                    {
                        ApproveFlowModel.approveFlowModel flowModel = getFlowIdBudgetByBudgetActivityIdOMT(ConfigurationManager.AppSettings["subjectBudgetFormId"], budgetActivityId);
                        return insertApproveByFlowBudget(flowModel, budget_approve_id, count_req_app);
                    }
                }
                else return 999; // alredy approve
            }
            catch (Exception ex)
            {
                throw new Exception("insertApproveBudget >> " + ex.Message);
            }
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

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult submitPreviewBudget(string GridHtml, string budgetActivityId, string companyEN, string count_req_approve)
        {

            var resultAjax = new AjaxResult();
            try
            {
                string budget_approve_id = "";
                int count_req_app = Int32.Parse(count_req_approve);

                if (BudgetApproveController.insertApproveForBudgetForm(budgetActivityId, companyEN, count_req_app) > 0) //usp_insertApproveDetail
                {
                    budget_approve_id = BudgetApproveController.getApproveBudgetId(budgetActivityId); // get last approve id
                    BudgetApproveController.updateApproveWaitingByRangNo(budget_approve_id);

                    var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id + "_");
                    GridHtml = GridHtml.Replace("<br>", "<br/>");

                    AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

                    //del signature file
                    bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budget_approve_id)));
                    if (folderExists)
                        Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budget_approve_id)),true);


                    TB_Bud_Image_Model getBudgetImageModel = new TB_Bud_Image_Model();
                    getBudgetImageModel.BudImageList = ImageAppCodeBudget.getImageBudgetByApproveId(budget_approve_id);

                    string[] pathFile = new string[getBudgetImageModel.BudImageList.Count + 1];
                    pathFile[0] = Server.MapPath(rootPathInsert);

                    if (getBudgetImageModel.BudImageList.Any())
                    {
                        int i = 1;
                        foreach (var item in getBudgetImageModel.BudImageList)
                        {
                            if (System.IO.File.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName))))
                            {
                                pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName));
                            }
                            else
                            {
                                pathFile = pathFile.Where((val, idx) => idx != i).ToArray();
                            }
                            i++;
                        }
                    }

                    var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id));
                    var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile);

                    BudgetApproveController.setCountWatingApproveBudget();
                    if (count_req_app > 0)
                    {
                        EmailAppCodes.sendApproveBudget(budget_approve_id, AppCode.ApproveType.Budget_form, false);
                    }
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

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult submitPreviewBudgetRegenPdf(string GridHtml, string budgetActivityId, string companyEN, string count_req_approve)
        {

            var resultAjax = new AjaxResult();
            try
            {
                string budget_approve_id = "";

                budget_approve_id = BudgetApproveController.getApproveBudgetId(budgetActivityId); // get last approve id


                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id + "_");
                GridHtml = GridHtml.Replace("<br>", "<br/>");

                AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));
                // del signature file
                bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budget_approve_id)));
                if (folderExists)
                    Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budget_approve_id)),true);

                TB_Bud_Image_Model getBudgetImageModel = new TB_Bud_Image_Model();
                getBudgetImageModel.BudImageList = ImageAppCodeBudget.getImageBudgetByApproveId(budget_approve_id);

                string[] pathFile = new string[getBudgetImageModel.BudImageList.Count + 1];
                pathFile[0] = Server.MapPath(rootPathInsert);

                if (getBudgetImageModel.BudImageList.Any())
                {
                    int i = 1;
                    foreach (var item in getBudgetImageModel.BudImageList)
                    {
                        if (System.IO.File.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName))))
                        {
                            pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName));
                        }
                        else
                        {
                            pathFile = pathFile.Where((val, idx) => idx != i).ToArray();
                        }
                        i++;
                    }
                }

                var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id));
                var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile);


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

    }
}