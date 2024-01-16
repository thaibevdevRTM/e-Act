using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;
using static eActForm.Models.TB_Act_ProductPrice_Model;

namespace eActForm.BusinessLayer
{
    public class AdminUserAppCode
    {
        public static int insertRole(string empId, string roleId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertRole"
                    , new SqlParameter[] {new SqlParameter("@empId",empId)
                    ,new SqlParameter("@roleId",roleId)
                    ,new SqlParameter("@createdBy",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertRole");
            }

            return result;
        }

        public static int delUserandAuthorByEmpId(string empId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_delUserandAuthorByEmpId"
                    , new SqlParameter[] {new SqlParameter("@empId",empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> delUserandAuthorByEmpId");
            }

            return result;
        }


        public static int insertAuthorized(string empId, string companyId, string customerId, string productTypeId, string regionId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertAuthorized"
                    , new SqlParameter[] {new SqlParameter("@empId",empId)
                    ,new SqlParameter("@companyId",companyId)
                    ,new SqlParameter("@customerId",customerId)
                    ,new SqlParameter("@productTypeId",productTypeId)
                    ,new SqlParameter("@regionId",regionId)
                    ,new SqlParameter("@createdBy",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertAuthorized");
            }

            return result;
        }


        public static List<AdminUserModel.User> getAllUserRole()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllUserRole");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AdminUserModel.User()
                             {
                                 empId = dr["empid"].ToString(),
                                 userName = dr["empFNameTH"].ToString(),
                                 userLName = dr["empLNameTH"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getAllUserRole >> " + ex.Message);
            }
        }

        public static List<AdminUserModel.RoleModel> getAllRole()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getAllRole");
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AdminUserModel.RoleModel()
                             {
                                 id = dr["roleId"].ToString(),
                                 roleName = dr["roleName"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getAllRole >> " + ex.Message);
            }
        }

        public static List<AdminUserModel.User> getUserRoleByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserRoleByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AdminUserModel.User()
                             {
                                 empId = dr["empId"].ToString(),
                                 roleId = dr["roleId"].ToString(),
                                 companyId = dr["empCompanyId"].ToString(),
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getUserRoleByEmpId >> " + ex.Message);
            }
        }

        public static List<AdminUserModel.Customer> getcustomerRoleByEmpId(string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getcustomerRoleByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", empId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new AdminUserModel.Customer()
                             {
                                 cusId = dr["customerId"].ToString(),
                                 customerName = dr["cusName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 companyId = dr["companyId"].ToString(),

                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getcustomerRoleByEmpId >> " + ex.Message);
            }
        }


        public static List<Models.TB_Act_Other_Model> getCompany()
        {
            return QueryGetMessage.getOtherByType("company");
        }

        public static int insertCustomer(Activity_Model model)
        {
            int rtn = 0;
            try
            {

                rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertCustomer"
                           , new SqlParameter[] {new SqlParameter("@id",model.customerModel.id)
                            ,new SqlParameter("@companyId",model.customerModel.companyId)
                            ,new SqlParameter("@cusTrading",model.customerModel.cusTrading)
                            ,new SqlParameter("@cusNameTH",model.customerModel.cusNameTH)
                            ,new SqlParameter("@cusNameEN",model.customerModel.cusNameEN)
                            ,new SqlParameter("@cusShort",model.customerModel.cusShortName)
                            ,new SqlParameter("@regionId",model.customerModel.regionId)
                            ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                           });

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "  " + "insertCustomer");
            }
        }

        public static int delCustomer(string id)
        {
            int rtn = 0;
            try
            {

                rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_delCustomer"
                           , new SqlParameter[] {new SqlParameter("@id",id)
                           ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                           });

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "  " + "delCustomer");
            }
        }


        public static bool checkEmpRoleSuperAdmin(string empId)
        {
            bool result;
            result = QueryOtherMaster.getOhterMaster("roleuser", "groupSuperAdmin").Where(x => x.val1.Contains(empId)).Any();
            return result;
        }

        public static int InserToTempProductPrice(string strCon, ProductPrice model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertTempProductPrice"
                , new SqlParameter[] {new SqlParameter("@cusNameEN",model.customerName)
                         ,new SqlParameter("@productId",model.productId)
                         ,new SqlParameter("@normalCost",model.normalCost)
                         ,new SqlParameter("@wholeSalesPrice",model.wholeSalesPrice)
                         ,new SqlParameter("@discount1",model.discount1)
                         ,new SqlParameter("@discount2",model.discount2)
                         ,new SqlParameter("@discount3",model.discount3)
                         ,new SqlParameter("@saleNormal",model.saleNormal)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InserToTempProductPrice => " + ex.Message);
            }

            return result;
        }

        public static int deleteTempProductPrice(string strCon, string empId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_deleteTempProductPrice"
                , new SqlParameter[] {new SqlParameter("@empId",empId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("deleteTempProductPrice => " + ex.Message);
            }

            return result;
        }

        public static List<ProductPrice> getProductPriceAterImport(string strCon, string empId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getProductPriceTemp"
                    , new SqlParameter[] { new SqlParameter("@empId",empId)
                    });


                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new ProductPrice()
                             {
                                 status = d["status"].ToString(),
                                 customerId = d["cusId"].ToString(),
                                 customerName = d["cusNameEN"].ToString(),
                                 productId = d["productId"].ToString(),
                                 normalCost = d["normalCost"] is DBNull ? 0 : (decimal?)d["normalCost"],
                                 old_normalCost = d["old_normalCost"] is DBNull ? 0 : (decimal?)d["old_normalCost"],
                                 wholeSalesPrice = d["wholeSalesPrice"] is DBNull ? 0 : (decimal?)d["wholeSalesPrice"],
                                 old_wholeSalesPrice = d["old_wholeSalesPrice"] is DBNull ? 0 : (decimal?)d["old_wholeSalesPrice"],
                                 discount1 = d["discount1"] is DBNull ? 0 : (decimal?)d["discount1"],
                                 old_discount1 = d["old_discount1"] is DBNull ? 0 : (decimal?)d["old_discount1"],
                                 discount2 = d["discount2"] is DBNull ? 0 : (decimal?)d["discount2"],
                                 old_discount2 = d["old_discount2"] is DBNull ? 0 : (decimal?)d["old_discount2"],
                                 discount3 = d["discount3"] is DBNull ? 0 : (decimal?)d["discount3"],
                                 old_discount3 = d["old_discount3"] is DBNull ? 0 : (decimal?)d["old_discount3"],
                                 saleNormal = d["saleNormal"] is DBNull ? 0 : (decimal?)d["saleNormal"],
                                 old_saleNormal = d["old_saleNormal"] is DBNull ? 0 : (decimal?)d["old_saleNormal"],
                                 createdByUserId = d["createdByUserId"].ToString(),
                             });
                return lists.ToList();


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getProductPriceAterImport => " + ex.Message);
                return null;
            }
        }

        public static int confirmInsertProductPrice(string strCon, string empId)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_confirmInsertProductPrice"
                , new SqlParameter[] {new SqlParameter("@empId",empId)
                     });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("confirmInsertProductPrice => " + ex.Message);
            }

            return result;
        }


    }
}