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
    public class QueryGetImageById 
    {
        public static List<TB_Act_Image_Model.ImageModel> GetImage(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getImageByActivityId"
                    , new SqlParameter("@activityId", activityId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_Image_Model.ImageModel()
                             {
                                 id = d["id"].ToString(),
                                 activityId = d["activityId"].ToString(),
                                 imageType = d["imageType"].ToString(),
                                 _image = (d["_image"] == null || d["_image"] is DBNull) ? new byte[0] : (byte[])d["_image"],
                                 _fileName = d["_fileName"].ToString(),
                                 extension = d["extension"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getImage => " + ex.Message);
                return new List<TB_Act_Image_Model.ImageModel>();
            }
        }
    }
}