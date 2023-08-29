using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Presenter.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using static eActForm.Models.ActUserModel;

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

                if (actType == Activity_Model.activityType.MT.ToString() || actType == Activity_Model.activityType.MT_AddOn.ToString())
                {
                    return ConfigurationManager.AppSettings["companyId_MT"].ToString();
                }
                else if (actType == Activity_Model.activityType.OMT.ToString() || actType == Activity_Model.activityType.OMT_AddOn.ToString())
                {
                    return ConfigurationManager.AppSettings["companyId_OMT"].ToString();
                }
                else if (actType == Activity_Model.activityType.TBM.ToString())
                {
                    return ConfigurationManager.AppSettings["companyId_TBM"].ToString();
                }
                else if (actType == Activity_Model.activityType.HCM.ToString())
                {
                    return ConfigurationManager.AppSettings["companyId_HCM"].ToString();
                }
                else if (actType == Activity_Model.activityType.EXPENSE.ToString())
                {
                    return ConfigurationManager.AppSettings["companyId_Thaibev"].ToString();
                }
                else if (actType == Activity_Model.activityType.HCForm.ToString())
                {
                    String compId = "";
                    if (UtilsAppCode.Session.User.isSuperAdmin
                        || UtilsAppCode.Session.User.isAdminHCBP)
                    {
                        List<TB_Act_Other_Model> lst = new List<TB_Act_Other_Model>();
                        lst = QueryOtherMaster.getOhterMaster("company", Activity_Model.groupCompany.NUM.ToString());
                        foreach (var item in lst)
                        {
                            compId += item.val1 + ",";
                        }

                        lst = QueryOtherMaster.getOhterMaster("company", Activity_Model.groupCompany.POM.ToString());
                        foreach (var item in lst)
                        {
                            compId += item.val1 + ",";
                        }

                        lst = QueryOtherMaster.getOhterMaster("company", Activity_Model.groupCompany.TTM.ToString());
                        foreach (var item in lst)
                        {
                            compId += item.val1 + ",";
                        }

                        if (UtilsAppCode.Session.User.isSuperAdmin)
                        {
                            lst = QueryOtherMaster.getOhterMaster("company", Activity_Model.groupCompany.CVM.ToString());
                            foreach (var item in lst)
                            {
                                compId += item.val1 + ",";
                            }
                        }

                        compId = compId.Substring(0, compId.Length - 1);
                    }
                    else
                    {
                        List<ActUserModel.UserAuthorized> lst = new List<ActUserModel.UserAuthorized>();
                        lst = UserAppCode.GetUserAuthorizedsByCompany(UtilsAppCode.Session.User.empCompanyGroup);
                        compId = lst.Count > 0 ? lst.FirstOrDefault().companyId : "";
                    }


                    return compId; //ถ้าเป็น superadmim ถึงจะดึงทั้ง 8 ถ้าไม่ดึงตัวเอง

                }
                else
                {
                    return UtilsAppCode.Session.User.empCompanyId;
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
                else if (comId == ConfigurationManager.AppSettings["companyId_ChangBeer"].ToString())
                {
                    return Activity_Model.activityType.Beer.ToString();
                }
                else if (comId == ConfigurationManager.AppSettings["companyId_HCM"].ToString())
                {
                    return Activity_Model.activityType.HCM.ToString();
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
        public static DateTime converStrToDateTime(string p_date)
        {
            return DateTime.ParseExact(p_date, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public static DateTime converStrToDatetimeWithFormat(string p_date, string formatDate)
        {
            return DateTime.ParseExact(p_date, formatDate, CultureInfo.InvariantCulture);
        }
        public static User getEmpFromApi(string empId)
        {
            ActUserModel.ResponseUserAPI response = new ActUserModel.ResponseUserAPI();
            response = AuthenAppCode.doAuthenInfo(empId);
            User userModel = new User();
            if (response != null && response.userModel.Count > 0)
            {
                userModel = response.userModel[0];
            }
            return userModel;
        }

        public static void WriteSignatureToDisk(ApproveModel.approveModels approveModels, string activityId)
        {

            var modelApproveDetail = approveModels.approveDetailLists.Where(x => x.statusId.Equals("3")).ToList();
            if (modelApproveDetail.Any())
            {
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], activityId)));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], activityId)));

                foreach (var item in modelApproveDetail)
                {
                    UtilsAppCode.Session.writeFileHistory(System.Web.HttpContext.Current.Server
                        , item.signature
                        , string.Format(ConfigurationManager.AppSettings["rootSignaByActURL"], activityId, item.empId));
                }
            }

        }



    }
}