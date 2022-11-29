using eActForm.BusinessLayer;
using eActForm.Models;
using eForms.Models.Forms;
using eForms.Models.Reports;
using eForms.Presenter.Reports;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using WebLibrary;
using Microsoft.ApplicationBlocks.Data;


namespace eActForm.Controllers
{
    [LoginExpire]
    public class actFormRepPostEvaController : Controller
    {
        // GET: actFormRepPostEva
        public ActionResult index(string typeForm)
        {
            SearchBudgetActivityPosEVAModels models = getPostEVAMasterDataForSearchReport(typeForm);
            models.customerslist = models.customerslist;
            return View(models);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult searchActForm()
        {
            try
            {

                return RedirectToAction("postEvaListView", new
                {
                    startDate = Request.Form["startDate"],
                    endDate = Request.Form["endDate"],
                    customerId = Request.Form["ddlCustomer"],
                    mountText = Request.Form["reportDate"],
                    productType = Request.Form["ddlProductType"],
                    productGroup = Request.Form["ddlProductGrp"],
                    productBrand = Request.Form["ddlProductBrand"],
                    actType = Request.Form["ddlTheme"]
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SearchBudgetActivityPosEVAModels getPostEVAMasterDataForSearchReport(string typeForm)
        {
            try
            {
                SearchBudgetActivityPosEVAModels models = new SearchBudgetActivityPosEVAModels();

                models.customerslist = getPostEVACustomers().ToList();
                models.productBrandList = getPostEVAProductBrand().OrderBy(x => x.brandName).ToList();
                models.activityGroupList = getPostEVAActivityType().Where(x => x.activityCondition.Contains("MT"))
                 .GroupBy(item => item.activitySales)
                 .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getPostEVAMasterDataForSearchReport >>" + ex.Message);
            }
        }
        public static List<TB_Act_Customers_Model.Customers_Model> getPostEVACustomers()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportPostEva_Customer"
                     , new SqlParameter[]
                     {
                         new SqlParameter("@company_id", @UtilsAppCode.Session.User.empCompanyId),
                         new SqlParameter("@emp_id", @UtilsAppCode.Session.User.empId)
                     });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Customers_Model.Customers_Model()
                             {
                                 id = d["id"].ToString(),
                                 cusTrading = d["cusTrading"].ToString(),
                                 cusNameTH = d["cusNameTH"].ToString(),
                                 cusNameEN = d["cusNameEN"].ToString(),
                                 cusShortName = d["cusShortName"].ToString(),
                                 cust = d["cust"].ToString(),
                                 chanel_Id = d["chanel_Id"].ToString(),
                                 delFlag = d["delFlag"].ToString(),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getgetPostEVACustomersMT >> " + ex.Message);
            }
        }
        public static List<TB_Act_ProductBrand_Model> getPostEVAProductBrand()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllProductBrand");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ProductBrand_Model()
                             {
                                 id = d["id"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 productGroupId = d["productGroupId"].ToString(),
                                 digit_EO = d["digit_EO"].ToString(),
                                 digit_IO = d["digit_IO"].ToString(),
                                 no_tbmmkt = d["no_tbmmkt"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllBrand => " + ex.Message);
                return new List<TB_Act_ProductBrand_Model>();
            }
        }
        public static List<TB_Act_ActivityGroup_Model> getPostEVAActivityType()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportPostEvaActivityGroup");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ActivityGroup_Model()
                             {
                                 id = d["id"].ToString(),
                                 activityTypeId = d["id"].ToString(),
                                 activitySales = d["activitySales"].ToString(),
                                 activityAccount = d["activityAccount"].ToString(),
                                 gl = d["gl"].ToString(),
                                 digit_Group = d["digit_Group"].ToString() + d["digit_SubGroup"].ToString(),
                                 digit_SubGroup = d["digit_SubGroup"].ToString(),
                                 activityCondition = d["activityCondition"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.OrderBy(x => x.activitySales).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getPostEVAActivityType => " + ex.Message);
                return new List<TB_Act_ActivityGroup_Model>();
            }
        }


        public ActionResult postEvaListView(string startDate, string endDate, string customerId, string mountText, string productType, string productGroup, string productBrand, string actType)
        {
            RepPostEvaModels model = null;
            RepPostEvaGroupBudgetStatusModels modelGroupBudgetStatus = null;

            try
            {
                ViewBag.mountText = mountText;
                model = getDataPostEva(AppCode.StrCon, startDate, endDate, customerId, null, productBrand, actType);
                Session["postEvaModel"] = model;


                modelGroupBudgetStatus = getDataPostEvaGroupBudgetStatus(AppCode.StrCon, startDate, endDate, customerId, actType);
                Session["postEvaGroupBudgetStatus"] = modelGroupBudgetStatus;

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("postEvaListView >> " + ex.Message);
            }
            return PartialView(model);
        }

        public ActionResult previewEvaByActId(string actId)
        {
            RepPostEvaModels model = null;
            try
            {
                ViewBag.actId = actId;
                model = getDataPostEva(AppCode.StrCon, "", "", "", actId, null,null);
                Session["postEvaModel"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("previewEvaByActId >> " + ex.Message);
            }
            return PartialView(model);
        }

        public ActionResult postEvaGroupBrandView(RepPostEvaModels model)
        {
            try
            {
                //model.repPostEvaGroupBrand = RepPostEvaPresenter.getPostEvaGroupByBrand(model.repPostEvaLists);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("PostEvaGroupBrandView >> " + ex.Message);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult postEvaGroupBrandData()
        {
            List<RepPostEvaGroup> list = null;
            try
            {
                RepPostEvaModels model = (RepPostEvaModels)Session["postEvaModel"];
                list = getPostEvaGroupByBrand(model.repPostEvaLists);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("PostEvaGroupBrandView >> " + ex.Message);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult postEvaGroupBudgetStatusData()
        {
            List<RepPostEvaGroupBudgetStatus> list = null;
            
            try
            {
                RepPostEvaGroupBudgetStatusModels model = (RepPostEvaGroupBudgetStatusModels)Session["postEvaGroupBudgetStatus"];

                list = model.repPostEvaGroupBudgetStatus.GroupBy(x => x.countActApprove)
                .Select(cl => new RepPostEvaGroupBudgetStatus
                {
                    countActApprove = cl.First().countActApprove,
                    countBudgetActive = cl.First().countBudgetActive,
                    countBudgetInactive = cl.First().countBudgetInactive,

                }).ToList();

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("postEvaGroupBudgetStatusData >> " + ex.Message);
            }
             
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public static RepPostEvaModels getDataPostEva(string strConn, string startDate, string endDate, string customerId, string actId, string brandId, string actTypeId)
        {
            try
            {
                RepPostEvaModels model = new RepPostEvaModels();
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getReportPostEva"
                    , new SqlParameter[] {new SqlParameter("@startDate",startDate)
                    , new SqlParameter("@endDate",endDate)
                    , new SqlParameter("@customerId",customerId)
                    , new SqlParameter("@actId",actId)
                    , new SqlParameter("@brandId",brandId)
                    , new SqlParameter("@actTypeId",actTypeId)
                    });

                #region toLists
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new RepPostEvaModel()
                             {
                                 id = dr["id"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 customerName = dr["customerName"].ToString(),
                                 preLoadStock = dr["preLoadStock"].ToString(),
                                 activitySales = dr["activitySales"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 activityName = dr["activityName"].ToString(),
                                 productCode = dr["productCode"].ToString(),
                                 productName = dr["productName"].ToString(),
                                 brandName = dr["brandName"].ToString(),
                                 groupName = dr["groupName"].ToString(),
                                 cusNameEN = dr["cusNameEN"].ToString(),
                                 size = dr["size"].ToString(),
                                 activityPeriodSt = (DateTime?)dr["activityPeriodSt"],
                                 activityPeriodEnd = (DateTime?)dr["activityPeriodEnd"],
                                 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                 le = dr["activitySales"].ToString() == "Promotion Support" ? dr["le"] is DBNull ? 0 : Convert.ToDouble(dr["le"].ToString()) : 100,
                                 unit = dr["unit"].ToString(),
                                 compensate = dr["compensate"].ToString(),
                                 dayAddStart = dr["dayAddStart"].ToString(),
                                 dayAddEnd = dr["dayAddEnd"].ToString(),
                                 productId = dr["productId"].ToString(),
                                 normalCost = dr["normalCost"] is DBNull ? 0 : Convert.ToDouble(dr["normalCost"].ToString()),
                                 themeCost = dr["themeCost"] is DBNull ? 0 : Convert.ToDouble(dr["themeCost"].ToString()),
                                 total = dr["total"] is DBNull ? 0 : Convert.ToDouble(dr["total"].ToString()),
                                 tempAPNormalCost = dr["tempAPNormalCost"] is DBNull ? 0 : Convert.ToDouble(dr["tempAPNormalCost"].ToString()),
                                 estimateSaleBathAll = dr["estimateSaleBathAll"] is DBNull ? 0 : Convert.ToDouble(dr["estimateSaleBathAll"].ToString()),
                                 actReportQuantity = dr["actReportQuantity"] is DBNull ? 0 : Convert.ToDouble(dr["actReportQuantity"].ToString()),
                                 actVolumeQuantity = dr["actVolumeQuantity"] is DBNull ? 0 : Convert.ToDouble(dr["actVolumeQuantity"].ToString()),
                                 actAmount = dr["actAmount"] is DBNull ? 0 : Convert.ToDouble(dr["actAmount"].ToString()),
                                 saleActual = dr["saleActual"] is DBNull ? 0 : Convert.ToDouble(dr["saleActual"].ToString()),
                                 billedQuantityMT = dr["billedQuantityMT"] is DBNull ? 0 : Convert.ToDouble(dr["billedQuantityMT"].ToString()),
                                 volumeMT = dr["volumeMT"] is DBNull ? 0 : Convert.ToDouble(dr["volumeMT"].ToString()),
                                 netValueMT = dr["netValueMT"] is DBNull ? 0 : Convert.ToDouble(dr["netValueMT"].ToString()),
                                 specialDiscountMT = dr["specialDiscountMT"] is DBNull ? 0 : Convert.ToDouble(dr["specialDiscountMT"].ToString()),
                                 activityTypeId = dr["activityTypeId"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productGroupId = dr["productGroupId"].ToString(),
                                 productBrandId = dr["productBrandId"].ToString(),

                                 countActApprove = dr["countActApprove"] is DBNull ? 0 : Convert.ToDouble(dr["countActApprove"].ToString()),
                                 countBudgetActive = dr["countBudgetActive"] is DBNull ? 0 : Convert.ToDouble(dr["countBudgetActive"].ToString()),
                                 countBudgetInactive = dr["countBudgetInactive"] is DBNull ? 0 : Convert.ToDouble(dr["countBudgetInactive"].ToString()),

                             }).ToList();
                #endregion

                model.repPostEvaLists = lists.OrderBy(x => x.activityNo).OrderBy(x => x.activityPeriodSt).ToList();


                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getDataPostEva >> " + ex.Message);
            }
        }

        public static RepPostEvaGroupBudgetStatusModels getDataPostEvaGroupBudgetStatus(string strConn, string startDate, string endDate, string customerId, string actTypeId)
        {
            try
            {
                RepPostEvaGroupBudgetStatusModels model = new RepPostEvaGroupBudgetStatusModels();
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getReportPostEvaBudgetStatusSummary"
                    , new SqlParameter[] {new SqlParameter("@startDate",startDate)
                    , new SqlParameter("@endDate",endDate)
                    , new SqlParameter("@customerId",customerId)
                    , new SqlParameter("@actTypeId",actTypeId)
                    });

                #region toLists
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new RepPostEvaGroupBudgetStatus()
                             {
                                 countActApprove = dr["countActApprove"] is DBNull ? 0 : Convert.ToDouble(dr["countActApprove"].ToString()),
                                 countBudgetActive = dr["countBudgetActive"] is DBNull ? 0 : Convert.ToDouble(dr["countBudgetActive"].ToString()),
                                 countBudgetInactive = dr["countBudgetInactive"] is DBNull ? 0 : Convert.ToDouble(dr["countBudgetInactive"].ToString()),

                             }).ToList();
                #endregion

                model.repPostEvaGroupBudgetStatus = lists.ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getDataPostEva >> " + ex.Message);
            }
        }

        public static List<RepPostEvaGroup> getPostEvaGroupByBrand(List<RepPostEvaModel> model)
        {
            try
            {
                var list = model.GroupBy(x => x.brandName)
                                .Select(cl => new RepPostEvaGroup
                                {
                                    name = cl.First().brandName.Trim(),
                                    value = (cl.Sum(c => c.total)).ToString(),
                                    sumActSalesParti = (cl.Sum(c => c.actAmount)).ToString(),
                                    sumNormalCase = (cl.Sum(c => c.normalCost)),
                                    sumPromotionCase = (cl.Sum(c => c.themeCost)),
                                    sumSalesInCase = (cl.Sum(c => c.actReportQuantity)),
                                    countGroup = cl.Count().ToString(),
                                    accuracySpendingBath = (cl.Sum(c => c.accuracySpendingBath)),
                                    saleActual = (cl.Sum(c => c.saleActual)),
                                    tempAPNormalCost = (cl.Sum(c => c.tempAPNormalCost)),
                                    estimateSaleBathAll = (cl.Sum(c => c.estimateSaleBathAll)),
                                    total = (cl.Sum(c => c.total)),
                                    actAmount = (cl.Sum(c => c.actAmount)),
                                    countActApprove = cl.First().countActApprove,
                                    countBudgetActive = cl.First().countBudgetActive,
                                    countBudgetInactive = cl.First().countBudgetInactive,

                                }).ToList();

                return list;

            }
            catch (Exception ex)
            {
                throw new Exception("getPostEvaGroupByBrand >> " + ex.Message);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult repExportExcel(string gridHtml)
        {
            try
            {
                //RepDetailModel.actFormRepDetails model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
                //gridHtml = gridHtml.Replace("\n", "<br>");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "DetailReport.xls");
        }

        
        
    }

}
