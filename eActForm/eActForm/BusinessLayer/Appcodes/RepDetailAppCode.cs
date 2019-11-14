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
        public static void genFilePDFBrandGroup(string actRepDetailId, string gridHtml, string htmlOS, string htmlEst, string htmlWA, string htmlSO)
        {
            try
            {
                string fileName = string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actRepDetailId);
                var rootPath = HttpContext.Current.Server.MapPath(fileName);
                List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);

                fileName = "OS&JJ_" + actRepDetailId;
                rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], fileName));
                file = AppCode.genPdfFile(htmlOS, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                TB_Act_Image_Model.ImageModel imageFormModel = new TB_Act_Image_Model.ImageModel
                {
                    activityId = actRepDetailId,
                    imageType = AppCode.ApproveType.Report_Detail.ToString(),
                    _fileName = fileName
                };
                int resultImg = ImageAppCode.insertImageForm(imageFormModel);

                fileName = "Est&HP_" + actRepDetailId;
                rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], fileName));
                file = AppCode.genPdfFile(htmlEst, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                imageFormModel._fileName = fileName;
                resultImg = ImageAppCode.insertImageForm(imageFormModel);

                fileName = "WA&CY_" + actRepDetailId;
                rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], fileName));
                file = AppCode.genPdfFile(htmlWA, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                imageFormModel._fileName = fileName;
                resultImg = ImageAppCode.insertImageForm(imageFormModel);

                fileName = "SO&WR_" + actRepDetailId;
                rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], fileName));
                file = AppCode.genPdfFile(htmlSO, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                imageFormModel._fileName = fileName;
                resultImg = ImageAppCode.insertImageForm(imageFormModel);

            }
            catch (Exception ex)
            {
                throw new Exception("genFilePDF >> " + ex.Message);
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

        public static RepDetailModel.actFormRepDetails getFilterRepDetailByActNo(RepDetailModel.actFormRepDetails model, string actNo)
        {
            try
            {

                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.activityNo == actNo).ToList();
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.activityNo == actNo).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByStatusId(RepDetailModel.actFormRepDetails model, string statusId)
        {
            try
            {
                if (statusId == ((int)AppCode.ApproveStatus.เพิ่มเติม).ToString())
                {
                    model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.createdDate >= r.activityPeriodSt).ToList();
                    model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.createdDate >= r.activityPeriodSt).ToList();
                }
                else
                {
                    model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.statusId == statusId && r.createdDate < r.activityPeriodSt).ToList();
                    model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.statusId == statusId && r.createdDate < r.activityPeriodSt).ToList();
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByCustomer(RepDetailModel.actFormRepDetails model, string customerId)
        {
            try
            {
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.customerId == customerId).ToList();
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.customerId == customerId).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }

        public static RepDetailModel.actFormRepDetails getFilterRepDetailByRegion(RepDetailModel.actFormRepDetails model, string regionId)
        {
            try
            {
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.regionId == regionId).ToList();
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.regionId == regionId).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByActivity(RepDetailModel.actFormRepDetails model, string activityId)
        {
            try
            {
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.theme == activityId).ToList();
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.theme == activityId).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByProductType(RepDetailModel.actFormRepDetails model, string productType)
        {
            try
            {
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.productTypeId == productType).ToList();
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.productTypeId == productType).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByProductType >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getFilterRepDetailByProductGroup(RepDetailModel.actFormRepDetails model, string productGroup)
        {
            try
            {
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.productGroupid == productGroup).ToList();
                model.actFormRepDetailGroupLists = model.actFormRepDetailGroupLists.Where(r => r.productGroupid == productGroup).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterRepDetailByActNo >>" + ex.Message);
            }
        }
        public static RepDetailModel.actFormRepDetails getRepDetailReportByCreateDateAndStatusId(string startDate, string endDate, string typeForm)
        {
            try
            {
                DataSet ds = new DataSet();
                if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
                {
                    string stored = typeForm == Activity_Model.activityType.MT.ToString() ? "usp_getReportDetailByCreateDate" : "usp_getReportDetailOMTByCreateDate";
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, stored
                    , new SqlParameter[] {
                        new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null).AddDays(1))
                    });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByCreateDateAndEmp"
                    , new SqlParameter[] {
                        new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null).AddDays(1))
                        ,new SqlParameter("@empId", UtilsAppCode.Session.User.empId)
                    });
                }

                return dataTableToRepDetailModels(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }

        public static RepDetailModel.actFormRepDetails getRepDetailReportByCreateDateAndStatusId(string repDetailId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportDetailByRepDetailId"
                    , new SqlParameter[] {
                        new SqlParameter("@repDetailId",repDetailId)
                    });

                RepDetailModel.actFormRepDetails model = new RepDetailModel.actFormRepDetails();
                model = dataTableToRepDetailModels(ds);
                model.actFormRepDetailGroupLists.Select(r => r.delFlag = false
                        ).ToList();
                model.actFormRepDetailLists.Select(r => r.delFlag = false
                        ).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }


        public static string getRepdetailByActNo(string actNo)
        {

            try
            {
                object obj = SqlHelper.ExecuteScalar(AppCode.StrCon, CommandType.StoredProcedure, "usp_getRepDetailIdByActNo"
                    , new SqlParameter[] { new SqlParameter("@actNo", actNo) });

                return obj.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }

        private static RepDetailModel.actFormRepDetails dataTableToRepDetailModels(DataSet ds)
        {
            try
            {
                RepDetailModel.actFormRepDetails actRepModel = new RepDetailModel.actFormRepDetails();
                actRepModel.actFormRepDetailLists = (from DataRow dr in ds.Tables[0].Rows
                                                     select new RepDetailModel.actFormRepDetailModel()
                                                     {
                                                         #region detail parse
                                                         id = dr["activityId"].ToString(),
                                                         reference = dr["reference"].ToString(),
                                                         statusId = dr["statusId"].ToString(),
                                                         statusName = dr["statusName"].ToString(),
                                                         activityNo = dr["activityNo"].ToString(),
                                                         documentDate = (DateTime?)dr["documentDate"] ?? null,
                                                         brandId = dr["brandId"].ToString(),
                                                         customerId = dr["customerId"].ToString(),
                                                         regionId = dr["regionId"].ToString(),
                                                         productCateId = dr["productCateId"].ToString(),
                                                         productGroupid = dr["productGroupid"].ToString(),
                                                         cusNameTH = dr["cusNameTH"].ToString(),
                                                         productId = dr["productId"].ToString(),
                                                         productName = dr["productName"].ToString(),
                                                         size = dr["size"].ToString(),
                                                         typeTheme = dr["typeTheme"].ToString(),
                                                         normalSale = dr["normalCase"] is DBNull ? 0 : (decimal?)dr["normalCase"],
                                                         promotionSale = dr["promotionCase"] is DBNull ? 0 : (decimal?)dr["promotionCase"],
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
                                                         delFlag = true,
                                                         createdDate = (DateTime?)dr["createdDate"],
                                                         perGrowth = dr["growth"] is DBNull ? 0 : (decimal?)dr["growth"],
                                                         perSE = dr["Le"] is DBNull ? 0 : (decimal?)dr["Le"],
                                                         perToSale = dr["perToSale"] is DBNull ? 0 : (decimal?)dr["perToSale"],
                                                         #endregion

                                                     }).ToList();

                actRepModel.actFormRepDetailGroupLists = actRepModel.actFormRepDetailLists
                    .GroupBy(item => new {
                        item.activityNo,
                        item.productGroupid
                    })
                    .Select((group, index) => new RepDetailModel.actFormRepDetailModel
                    {

                        #region detail parse
                        id = group.First().id,
                        reference = group.First().reference,
                        statusId = group.First().statusId,
                        statusName = group.First().statusName,
                        activityNo = group.First().activityNo,
                        documentDate = group.First().documentDate,
                        brandId = group.First().brandId,
                        customerId = group.First().customerId,
                        regionId = group.First().regionId,
                        productCateId = group.First().productCateId,
                        productGroupid = group.First().productGroupid,
                        cusNameTH = group.First().cusNameTH,
                        productId = group.First().productId,
                        productName = group.First().productName,
                        size = group.First().size,
                        typeTheme = group.First().typeTheme,
                        normalSale = group.Sum(x => x.normalSale),
                        promotionSale = group.Sum(x => x.promotionSale),
                        total = group.Sum(x => x.total),
                        specialDisc = group.Sum(x => x.specialDisc),
                        specialDiscBaht = group.Sum(x => x.specialDiscBaht),
                        promotionCost = group.Sum(x => x.promotionCost),
                        channelName = group.First().channelName,
                        productTypeId = group.First().productTypeId,
                        activityPeriodSt = group.First().activityPeriodSt,
                        activityPeriodEnd = group.First().activityPeriodEnd,
                        costPeriodSt = group.First().costPeriodSt,
                        costPeriodEnd = group.First().costPeriodEnd,
                        activityName = group.First().activityName,
                        theme = group.First().theme,
                        activityDetail = group.First().activityDetail,
                        compensate = group.Sum(x => x.compensate),
                        delFlag = group.First().delFlag,
                        createdDate = group.First().createdDate,
                        perGrowth = group.Sum(x => x.perGrowth),
                        perSE = group.Sum(x => x.perSE),
                        perToSale = group.Sum(x => x.perToSale),
                        #endregion


                    }).OrderBy(x => x.activityNo).ToList();

                return actRepModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<RepDetailModel.actApproveRepDetailModel> getActNoByRepId(string repId)
        {

            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getActNoByRepId"
                    , new SqlParameter[] { new SqlParameter("@repId", repId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new RepDetailModel.actApproveRepDetailModel()
                             {
                                 activityNo = dr["activityNo"].ToString(),

                             }).ToList();
                return lists;

            }
            catch (Exception ex)
            {
                throw new Exception("getActNoByRepId >>" + ex.Message);
            }
        }

    }
}