using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;

namespace eActForm.BusinessLayer.Appcodes
{
    public class BaseAppCodes
    {
        public static Activity_Model.activityType getCompanyTypeForm()
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

        public static string getCompanyIdByactivityType(string actType)
        {
            try
            {

                if (actType == Activity_Model.activityType.MT.ToString())
                {
                    return ConfigurationManager.AppSettings["companyId_MT"].ToString();
                }
                else if (actType == Activity_Model.activityType.OMT.ToString())
                {
                    return ConfigurationManager.AppSettings["companyId_OMT"].ToString();
                }
                else if (actType == Activity_Model.activityType.TBM.ToString())
                {
                    return ConfigurationManager.AppSettings["companyId_TBM"].ToString();
                }
                else
                {
                    return ConfigurationManager.AppSettings["companyId_MT"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string getactivityTypeByCompanyId(string comId)
        {
            try
            {

                if (comId == ConfigurationManager.AppSettings["companyId_MT"].ToString())
                {
                    return Activity_Model.activityType.MT.ToString();
                }
                else if (comId == ConfigurationManager.AppSettings["companyId_OMT"].ToString())
                {
                    return Activity_Model.activityType.OMT.ToString();
                }
                else if (comId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                {
                    return Activity_Model.activityType.TBM.ToString();
                }
                else
                {
                    return Activity_Model.activityType.MT.ToString();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool ValidateExtension(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return true;
                case ".png":
                    return true;
                case ".gif":
                    return true;
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }

        public static DateTime converStrToDate(string p_date)
        {
            return DateTime.ParseExact(p_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

    }
}