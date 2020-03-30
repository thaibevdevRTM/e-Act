using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data.SqlClient;

namespace eForms.Presenter.Forms
{
    public class activityFormOrderProductPresenter
    {
        public static int getCountOrderProductOnweek(string strConn, string empId, string startDate, string endDate)
        {
            try
            {
                object obj = SqlHelper.ExecuteScalar(strConn, "usp_getOrderOnCurrentWeek"
                    , new SqlParameter[] {new SqlParameter("@empId",empId)
                    ,new SqlParameter("@startDate",startDate)
                    ,new SqlParameter("@endDate",endDate)
                    });
                return obj != null ? (int)obj : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("getCountOrderProductOnweek >> " + ex.Message);
            }
        }
    }
}
