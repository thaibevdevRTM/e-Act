using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Controllers;
using eActForm.Models;
using eForms.Models.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class EmailAppCodes
    {
        //public static string[] departmentUseEN = { "941F6AC9-A36E-48A4-8769-1A267EB8BC3A", "8C0DD2A1-9110-4EEC-8BE3-578E4FA59520" };

        //public static string departmentInterSale = "941F6AC9-A36E-48A4-8769-1A267EB8BC3A";

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

                string getSubject = UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isAdminOMT ? ConfigurationManager.AppSettings["emailRequestCancelByAdmin"] : ConfigurationManager.AppSettings["emailRequestCancelSubject"];
                    
                sendEmail(mailTo
                    , ConfigurationManager.AppSettings["emailApproveCC"]
                    , getSubject
                    , strBody
                    , actFormId
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
                    ,""
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
                    ,""
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

                    string emailRejectBody = "";
                    string emailRejectSubject = "";
                    string txtEmpUser = "";
                    Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actFormId);

                    string strLink = string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId);

                    emailRejectBody = ConfigurationManager.AppSettings["emailRejectBody"];
                    emailRejectSubject = ConfigurationManager.AppSettings["emailRejectSubject"];
                    txtEmpUser = empUser.FirstOrDefault().empName;
                    if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                    {
                        if (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"])// Inter sale
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
                EmailAppCodes.sendEmail(
                   EmailAppCodes.GetDataEmailIsDev(actFormId).FirstOrDefault().e_to
                  , ""
                  , "eAct sendApprove non backgroup Error"
                  , actFormId + " " + ex.Message
                  , actFormId
                  , null);
                //ExceptionManager.WriteError("sendRejectActForm >>" + ex.Message + " " + actFormId);
                throw new Exception("sendRejectActForm >>" + ex.Message + " " + actFormId);
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
                        if (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"])// Inter sale
                        {
                            emailAllApprovedSubject = ConfigurationManager.AppSettings["emailApproveSubject_EN"];
                        }
                    }

                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                    {
                        strBody = getEmailBody(item, emailType, actFormId, false);
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
                                if (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"])// Inter sale
                                {
                                    createUsersName = createUsers.FirstOrDefault().empName_EN;
                                    emailAllApprovedSubject = ConfigurationManager.AppSettings["emailAllApprovedSubject_EN"];
                                    txtemailAllApproveBody = ConfigurationManager.AppSettings["emailAllApproveBody_EN"];
                                }
                            }


                            string pathFile = "";
                            var tbActImageList = ImageAppCode.GetImage(actFormId);
                            if (tbActImageList.Any())
                            {
                                int i = 1;
                                foreach (var loop in tbActImageList)
                                {
                                    if (loop.imageType == AppCode.ApproveType.Report_Detail.ToString())
                                    {
                                        pathFile += string.Format(ConfigurationManager.AppSettings["formatLinkfiles"], HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], loop._fileName)), loop._fileName) + "<br/>";
                                    }
                                    else
                                    {
                                        pathFile += string.Format(ConfigurationManager.AppSettings["formatLinkfiles"], string.Format(ConfigurationManager.AppSettings["controllerGetFile"], loop._fileName), loop._fileName) + "<br/>";
                                    }
                                    i++;
                                }
                            }


                            strBody = (emailType == AppCode.ApproveType.Activity_Form)
                                ? string.Format(txtemailAllApproveBody
                                    , createUsersName
                                    , createUsers.FirstOrDefault().activityNo
                                    , pathFile
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId))
                                : string.Format(ConfigurationManager.AppSettings["emailAllApproveRepDetailBody"]
                                    , createUsersName
                                    , pathFile
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

        public static void sendApprove(string actFormId, AppCode.ApproveType emailType, bool isResend, bool callKafka)
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
                if (activity_TBMMKT_Model.activityFormTBMMKT != null)
                {

                    if (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"])//Inter sale
                    {
                        strSubject = ConfigurationManager.AppSettings["emailApproveSubject_EN"];
                    }

                }



                if (lists.Count > 0)
                {
                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                    {
                        try
                        {
                            SentKafkaLogModel kafka = new SentKafkaLogModel(item.empId, actFormId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == item.statusId).FirstOrDefault().displayVal, "producer", DateTime.Now, "","", "Log Before Call Kafka");
                            var resultLog = ApproveAppCode.insertLog_Kafka(kafka);

                            if (callKafka && !string.IsNullOrEmpty(item.statusId))
                            {
                                ApproveAppCode.apiProducerApproveAsync(item.empId, actFormId, QueryOtherMaster.getOhterMaster("statusAPI", "").Where(x => x.val1 == item.statusId).FirstOrDefault().displayVal);
                            }
                        }
                        catch(Exception ex)
                        {
                            throw new Exception("Call Kafka Producer empId :"+ item.empId + " activityId : "+ actFormId +" >>>>" + ex.Message);
                        }



                        try
                        {
                            strBody = getEmailBody(item, emailType, actFormId, false);
                            strSubject = isResend ? "RE: " + strSubject : strSubject;
                            sendEmailActForm(actFormId
                                , item.empEmail
                                , ""
                                , strSubject
                                , strBody
                                , emailType);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Call Email " + ex.Message);
                        }

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
                                if (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"])// Inter sale
                                {
                                    createUsersName = createUsers.FirstOrDefault().empName_EN;
                                    emailAllApprovedSubject = ConfigurationManager.AppSettings["emailAllApprovedSubject_EN"];
                                    txtemailAllApproveBody = ConfigurationManager.AppSettings["emailAllApproveBody_EN"];
                                }
                            }

                            string pathFile = "";
                            var tbActImageList = ImageAppCode.GetImage(actFormId);
                            if (tbActImageList.Any())
                            {
                                int i = 1;
                                foreach (var loop in tbActImageList)
                                {
                                    if (loop.imageType == AppCode.ApproveType.Report_Detail.ToString())
                                    {
                                        pathFile += string.Format(ConfigurationManager.AppSettings["formatLinkfiles"], HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], loop._fileName)), loop._fileName) + "<br/>";
                                    }
                                    else
                                    {
                                        pathFile += string.Format(ConfigurationManager.AppSettings["formatLinkfiles"], string.Format(ConfigurationManager.AppSettings["controllerGetFile"], loop._fileName), loop._fileName) + "<br/>";
                                    }
                                    i++;
                                }
                            }

                            strBody = (emailType == AppCode.ApproveType.Activity_Form)
                                ? string.Format(txtemailAllApproveBody
                                    , createUsersName
                                    , createUsers.FirstOrDefault().activityNo
                                    , pathFile
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId))
                                : string.Format(ConfigurationManager.AppSettings["emailAllApproveRepDetailBody"]
                                    , createUsersName
                                    , pathFile
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId));

                            //=============New Process Peerapop ส่งเมลล์ CC===============peerapop.i dev date 20200525======
                            string[] formNeedStyleEdocAfterApproved = { ConfigurationManager.AppSettings["formCR_IT_FRM_314"] };
                            string ccEmailNormalProcess = ApproveAppCode.getEmailCCByActId(actFormId);
                            if (activity_TBMMKT_Model.activityFormTBMMKT != null)
                            {
                                if (formNeedStyleEdocAfterApproved.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                                {
                                    ccEmailNormalProcess = "";
                                }
                            }
                            //=====END========New Process Peerapop ส่งเมลล์ CC===============peerapop.i dev date 20200525======

                            sendEmailActForm(actFormId
                            , createUsers.FirstOrDefault().empEmail
                            , ccEmailNormalProcess
                            , emailAllApprovedSubject
                            , strBody
                            , emailType);

                            //=============New Process Peerapop ส่งเมลล์ ในรูปแบบเหมือนส่งอนุมัติปกติ แต่ส่งหลังApproveครบ========peerapop.i dev date 20200525======
                            if (activity_TBMMKT_Model.activityFormTBMMKT != null)
                            {
                                if (formNeedStyleEdocAfterApproved.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                                {
                                    string[] groupApproveForCC_CreatedBy = { ConfigurationManager.AppSettings["approveGroupForProcess"] };

                                    lists = getDataEmpGroupFinishApprovedFormatLikeEdoc(actFormId);
                                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                                    {
                                        string ccEmailgetEmailGroupContinueProcess = "";
                                        if (groupApproveForCC_CreatedBy.Contains(item.approveGroupId))
                                        {
                                            ccEmailgetEmailGroupContinueProcess = createUsers.FirstOrDefault().empEmail;
                                        }
                                        strBody = getEmailBody(item, emailType, actFormId, true);
                                        strSubject = isResend ? "RE: " + strSubject : strSubject;
                                        sendEmailActForm(actFormId
                                            , item.empEmail
                                            , ccEmailgetEmailGroupContinueProcess
                                            , emailAllApprovedSubject
                                            , strBody
                                            , emailType);
                                    }
                                }
                            }
                            //=====END========New Process Peerapop ส่งเมลล์ ในรูปแบบเหมือนส่งอนุมัติปกติ แต่ส่งหลังApproveครบ========peerapop.i dev date 20200525====



                            if (ConfigurationManager.AppSettings["formTransferbudget"].Equals(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                            {
                                //waiting update budgetControl
                                bool resultTransfer = TransferBudgetAppcode.transferBudgetAllApprove(actFormId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ExceptionManager.WriteError("Email sendApproveActForm >> " + ex.Message); backgroud can't write error
                throw new Exception("sendEmailApprove" + ex.Message);
            }
        }

        private static void sendEmailActForm(string actFormId, string mailTo, string mailCC, string strSubject, string strBody, AppCode.ApproveType emailType)
        {
            var checkMail = "";
            List<Attachment> files = new List<Attachment>();
            string[] pathFile = new string[1];
            try
            {
                //================fream devdate 20191213 ดึงค่าเพื่อเอาเลขที่เอกสารไปRenameชื่อไฟล์แนบ==================
                ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();
                //=====END===========fream devdate 20191213 ดึงค่าเพื่อเอาเลขที่เอกสารไปRenameชื่อไฟล์แนบ==================

                checkMail = "<br>mailTo : " + mailTo + "<br> mailCC : " + mailCC;
                //mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"].ToString() : mailTo;
                //mailCC = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailApproveCC"].ToString() : mailCC;//ถ้าจะเทส ดึงCC จากDevไปเปลี่ยนรหัสพนักงานเองเลยที่ตาราง TB_Reg_ApproveDetail            
                mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? GetDataEmailIsDev(actFormId).FirstOrDefault().e_to : mailTo;
                mailCC = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? GetDataEmailIsDev(actFormId).FirstOrDefault().e_cc : mailCC;

                if (bool.Parse(ConfigurationManager.AppSettings["isDevelop"]))
                {
                    strBody += checkMail;
                }

                //Get Document
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

                var i_loop_change_name = 0;
                foreach (var item in pathFile)
                {
                    if (System.IO.File.Exists(item))
                    {
                        string extTypeFile = Path.GetExtension(item);
                        Attachment attachment;
                        attachment = new Attachment(item);
                        if (i_loop_change_name == 0 && emailType == AppCode.ApproveType.Activity_Form)
                        {
                            attachment.Name = replaceWordDangerForNameFile(activityFormTBMMKT.activityNo) + ".pdf";
                        }

                        var dontAttach = 0;
                        if (extTypeFile.ToLower() == ".pdf" && i_loop_change_name > 0)
                        {
                            dontAttach = 1;
                        }
                        if (dontAttach == 0)
                        {
                            files.Add(attachment);
                        }

                        i_loop_change_name++;
                    }
                }


                sendEmail(mailTo
                        , mailCC
                        , strSubject
                        , strBody
                        , actFormId
                        , files);

            }
            catch (Exception ex)
            {
                try
                {
                    sendEmail(mailTo
                        , mailCC
                        , strSubject
                        , strBody
                        , actFormId
                        , null);
                }
                catch (Exception exs)
                {
                    EmailAppCodes.sendEmail(
                    EmailAppCodes.GetDataEmailIsDev(actFormId).FirstOrDefault().e_to
                   , ""
                   , "eAct sendApprove Catch in Catch"
                   , actFormId + " " + exs.Message
                   , actFormId
                   , null);
                }

                throw new Exception("sendEmailActForm >> " + ex.Message + "_" + checkMail);

            }
        }

        public static string replaceWordDangerForNameFile(string txt)
        {
            return txt.Replace("/", "_").Replace(" ", "_").Replace(@"\", "_");
        }


        private static string getEmailBody(ApproveModel.approveEmailDetailModel item, AppCode.ApproveType emailType, string actId, bool statusApproveSuccess)
        {
            try
            {


                //=============dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========
                Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(actId);
                var emailTypeTxt = "";
                string[] arrayFormStyleV1 = { ConfigurationManager.AppSettings["formBgTbmId"], ConfigurationManager.AppSettings["formAdvTbmId"], ConfigurationManager.AppSettings["formAdvHcmId"], ConfigurationManager.AppSettings["masterEmpExpense"], ConfigurationManager.AppSettings["formPaymentVoucherTbmId"], ConfigurationManager.AppSettings["formReceptions"], ConfigurationManager.AppSettings["formPurchaseTbm"] };
                string[] arrayFormStyleV2 = { ConfigurationManager.AppSettings["formPosTbmId"], ConfigurationManager.AppSettings["formTrvTbmId"], ConfigurationManager.AppSettings["formTrvHcmId"], ConfigurationManager.AppSettings["formExpTrvNumId"], ConfigurationManager.AppSettings["formExpMedNumId"], ConfigurationManager.AppSettings["formCR_IT_FRM_314"] };
                string[] arrayFormStyleV3 = { ConfigurationManager.AppSettings["formPosTbmId"], ConfigurationManager.AppSettings["formCR_IT_FRM_314"] };
                string[] arrayFormStyleV4 = { ConfigurationManager.AppSettings["formCR_IT_FRM_314"] };

                if (activity_TBMMKT_Model.activityFormTBMMKT != null)
                {

                    if (arrayFormStyleV1.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id) || arrayFormStyleV2.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id) || arrayFormStyleV3.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                    {
                        emailTypeTxt = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm;
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                        {
                            if (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"])//Inter sale
                            {
                                emailTypeTxt = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id).FirstOrDefault().nameForm_EN;
                            }
                        }
                    }
                    else
                    {
                        emailTypeTxt = emailType.ToString().Replace("_", " ");
                    }
                }
                //=======END======dev date fream 20200115 เพิ่มดึงค่าว่าเป็นฟอร์มอะไร========

                string strBody = "";
                string empNameResult = "";
                string txtApprove = "";
                string txtcreateBy = "";
                string txtCompanyname = "";
                string strPiority = "";

                switch (emailType)
                {
                    case AppCode.ApproveType.Activity_Form:
                        var models = ActFormAppCode.getUserCreateActForm(actId);
                        strBody = ConfigurationManager.AppSettings["emailApproveBody"];
                        strPiority = ConfigurationManager.AppSettings["emailpiority"];
                        empNameResult = item.empName;
                        txtApprove = string.IsNullOrEmpty(item.statusId) ? AppCode.ApproveStatus.เรียนเพื่อทราบ.ToString() : AppCode.ApproveStatus.รออนุมัติ.ToString();
                        txtcreateBy = item.createBy;
                        txtCompanyname = models[0].companyName;

                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther != null)
                        {
                            if (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"])//Inter sale
                            {
                                strBody = ConfigurationManager.AppSettings["emailApproveBody_EN"];
                                strPiority = ConfigurationManager.AppSettings["emailpiority_EN"];
                                empNameResult = item.empName_EN;
                                txtApprove = "";
                                txtcreateBy = item.createBy_EN;
                                txtCompanyname = models[0].companyNameEN;
                            }
                        }

                        if (ActFormAppCode.isOtherCompanyMTOfDoc(activity_TBMMKT_Model.activityFormTBMMKT.companyId) && activity_TBMMKT_Model.activityFormTBMMKT.piorityDoc != "")
                        {
                            string strPiorityDoc = "";
                            strPiorityDoc = (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"]) ?
                                   QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice("master", "piorityDoc").Where(x => x.id == activity_TBMMKT_Model.activityFormTBMMKT.piorityDoc).AsEnumerable().FirstOrDefault().nameEN :
                                   QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice("master", "piorityDoc").Where(x => x.id == activity_TBMMKT_Model.activityFormTBMMKT.piorityDoc).AsEnumerable().FirstOrDefault().name;

                            strPiority = string.Format(strPiority, ActFormAppCode.getStatusNeedDocColor(activity_TBMMKT_Model.activityFormTBMMKT.piorityDoc), strPiorityDoc);
                            strBody = strBody.Replace("||piority||", strPiority);
                        }
                        else
                        {
                            strBody = strBody.Replace("||piority||", "");
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
                        if (arrayFormStyleV4.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id) && statusApproveSuccess == true)
                        {
                            strBody = strBody.Replace("คุณสามารถตรวจสอบรายละเอียดเพิ่มเติม และ Approve รายการได้ตามลิ้งค์นี้ :<a href=\"{9} \" > Click </a>", "");
                            strBody = strBody.Replace("To approve and review expenses details, please click here: :<a href=\"{9} \" > Click </a>", "");
                        }

                        //==============peerapop dev date 20200525=====formNeedStyleEdocAfterApproved=========
                        string[] formNeedStyleEdocAfterApproved = { ConfigurationManager.AppSettings["formCR_IT_FRM_314"] };
                        if (formNeedStyleEdocAfterApproved.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id))
                        {
                            if (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"])
                            {
                                strBody = strBody.Replace("You have list of pending approval requests <b>{1}</b> follow by below details", "You have list of pending <b>{1}</b> follow by below details");
                                txtApprove = item.approveGroupEN;
                            }
                            else
                            {
                                txtApprove = item.approveGroupTH;
                            }
                        }
                        //======END========peerapop dev date 20200525=====formNeedStyleEdocAfterApproved=========


                        string pathFile = "";
                        var tbActImageList = ImageAppCode.GetImage(actId);
                        if (tbActImageList.Any())
                        {
                            int i = 1;
                            foreach (var loop in tbActImageList)
                            {
                                if (loop.imageType == AppCode.ApproveType.Report_Detail.ToString())
                                {
                                    pathFile += string.Format(ConfigurationManager.AppSettings["formatLinkfiles"], HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], loop._fileName)), loop._fileName) + "<br/>";
                                }
                                else
                                {
                                    pathFile += string.Format(ConfigurationManager.AppSettings["formatLinkfiles"], string.Format(ConfigurationManager.AppSettings["controllerGetFile"], loop._fileName), loop._fileName) + "<br/>";
                                }
                                i++;
                            }
                        }

                        if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formEactBeer"])
                        {
                            emailType = AppCode.ApproveType.ActivityBeer;
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
                        , pathFile
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
                              select new ApproveModel.approveEmailDetailModel(dr["empId"].ToString(), dr["empEmail"].ToString())
                              {
                                  empPrefix = dr["empPrefix"].ToString(),
                                  activityName = dr["activityName"].ToString(),
                                  activitySales = dr["activitySales"].ToString(),
                                  activityNo = dr["activityNo"].ToString(),
                                  sumTotal = dr["sumTotal"] is DBNull ? 0 : (decimal)dr["sumTotal"],
                                  createBy = dr["createBy"].ToString(),
                                  createBy_EN = dr["createBy_EN"].ToString(),
                                  statusId = dr["statusId"].ToString(),
                                  empId = dr["empId"].ToString(),

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
                              select new ApproveModel.approveEmailDetailModel(dr["empId"].ToString(), dr["empEmail"].ToString())
                              {
                                  empPrefix = dr["empPrefix"].ToString(),
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
                              select new ApproveModel.approveEmailDetailModel(dr["empId"].ToString(), dr["empEmail"].ToString())
                              {
                                  empPrefix = dr["empPrefix"].ToString(),
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


        public static void sendEmail(string mailTo, string cc, string subject, string body,string actFormId, List<Attachment> files)
        {
            GMailer.Mail_From = ConfigurationManager.AppSettings["emailFrom"];
            GMailer.GmailPassword = ConfigurationManager.AppSettings["emailFromPass"];

            string checkMail = "<br>mailTo : " + mailTo + "<br> mailCC : " + cc;
            mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? GetDataEmailIsDev(actFormId).FirstOrDefault().e_to : mailTo;
            cc = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? GetDataEmailIsDev(actFormId).FirstOrDefault().e_cc : cc;

            if (bool.Parse(ConfigurationManager.AppSettings["isDevelop"]))
            {
                body += checkMail;
            }


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

                    strMailCc = string.Format(ConfigurationManager.AppSettings["emailBudgetRejectCC"]);

                    //foreach (ApproveModel.approveDetailModel m in lists)
                    //{
                    //    strMailCc += (strMailCc == "") ? m.empEmail : "," + m.empEmail; // get list email
                    //}
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

                    ApproveModel.approveEmailDetailModel item = lists[0];
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
                            budget_activity_model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityList(null, null, null, actFormId, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null, null).FirstOrDefault();

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
                              select new ApproveModel.approveEmailDetailModel(dr["empId"].ToString(), dr["empEmail"].ToString())
                              {
                                  empPrefix = dr["empPrefix"].ToString(),
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
                getBudgetImageModel.BudImageList = ImageAppCodeBudget.getBudgetInvoiceByApproveId(actFormId);
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
                        , actFormId
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

        private static List<ApproveModel.approveEmailDetailModel> getDataEmpGroupFinishApprovedFormatLikeEdoc(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDataEmpGroupFinishApprovedFormatLikeEdoc"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                var models = (from DataRow dr in ds.Tables[0].Rows
                              select new ApproveModel.approveEmailDetailModel(dr["empId"].ToString(), dr["empEmail"].ToString())
                              {
                                  empPrefix = dr["empPrefix"].ToString(),
                                  activityName = dr["activityName"].ToString(),
                                  activitySales = dr["activitySales"].ToString(),
                                  activityNo = dr["activityNo"].ToString(),
                                  sumTotal = dr["sumTotal"] is DBNull ? 0 : (decimal)dr["sumTotal"],
                                  createBy = dr["createBy"].ToString(),
                                  createBy_EN = dr["createBy_EN"].ToString(),
                                  approveGroupId = dr["approveGroupId"].ToString(),
                                  approveGroupTH = dr["approveGroupTH"].ToString(),
                                  approveGroupEN = dr["approveGroupEN"].ToString(),
                              }).ToList();
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getDataEmpGroupFinishApprovedFormatLikeEdoc >> " + ex.Message);
            }
        }


        public class getEmailisDevelop
        {
            public string e_to { get; set; }
            public string e_cc { get; set; }
            public string e_bcc { get; set; }
        }

        public static List<getEmailisDevelop> GetDataEmailIsDev(string activityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_GetDataEmailIsDev", new SqlParameter("@activityId", activityId));
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new getEmailisDevelop()
                             {
                                 e_to = d["e_to"].ToString(),
                                 e_cc = d["e_cc"].ToString(),
                                 e_bcc = d["e_bcc"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetDataEmailIsDev => " + ex.Message);
                return new List<getEmailisDevelop>();
            }
        }

    }
}