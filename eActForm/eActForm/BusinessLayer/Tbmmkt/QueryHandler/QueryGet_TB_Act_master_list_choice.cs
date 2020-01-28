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
    public class QueryGet_TB_Act_master_list_choice
    {
        public static List<TB_Act_master_list_choiceModel> get_TB_Act_master_list_choice(string master_type_form_id, string type)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_TB_Act_master_list_choice"
                       , new SqlParameter("@master_type_form_id", master_type_form_id)
                           , new SqlParameter("@type", type));
                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_master_list_choiceModel()
                              {
                                  id = d["id"].ToString(),
                                  name = d["name"].ToString(),
                                  sub_name = d["sub_name"].ToString(),
                                  type = d["type"].ToString(),
                                  master_type_form_id = d["master_type_form_id"].ToString(),
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
                ExceptionManager.WriteError("get_channelMasterType => " + ex.Message);
                return new List<TB_Act_master_list_choiceModel>();
            }
        }


        //public JsonResult getProductGroup(string cateId)
        //{
        //    var result = new AjaxResult();
        //    try
        //    {
        //        var getProductGroup = QueryGetAllProductGroup.getAllProductGroup().Where(x => x.cateId.Trim() == cateId.Trim()).ToList();
        //        var resultData = new
        //        {
        //            //productGroup = getProductGroup.GroupBy(item => item.productGroup)
        //            //.Select(group => new TB_Act_Product_Cate_Model.Product_Cate_Model
        //            //{
        //            //    id = group.First().id,
        //            //    productGroup = group.First().productGroup,
        //            //}).ToList(),
        //            productGroup = getProductGroup.ToList(),
        //        };
        //        result.Data = resultData;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Success = false;
        //        result.Message = ex.Message;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
    }
}