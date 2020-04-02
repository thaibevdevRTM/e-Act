using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using System.Net.Mail;
using ClosedXML.Excel;
using System.IO;

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
                        string filePath = ExportDataSetToExcel(dsOrder, "orderProductBySaleTeam" + dr["saleTeamId"].ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
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

        private static string ExportDataSetToExcel(DataSet ds, string fileName)
        {
            string AppLocation = "";
            AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            AppLocation = AppLocation.Replace("file:\\", "");
            string file = AppLocation + "\\excelFiles\\" + fileName;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds.Tables[0]);
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

    }
}
