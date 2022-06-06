using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers   //update 21-04-2020
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

            string companyEN = Session["var_companyEN"].ToString();
            DateTime startDate = DateTime.ParseExact(Request.Form["startDate"].Trim(), "MM/dd/yyyy", null);
            DateTime endDate = DateTime.ParseExact(Request.Form["endDate"].Trim(), "MM/dd/yyyy", null);

            if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
            {
                model.budgetFormLists = getBudgetListsByEmpId(null, companyEN, startDate, endDate);
            }
            else
            {
                model.budgetFormLists = getBudgetListsByEmpId(UtilsAppCode.Session.User.empId, companyEN, startDate, endDate);
            }

            if (Request.Form["txtActivityNo"] != "")
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
            }

            if (Request.Form["ddlStatus"] != "")
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


        public ActionResult myDocBudget()
        {
            Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
            model = new Budget_Approve_Detail_Model.budgetForms();

            string companyEN = Session["var_companyEN"].ToString();
            DateTime startDate = DateTime.Now.AddDays(-30);
            DateTime endDate = DateTime.Now.AddDays(1);

            if (TempData["SearchDataModelBudget"] != null)
            {
                model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["SearchDataModelBudget"];
            }
            else
            {
                if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
                {
                    model.budgetFormLists = getBudgetListsByEmpId(null, companyEN, startDate, endDate);
                }
                else
                {
                    model.budgetFormLists = getBudgetListsByEmpId(UtilsAppCode.Session.User.empId, companyEN, startDate, endDate);
                }
            }
            return PartialView(model);
        }

        public static List<Budget_Approve_Detail_Model.budgetForm> getBudgetListsByEmpId(string empId, string companyEN, DateTime createdDateStart, DateTime createdDateEnd)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetFormByEmpId"
                    , new SqlParameter[] {
                    new SqlParameter("@empId", empId),
                    new SqlParameter("@companyEN", companyEN),
                    new SqlParameter("@createdDateStart", createdDateStart),
                    new SqlParameter("@createdDateEnd", createdDateEnd)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Budget_Approve_Detail_Model.budgetForm()
                             {
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 activityId = dr["activityId"].ToString(),
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
                ExceptionManager.WriteError("getBudgetListsByEmpId >> " + ex.Message);
                return new List<Budget_Approve_Detail_Model.budgetForm>();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult genPdfApprove(string GridHtml, string budgetApproveId)
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
                    Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)), true);


                TB_Bud_Image_Model getBudgetImageModel = new TB_Bud_Image_Model();
                getBudgetImageModel.BudImageList = ImageAppCodeBudget.getBudgetInvoiceByApproveId(budgetApproveId);

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
                var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile, budgetApproveId);

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