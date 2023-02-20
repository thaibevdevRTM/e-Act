using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eForms.Presenter.AppCode
{
    public class ImportFlowPresenter
    {
        public static string getFlowIdByDetail(string strCon, ImportFlowModel.ImportFlowModels model, bool checkSubject, string getSubjectId)
        {
            string getLimitId = "", getFlowId = "";
            try
            {
                //เหลือดัก ไม่ให้ add subject ซ้ำ
                if (checkSubject)
                {
                    getLimitId = insertLimit(strCon, getSubjectId, model);
                    return getFlowId = insertFlowMain(strCon, getSubjectId, getLimitId, model);
                }
                else
                {
                    return model.flowId;

                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowIdByDetail => " + ex.Message);
                return null;
            }
        }
        public static bool checkFlowApprove(string strCon, string flowId, string companyId, string masterTypeId, string empGroup)
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getFlowApproveExistNew"
                , new SqlParameter("@flowId", flowId)
                , new SqlParameter("@companyId", companyId)
                , new SqlParameter("@masterTypeId", masterTypeId)
                , new SqlParameter("@empGroup", empGroup));

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new
                             {
                                 id = d["id"].ToString(),
                             });

                return lists.Any() ? true : false;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("checkFlowApprove => " + ex.Message);
                return false;
            }
        }


        public static int InsertFlow(string strCon, ImportFlowModel.ImportFlowModels model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertFlowApprove"
                , new SqlParameter[] {new SqlParameter("@companyId",model.companyId)
                         ,new SqlParameter("@flowId",model.flowId)
                         ,new SqlParameter("@empId",model.empId)
                         ,new SqlParameter("@approveGroupId",model.approveGroupId)
                         ,new SqlParameter("@rangNo",model.rang)
                         ,new SqlParameter("@empGroup",model.empGroup)
                         ,new SqlParameter("@showInDoc",model.IsShow)
                         ,new SqlParameter("@isApprove",model.IsApprove)
                         ,new SqlParameter("@delFlag","0")
                         ,new SqlParameter("@createdDate",DateTime.Now)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                         ,new SqlParameter("@updatedDate",DateTime.Now)
                         ,new SqlParameter("@updatedByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertFlow => " + ex.Message);
            }

            return result;
        }

        public static int deleteTempFlow(string strCon, string empId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_deleteTempImportFlow"
                , new SqlParameter[] {new SqlParameter("@empId",empId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("deleteTempFlow => " + ex.Message);
            }

            return result;
        }

        public static string checkValueForImport(string value)
        {
            if (value.Contains("#N/A"))
            {
                value = "";
            }

            return value;
        }


        //public static bool checkFormAddSubject(string strCon, string masterTypeId)
        //{
        //    bool result = false;
        //    try
        //    {
        //        DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_checkFormForAddSubject"
        //            , new SqlParameter[] { new SqlParameter("@masterTypeId", masterTypeId) });
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            result = true;
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.WriteError("checkFormAddSubject => " + ex.Message);
        //        return result;
        //    }
        //}

        public static bool checkFormAddSubject(string strCon, string masterTypeId)
        {
            bool haveForm = false;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_checkFormForAddSubject"
                    , new SqlParameter("@masterTypeId", masterTypeId));


                if (ds.Tables[0].Rows.Count > 0)
                {
                    haveForm = true;
                }
                return haveForm;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("checkFormAddSubject => " + ex.Message);
                return haveForm;
            }
        }


        public static string insertSubject(string strCon, ImportFlowModel.ImportFlowModels model)
        {
            string subjectId = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_InsertSubject"
                    , new SqlParameter[] { new SqlParameter("@companyId",model.companyId)
                    , new SqlParameter ("@subjectName", model.subject)
                    , new SqlParameter ("@masterTypeId", model.masterTypeId)
                    , new SqlParameter ("@createBy", model.createdByUserId)
                    , new SqlParameter ("@empGroup", model.empGroup)
                    , new SqlParameter ("@limitBegin", model.limitBegin)
                    , new SqlParameter ("@limitTo", model.limitTo)
                    , new SqlParameter ("@productBrandId", model.productBrandId)
                    , new SqlParameter ("@channelId", model.channelId)
                    , new SqlParameter ("@actType", model.actType)
                    });


                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow d in ds.Tables[0].Rows
                                 select new ImportFlowModel.ImportFlowModels()
                                 {
                                     subjectId = d["subjectId"].ToString(),
                                 });
                    subjectId = lists.FirstOrDefault().subjectId;
                }
                return subjectId;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("checkFormAddSubject => " + ex.Message);
                return subjectId;
            }
        }

        public static string insertLimit(string strCon, string subjectId, ImportFlowModel.ImportFlowModels model)
        {
            string result = "";
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_insertLimit"
                        , new SqlParameter[] {new SqlParameter("@subjectId",subjectId)
                         ,new SqlParameter("@limitBegin",model.limitBegin)
                         ,new SqlParameter("@limitTo",model.limitTo)
                         ,new SqlParameter("@limitDisplay",model.limitDisplay)
                         ,new SqlParameter("@companyId",model.companyId)
                         ,new SqlParameter("@createBy",model.createdByUserId)
                         ,new SqlParameter("@masterTypeId",model.masterTypeId)
                         ,new SqlParameter("@empGroup",model.empGroup)
                         ,new SqlParameter("@productBrandId",model.productBrandId)
                         ,new SqlParameter("@channelId",model.channelId)
                         });


                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow d in ds.Tables[0].Rows
                                 select new ImportFlowModel.ImportFlowModels()
                                 {
                                     limitId = d["limitId"].ToString(),
                                 });
                    result = lists.FirstOrDefault().limitId;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("insertSubject => " + ex.Message);
                return result;
            }
        }


        public static string insertFlowMain(string strCon, string subjectId, string limitId, ImportFlowModel.ImportFlowModels model)
        {
            string result = "";
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_insertFlowMain"
                        , new SqlParameter[] {new SqlParameter("@subjectId",subjectId)
                         ,new SqlParameter("@companyId",model.companyId)
                         ,new SqlParameter("@customerId",model.customerId)
                         ,new SqlParameter("@productCate",model.productCateId)
                         ,new SqlParameter("@productTypeId",model.productTypeId)
                         ,new SqlParameter("@flowLimitId",limitId)
                         ,new SqlParameter("@chanelId",model.channelId)
                         ,new SqlParameter("@productBrandId",model.productBrandId)
                         ,new SqlParameter("@departmentId",model.departmentId)
                         ,new SqlParameter("@createBy",model.createdByUserId)
                         ,new SqlParameter("@empGroup",model.empGroup)
                         ,new SqlParameter("@masterTypeId",model.masterTypeId)
                         });


                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow d in ds.Tables[0].Rows
                                 select new ImportFlowModel.ImportFlowModels()
                                 {
                                     flowId = d["flowId"].ToString(),
                                 });
                    result = lists.FirstOrDefault().flowId;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("insertFlowMain => " + ex.Message);
                return result;
            }
        }


        public static int InserToTemptFlow(string strCon, ImportFlowModel.ImportFlowModels model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertTempFlow"
                , new SqlParameter[] {new SqlParameter("@masterTypeId",model.masterTypeId)
                         ,new SqlParameter("@companyId",model.companyId)
                         ,new SqlParameter("@company",model.company)
                         ,new SqlParameter("@actType",model.actType)
                         ,new SqlParameter("@subject",model.subject)
                         ,new SqlParameter("@customerId",model.customerId)
                         ,new SqlParameter("@customer",model.customer)
                         ,new SqlParameter("@productCateId",model.productCateId)
                         ,new SqlParameter("@productCate",model.productCate)
                         ,new SqlParameter("@productTypeId",model.productTypeId)
                         ,new SqlParameter("@productType",model.productType)
                         ,new SqlParameter("@productBrand",model.productBrand)
                         ,new SqlParameter("@productBrandId",model.productBrandId)
                         ,new SqlParameter("@channel",model.channel)
                         ,new SqlParameter("@channelId",model.channelId)
                         ,new SqlParameter("@departmentId",model.departmentId)
                         ,new SqlParameter("@department",model.department)
                         ,new SqlParameter("@limitBegin",model.limitBegin)
                         ,new SqlParameter("@limitTo",model.limitTo)
                         ,new SqlParameter("@limitDisplay",model.limitDisplay)
                         ,new SqlParameter("@rang",model.rang)
                         ,new SqlParameter("@approveGroupId",model.approveGroupId)
                         ,new SqlParameter("@approveGroup",model.approveGroup)
                         ,new SqlParameter("@IsShow",model.IsShow)
                         ,new SqlParameter("@IsApprove",model.IsApprove)
                         ,new SqlParameter("@empId",model.empId)
                         ,new SqlParameter("@empGroup",model.empGroup)
                         ,new SqlParameter("@name",model.name)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InserToTemptFlow => " + ex.Message);
            }

            return result;
        }


        public static List<ImportFlowModel.ImportFlowModels> getFlowAterImport(string strCon, string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getFlowDetailTemp"
                    , new SqlParameter[] { new SqlParameter("@empId",empId)
                    });


                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new ImportFlowModel.ImportFlowModels()
                             {
                                 flowId = d["flowId"].ToString(),
                                 masterTypeId = d["masterTypeId"].ToString(),
                                 company = d["company"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 actType = d["actTypeId"].ToString(),
                                 subject = d["subjectId"].ToString(),
                                 customer = d["customer"].ToString(),
                                 customerId = d["customerId"].ToString(),
                                 productCate = d["productCate"].ToString(),
                                 productCateId = d["productCateId"].ToString(),
                                 productType = d["productType"].ToString(),
                                 productTypeId = d["productTypeId"].ToString(),
                                 productBrand = d["productBrand"].ToString(),
                                 productBrandId = d["productBrandId"].ToString(),
                                 channel = d["channel"].ToString(),
                                 channelId = d["channelId"].ToString(),
                                 department = d["department"].ToString(),
                                 departmentId = d["departmentId"].ToString(),
                                 limitBegin = d["limitBegin"].ToString(),
                                 limitTo = d["limitTo"].ToString(),
                                 limitDisplay = d["limitDisplay"].ToString(),
                                 rang = d["rang"].ToString(),
                                 approveGroup = d["approveGroup"].ToString(),
                                 approveGroupId = d["approveGroupId"].ToString(),
                                 IsShow = d["IsShow"].ToString(),
                                 IsApprove = d["IsApprove"].ToString(),
                                 empId = d["empId"].ToString(),
                                 empGroup = d["empGroup"].ToString(),
                                 name = d["name"].ToString(),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 checkFlowExist = bool.Parse(d["checkFlow"].ToString()),

                             });
                return lists.ToList();


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowAterImport => " + ex.Message);
                return null;
            }
        }
    }
}


