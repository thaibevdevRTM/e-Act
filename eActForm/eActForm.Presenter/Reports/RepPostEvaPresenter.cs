using System;
using System.Data;
using System.Data.SqlClient;
using eActForm.Models.Reports;
using Microsoft.ApplicationBlocks.Data;
namespace eActForm.Presenter.Reports
{
    public class RepPostEvaPresenter : BasePresenter
    {
        public static RepPostEvaModels getDataPostEva(string strConn, string startDate, string endDate, string customerId)
        {
            try
            {
                RepPostEvaModels model = new RepPostEvaModels();

                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getReportPostEva"
                    , new SqlParameter[] {new SqlParameter("@startDate",startDate)
                    , new SqlParameter("@endDate",endDate)
                    , new SqlParameter("@customerId",customerId)});

                model.repPostEvaLists = ToGenericList<RepPostEvaModel>(ds.Tables[0]);

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getDataPostEva >> " + ex.Message);
            }
        }
    }
}
