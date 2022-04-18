
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace updateDataReportTBM
{
    public class Program
    {
        static void Main(string[] args)
        {
            StreamWriter strLogs = File.AppendText(new Program().LogsFileName);
            try
            {
                strLogs.WriteLine("== Start Service " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss ") + "==");
                var result = AppCode.updateDataToTable();
                result = AppCode.updateDataReportMaketingToTable();
                if (result)
                {
                    Console.WriteLine("Update Report Success");
                    strLogs.WriteLine("== Update Report Success " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss "));
                }
            }
            catch (Exception ex)
            {
                strLogs.WriteLine(" Error User :" + ex.Message);
                strLogs.WriteLine("== END " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss ") + "==");
            }
            finally
            {
                strLogs.Flush();
                strLogs.Close();
            }
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
