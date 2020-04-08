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
                    sentReportByAllRegion(strLogs);
                }
                else if (args[0] == "byDaily")
                {
                    sentReportByDaily(strLogs);
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

        public static void sentReportByDaily(StreamWriter strLogs)
        {
            try
            {
                DataSet dsOrder = SqlHelper.ExecuteDataset(Properties.Settings.Default.strConn, CommandType.StoredProcedure, "usp_getOrderByDaily"
                , new SqlParameter[] { new SqlParameter("@date",DateTime.Now.ToString("yyyyMMdd"))});

                if (dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = setRecivedDate(dsOrder.Tables[0]);
                    string filePath = ExportDataSetToExcel(dt, "orderProductByDaily" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                    List<Attachment> fileLists = new List<Attachment>
                        {
                            new Attachment(filePath)
                        };

                    sendEmail("theeradech.r@thaibev.com,voranuch.t@thaibev.com"
                        , ""
                        , "รายงานการสั่งซื้อสินค้า crystal วันที่ " + DateTime.Now.ToString("dd/MM/yyyy")
                        , Properties.Settings.Default.strBody
                        , fileLists);

                    strLogs.WriteLine("== Email to theeradech.r@thaibev.com,voranuch.t@thaibev.com is success. ==");
                }
                else
                {
                    strLogs.WriteLine("== can't found order in to day ==");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("sentReportByDaily >> " + ex.Message);
            }
        }

        private static void sentReportByAllRegion(StreamWriter strLogs)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(Properties.Settings.Default.strConn, CommandType.StoredProcedure, "usp_getOrderByAllRegion"
                    , new SqlParameter[] {new SqlParameter("@startDate",DateTime.Now.AddDays(-7))
                    ,new SqlParameter("@endDate",DateTime.Now)

                    });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtTemp = ds.Tables[0].Clone();
                    var groupRegion = from b in ds.Tables[0].AsEnumerable()
                                      group b by b.Field<string>("emailContact") into g
                                      select new
                                      {
                                          emailContact = g.ToList().FirstOrDefault()["emailContact"].ToString()
                                      };

                    foreach (var s in groupRegion)
                    {
                        Console.WriteLine(s.emailContact);
                        var groupLists = from b in ds.Tables[0].AsEnumerable()
                                         where b.Field<string>("emailContact") == s.emailContact
                                         select b;
                        foreach (var b in groupLists)
                        {
                            Console.WriteLine(b.Field<DateTime>("orderDate").ToString());
                            dtTemp.ImportRow(b);
                        }


                        if (dtTemp.Rows.Count > 0)
                        {
                            dtTemp = setRecivedDate(dtTemp);
                            string filePath = ExportDataSetToExcel(dtTemp, "orderProductByRegion" + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx");
                            List<Attachment> fileLists = new List<Attachment>
                        {
                            new Attachment(filePath)
                        };

                            sendEmail(s.emailContact
                                , "theeradech.r@thaibev.com,voranuch.t@thaibev.com,tossaporn.p@sales.thaibev.com,nattawat.h@thaibev.com,parnupong.k@thaibev.com"
                                , "รายงานการสั่งซื้อสินค้า crystal วันที่ " + DateTime.Now.ToString("dd/MM/yyyy")
                                , Properties.Settings.Default.strBody
                                , fileLists);

                            strLogs.WriteLine("== Email to " + s.emailContact + " is success. ==");
                            dtTemp.Clear();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("sentReportByAllRegion >> " + ex.Message);
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

                        sendEmail(dr["email"].ToString()
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
                dr["recivedDate"] = thisWeekEnd.AddDays(+3).ToString("dddd, dd MMMM yyyy");
            }

            return dt;
        }

    }
}
