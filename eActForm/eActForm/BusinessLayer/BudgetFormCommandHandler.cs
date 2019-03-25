using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
	public class BudgetFormCommandHandler
	{
		public static int insertInvoiceProduct(Budget_Activity_Model.Budget_Activity_Invoice_Att model)
		{

			int result = 0;

			try
			{
				
				result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertBudgetActivityInvoice"
					, new SqlParameter[] {new SqlParameter("@id", Guid.NewGuid().ToString())
					,new SqlParameter("@activityId",model.activityId)
					,new SqlParameter("@activityNo",model.activityNo)
					,new SqlParameter("@productId",model.productId)
					,new SqlParameter("@paymentNo",model.paymentNo)
					,new SqlParameter("@invoiceNo",model.invoiceNo)
					,new SqlParameter("@invTotalBath",model.invTotalBath)
					,new SqlParameter("@actionDate",model.actionDate)
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

		public static int deleteInvoiceProduct(Budget_Activity_Model.Budget_Activity_Invoice_Att model)
		{

			int result = 0;

			try
			{

				result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteBudgetActivityInvoice"
					, new SqlParameter[] {new SqlParameter("@activityId",model.activityId)
					,new SqlParameter("@productId",model.productId)
					,new SqlParameter("@invoiceNo",model.invoiceNo)
					});
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError(ex.Message + ">> usp_deleteBudgetActivityInvoice");
			}

			return result;
		}


	}
}