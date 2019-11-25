using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace eActForm.BusinessLayer.Appcodes
{
    public class BaseAppCodes
    {
        public static Activity_Model.activityType getCompanyTypForm()
        {
            try
            {

                if (ConfigurationManager.AppSettings["companyId_MT"].ToString() == UtilsAppCode.Session.User.empCompanyId)
                {
                    return Activity_Model.activityType.MT;
                }
                else if (ConfigurationManager.AppSettings["companyId_OMT"].ToString() == UtilsAppCode.Session.User.empCompanyId)
                {
                    return Activity_Model.activityType.OMT;
                }
                else if (ConfigurationManager.AppSettings["companyId_TBM"].ToString() == UtilsAppCode.Session.User.empCompanyId)
                {
                    return Activity_Model.activityType.TBM;
                }
                else
                {
                    return Activity_Model.activityType.MT;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}