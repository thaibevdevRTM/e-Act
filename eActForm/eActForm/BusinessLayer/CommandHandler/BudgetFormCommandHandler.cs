using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class BudgetFormCommandHandler
    {
        //update Invoice Product
        public static int updateInvoiceProduct(Budget_Activity_Model.Budget_Activity_Invoice_Att model)
        {
            int result = 0;
            model.dateInvoiceAction = DateTime.ParseExact(model.invoiceActionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateBudgetActivityInvoice"
                    , new SqlParameter[] {new SqlParameter("@id", model.invoiceId)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@activityNo",model.activityNo)
                    ,new SqlParameter("@productId",model.productId)
                    ,new SqlParameter("@activityOfEstimateId",model.activityOfEstimateId)
                    ,new SqlParameter("@paymentNo",model.paymentNo)

                    ,new SqlParameter("@budgetImageId",model.budgetImageId)

                    ,new SqlParameter("@invoiceBudgetStatusId",model.invoiceBudgetStatusId)
                    ,new SqlParameter("@invoiceNo",model.invoiceNo)
                    ,new SqlParameter("@invoiceTotalBath",model.invoiceTotalBath)
                    ,new SqlParameter("@actionDate",model.dateInvoiceAction) //invoiceActionDate
					,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_updateBudgetActivityInvoice");
            }

            return result;
        }

        public static int insertInvoiceProduct(Budget_Activity_Model.Budget_Activity_Invoice_Att model)
        {

            int result = 0;
            model.dateInvoiceAction = DateTime.ParseExact(model.invoiceActionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertBudgetActivityInvoice"
                    , new SqlParameter[] {new SqlParameter("@id", Guid.NewGuid().ToString())
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@activityNo",model.activityNo)
                    ,new SqlParameter("@productId",model.productId)
                    ,new SqlParameter("@activityOfEstimateId",model.activityOfEstimateId)
                    ,new SqlParameter("@paymentNo",model.paymentNo)
                    ,new SqlParameter("@budgetImageId",model.budgetImageId)

                    ,new SqlParameter("@invoiceBudgetStatusId",model.invoiceBudgetStatusId)
                    ,new SqlParameter("@invoiceNo",model.invoiceNo)
                    ,new SqlParameter("@invoiceTotalBath",model.invoiceTotalBath)
					//,new SqlParameter("@actionDate",model.invoiceActionDate)
					,new SqlParameter("@actionDate",model.dateInvoiceAction)
                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_insertBudgetActivityInvoice");
            }

            return result;
        }

        public static int deleteInvoiceProduct(string activityId, string estimateId, string invoiceId, string delType)
        //public static int deleteInvoiceProduct(string activityId, string estimateId, string invoiceNo)
        {

            int result = 0;

            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteBudgetActivityInvoice"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    ,new SqlParameter("@activityOfEstimateId",estimateId)
                    ,new SqlParameter("@invoiceId",invoiceId)
                    ,new SqlParameter("@delType",delType)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_deleteBudgetActivityInvoice");
            }

            return result;
        }

        public static int deleteBudgetApproveByActNo(string activityNo)
        {

            int result = 0;

            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteBudgetApproveByActNo"
                    , new SqlParameter[] {new SqlParameter("@activityNo",activityNo)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteBudgetApproveByActNo");
            }

            return result;
        }

    }
}