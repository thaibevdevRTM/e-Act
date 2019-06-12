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
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getUserAdmin");
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
                    string strMailTo = "";
                    foreach (ApproveModel.approveDetailModel m in lists)
                    {
                        strMailTo = (strMailTo == "") ? m.empEmail : "," + m.empEmail; // get list email
                    }
                    List<ApproveModel.approveDetailModel> createUsers = ActFormAppCode.getUserCreateActForm(actFormId);
                    foreach (ApproveModel.approveDetailModel m in createUsers)
                    {
                        strMailTo = (strMailTo == "") ? m.empEmail : "," + m.empEmail; // get list email
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
                        , ConfigurationManager.AppSettings["emailRejectSubject"]
                        , strBody
                        , emailType);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sendRejectActForm >>" + ex.Message);
            }
        }
        public static void sendApprove(string actFormId, AppCode.ApproveType emailType, bool isResend)
        {
            try
            {
                List<ApproveModel.approveEmailDetailModel> lists = (emailType == AppCode.ApproveType.Activity_Form) ? getEmailApproveNextLevel(actFormId)
                    : getEmailApproveRepDetailNextLevel(actFormId);
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

        private static void sendEmailActForm(string actFormId, string mailTo, string strSubject, string strBody, AppCode.ApproveType emailType)
        {
            List<Attachment> files = new List<Attachment>();
            mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"] : mailTo;
            string pathFile = emailType == AppCode.ApproveType.Activity_Form ?
                HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actFormId))
                : HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actFormId));

            if (System.IO.File.Exists(pathFile))
            {
                files.Add(new Attachment(pathFile, new ContentType("application/pdf")));
            }

            sendEmail(mailTo
                    , ConfigurationManager.AppSettings["emailApproveCC"]
                    , strSubject
                    , strBody
                    , files);
        }

        private static string getEmailBody(ApproveModel.approveEmailDetailModel item, AppCode.ApproveType emailType, string actId)
        {
            try
            {

                string strBody = (emailType == AppCode.ApproveType.Activity_Form) ?
                            string.Format(ConfigurationManager.AppSettings["emailApproveBody"]
                            , item.empPrefix + " " + item.empName //เรียน
                            , AppCode.ApproveStatus.รออนุมัติ.ToString()
                            , emailType.ToString().Replace("_", " ")
                            , item.activityName
                            , item.activitySales
                            , item.activityNo
                            , String.Format("{0:0,0.00}", item.sumTotal)
                            , item.createBy
                            , string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actId)
                            ) :
                            string.Format(ConfigurationManager.AppSettings["emailApproveRepDetailBody"]
                            , item.empPrefix + " " + item.empName //เรียน
                            , AppCode.ApproveStatus.รออนุมัติ.ToString()
                            , emailType.ToString().Replace("_", " ")
                            , item.customerName
                            , item.productTypeName
                            , item.createBy
                            , string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actId)
                            );

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
                    string strMailTo = "";
                    foreach (ApproveModel.approveDetailModel m in lists)
                    {
                        strMailTo = (strMailTo == "") ? m.empEmail : "," + m.empEmail; // get list email
                    }
                    List<ApproveModel.approveDetailModel> createUsers = BudgetApproveController.getUserCreateBudgetForm(actFormId);
                    foreach (ApproveModel.approveDetailModel m in createUsers)
                    {
                        strMailTo = (strMailTo == "") ? m.empEmail : "," + m.empEmail; // get list email
                    }
                    #endregion

                    var empUser = models.approveDetailLists.Where(r => r.empId == UtilsAppCode.Session.User.empId).ToList(); // get current user
                    string strBody = string.Format(ConfigurationManager.AppSettings["emailRejectBodyBudget"]
                        , models.approveModel.actNo
                        , empUser.FirstOrDefault().empPrefix + " " + empUser.FirstOrDefault().empName
                        , empUser.FirstOrDefault().remark
                        );

                    sendEmailBudgetForm(actFormId
                        , strMailTo
                        , ConfigurationManager.AppSettings["emailRejectSubjectBudget"]
                        , strBody
                        , emailType);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sendRejectActForm >>" + ex.Message);
            }
        }

        public static void sendApproveBudget(string actFormId, AppCode.ApproveType emailType)
        {
            try
            {
                List<ApproveModel.approveEmailDetailModel> lists = (emailType == AppCode.ApproveType.Budget_form) ? getEmailApproveNextLevelBudget(actFormId)
                    : getEmailApproveRepDetailNextLevel(actFormId);
                string strBody = "";
                if (lists.Count > 0)
                {
                    foreach (ApproveModel.approveEmailDetailModel item in lists)
                    {
                        strBody = getEmailBodyBudget(item, emailType, actFormId);
                        sendEmailBudgetForm(actFormId
                            , item.empEmail
                            , ConfigurationManager.AppSettings["emailApprovedSubjectBudget"]
                            , strBody
                            , emailType);
                    }
                }
                else
                {
                    //// case all updated
                    //DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCountStatusApproveDetail"
                    //	, new SqlParameter[] {new SqlParameter("@actFormId",actFormId)
                    //	,new SqlParameter("@statusId",(int)AppCode.ApproveStatus.อนุมัติ)});

                    //if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    //{
                    //	DataRow dr = ds.Tables[0].Rows[0];
                    //	if (dr["countAll"].ToString() == dr["countStatusApproved"].ToString())
                    //	{
                    //		// all approved then send the email notification to user create
                    //		List<ApproveModel.approveDetailModel> createUsers = ActFormAppCode.getUserCreateActForm(actFormId);
                    //		strBody = string.Format(ConfigurationManager.AppSettings["emailAllApproveBody"]
                    //			, createUsers.FirstOrDefault().empName
                    //			, createUsers.FirstOrDefault().activityNo
                    //			, string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actFormId));

                    //		sendEmailBudgetForm(actFormId
                    //		, createUsers.FirstOrDefault().empEmail
                    //		, ConfigurationManager.AppSettings["emailAllApprovedSubject"]
                    //		, strBody
                    //		, emailType);
                    //	}
                    //}
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Email sendApproveBudgetForm >> " + ex.Message);
                throw new Exception("sendEmailApprove" + ex.Message);
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
                              }).ToList();
                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getEmailNextLevel >> " + ex.Message);
            }
        }


        private static void sendEmailBudgetForm(string actFormId, string mailTo, string strSubject, string strBody, AppCode.ApproveType emailType)
        {
            List<Attachment> files = new List<Attachment>();
            mailTo = (bool.Parse(ConfigurationManager.AppSettings["isDevelop"])) ? ConfigurationManager.AppSettings["emailForDevelopSite"] : mailTo;
            string pathFile = emailType == AppCode.ApproveType.Budget_form ?
                HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], actFormId))
                : HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], actFormId));
            files.Add(new Attachment(pathFile, new ContentType("application/pdf")));

            sendEmail(mailTo
                    , ConfigurationManager.AppSettings["emailApproveCC"]
                    , strSubject
                    , strBody
                    , files);
        }

        private static string getEmailBodyBudget(ApproveModel.approveEmailDetailModel item, AppCode.ApproveType emailType, string actId)
        {
            try
            {

                string strBody = (emailType == AppCode.ApproveType.Budget_form) ?
                            string.Format(ConfigurationManager.AppSettings["emailApproveBodyBudget"]
                            , item.empPrefix + " " + item.empName //เรียน
                            , AppCode.ApproveStatus.รออนุมัติ.ToString()
                            , emailType.ToString().Replace("_", " ")
                            , item.activityName
                            , item.activitySales
                            , item.activityNo
                            , String.Format("{0:0,0.00}", item.sumTotal)
                            , item.createBy
                            , string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actId)
                            ) :
                            string.Format(ConfigurationManager.AppSettings["emailApproveRepDetailBody"]
                            , item.empPrefix + " " + item.empName //เรียน
                            , AppCode.ApproveStatus.รออนุมัติ.ToString()
                            , emailType.ToString().Replace("_", " ")
                            , item.customerName
                            , item.productTypeName
                            , item.createBy
                            , string.Format(ConfigurationManager.AppSettings["urlApprove_" + emailType.ToString()], actId)
                            );

                return strBody;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}