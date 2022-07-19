using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Presenter.AppCode;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer.Appcodes
{
    public class managementFlowAppCode
    {
        public static List<TB_Act_Other_Model> getCompany()
        {
            return QueryOtherMaster.getOhterMaster("company", "");
        }

        public static List<TB_Reg_Subject_Model> getSubject(string companyId)
        {
            //if (companyId == "5601") { companyId = "5600"; }
            return QueryGetSubject.getAllSubjectByFlowCompany(companyId).OrderBy(x => x.nameTH).ToList();
        }

        public static List<TB_Reg_FlowLimit_Model> getLimit(string subjectId, string companyId)
        {
            var result = QueryGetAllFlowLimit.getAllFlowLimit().Where(x => x.subjectId.Equals(subjectId)).ToList();


            if (subjectId == ConfigurationManager.AppSettings["subjectActBeer"])
            {
                return result;
            }
            else
            {
                return result.Where(x => x.companyId.Equals(companyId)).ToList();
            }


        }

        public static List<TB_Act_Other_Model> getApproveShow()
        {
            return QueryOtherMaster.getOhterMaster("approveShow", "");
        }

        public static List<TB_Act_Other_Model> getApprove()
        {
            return QueryOtherMaster.getOhterMaster("approve", "");
        }

        public static List<TB_Act_Other_Model> getActive()
        {
            return QueryOtherMaster.getOhterMaster("active", "");
        }

        public static List<TB_Act_Customers_Model.Customers_Model> getCustomer(string companyId)
        {
            if (companyId == ConfigurationManager.AppSettings["companyId_MT"])
            {
                return QueryGetAllCustomers.getCustomersMT();
            }
            else
            {
                return QueryGetAllCustomers.getCustomersOMT();
            }
        }

        public static List<TB_Act_ProductCate_Model> getProductCate(string companyId)
        {
            return QuerygetAllProductCate.getAllProductCate();
        }

        public static List<TB_Act_Chanel_Model.Chanel_Model> getChanel(string typeChanel)
        {
            return QueryGetAllChanel.getAllChanel().Where(x => x.typeChannel.Equals(typeChanel)).ToList();
        }

        public static List<TB_Act_ProductBrand_Model> getProductBrand()
        {
            return QueryGetAllBrand.GetAllBrand().Where(x => x.no_tbmmkt != null).ToList();
        }

        public static List<TB_Reg_ApproveGroup_Model> getApproveGroup()
        {
            return QueryGetAllApproveGroup.getAllApproveGroup().OrderBy(x => x.nameTH).ToList();
        }

        public static List<TB_Act_ProductType_Model> getProductType()
        {
            return QuerygetAllProductCate.getAllProductType();
        }

        public static int insertFlowApprove(ManagementFlow_Model model)
        {
            int i = 0;
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteFlowApprove"
                      , new SqlParameter[] { new SqlParameter("@flowId", model.p_flowId[0])
                      , new SqlParameter("@empId", model.p_empGroup[0])
                      });

                string getLimitId = "", flowId = model.p_flowId[0];
                if (string.IsNullOrEmpty(model.p_flowId[0]))
                {
                    flowId = getFlowtLimitByCondition(AppCode.StrCon, model);
                }

                foreach (var item in model.p_appovedGroupList)
                {
                    result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertFlowApprove"

                    , new SqlParameter[] {new SqlParameter("@companyId",model.p_companyId)
                    ,new SqlParameter("@flowId",flowId)
                    ,new SqlParameter("@empId",model.p_empIdList[i])
                    ,new SqlParameter("@approveGroupId",model.p_appovedGroupList[i])
                    ,new SqlParameter("@rangNo",model.p_rangNoList[i])
                    ,new SqlParameter("@empGroup",model.p_empGroup[0])
                    ,new SqlParameter("@showInDoc",model.p_isShowList[i])
                    ,new SqlParameter("@isApprove",model.p_isApproveList[i])
                    ,new SqlParameter("@delFlag",model.p_active[i])
                    ,new SqlParameter("@createdDate",DateTime.Now)
                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updatedDate",DateTime.Now)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                      });
                    i++;
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("insertFlowApprove => " + ex.Message);
            }

            return result;
        }
        public static int insertFlowApproveAddOn(ManagementFlow_Model model)
        {
            int i = 0;
            int result = 0;
            try
            {
                foreach (var item in model.addOn_Id)
                {
                    result += SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteFlowApproveAddOn"
                          , new SqlParameter[] { new SqlParameter("@addOnId", item)
                       });
                }


                foreach (var item in model.p_appovedGroupList)
                {
                    result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertFlowApprove_AddOn"

                    , new SqlParameter[] {new SqlParameter("@companyId",model.p_companyId)
                    ,new SqlParameter("@subjectId",model.p_subjectId)
                    ,new SqlParameter("@productTypeId", model.p_productType)
                    ,new SqlParameter("@productCateId", model.p_productCateId)
                    ,new SqlParameter("@productBrandId", model.p_productBrandId)
                    ,new SqlParameter("@channelId", model.p_channelId)
                    ,new SqlParameter("@flowLimitId", model.p_flowLimitId)
                    ,new SqlParameter("@empId",model.p_empIdList[i])
                    ,new SqlParameter("@approveGroupId",model.p_appovedGroupList[i])
                    ,new SqlParameter("@rangNo",model.p_rangNoList[i])
                    ,new SqlParameter("@empGroup", string.IsNullOrEmpty(model.p_empGroup[0])? "no" : model.p_empGroup[0])
                    ,new SqlParameter("@showInDoc",model.p_isShowList[i])
                    ,new SqlParameter("@isApprove",model.p_isApproveList[i])
                    ,new SqlParameter("@activityTypeId",model.activityTypeId)
                    ,new SqlParameter("@customerId",model.customerId)
                    ,new SqlParameter("@delFlag",model.p_active[i])
                    ,new SqlParameter("@createdDate",DateTime.Now)
                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updatedDate",DateTime.Now)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                      });
                    i++;
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("insertFlowApproveAddOn => " + ex.Message);
            }

            return result;
        }


        public static int delFlowAddOnByEmpId(string id)
        {

            var result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteFlowApproveAddOn"
                   , new SqlParameter[] { new SqlParameter("@addOnId", id)
                });

            return result;
        }

        public static int delFlowApproveByEmpId(string approveId)
        {

            var result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteFlowApproveByEmpId"
                   , new SqlParameter[] { new SqlParameter("@approveId", approveId)
                   ,new SqlParameter("@updateByUser",UtilsAppCode.Session.User.empId)
                });

            return result;
        }


        public static string getFlowtLimitByCondition(string strCon, ManagementFlow_Model model)
        {
            string result = "";
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getFlowLimitByCondition"
                        , new SqlParameter[] {new SqlParameter("@subjectId",model.p_subjectId)
                         ,new SqlParameter("@flowLimitId",model.p_flowLimitId)
                         ,new SqlParameter("@companyId",model.p_companyId)
                         ,new SqlParameter("@customerId",model.customerId)
                         ,new SqlParameter("@productBrandId",model.p_productBrandId)
                         ,new SqlParameter("@channelId",model.p_channelId)
                         ,new SqlParameter("@productCate",model.p_productCateId)
                         ,new SqlParameter("@createBy",UtilsAppCode.Session.User.empId)
                         });


                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow d in ds.Tables[0].Rows
                                 select new ManagementFlow_Model()
                                 {
                                     flowId = d["flowId"].ToString(),
                                 });
                    result = lists.FirstOrDefault().flowId;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("insertSubject => " + ex.Message);
                return result;
            }
        }
    }
}