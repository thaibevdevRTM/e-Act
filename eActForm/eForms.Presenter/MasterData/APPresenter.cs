using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace eForms.Presenter.MasterData
{
    public class APPresenter
    {
        public static List<APModel> getDataAP(string strConn, string apCode)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getDataAP", new SqlParameter("@APCode", apCode));
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new APModel()
                             {
                                 id = dr["id"].ToString(),
                                 APCode = dr["APCode"].ToString(),
                                 Name1 = dr["Name1"].ToString(),
                                 CoNo = dr["CoNo"].ToString(),
                                 HouseNo = dr["HouseNo"].ToString(),
                                 Street = dr["Street"].ToString(),
                                 Street4 = dr["Street4"].ToString(),
                                 District = dr["District"].ToString(),
                                 City = dr["City"].ToString(),
                                 PostCode = dr["PostCode"].ToString(),
                                 Tel = dr["Tel"].ToString(),
                                 FaxNo = dr["FaxNo"].ToString(),
                                 FullAddress = dr["FullAddress"].ToString(),
                                 TelAndFax = dr["TelAndFax"].ToString(),
                                 delFlag = bool.Parse(dr["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(dr["createdDate"].ToString()),
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(dr["updatedDate"].ToString()),
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getDataAP >>" + ex.Message);
            }
        }

        public static int insert_TB_Act_Master_AP(string strConn, APModel aPModel)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "usp_insert_TB_Act_Master_AP"
                    , new SqlParameter[] {
                     new SqlParameter("@APCode",aPModel.APCode)
                    ,new SqlParameter("@Name1",aPModel.Name1)
                    ,new SqlParameter("@CoNo",aPModel.CoNo)
                    ,new SqlParameter("@HouseNo",aPModel.HouseNo)
                    ,new SqlParameter("@Street",aPModel.Street)
                    ,new SqlParameter("@Street4",aPModel.Street4)
                    ,new SqlParameter("@District",aPModel.District)
                    ,new SqlParameter("@City",aPModel.City)
                    ,new SqlParameter("@PostCode",aPModel.PostCode)
                    ,new SqlParameter("@Tel",aPModel.Tel)
                    ,new SqlParameter("@FaxNo",aPModel.FaxNo)
                    ,new SqlParameter("@delFlag",aPModel.delFlag)
                    ,new SqlParameter("@createdDate",aPModel.createdDate)
                    ,new SqlParameter("@createdByUserId",aPModel.createdByUserId)
                    ,new SqlParameter("@updatedDate",aPModel.updatedDate)
                    ,new SqlParameter("@updatedByUserId",aPModel.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("insert_TB_Act_Master_AP >>" + ex.Message);
            }

            return result;
        }

    }
}
