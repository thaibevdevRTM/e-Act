using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using WebLibrary;


namespace eActForm.BusinessLayer
{
    public class QueryProcess_TB_Act_master_material
    {
        public static int insert_TB_Act_master_material(TB_Act_master_material_Model tB_Act_Master_Material_Model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertTB_Act_master_material"
                    , new SqlParameter[] {
                     new SqlParameter("@plnt",tB_Act_Master_Material_Model.plnt)
                    ,new SqlParameter("@material",tB_Act_Master_Material_Model.material)
                    ,new SqlParameter("@materialDescription",tB_Act_Master_Material_Model.materialDescription)
                    ,new SqlParameter("@sloc",tB_Act_Master_Material_Model.sloc)
                    ,new SqlParameter("@qty",tB_Act_Master_Material_Model.qty)
                    ,new SqlParameter("@tB_Act_master_list_choice_id",tB_Act_Master_Material_Model.tB_Act_master_list_choice_id)
                    ,new SqlParameter("@tB_Act_master_list_choice_id_InOutStock",tB_Act_Master_Material_Model.tB_Act_master_list_choice_id_InOutStock)
                    ,new SqlParameter("@qtyName",tB_Act_Master_Material_Model.qtyName)
                    ,new SqlParameter("@delFlag",tB_Act_Master_Material_Model.delFlag)
                    ,new SqlParameter("@createdDate",tB_Act_Master_Material_Model.createdDate)
                    ,new SqlParameter("@createdByUserId",tB_Act_Master_Material_Model.createdByUserId)
                    ,new SqlParameter("@updatedDate",tB_Act_Master_Material_Model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",tB_Act_Master_Material_Model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insert_TB_Act_master_material");
            }

            return result;
        }

    }
}