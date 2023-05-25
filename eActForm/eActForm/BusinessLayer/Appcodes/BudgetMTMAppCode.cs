using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using System.Globalization;
using WebLibrary;


//namespace eActForm.BusinessLayer
//{
//    //public class BudgetApproveListAppCode
//    //{
//    //    public static List<Activity_Model.actForm> getFilterFormByStatusId(List<Activity_Model.actForm> lists, int statusId)
//    //    {
//    //        try
//    //        {
//    //            return lists.Where(r => r.statusId == statusId.ToString()).ToList();
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            throw new Exception("getFilterFormByStatusId >> " + ex.Message);
//    //        }
//    //    }

//    //    public static List<Activity_Model.actForm> getApproveListsByEmpId(string empId)
//    //    {
//    //        try
//    //        {
//    //            DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveFormByEmpId"
//    //                , new SqlParameter[] { new SqlParameter("@empId", empId) });
//    //            var lists = (from DataRow dr in ds.Tables[0].Rows
//    //                         select new Activity_Model.actForm()
//    //                         {
//    //                             id = dr["id"].ToString(),
//    //                             statusId = dr["statusId"].ToString(),
//    //                             statusName = dr["statusName"].ToString(),
//    //                             activityNo = dr["activityNo"].ToString(),
//    //                             documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],
//    //                             reference = dr["reference"].ToString(),
//    //                             customerId = dr["customerId"].ToString(),
//    //                             channelName = dr["channelName"].ToString(),
//    //                             productTypeId = dr["productTypeId"].ToString(),
//    //                             productTypeNameEN = dr["nameEN"].ToString(),
//    //                             cusShortName = dr["cusShortName"].ToString(),
//    //                             productCategory = dr["productCateText"].ToString(),
//    //                             productGroup = dr["productGroupId"].ToString(),
//    //                             groupName = dr["groupName"].ToString(),
//    //                             activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
//    //                             activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
//    //                             costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
//    //                             costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
//    //                             activityName = dr["activityName"].ToString(),
//    //                             theme = dr["theme"].ToString(),
//    //                             objective = dr["objective"].ToString(),
//    //                             trade = dr["trade"].ToString(),
//    //                             activityDetail = dr["activityDetail"].ToString(),
//    //                             delFlag = (bool)dr["delFlag"],
//    //                             createdDate = (DateTime?)dr["createdDate"],
//    //                             createdByUserId = dr["createdByUserId"].ToString(),
//    //                             updatedDate = (DateTime?)dr["updatedDate"],
//    //                             updatedByUserId = dr["updatedByUserId"].ToString(),
//    //                             normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
//    //                             themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
//    //                             totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
//    //                             createByUserName = dr["createByName"].ToString(),
//    //                         }).ToList();
//    //            return lists;
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            throw new Exception("getApproveListsByEmpId >> " + ex.Message);
//    //        }
//    //    }
//    //}

//    public class BudgetFormCommandHandler
//    {
//        //update Invoice Product
//        public static int updateInvoiceProduct(Budget_Activity_Model.Budget_Activity_Invoice_Att model)
//        {
//            int result = 0;
//            model.dateInvoiceAction = DateTime.ParseExact(model.invoiceActionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

//            try
//            {

//                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityInvoiceUpdate"
//                    , new SqlParameter[] {new SqlParameter("@id", model.invoiceId)
//                    ,new SqlParameter("@activityId",model.activityId)
//                    ,new SqlParameter("@activityNo",model.activityNo)
//                    ,new SqlParameter("@productId",model.productId)
//                    ,new SqlParameter("@activityOfEstimateId",model.activityOfEstimateId)
//                    ,new SqlParameter("@paymentNo",model.paymentNo)

//                    ,new SqlParameter("@budgetImageId",model.budgetImageId)

//                    ,new SqlParameter("@invoiceBudgetStatusId",model.invoiceBudgetStatusId)
//                    ,new SqlParameter("@invoiceNo",model.invoiceNo)
//                    ,new SqlParameter("@invoiceTotalBath",model.invoiceTotalBath)
//                    ,new SqlParameter("@actionDate",model.dateInvoiceAction) //invoiceActionDate
//                    ,new SqlParameter("@invoiceRemark",model.invoiceRemark)

//                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
//                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
//                    });
//            }
//            catch (Exception ex)
//            {
//                ExceptionManager.WriteError(ex.Message + ">> usp_mtm_BudgetActivityInvoiceUpdate");
//            }

//            return result;
//        }

//        public static int insertInvoiceProduct(Budget_Activity_Model.Budget_Activity_Invoice_Att model)
//        {

//            int result = 0;
//            model.dateInvoiceAction = DateTime.ParseExact(model.invoiceActionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

//            try
//            {

//                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityInvoiceInsert"
//                    , new SqlParameter[] {new SqlParameter("@id", Guid.NewGuid().ToString())
//                    ,new SqlParameter("@activityId",model.activityId)
//                    ,new SqlParameter("@activityNo",model.activityNo)
//                    ,new SqlParameter("@productId",model.productId)
//                    ,new SqlParameter("@activityOfEstimateId",model.activityOfEstimateId)
//                    ,new SqlParameter("@paymentNo",model.paymentNo)
//                    ,new SqlParameter("@budgetImageId",model.budgetImageId)

//                    ,new SqlParameter("@invoiceBudgetStatusId",model.invoiceBudgetStatusId)
//                    ,new SqlParameter("@invoiceNo",model.invoiceNo)
//                    ,new SqlParameter("@invoiceTotalBath",model.invoiceTotalBath)
//					//,new SqlParameter("@actionDate",model.invoiceActionDate)
//					,new SqlParameter("@actionDate",model.dateInvoiceAction)
//                    ,new SqlParameter("@invoiceRemark",model.invoiceRemark)
//                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
//                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
//                    });
//            }
//            catch (Exception ex)
//            {
//                ExceptionManager.WriteError(ex.Message + ">> usp_mtm_BudgetActivityInvoiceInsert");
//            }

//            return result;
//        }

//        public static int deleteInvoiceProduct(string activityId, string estimateId, string invoiceId, string delType)
//        {

//            int result = 0;

//            try
//            {

//                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivityInvoiceDelete"
//                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
//                    ,new SqlParameter("@activityOfEstimateId",estimateId)
//                    ,new SqlParameter("@invoiceId",invoiceId)
//                    ,new SqlParameter("@delType",delType)
//                    });
//            }
//            catch (Exception ex)
//            {
//                ExceptionManager.WriteError(ex.Message + ">> usp_mtm_BudgetActivityInvoiceDelete");
//            }

//            return result;
//        }

//        public static int deleteBudgetApproveByActNo(string activityNo)
//        {

//            int result = 0;

//            try
//            {

//                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveDelete"
//                    , new SqlParameter[] {new SqlParameter("@activityNo",activityNo)
//                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
//                    });
//            }
//            catch (Exception ex)
//            {
//                ExceptionManager.WriteError(ex.Message + ">> deleteBudgetApproveByActNo");
//            }

//            return result;
//        }

//    }


//}