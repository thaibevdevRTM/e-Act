﻿using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
            if(companyId == "5601") { companyId = "5600"; }
            return QueryGetSubject.getAllSubject().Where(x => x.companyId.Contains(companyId)).OrderBy(x => x.nameTH).ToList();
        }

        public static List<TB_Act_Other_Model> getLimit(string subjectId)
        {
            return QueryOtherMaster.getOhterMaster("limitFlow", "").Where(w=> w.val1.Equals(subjectId)).ToList();
        }

        public static List<TB_Act_Other_Model> getApproveShow()
        {
            return QueryOtherMaster.getOhterMaster("approveShow", "");
        }

        public static List<TB_Act_Other_Model> getApprove()
        {
            return QueryOtherMaster.getOhterMaster("approve", "");
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

        public static int insertFlowApprove(ManagementFlow_Model model)
        {
            int i = 0;
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteFlowApprove"
                      , new SqlParameter[] {new SqlParameter("@flowId", model.p_flowId[0]) });
                foreach (var item in model.p_appovedGroupList)
                {
                    result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertFlowApprove"
                      , new SqlParameter[] {new SqlParameter("@companyId",model.p_companyId)
                     ,new SqlParameter("@flowId",model.p_flowId[0])
                    ,new SqlParameter("@empId",model.p_empIdList[i])
                    ,new SqlParameter("@approveGroupId",model.p_appovedGroupList[i])
                    ,new SqlParameter("@rangNo",model.p_rangNoList[i])
                    ,new SqlParameter("@showInDoc",model.p_isShowList[i])
                    ,new SqlParameter("@isApprove",model.p_isApproveList[i])
                    ,new SqlParameter("@delFlag",'0')
                    ,new SqlParameter("@createdDate",DateTime.Now)
                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updatedDate",DateTime.Now)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                      });
                    i++;
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("insertFlowApprove => " + ex.Message);
            }

            return result;
        }




    }
}