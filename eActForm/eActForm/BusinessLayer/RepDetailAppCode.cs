using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using eActForm.Models;
namespace eActForm.BusinessLayer
{
    public class RepDetailAppCode
    {
        public static RepDetailModel.actFormRepDetails getRepDetailReportByCreateDateAndStatusId(string statusId, string startDate, string endDate)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("getRepDetailReportByCreateDateAndStatusId >>" + ex.Message);
            }
        }
    }
}