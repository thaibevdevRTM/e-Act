using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;

namespace WebLibrary
{
    public class LogsManager
    {

        public static void WriteLogs(string connStr,string fileName, string resultMessage, string logsMessage)
        {
            try
            {

                OrclHelper.ExecuteNonQuery(connStr, CommandType.StoredProcedure, "WAP_PAYMENT.SectionPaymentLogsInsert"
            , new OracleParameter[] {OracleDataAccress.OrclHelper.GetOracleParameter("p_file_name", fileName, OracleType.VarChar, ParameterDirection.Input) 
            , OracleDataAccress.OrclHelper.GetOracleParameter("p_file_detail", resultMessage , OracleType.VarChar, ParameterDirection.Input) 
            , OracleDataAccress.OrclHelper.GetOracleParameter("p_file_date", DateTime.Now, OracleType.DateTime, ParameterDirection.Input)});
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message);
                ExceptionManager.WriteError("WriteLogs >> " + ex.Message);
            }

        }

        public static void WriteLogs(string fileName,string resultMessage,string logsMessage)
        {
            try
            {
                string path = "~/Logs/" + fileName +DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    string err = "Logs in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                                  ". Logs result: " + resultMessage +
                                  ". Logs Message: " + logsMessage;
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
               // throw new Exception(ex.Message);
                ExceptionManager.WriteError("WriteLogs >> " + ex.Message);
            }

        }
    }
}
