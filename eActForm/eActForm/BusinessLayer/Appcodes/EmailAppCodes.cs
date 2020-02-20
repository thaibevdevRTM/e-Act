using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using eActForm.Models;
using eActForm.Controllers;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace eActForm.BusinessLayer
{
    public class EmailAppCodes
    {
        public static string departmentInterSale = "941F6AC9-A36E-48A4-8769-1A267EB8BC3A";
        public static void sendRequestCancelToAdmin(string actFormId)
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserAdminByActId"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });
                string actNo = QueryGetActivityById.getActivityById(actFormId)[0].activityNo;
                string strLink = string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId);
                string strBody = string.Format(ConfigurationManager.AppSettings["emailRequestCancelBody"], actNo
                    , UtilsAppCode.Session.User.empFNameTH + " " + UtilsAppCode.Session.User.empLNameTH
                    , strLink);
                string mailTo = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mailTo += mailTo == "" ? dr["empEmail"].ToString() : "," + dr["empEmail"].ToString();
                }

                sendEmail(mailTo
                    , ConfigurationManager.AppSettings["emailApproveCC"]
                    , ConfigurationManager.AppSettings["emailRequestCancelSubject"]
                    , strBody
                    , null);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sendRequestCancelToAdmin >>" + ex.Message);
            }
        }

        public static void sendRejectRepDetail()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserAdmin");
                string strLink = string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], "");
                string strBody = string.Format(ConfigurationManager.AppSettings["emailRejectsummaryDetailBody"], strLink);
                string mailTo = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mailTo += mailTo == "" ? dr["empEmail"].ToString() : "," + dr["empEmail"].ToString();
                }
                sendEmail(mailTo
                    , ConfigurationManager.AppSettings["emailApproveCC"]
                    , ConfigurationManager.AppSettings["emailRejectSubject"]
                    , strBody
                    , null);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sendRequestCancelToAdmin >>" + ex.Message);
            }
        }


        public static void sendRejectsummaryDetail()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserAdmin");
                string strLink = string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], "");
                string strBody = string.Format(ConfigurationManager.AppSettings["emailRejectRepDetailBody"], strLink);
                string mailTo = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mailTo += mailTo == "" ? dr["empEmail"].ToString() : "," + dr["empEmail"].ToString();
                }
                sendEmail(mailTo
                    , ConfigurationManager.AppSettings["emailApproveCC"]
                    , ConfigurationManager.AppSettings["emailRejectSubject"]
                    , strBody
                    , null);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sendRequestCancelToAdmin >>" + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actFormId"></param>
        /// <param name="emailType"></param>
        /// <param name="currentEmpId"> fixed support API Background Service </param>
        public static void sendReject(string actFormId, AppCode.ApproveType emailType, string currentEmpId)
        {
            try
            {
                ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actFormId, currentEmpId);
                if (models.approveDetailLists != null && models.approveDetailLists.Count > 0)
                {
                    #region get mail to
                    var lists = (from m in models.approveDetailLists
                                 where (m.statusId != "")
                                 select m).ToList();
                    string strMailTo = "", strMailCc = "";
                    foreach (ApproveModel.approveDetailModel m in lists)
                    {
                        strMailCc += (strMailCc == "") ? m.empEmail : "," + m.empEmail; // get list email
                    }
                    List<ApproveModel.approveDetailModel> createUsers = ActFormAppCode.getUserCreateActForm(actFormId);
                    foreach (ApproveModel.approveDetailModel m in createUsers)
                    {
                        strMailTo += (strMailTo == "") ? m.empEmail : "," + m.empEmail; // get list email
                    }
                    #endregion

                    var empUser = models.approveDetailLists.Where(r => r.empId == currentEmpId).ToList(); // get current user
                    string strLink = string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId);

                    string emailRejectBody = "";
                    string emailRejectSubject = "";
                    string txtEmpUser = "";
                    Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actFormId);

                    emailRejectBody = ConfigurationManager.AppSettings["emailRejectBody"];
                    emailRejectSubject = ConfigurationManager.AppSettings["emailRejectSubject"];
                    txtEmpUser = empUser.FirstOrDefault().empName;
                    if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                    {
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId == departmentInterSale)// Inter sale
                        {
                            emailRejectBody = ConfigurationManager.AppSettings["emailRejectBody_EN"];
                            emailRejectSubject = ConfigurationManager.AppSettings["emailRejectSubject_EN"];
                            txtEmpUser = empUser.FirstOrDefault().empName_EN;
                        }
                    }

                    string strBody = string.Format(emailRejectBody
                        , models.approveModel.actNo
                        , empUser.FirstOrDefault().empPrefix + " " + txtEmpUser
                        , empUser.FirstOrDefault().remark
                        , strLink
                        );

                    sendEmailActForm(actFormId
                        , strMailTo
                        , strMailCc
                        , emailRejectSubject
                        , strBody
                        , emailType);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sendRejectActForm >>" + ex.Message + " " + actFormId);
            }
        }

        public static async Task<string> sendApproveAsync(string actFormId, AppCode.ApproveType emailType, bool isResend)
        {
            try
            {
                List<ApproveModel.approveEmailDetailModel> lists = new List<ApproveModel.approveEmailDetailModel>();

                switch (emailType)
                {
                    case AppCode.ApproveType.Activity_Form:
                        lists = getEmailApproveNextLevel(actFormId);
                        break;
                    case AppCode.ApproveType.Report_Detail:
                        lists = getEmailApproveRepDetailNextLevel(actFormId);
                        break;
                    case AppCode.ApproveType.Report_Summary:
                        lists = getEmailApproveSummaryNextLevel(actFormId);
                        break;
                }

                string emailAllApprovedSubject = "";
                string createUsersName = "";
                string txtemailAllApproveBody = "";
                Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actFormId);

                string strBody = "", strSubject = "";
                if (lists.Count > 0)
                {
                    emailAllApprovedSubject = ConfigurationManager.AppSettings["emailApproveSubject"];
                    if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                    {
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId == departmentInterSale)// Inter sale
                        {
                            emailAllApprovedSubject = ConfigurationManager.AppSettings["emailApproveSubject_EN"];
                        }
                    }

                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                    {
                        strBody = getEmailBody(item, emailType, actFormId);
                        strSubject = emailAllApprovedSubject;
                        strSubject = isResend ? "RE: " + strSubject : strSubject;
                        sendEmailActForm(actFormId
                            , item.empEmail
                            , ""
                            , strSubject
                            , strBody
                            , emailType);
                    }
                }
                else
                {
                    // case all updated
                    DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountStatusApproveDetail"
                        , new SqlParameter[] {new SqlParameter("@actFormId",actFormId)
                        ,new SqlParameter("@statusId",(int)AppCode.ApproveStatus.อนุมัติ)});

                    if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (dr["countAll"].ToString() == dr["countStatusApproved"].ToString())
                        {
                            // all approved then send the email notification to user create
                            List<ApproveModel.approveDetailModel> createUsers = (emailType == AppCode.ApproveType.Activity_Form) ? ActFormAppCode.getUserCreateActForm(actFormId)
                                : RepDetailAppCode.getUserCreateRepDetailForm(actFormId);

                            createUsersName = createUsers.FirstOrDefault().empName;
                            emailAllApprovedSubject = ConfigurationManager.AppSettings["emailAllApprovedSubject"];
                            txtemailAllApproveBody = ConfigurationManager.AppSettings["emailAllApproveBody"];
                            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                            {
                                if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId == departmentInterSale)// Inter sale
                                {
                                    createUsersName = createUsers.FirstOrDefault().empName_EN;
                                    emailAllApprovedSubject = ConfigurationManager.AppSettings["emailAllApprovedSubject_EN"];
                                    txtemailAllApproveBody = ConfigurationManager.AppSettings["emailAllApproveBody_EN"];
                                }
                            }

                            strBody = (emailType == AppCode.ApproveType.Activity_Form)
                                ? string.Format(txtemailAllApproveBody
                                    , createUsersName
                                    , createUsers.FirstOrDefault().activityNo
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId))
                                : string.Format(ConfigurationManager.AppSettings["emailAllApproveRepDetailBody"]
                                    , createUsersName
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId));

                            sendEmailActForm(actFormId
                            , createUsers.FirstOrDefault().empEmail
                            , ""
                            , emailAllApprovedSubject
                            , strBody
                            , emailType);
                        }
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Email sendApproveActForm >> " + ex.Message);
                throw new Exception("sendEmailApprove " + ex.Message);
            }
        }

        public static void sendApprove(string actFormId, AppCode.ApproveType emailType, bool isResend)
        {
            try
            {
                List<ApproveModel.approveEmailDetailModel> lists = new List<ApproveModel.approveEmailDetailModel>();

                switch (emailType)
                {
                    case AppCode.ApproveType.Activity_Form:
                        lists = getEmailApproveNextLevel(actFormId);
                        break;
                    case AppCode.ApproveType.Report_Detail:
                        lists = getEmailApproveRepDetailNextLevel(actFormId);
                        break;
                    case AppCode.ApproveType.Report_Summary:
                        lists = getEmailApproveSummaryNextLevel(actFormId);
                        break;
                }

                string emailAllApprovedSubject = "";
                string createUsersName = "";
                string txtemailAllApproveBody = "";
                Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actFormId);

                string strBody = "", strSubject = "";
                strSubject = ConfigurationManager.AppSettings["emailApproveSubject"];
                if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                {
                    if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId == departmentInterSale)//Inter sale
                    {
                        strSubject = ConfigurationManager.AppSettings["emailApproveSubject_EN"];
                    }
                }

                if (lists.Count > 0)
                {
                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                    {
                        strBody = getEmailBody(item, emailType, actFormId);
                        strSubject = isResend ? "RE: " + strSubject : strSubject;
                        sendEmailActForm(actFormId
                            , item.empEmail
                            , ""
                            , strSubject
                            , strBody
                            , emailType);
                    }
                }
                else
                {
                    // case all updated
                    DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountStatusApproveDetail"
                        , new SqlParameter[] {new SqlParameter("@actFormId",actFormId)
                        ,new SqlParameter("@statusId",(int)AppCode.ApproveStatus.อนุมัติ)});

                    if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (dr["countAll"].ToString() == dr["countStatusApproved"].ToString())
                        {
                            // all approved then send the email notification to user create
                            List<ApproveModel.approveDetailModel> createUsers = (emailType == AppCode.ApproveType.Activity_Form) ? ActFormAppCode.getUserCreateActForm(actFormId)
                                : RepDetailAppCode.getUserCreateRepDetailForm(actFormId);

                            createUsersName = createUsers.FirstOrDefault().empName;
                            emailAllApprovedSubject = ConfigurationManager.AppSettings["emailAllApprovedSubject"];
                            txtemailAllApproveBody = ConfigurationManager.AppSettings["emailAllApproveBody"];
                            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                            {
                                if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId == departmentInterSale)// Inter sale
                                {
                                    createUsersName = createUsers.FirstOrDefault().empName_EN;
                                    emailAllApprovedSubject = ConfigurationManager.AppSettings["emailAllApprovedSubject_EN"];
                                    txtemailAllApproveBody = ConfigurationManager.AppSettings["emailAllApproveBody_EN"];
                                }
                            }

                            strBody = (emailType == AppCode.ApproveType.Activity_Form)
                                ? string.Format(txtemailAllApproveBody
                                    , createUsersName
                                    , createUsers.FirstOrDefault().activityNo
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId))
                                : string.Format(ConfigurationManager.AppSettings["emailAllApproveRepDetailBody"]
                                    , createUsersName
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId));

                            sendEmailActForm(actFormId
                            , createUsers.FirstOrDefault().empEmail
                            , ApproveAppCode.getEmailCCByActId(actFormId)
                            , emailAllApprovedSubject
                            , strBody
                            , emailType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Email sendApproveActForm >> " + ex.Message);
                throw new Exception("sendEmailApprove" + ex.Message);
            }
        }

        private static void sendEmailActForm(string actFormId, string mailTo, string mailCC, string strSubject, string strBody, AppCode.ApproveType emailType)
        {
            //================fream devdate 20191213 ดึงค่าเพื่อเอาเลขที่เอกสารไปRenameชื่อไฟล์แนบ==================
            ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();
            //=====END===========fream devdate 20191213 ดึงค่าเพื่อเอาเลขที่เอกสารไปRenameชื่อไฟล์แนบ==================

            List<Attachment> files = new List<Attachment>();
            string[] pathFile = new string[10];
            mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"].ToString() : mailTo;
            mailCC = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailApproveCC"].ToString() : mailCC;//ถ้าจะเทส ดึงCC จากDevไปเปลี่ยนรหัสพนักงานเองเลยที่ตาราง TB_Reg_ApproveDetail

            switch (emailType)
            {
                case AppCode.ApproveType.Activity_Form:
                    pathFile[0] = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actFormId));
                    activityFormTBMMKT = QueryGetActivityByIdTBMMKT.getActivityById(actFormId).FirstOrDefault();
                    break;
                case AppCode.ApproveType.Report_Detail:
                    pathFile[0] = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actFormId));

                    break;
                case AppCode.ApproveType.Report_Summary:
                    pathFile[0] = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootSummaryDetailPdftURL"], actFormId));

                    break;
            }

            TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
            getImageModel.tbActImageList = ImageAppCode.GetImage(actFormId);
            if (getImageModel.tbActImageList.Any())
            {
                int i = 1;
                foreach (var item in getImageModel.tbActImageList)
                {
                    //=== use case this background service (bg service cannot get data session)===
                    string companyId = "";
                    try
                    {
                        companyId = UtilsAppCode.Session.User.empCompanyId;
                    }
                    catch
                    {
                        companyId = QueryGetActivityById.getActivityById(actFormId)[0].companyId;
                    }
                    // ==================================================
                    if (companyId == ConfigurationManager.AppSettings["companyId_TBM"])
                    {
                        pathFile[i] = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                    }
                    else
                    {
                        if (item.imageType == AppCode.ApproveType.Report_Detail.ToString())
                        {
                            pathFile[i] = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], item._fileName));
                        }
                    }
                    i++;
                }
            }

            var i_loop_change_name = 0;
            foreach (var item in pathFile)
            {
                if (System.IO.File.Exists(item))
                {
                    //files.Add(new Attachment(item));// ของเดิมก่อนทำเปลี่ยนชื่อไฟล์ 20191213
                    Attachment attachment;
                    attachment = new Attachment(item);
                    if (i_loop_change_name == 0 && emailType == AppCode.ApproveType.Activity_Form)
                    {
                        attachment.Name = replaceWordDangerForNameFile(activityFormTBMMKT.activityNo) + ".pdf";
                    }
                    files.Add(attachment);
                    i_loop_change_name++;
                }
            }

            sendEmail(mailTo
                    , mailCC
                    , strSubject
                    , strBody
                    , files);
        }

        public static string replaceWordDangerForNameFile(string txt)
        {
            return txt.Replace("/", "_").Replace(" ", "_").Replace(@"\", "_");
        }


        private static string getEmailBody(ApproveModel.approveEmailDetailModel item, AppCode.ApproveType emailType, string actId)
        {
            try
            {

                //=============dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========
                Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actId);
                var emailTypeTxt = "";
                string[] arrayFormStyleV1 = { "488F7C0D-8B59-4BDE-90E4-F157BFF67829", "8C4511BA-E0D6-4E6F-AD8D-62A5431E4BD4", "D3741442-EAD8-40F6-9A94-339DA65C0428" };
                string[] arrayFormStyleV2 = { "24BA9F57-586A-4A8E-B54C-00C23C41BFC5", "294146B1-A6E5-44A7-B484-17794FA368EB", "B4F405D7-8AAF-4B03-8AFB-3EC8F292AA90" };
                string[] arrayFormStyleV3 = { "24BA9F57-586A-4A8E-B54C-00C23C41BFC5" };
                if (arrayFormStyleV1.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id) || arrayFormStyleV2.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id) || arrayFormStyleV3.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                {
                    emailTypeTxt = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm;
                    if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                    {
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId == departmentInterSale)//Inter sale
                        {
                            emailTypeTxt = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm_EN;
                        }
                    }
                }
                else
                {
                    emailTypeTxt = emailType.ToString().Replace("_", " ");
                }
                //=======END======dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========

                string strBody = "";
                string empNameResult = "";
                string txtApprove = "";
                string txtcreateBy = "";
                string txtCompanyname = "";
                switch (emailType)
                {
                    case AppCode.ApproveType.Activity_Form:
                        var models = ActFormAppCode.getUserCreateActForm(actId);
                        strBody = ConfigurationManager.AppSettings["emailApproveBody"];
                        empNameResult = item.empName;
                        txtApprove = AppCode.ApproveStatus.รออนุมัติ.ToString();
                        txtcreateBy = item.createBy;
                        txtCompanyname = models[0].companyName;
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                        {
                            if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId == departmentInterSale)//Inter sale
                            {
                                strBody = ConfigurationManager.AppSettings["emailApproveBody_EN"];
                                empNameResult = item.empName_EN;
                                txtApprove = "";
                                txtcreateBy = item.createBy_EN;
                                txtCompanyname = models[0].companyNameEN;
                            }
                        }

                        if (arrayFormStyleV1.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                        {
                            strBody = strBody.Replace("<b>ประเภทกิจกรรม :</b> {4}<br>", "");
                            strBody = strBody.Replace("<b>Type Activity :</b> {4}<br>", "");
                        }
                        if (arrayFormStyleV2.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                        {
                            strBody = strBody.Replace("<b>เรื่อง :</b> {3}<br>", "").Replace("<b>ประเภทกิจกรรม :</b> {4}<br>", "");
                            strBody = strBody.Replace("<b>Subject :</b> {3}<br>", "").Replace("<b>Type Activity :</b> {4}<br>", "");
                        }
                        if (arrayFormStyleV3.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                        {
                            strBody = strBody.Replace("<b>จำนวนเงินที่ขอนุมัติ :</b> {6} บาท<br>", "");
                            strBody = strBody.Replace("<b>Amount Requested :</b> {6} Bath<br>", "");
                        }
                        strBody = string.Format(strBody
                            , item.empPrefix + " " + empNameResult
                            , txtApprove
                            , emailTypeTxt
                            , item.activityName
                            , item.activitySales
                            , item.activityNo
                            , String.Format("{0:0,0.00}", item.sumTotal)
                            , (models != null && models.Count > 0) ? txtCompanyname : ""
                            , txtcreateBy
                            , string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actId));
                        break;
                    case AppCode.ApproveType.Report_Detail:
                        strBody = string.Format(ConfigurationManager.AppSettings["emailApproveRepDetailBody"]
                            , item.empPrefix + " " + item.empName //เรียน
                            , AppCode.ApproveStatus.รออนุมัติ.ToString()
                            , item.activityNo
                            , emailType.ToString().Replace("_", " ")
                            , item.customerName
                            , item.productTypeName
                            , item.createBy
                            , string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actId));
                        break;
                    case AppCode.ApproveType.Report_Summary:
                        strBody = string.Format(ConfigurationManager.AppSettings["emailApproveSummaryDetailBody"]
                            , item.empPrefix + " " + item.empName //เรียน
                            , AppCode.ApproveStatus.รออนุมัติ.ToString()
                            , item.activityNo
                            , emailType.ToString().Replace("_", " ")
                            , item.createBy
                            , string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actId));
                        break;
                }

                return strBody;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static List<ApproveModel.approveEmailDetailModel> getEmailApproveNextLevel(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveNextLevel"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                var models = (from DataRow dr in ds.Tables[0].Rows
                              select new ApproveModel.approveEmailDetailModel()
                              {
                                  empEmail = dr["empEmail"].ToString(),
                                  empPrefix = dr["empPrefix"].ToString(),
                                  empName = dr["empName"].ToString(),
                                  empName_EN = dr["empName_EN"].ToString(),
                                  activityName = dr["activityName"].ToString(),
                                  activitySales = dr["activitySales"].ToString(),
                                  activityNo = dr["activityNo"].ToString(),
                                  sumTotal = dr["sumTotal"] is DBNull ? 0 : (decimal)dr["sumTotal"],
                                  createBy = dr["createBy"].ToString(),
                                  createBy_EN = dr["createBy_EN"].ToString(),
                              }).ToList();
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getEmailNextLevel >> " + ex.Message);
            }
        }

        private static List<ApproveModel.approveEmailDetailModel> getEmailApproveRepDetailNextLevel(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveRepDetailNextLevel"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                var models = (from DataRow dr in ds.Tables[0].Rows
                              select new ApproveModel.approveEmailDetailModel()
                              {
                                  empEmail = dr["empEmail"].ToString(),
                                  empPrefix = dr["empPrefix"].ToString(),
                                  empName = dr["empName"].ToString(),
                                  productTypeName = dr["productTypeName"].ToString(),
                                  customerName = dr["customerName"].ToString(),
                                  activityNo = dr["activityNo"].ToString(),
                                  createBy = dr["createBy"].ToString(),
                              }).ToList();
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getEmailNextLevel >> " + ex.Message);
            }
        }

        private static List<ApproveModel.approveEmailDetailModel> getEmailApproveSummaryNextLevel(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveSummaryNextLevel"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                var models = (from DataRow dr in ds.Tables[0].Rows
                              select new ApproveModel.approveEmailDetailModel()
                              {
                                  empEmail = dr["empEmail"].ToString(),
                                  empPrefix = dr["empPrefix"].ToString(),
                                  empName = dr["empName"].ToString(),
                                  productTypeName = dr["productTypeName"].ToString(),
                                  customerName = dr["customerName"].ToString(),
                                  activityNo = dr["activityNo"].ToString(),
                                  createBy = dr["createBy"].ToString(),
                              }).ToList();
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getEmailNextLevel >> " + ex.Message);
            }
        }

        public static void sendEmail(string mailTo, string cc, string subject, string body, List<Attachment> files)
        {
            //string slog = "";
            //slog = "begin sendEmail ";
            //slog = slog + "emailFrom=>" + ConfigurationManager.AppSettings["emailFrom"];
            //slog = slog + "emailFromPass=>" + ConfigurationManager.AppSettings["emailFromPass"];
            //slog = slog + "mailTo=>" + mailTo;
            //slog = slog + "subject=>" + subject;
            //slog = slog + "cc=>" + cc;
            //ExceptionManager.WriteError("sendEmail >> " + slog);
            //mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"] : mailTo;

            GMailer.Mail_From = ConfigurationManager.AppSettings["emailFrom"];
            GMailer.GmailPassword = ConfigurationManager.AppSettings["emailFromPass"];
            GMailer mailer = new GMailer();
            mailer.ToEmail = mailTo;
            mailer.Subject = subject;
            mailer.Body = body;
            mailer.p_Attachment = files;
            if (!String.IsNullOrEmpty(cc))
            {
                mailer.CC = cc;
            }

            mailer.IsHtml = true;
            mailer.Send();


            //slog = "mailer.Send() => ok";
            //ExceptionManager.WriteError("sendEmail=> " + slog);
        }

        public static void resendHistory(string actId)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "ups_insertResendHistory"
                    , new SqlParameter[] {
                        new SqlParameter("@id",Guid.NewGuid().ToString())
                        ,new SqlParameter("@actFormId",actId)
                        ,new SqlParameter("@delFlag",false)
                        ,new SqlParameter("@createdDate",DateTime.Now)
                        ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                        ,new SqlParameter("@updatedDate",DateTime.Now)
                        ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("resendHistory>> " + ex.Message);
            }
        }

        //--------------------------------------------------------------------------//

        public static void sendRejectBudget(string actFormId, AppCode.ApproveType emailType)
        {
            try
            {
                ApproveModel.approveModels models = BudgetApproveController.getApproveByBudgetApproveId(actFormId);
                if (models.approveDetailLists != null && models.approveDetailLists.Count > 0)
                {
                    #region get mail to
                    var lists = (from m in models.approveDetailLists
                                 where (m.statusId != "")
                                 select m).ToList();
                    string strMailTo = "", strMailCc = "";
                    foreach (ApproveModel.approveDetailModel m in lists)
                    {
                        strMailCc += (strMailCc == "") ? m.empEmail : "," + m.empEmail; // get list email
                    }
                    List<ApproveModel.approveDetailModel> createUsers = BudgetApproveController.getUserCreateBudgetForm(actFormId);
                    foreach (ApproveModel.approveDetailModel m in createUsers)
                    {
                        strMailTo += (strMailTo == "") ? m.empEmail : "," + m.empEmail; // get list email
                    }
                    #endregion

                    var empUser = models.approveDetailLists.Where(r => r.empId == UtilsAppCode.Session.User.empId).ToList(); // get current user
                    string strLink = string.Format(ConfigurationManager.AppSettings["urlDocument_Budget_Form"], actFormId);
                    string strBody = string.Format(ConfigurationManager.AppSettings["emailRejectBodyBudget"]
                        , models.approveModel.actNo
                        , empUser.FirstOrDefault().empPrefix + " " + empUser.FirstOrDefault().empName
                        , empUser.FirstOrDefault().remark
                        , strLink
                        );


                    sendEmailBudgetForm(actFormId
                        , strMailTo
                        , strMailCc
                        , ConfigurationManager.AppSettings["emailRejectSubjectBudget"]
                        , strBody
                        , emailType);
                }


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sendRejectBudget >>" + ex.Message + " " + actFormId);
            }
        }

        public static void sendApproveBudget(string actFormId, AppCode.ApproveType emailType, bool isResend)
        {
            try
            {
                string strBody = "", strSubject = "";

                List<ApproveModel.approveEmailDetailModel> lists = getEmailApproveNextLevelBudget(actFormId);
                if (lists.Count > 0)
                {
                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                    {
                        strBody = getEmailBodyBudget(item, emailType, actFormId);
                        strSubject = ConfigurationManager.AppSettings["emailApprovedSubjectBudget"];
                        strSubject = isResend ? "RE: " + strSubject : strSubject;
                        sendEmailBudgetForm(actFormId
                            , item.empEmail
                            , ""
                            , strSubject
                            , strBody
                            , emailType);
                    }
                }
                else
                {
                    // case all updated


                    DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountStatusApproveDetail"
                        , new SqlParameter[] {new SqlParameter("@actFormId",actFormId)
                        ,new SqlParameter("@statusId",(int)AppCode.ApproveStatus.อนุมัติ)});

                    if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (dr["countAll"].ToString() == dr["countStatusApproved"].ToString())
                        {

                            Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
                            budget_activity_model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivity(null, null, null, actFormId, null, null, null, null).FirstOrDefault();

                            string var_link = "";
                            var_link = "activityProduct?activityId=" + budget_activity_model.Budget_Activity.act_form_id + "&activityNo=" + budget_activity_model.Budget_Activity.act_activityNo + "&companyEN=" + budget_activity_model.Budget_Activity.act_companyEN;

                            //all approved then send the email notification to user create
                            List<ApproveModel.approveDetailModel> createUsers = BudgetApproveController.getUserCreateBudgetForm(actFormId);

                            strBody = string.Format(ConfigurationManager.AppSettings["emailAllApproveBodyBudget"]
                                    , createUsers.FirstOrDefault().empName
                                    , createUsers.FirstOrDefault().activityNo
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Budget_Form"], var_link))
                                    ;

                            sendEmailBudgetForm(actFormId
                            , createUsers.FirstOrDefault().empEmail
                            , ""
                            , ConfigurationManager.AppSettings["emailApprovedSubjectBudget"]
                            , strBody
                            , emailType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Email sendApproveBudgetForm >> " + ex.Message + actFormId);
                throw new Exception("Email sendApproveBudgetForm" + ex.Message);
            }
        }


        private static List<ApproveModel.approveEmailDetailModel> getEmailApproveNextLevelBudget(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetApproveNextLevel"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                var models = (from DataRow dr in ds.Tables[0].Rows
                              select new ApproveModel.approveEmailDetailModel()
                              {
                                  empEmail = dr["empEmail"].ToString(),
                                  empPrefix = dr["empPrefix"].ToString(),
                                  empName = dr["empName"].ToString(),
                                  activityName = dr["activityName"].ToString(),
                                  activitySales = dr["activitySales"].ToString(),
                                  activityNo = dr["activityNo"].ToString(),
                                  sumTotal = dr["sumTotal"] is DBNull ? 0 : (decimal)dr["sumTotal"],
                                  createBy = dr["createBy"].ToString(),
                                  createdByUserId = dr["createdByUserId"].ToString(),
                              }).ToList();
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getEmailNextLevel >> " + ex.Message);
            }
        }


        private static void sendEmailBudgetForm(string actFormId, string mailTo, string mailCC, string strSubject, string strBody, AppCode.ApproveType emailType)
        {

            try
            {

                List<Attachment> files = new List<Attachment>();
                string[] pathFile = new string[10];
                string[] pathFileAtt = new string[10];

                mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"] : mailTo;
                mailCC = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailApproveCC"] : mailCC;

                pathFile[0] = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], actFormId)); ;

                foreach (var item in pathFile)
                {
                    if (System.IO.File.Exists(item))
                    {
                        files.Add(new Attachment(item, new ContentType("application/pdf")));
                    }
                }

                TB_Bud_Image_Model getBudgetImageModel = new TB_Bud_Image_Model();
                getBudgetImageModel.BudImageList = ImageAppCodeBudget.getImageBudgetByApproveId(actFormId);
                if (getBudgetImageModel.BudImageList.Any())
                {
                    int i = 1;
                    foreach (var item in getBudgetImageModel.BudImageList)
                    {
                        pathFileAtt[i] = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName));
                        i++;
                    }
                }

                foreach (var item in pathFileAtt)
                {
                    if (System.IO.File.Exists(item))
                    {
                        files.Add(new Attachment(item, new ContentType("application/pdf")));
                    }
                }

                sendEmail(mailTo
                        , mailCC == "" ? ConfigurationManager.AppSettings["emailBudgetApproveCC"] : mailCC
                        , strSubject
                        , strBody
                        , files);

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sendEmailBudgetForm >> " + ex.Message + " : mailto >> " + mailTo);
                throw new Exception("sendEmailBudgetForm >> " + ex.Message);
            }
        }

        private static string getEmailBodyBudget(ApproveModel.approveEmailDetailModel item, AppCode.ApproveType emailType, string actId)
        {
            try
            {

                string strBody =
                            string.Format(ConfigurationManager.AppSettings["emailApproveBodyBudget"]
                            , item.empPrefix + " " + item.empName //เรียน
                            , AppCode.ApproveStatus.รออนุมัติ.ToString()
                            , emailType.ToString().Replace("_", " ")
                            , item.activityName
                            , item.activitySales
                            , item.activityNo
                            , String.Format("{0:0,0.00}", item.sumTotal)
                            , item.createBy
                            , string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actId))
                            ;

                return strBody;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmailBodyBudget >> " + ex.Message);
                throw new Exception(ex.Message);
            }
        }


    }
}