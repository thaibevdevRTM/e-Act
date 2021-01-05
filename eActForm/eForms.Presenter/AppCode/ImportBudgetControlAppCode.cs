using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary;

namespace eForms.Presenter.AppCode
{
    public class ImportBudgetControlAppCode
    {
        public static int InsertBudgetControl(string strCon, ImportBudgetControlModel.BudgetControlModels model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetControl"
                , new SqlParameter[] {new SqlParameter("@companyId",model.companyId)
                         ,new SqlParameter("@budgetGroupType",model.budgetGroupType)
                         ,new SqlParameter("@customerId",model.customerId)
                         ,new SqlParameter("@chanelId",model.chanelId)
                         ,new SqlParameter("@brandId",model.brandId)
                         ,new SqlParameter("@startDate",model.startDate)
                         ,new SqlParameter("@endDate",model.endDate)
                         ,new SqlParameter("@amount",model.amount)
                         ,new SqlParameter("@LE",model.LE)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetControl => " + ex.Message);
            }

            return result;
        }

        public static int InsertBudgetLE(string strCon, ImportBudgetControlModel.BudgetControl_LEModel model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertFlowApprove"
                , new SqlParameter[] {new SqlParameter("@companyId",model.budgetId)
                         ,new SqlParameter("@budgetGroupType",model.startDate)
                         ,new SqlParameter("@customerId",model.endDate)
                         ,new SqlParameter("@chanelId",model.amount)
                         ,new SqlParameter("@brandId",model.descripion)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                         ,new SqlParameter("@updatedByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetControl => " + ex.Message);
            }

            return result;
        }

    }
}
