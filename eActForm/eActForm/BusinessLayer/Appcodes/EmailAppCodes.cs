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

namespace eActForm.BusinessLayer
{
    public class EmailAppCodes
    {
        public static void sendRequestCancelToAdmin(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserAdminByCompanyId"
                    , new SqlParameter[] { new SqlParameter("@companyId", UtilsAppCode.Session.User.empCompanyId) });
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

        public static void sendReject(string actFormId, AppCode.ApproveType emailType)
        {
            try
            {
                ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actFormId);
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

                    var empUser = models.approveDetailLists.Where(r => r.empId == UtilsAppCode.Session.User.empId).ToList(); // get current user
                    string strLink = string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId);
                    string strBody = string.Format(ConfigurationManager.AppSettings["emailRejectBody"]
                        , models.approveModel.actNo
                        , empUser.FirstOrDefault().empPrefix + " " + empUser.FirstOrDefault().empName
                        , empUser.FirstOrDefault().remark
                        , strLink
                        );

                    sendEmailActForm(actFormId
                        , strMailTo
                        , strMailCc
                        , ConfigurationManager.AppSettings["emailRejectSubject"]
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


                string strBody = "", strSubject = "";
                if (lists.Count > 0)
                {
                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                    {
                        strBody = getEmailBody(item, emailType, actFormId);
                        strSubject = ConfigurationManager.AppSettings["emailApproveSubject"];
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

                            strBody = (emailType == AppCode.ApproveType.Activity_Form)
                                ? string.Format(ConfigurationManager.AppSettings["emailAllApproveBody"]
                                    , createUsers.FirstOrDefault().empName
                                    , createUsers.FirstOrDefault().activityNo
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId))
                                : string.Format(ConfigurationManager.AppSettings["emailAllApproveRepDetailBody"]
                                    , createUsers.FirstOrDefault().empName
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId));

                            sendEmailActForm(actFormId
                            , createUsers.FirstOrDefault().empEmail
                            , ""
                            , ConfigurationManager.AppSettings["emailAllApprovedSubject"]
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
                throw new Exception("sendEmailApprove" + ex.Message);
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


                string strBody = "", strSubject = "";
                if (lists.Count > 0)
                {
                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                    {
                        strBody = getEmailBody(item, emailType, actFormId);
                        strSubject = ConfigurationManager.AppSettings["emailApproveSubject"];
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

                            strBody = (emailType == AppCode.ApproveType.Activity_Form)
                                ? string.Format(ConfigurationManager.AppSettings["emailAllApproveBody"]
                                    , createUsers.FirstOrDefault().empName
                                    , createUsers.FirstOrDefault().activityNo
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId))
                                : string.Format(ConfigurationManager.AppSettings["emailAllApproveRepDetailBody"]
                                    , createUsers.FirstOrDefault().empName
                                    , string.Format(ConfigurationManager.AppSettings["urlDocument_Activity_Form"], actFormId));

                            sendEmailActForm(actFormId
                            , createUsers.FirstOrDefault().empEmail
                            , ApproveAppCode.getEmailCCByActId(actFormId)
                            , ConfigurationManager.AppSettings["emailAllApprovedSubject"]
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
            List<Attachment> files = new List<Attachment>();
            string[] pathFile = new string[10];
            mailCC = mailCC != "" ? "," + mailCC : "";
            mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"] : mailTo;
            mailCC = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"] : mailCC;
            //pathFile[0] = emailType == AppCode.ApproveType.Activity_Form ?

            //    HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actFormId))
            //    : HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actFormId));


            switch (emailType)
            {
                case AppCode.ApproveType.Activity_Form:
                    pathFile[0] = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actFormId));
                    break;
                case AppCode.ApproveType.Report_Detail:
                    pathFile[0] = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actFormId));
                    break;
                case AppCode.ApproveType.Report_Summary:
                    pathFile[0] = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootSummaryDetailPdftURL"], actFormId));
                    break;
            }

            TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
            getImageModel.tbActImageList = ImageAppCode.GetImage(actFormId);
            if (getImageModel.tbActImageList.Any())
            {
                int i = 1;
                foreach (var item in getImageModel.tbActImageList)
                {
                    if(UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_TBM"])
                    {
                        pathFile[i] = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                    }
                    else
                    {
                        if (item.imageType == AppCode.ApproveType.Report_Detail.ToString())
                        {
                            pathFile[i] = HttpContext.Current.Server.MapPath(item._fileName);
                        }
                    }


                    //else if (item.extension == ".pdf")
                    //{
                    //    pathFile[i] = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                    //}
                    i++;
                }
            }

            foreach (var item in pathFile)
            {
                if (System.IO.File.Exists(item))
                {
                    files.Add(new Attachment(item));
                }
            }


            sendEmail(mailTo
                    , mailCC == "" ? ConfigurationManager.AppSettings["emailApproveCC"] : mailCC
                    , strSubject
                    , strBody
                    , files);
        }





        private static string getEmailBody(ApproveModel.approveEmailDetailModel item, AppCode.ApproveType emailType, string actId)
        {
            try
            {
                string strBody = "";
                switch (emailType)
                {
                    case AppCode.ApproveType.Activity_Form:
                        var models = ActFormAppCode.getUserCreateActForm(actId);
                        strBody = string.Format(ConfigurationManager.AppSettings["emailApproveBody"]
                            , item.empPrefix + " " + item.empName //เรียน
                            , AppCode.ApproveStatus.รออนุมัติ.ToString()
                            , emailType.ToString().Replace("_", " ")
                            , item.activityName
                            , item.activitySales
                            , item.activityNo
                            , String.Format("{0:0,0.00}", item.sumTotal)
                            , (models != null && models.Count > 0) ? models[0].companyName : ""
                            , item.createBy
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
                                  activityName = dr["activityName"].ToString(),
                                  activitySales = dr["activitySales"].ToString(),
                                  activityNo = dr["activityNo"].ToString(),
                                  sumTotal = dr["sumTotal"] is DBNull ? 0 : (decimal)dr["sumTotal"],
                                  createBy = dr["createBy"].ToString(),
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
            mailer.CC = cc;
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
                    string strLink = string.Format(ConfigurationManager.AppSettings["urlDocument_Budget_Form"]);
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
                mailCC = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"] : mailCC;

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