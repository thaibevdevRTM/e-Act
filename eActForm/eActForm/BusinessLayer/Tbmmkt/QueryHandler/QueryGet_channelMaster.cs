﻿using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGet_channelMaster
    {
        public static List<TB_Act_ActivityForm_SelectBrandOrChannel> get_channelMaster(string master_type_form_id, string companyId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getChannelMaster"
                       , new SqlParameter("@master_type_form_id", master_type_form_id)
                         , new SqlParameter("@companyId", companyId));
                //, new SqlParameter("@groupName", groupName)
                var result = (from DataRow d in ds.Tables[0].Rows
                              select new TB_Act_ActivityForm_SelectBrandOrChannel()
                              {
                                  txt = d["groupName"].ToString(),
                                  val = d["subTypeName"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_channelMasterType => " + ex.Message);
                return new List<TB_Act_ActivityForm_SelectBrandOrChannel>();
            }
        }

        public static List<ChannelMasterType> get_channelMasterBySubTypeId(string groupName, string subTypeId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getChannelMasterBySubTypeId"
                       , new SqlParameter("@groupName", groupName)
                         , new SqlParameter("@subTypeId", subTypeId));
                //, new SqlParameter("@groupName", groupName)
                var result = (from DataRow d in ds.Tables[0].Rows
                              select new ChannelMasterType()
                              {
                                  groupName = d["groupName"].ToString(),
                                  costCenter = d["costCenter"].ToString(),
                              });

                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_channelMasterBySubTypeId => " + ex.Message);
                return new List<ChannelMasterType>();
            }
        }

    }
}