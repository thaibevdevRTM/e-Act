using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using eActForm.Models;
using System.Net.Mail;
using iTextSharp.text;
using System.Configuration;

namespace eActForm.BusinessLayer
{
    public class RepDetailAppCode
    {
        public static void genFilePDFBrandGroup(string actRepDetailId,string gridHtml,string htmlOS, string htmlEst, string htmlWA, string htmlSO)
        {
            try
            {
                string fileName = string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actRepDetailId);
                var rootPath = HttpContext.Current.Server.MapPath(fileName);
                List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), rootPath);

                fileName = string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actRepDetailId + "_OS");
                rootPath = HttpContext.Current.Server.MapPath(fileName);
                file = AppCode.genPdfFile(htmlOS, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), rootPath);
                TB_Act_Image_Model.ImageModel imageFormModel = new TB_Act_Image_Model.ImageModel
                {
                    activityId = actRepDetailId,
                    imageType = AppCode.ApproveType.Report_Detail.ToString(),
                    _fileName = fileName
                };
                int resultImg = ActivityFormCommandHandler.insertImageForm(imageFormModel);

                fileName = string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actRepDetailId + "_Est");
                rootPath = HttpContext.Current.Server.MapPath(fileName);
                file = AppCode.genPdfFile(htmlEst, new Document(PageSize.A4.Rotate(), 2, 2, 10, 10), rootPath);
                imageFormModel._fileName = fileName;
                resultImg = ActivityFormCommandHandler.insertImageForm(imageFormModel);

            }
            catch (Exception ex)
            {
                throw new Exception("genFilePDFBrandGroup >> " + ex.Message);
            }
        }
        public static int getRepDetailStatus(string repDetailId)
        {
            try
            {

                object obj = SqlHelper.ExecuteScalar(AppCode.StrCon, CommandType.StoredProcedure, "usp_getRepDetailStatus"
                    , new SqlParameter[] { new SqlParameter("@repDetailId", repDetailId) });

                return int.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailStatus >> " + ex.Message);
            }
        }
        public static List<ApproveModel.approveDetailModel> getUserCreateRepDetailForm(string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserCreateRepDetailForm"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveDetailModel()
                             {
                                 empId = dr["empId"].ToString(),
                                 empName = dr["empName"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getUserCreateRepDetailForm >>" + ex.Message);
            }
        }

        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByActNo(List<RepDetailModel.actFormRepDetailModel> lists, string actNo)
        {
            try
            {
                return lists.Where(r => r.activityNo == actNo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByStatusId(List<RepDetailModel.actFormRepDetailModel> lists, string statusId)
        {
            try
            {
                if (statusId == ((int)AppCode.ApproveStatus.เพิ่มเติม).ToString())
                {
                    return lists.Where(r => r.createdDate >= r.activityPeriodSt).ToList();
                }
                else
                {
                    return lists.Where(r => r.statusId == statusId && r.createdDate < r.activityPeriodSt).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByCustomer(List<RepDetailModel.actFormRepDetailModel> lists, string customerId)
        {
            try
            {
                return lists.Where(r => r.customerId == customerId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByActivity(List<RepDetailModel.actFormRepDetailModel> lists, string activityId)
        {
            try
            {
                return lists.Where(r => r.theme == activityId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByProductType(List<RepDetailModel.actFormRepDetailModel> lists, string productType)
        {
            try
            {
                return lists.Where(r => r.productTypeId == productType).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByProductType >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getFilterRepDetailByProductGroup(List<RepDetailModel.actFormRepDetailModel> lists, string productGroup)
        {
            try
            {
                return lists.Where(r => r.productGroupid == productGroup).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static List<RepDetailModel.actFormRepDetailModel> getRepDetailReportByCreateDateAndStatusId(string startDate, string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByCreateDate"
                    , new SqlParameter[] {
                        new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                    });

                return dataTableToRepDetailModels(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }

        public static List<RepDetailModel.actFormRepDetailModel> getRepDetailReportByCreateDateAndStatusId(string repDetailId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByRepDetailId"
                    , new SqlParameter[] {
                        new SqlParameter("@repDetailId",repDetailId)
                    });

                return dataTableToRepDetailModels(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }

        private static List<RepDetailModel.actFormRepDetailModel> dataTableToRepDetailModels(DataSet ds)
        {
            try
            {
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new RepDetailModel.actFormRepDetailModel()
                             {
                                 #region detail parse
                                 id = dr["activityId"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 documentDate = (DateTime?)dr["documentDate"] ?? null,
                                 brandId = dr["brandId"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 productCateId = dr["productCateId"].ToString(),
                                 productGroupid = dr["productGroupid"].ToString(),
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 productId = dr["productId"].ToString(),
                                 productName = dr["productName"].ToString(),
                                 size = dr["size"].ToString(),
                                 typeTheme = dr["typeTheme"].ToString(),
                                 normalSale = dr["normalCase"].ToString(),
                                 promotionSale = dr["promotionCase"].ToString(),
                                 total = dr["total"] is DBNull ? 0 : (decimal?)dr["total"],
                                 specialDisc = dr["specialDisc"] is DBNull ? 0 : (decimal?)dr["specialDisc"],
                                 specialDiscBaht = dr["specialDiscBaht"] is DBNull ? 0 : (decimal?)dr["specialDiscBaht"],
                                 promotionCost = dr["promotionCost"] is DBNull ? 0 : (decimal?)dr["promotionCost"],
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
                                 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
                                 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                 activityName = dr["activityName"].ToString(),
                                 theme = dr["theme"].ToString(),
                                 activityDetail = dr["activityDetail"].ToString(),
                                 compensate = dr["compensate"] is DBNull ? 0 : (decimal)dr["compensate"],
                                 delFlag = false,
                                 createdDate = (DateTime?)dr["createdDate"],
                                 #endregion

                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}