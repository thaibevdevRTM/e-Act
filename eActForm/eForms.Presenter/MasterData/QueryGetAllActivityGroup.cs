using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary;

namespace eForms.Presenter.MasterData
{
    public class QueryGetAllActivityGroup
    {
        public static List<TB_Act_ActivityGroup_Model> getAllActivityGroup(string strCode)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCode, CommandType.StoredProcedure, "usp_getAllActivityGroup");
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
                ExceptionManager.WriteError("getAllActivityGroup => " + ex.Message);
                return new List<TB_Act_ActivityGroup_Model>();
            }
        }
    }
}
