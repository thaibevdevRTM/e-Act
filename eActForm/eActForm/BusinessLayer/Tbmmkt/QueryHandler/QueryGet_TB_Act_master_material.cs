using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;


namespace eActForm.BusinessLayer
{
    public class QueryGet_TB_Act_master_material
    {
        public static List<TB_Act_master_material_Model> get_TB_Act_master_material_autoComplete(ObjGetData_master_material_Model objGetData_Master_Material_Model)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_TB_Act_master_material_autoComplete"
                       , new SqlParameter("@select_tB_Act_master_list_choice_id", objGetData_Master_Material_Model.select_tB_Act_master_list_choice_id)
                       , new SqlParameter("@select_material", objGetData_Master_Material_Model.select_material)
                           , new SqlParameter("@select_materialDescription", objGetData_Master_Material_Model.select_materialDescription));
                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_master_material_Model()
                              {
                                  id = d["id"].ToString(),
                                  plnt = d["plnt"].ToString(),
                                  material = d["material"].ToString(),
                                  materialDescription = d["materialDescription"].ToString(),
                                  sloc = d["sloc"].ToString(),
                                  qty = int.Parse(d["qty"].ToString()),
                                  tB_Act_master_list_choice_id = d["tB_Act_master_list_choice_id"].ToString(),
                                  qtyName = d["qtyName"].ToString(),
                                  delFlag = bool.Parse(d["delFlag"].ToString()),
                                  createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                  createdByUserId = d["createdByUserId"].ToString(),
                                  updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                  updatedByUserId = d["updatedByUserId"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_TB_Act_master_material_autoComplete => " + ex.Message);
                return new List<TB_Act_master_material_Model>();
            }
        }

    }
}