using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;


namespace eActForm.BusinessLayer
{
    public class QueryGet_channelByGroup
    {
        public static List<ChannelMasterType> get_channelByGroup(string master_type_form_id, string companyId, string groupName)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getChannelByGroup"
                       , new SqlParameter("@master_type_form_id", master_type_form_id)
                           //, new SqlParameter("@companyId", companyId)
                           , new SqlParameter("@groupName", groupName));
                //, new SqlParameter("@groupName", groupName)
                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ChannelMasterType()
                              {
                                  subTypeId = d["subTypeId"].ToString(),
                                  subName = d["subName"].ToString(),
                                  costCenter = d["costCenter"].ToString(),

                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_channelMasterType => " + ex.Message);
                return new List<ChannelMasterType>();
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