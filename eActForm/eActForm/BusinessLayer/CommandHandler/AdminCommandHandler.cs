using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
    }


}