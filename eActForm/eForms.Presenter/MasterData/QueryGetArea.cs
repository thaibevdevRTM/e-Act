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

namespace eForms.Presenter.MasterData
{
    public class QueryGetArea
    {
        public static List<TB_Act_Area_Model> getAreaByCondition(string strCode , string condition)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCode, CommandType.StoredProcedure, "usp_getAreaByCondition"
                    , new SqlParameter[] { new SqlParameter("@condition", condition) });
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Area_Model()
                             {
                                 id = d["id"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 region = d["region"].ToString(),
                                 area = d["area"].ToString(),
                                 province = d["province"].ToString(),
                                 condition = d["condition"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.OrderBy(x => x.area).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAreaByCondition => " + ex.Message);
                return new List<TB_Act_Area_Model>();
            }
        }
    }
}
