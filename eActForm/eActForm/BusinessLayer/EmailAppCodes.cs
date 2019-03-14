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
using System.Net.Mail;
using System.Net.Mime;

namespace eActForm.BusinessLayer
{
    public class EmailAppCodes
    {
        public static string sendRejectActForm(string actFormId)
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("sendRejectActForm >> " + ex.Message);
            }
        }
        public static string sendApproveActForm(string actFormId)
        {
            try
            {
                List<ApproveModel.approveEmailDetailModel> lists = getEmailNextLevel(actFormId);
                foreach (ApproveModel.approveEmailDetailModel item in lists)
                {
                    string strBody = string.Format(ConfigurationManager.AppSettings["emailApproveBody"]
                        , item.empPrefix + " " + item.empName //เรียน
                        , "รออนุมัติ"
                        , "Activity Form"
                        , item.activityName
                        , item.activitySales
                        , item.activityNo
                        , item.sumTotal
                        , item.createBy
                        , string.Format(ConfigurationManager.AppSettings["urlApprove"], actFormId)
                        );

                    List<Attachment> files = new List<Attachment>();
                    string pathFile = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"],actFormId));                    
                    files.Add(new Attachment(pathFile, new ContentType("application/pdf")));

                    sendEmail("parnupong.k@thaibev.com"//item.empEmail
                        , ConfigurationManager.AppSettings["emailApproveCC"]
                        , ConfigurationManager.AppSettings["emailApproveSubject"]
                        , strBody
                        , files);
                }

                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("sendApproveActForm >> " + ex.Message);
            }
        }

        private static List<ApproveModel.approveEmailDetailModel> getEmailNextLevel(string actFormId)
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
                                  sumTotal = dr["sumTotal"].ToString(),
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
    }
}