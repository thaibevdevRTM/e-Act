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

namespace eForms.Presenter.AppCode
{
    public class ImportFlowPresenter
    {
        public static string getFlowIdByDetail(string strCon, ImportFlowModel.ImportFlowModels model, bool checkSubject, string getSubjectId)
        {
            try
            {
                string getLimitId = "", getFlowId = "";
                //เหลือดัก ไม่ให้ add subject ซ้ำ
                if (checkSubject)
                {
                    getLimitId = insertLimit(strCon, getSubjectId, model);
                    getFlowId = insertFlowMain(strCon, getSubjectId, getLimitId, model);
                }

                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getFlowIdByDetail"
                , new SqlParameter("@companyId", model.companyId)
                , new SqlParameter("@masterTypeId", model.masterTypeId)
                , new SqlParameter("@subjectId", getSubjectId)
                , new SqlParameter("@customerId", model.customerId)
                , new SqlParameter("@productCateId", model.productCateId)
                , new SqlParameter("@productTypeId", model.productTypeId)
                , new SqlParameter("@productBrandId", model.productBrandId)
                , new SqlParameter("@channelId", model.channelId)
                , new SqlParameter("@departmentId", model.departmentId)
                , new SqlParameter("@limit", model.limitTo)
                , new SqlParameter("@empGroup", model.empGroup));
                if (ds.Tables.Count > 0)
                {
                    var lists = (from DataRow d in ds.Tables[0].Rows
                                 select new ImportFlowModel.ImportFlowModels()
                                 {
                                     flowId = d["id"].ToString(),
                                 });
                    return lists.Any() ? lists.FirstOrDefault().flowId : "";
                }
                else
                {
                    return "";
                }
               
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowIdByDetail => " + ex.Message);
                return null;
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
                    , new SqlParameter ("@limit", model.limitBegin)
                    , new SqlParameter ("@productBrandId", model.productBrandId)
                    , new SqlParameter ("@channelId", model.channelId)
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
    }
}


