using eActForm.Models;
using System;
using System.IO;
using WebLibrary;

namespace eActConsoleInsertBudgetToTB
{
    public class AppCode
    {
        public static bool insertDataReportToTB()
        {

            try
            {
                bool result = false;
                int rtn = 0;
                rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_transferBudgetAllApprove"
                    , new SqlParameter[] { new SqlParameter("@activityId", actId) });
                if (rtn > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("transferBudgetAllApprove >>" + ex.Message);
            }

        }
    }
}
