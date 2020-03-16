using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using WebLibrary;

namespace eActForm.BusinessLayer.CommandHandler
{
    public class AdminCommandHandler
    {
        public static int updatePriceProduct(TB_Act_ProductPrice_Model.ProductPrice model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateProductPrice"
                    , new SqlParameter[] {new SqlParameter("@productId",model.productCode)
                     ,new SqlParameter("@customerId",model.customerId)
                    ,new SqlParameter("@normalCost",model.normalCost)
                    ,new SqlParameter("@wholeSalesPrice",model.wholeSalesPrice)
                    ,new SqlParameter("@discount1",model.discount1)
                    ,new SqlParameter("@discount2",model.discount2)
                    ,new SqlParameter("@discount3",model.discount3)
                    ,new SqlParameter("@saleNormal",model.saleNormal)
                    ,new SqlParameter("@updatedDate",DateTime.Now)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updatePriceProduct");
            }

            return result;
        }

        public static int delProductMasterAndPrice(string productId)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_delProductAndProductPrice"
                    , new SqlParameter[] {new SqlParameter("@productId",productId)
                    ,new SqlParameter("@updatedDate",DateTime.Now)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> delProductMasterAndPrice");
            }

            return result;
        }


        public static int insertProduct(TB_Act_Product_Model.Product_Model model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertProductMaster"
                    , new SqlParameter[] {new SqlParameter("@productName",model.productName)
                     ,new SqlParameter("@productCode",model.productCode)
                    ,new SqlParameter("@cateId",model.cateId)
                    ,new SqlParameter("@groupId",model.groupId)
                    ,new SqlParameter("@brandId",model.brandId)
                    ,new SqlParameter("@smellId",model.smellId)
                    ,new SqlParameter("@size",model.size)
                    ,new SqlParameter("@pack",model.pack)
                    ,new SqlParameter("@unit",model.unit)
                    ,new SqlParameter("@digit_IO",model.digit_IO)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",DateTime.Now)
                    ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updatedDate",DateTime.Now)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertProduct");
            }

            return result;
        }




        public static int updateProduct(TB_Act_Product_Model.Product_Model model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateProductMaster"
                    , new SqlParameter[] {new SqlParameter("@productName",model.productName)
                     ,new SqlParameter("@productCode",model.productCode)
                    ,new SqlParameter("@cateId",model.cateId)
                    ,new SqlParameter("@groupId",model.groupId)
                    ,new SqlParameter("@brandId",model.brandId)
                    ,new SqlParameter("@smellId",model.smellId)
                    ,new SqlParameter("@size",model.size)
                    ,new SqlParameter("@pack",model.pack)
                    ,new SqlParameter("@unit",model.unit)
                    ,new SqlParameter("@digit_IO",model.digit_IO)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@updatedDate",DateTime.Now)
                    ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateProduct");
            }

            return result;
        }


        public static int insertProductPrice(string productCode, string customerId)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertProductPriceMaster"
                    , new SqlParameter[] {new SqlParameter("@productId",productCode)
                    , new SqlParameter("@customerId", customerId)
                    , new SqlParameter("@delFlag", 0)
                    , new SqlParameter("@createdDate", DateTime.Now)
                    , new SqlParameter("@createdByUserId", UtilsAppCode.Session.User.empId)
                    , new SqlParameter("@updatedDate", DateTime.Now)
                    , new SqlParameter("@updatedByUserId", UtilsAppCode.Session.User.empId)
                        });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertProductPrice");
            }

            return result;
        }


    }

}