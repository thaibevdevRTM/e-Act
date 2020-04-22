using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGet_PlaceDetailByActivityId
    {
        public static List<PlaceDetailModel> getPlaceDetailByActivityId(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getPlaceDetailByActivityId"
                     , new SqlParameter("@activityId", activityId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new PlaceDetailModel()
                             {

                                 id = d["id"].ToString(),
                                 rowNo = Convert.ToInt32(d["rowNo"].ToString()),
                                 place = d["place"].ToString(),
                                 forProject = d["forProject"].ToString(),
                                 period = d["period"].ToString(),
                                 departureDateStr = d["departureDateStr"].ToString(),
                                 arrivalDateStr = d["arrivalDateStr"].ToString(),
                                 depart = d["depart"].ToString(),
                                 arrived = d["arrived"].ToString(),
                                 // departureDate =d["departureDate"].ToString() == "" ? (DateTime?)null : (Convert.ToDateTime(d["departureDate"].ToString())),
                                 //  arrivalDate= Convert.ToDateTime(d["arrivalDate"].ToString()),


                             });
                return lists.OrderBy(x => x.rowNo).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllActivityGroup => " + ex.Message);
                return new List<PlaceDetailModel>();
            }
        }
    }
}