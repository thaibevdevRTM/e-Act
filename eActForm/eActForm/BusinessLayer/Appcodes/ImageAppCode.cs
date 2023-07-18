using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using WebLibrary;
using static eActForm.Models.TB_Act_Image_Model;
using static iTextSharp.text.pdf.AcroFields;

namespace eActForm.BusinessLayer
{
    public class ImageAppCode
    {
        public static List<TB_Act_Image_Model.ImageModel> GetImage(string activityId, string type)
        {
            try
            {
                List<TB_Act_Image_Model.ImageModel> lists = GetImage(activityId);
                if (lists == null) return null;
                else return lists.Where(x => x.extension == type).ToList();
            }
            catch (Exception ex)
            {
                //ExceptionManager.WriteError("getImage => " + ex.Message); // background service use this. then can't write logs
                return new List<TB_Act_Image_Model.ImageModel>();
            }
        }
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
                //ExceptionManager.WriteError("getImage => " + ex.Message); // background service use this. then can't write logs
                return new List<TB_Act_Image_Model.ImageModel>();
            }
        }

        public static int insertImageForm(TB_Act_Image_Model.ImageModel model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertImageForm"
                    , new SqlParameter[] {new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@imageType",model.imageType)
                    ,new SqlParameter("@image",model._image)
                    ,new SqlParameter("@fileName",model._fileName)
                    ,new SqlParameter("@extension",model.extension)
                    ,new SqlParameter("@remark",model.remark)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)

                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertImageForm");
            }

            return result;
        }


        public static int deleteImg(string fileName, string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteImgbyactivityIdName"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    ,new SqlParameter("@fileName",fileName)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteImg");
            }

            return result;
        }

        public static int deleteImgById(string id, string filename)
        {

            int result = 0;
            try
            {
                string getPathFile = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], filename));
                File.Delete(getPathFile);

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteImgbyId"
                    , new SqlParameter[] {new SqlParameter("@id",id)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteImg");
            }

            return result;
        }

        public static List<ImageModel> GetSizeFiles(string activityId)
        {
            try
            {
                TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
                getImageModel.tbActImageList = ImageAppCode.GetImage(activityId);
                string[] pathFile = new string[getImageModel.tbActImageList.Count];
                if (getImageModel.tbActImageList.Any())
                {
                    int i = 0;
                    foreach (var item in getImageModel.tbActImageList)
                    {
                        var pathCheck = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                        long lengthPathCheck = new System.IO.FileInfo(pathCheck).Length;
                        item.sizeFiles = lengthPathCheck.ToString();
                        i++;
                    }
                }

                return getImageModel.tbActImageList;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetSizeFiles => " + ex.Message);
                return new List<ImageModel>();
            }
        }

    }
}