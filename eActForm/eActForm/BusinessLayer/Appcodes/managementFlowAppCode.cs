using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

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
            return QueryGetSubject.getAllSubject().Where(x => x.companyId.Contains(companyId)).ToList();
        }

        public static List<TB_Act_Other_Model> getLimit()
        {
            return QueryOtherMaster.getOhterMaster("limitFlow", "");
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



    }
}