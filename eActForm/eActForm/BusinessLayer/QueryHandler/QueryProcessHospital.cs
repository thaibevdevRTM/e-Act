using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebLibrary;


namespace eActForm.BusinessLayer
{
    public class QueryProcessHospital
    {
        public static int updateHospital(HospitalModel hospList)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateHospitalById"
                            , new SqlParameter[] {
                      new SqlParameter("@id",hospList.id)
                    , new SqlParameter("@hospTypeId",hospList.hospTypeId)
                    , new SqlParameter("@hospNameTH",hospList.hospNameTH)
                    , new SqlParameter("@hospNameEN",hospList.hospNameEN)
                    , new SqlParameter("@provinceId",hospList.provinceId)
                    , new SqlParameter("@region",hospList.region)
                    , new SqlParameter("@delFlag",hospList.delFlag)
                    , new SqlParameter("@UserId",  UtilsAppCode.Session.User.empId)
                            });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateHospital");
            }

            return result;
        }

    }
}