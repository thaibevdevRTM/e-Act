using ClosedXML.Excel;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using WebLibrary;

namespace eActConsoleServiceWeekly
{
    public class appCodeWeeklySummaryOrder
    {
        static void Main(string[] args)
        {
            StreamWriter strLogs = File.AppendText(new appCodeWeeklySummaryOrder().LogsFileName);

            try
            {

                if (args[0] == "bySaleTeam")
                {
                    sentReportBySaleTeam(strLogs);
                }
                else if (args[0] == "byRegion")
                {

                }
            }
            catch (Exception ex)
            {
                strLogs.WriteLine(" Error Main : " + ex.Message);
                strLogs.WriteLine("== END " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss ") + "==");
            }
            finally
            {
                strLogs.Flush();
                strLogs.Close();
            }
        }

        public static void sentReportBySaleTeam(StreamWriter strLogs)
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(Properties.Settings.Default.strConn, CommandType.StoredProcedure, "usp_getSaleTeamAll");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataSet dsOrder = SqlHelper.ExecuteDataset(Properties.Settings.Default.strConn, CommandType.StoredProcedure, "usp_getOrderBySaleTeamId"
                    , new SqlParameter[] { new SqlParameter("@saleTeamId", dr["saleTeamId"].ToString())
                        , new SqlParameter("@startDate",DateTime.Now.AddDays(-7))});

                    if (dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = setRecivedDate(dsOrder.Tables[0]);
                        string filePath = ExportDataSetToExcel(dt, "orderProductBySaleTeam" + dr["saleTeamId"].ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                        List<Attachment> fileLists = new List<Attachment>
                    {
                        new Attachment(filePath)
                    };

                        sendEmail("parnupong.k@thaibev.com" // dr["email"].ToString()
                            , ""
                            , "รายงานการสั่งซื้อสินค้า crystal วันที่ " + DateTime.Now.ToString("dd/MM/yyyy")
                            , Properties.Settings.Default.strBody
                            , fileLists);

                        strLogs.WriteLine("== Email to " + dr["email"].ToString() + " is success. ==");
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception("sentReportBySaleTeam >> " + ex.Message);
            }
        }

        private static string ExportDataSetToExcel(DataTable dt, string fileName)
        {
            string AppLocation = "";
            AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            AppLocation = AppLocation.Replace("file:\\", "");
            string file = AppLocation + "\\excelFiles\\" + fileName;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                wb.SaveAs(file);
            }

            return file;
        }

        private static void sendEmail(string mailTo, string cc, string subject, string body, List<Attachment> file)
        {
            GMailer.Mail_From = Properties.Settings.Default.strMailUser;
            GMailer.GmailPassword = Properties.Settings.Default.strMailPassword;
            GMailer mailer = new GMailer();
            mailer.ToEmail = mailTo;
            mailer.Subject = subject;
            mailer.Body = body;
            if (!string.IsNullOrEmpty(cc))
            {
                mailer.CC = cc;
            }
            mailer.IsHtml = true;
            mailer.p_Attachment = file;
            mailer.Send();
        }

        private string LogsFileName
        {
            get
            {
                var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                string str = string.Format(Properties.Settings.Default.logsFileName, new string[] { Path.GetDirectoryName(location), DateTime.Now.ToString("ddMMyyyy") });
                return str;
            }
        }

        private static DataTable setRecivedDate(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                DateTime baseDate = (DateTime)dr["orderDate"];
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday: baseDate = baseDate.AddDays(-2); break;
                    case DayOfWeek.Tuesday: baseDate = baseDate.AddDays(-3); break;
                }
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek).AddDays(+3);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                //dr["orderDate"] = baseDate.ToString("dd/MM/yyyy HH:mm");
                dr["recivedDate"] = thisWeekEnd.AddDays(+4).ToString("dd/MM/yyyy HH:mm");
            }

            return dt;
        }

    }
}
