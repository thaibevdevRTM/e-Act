using System;
using System.IO;

namespace WebLibrary
{
    public class LogsManager
    {

        public static void WriteLogs(string fileName, string resultMessage, string logsMessage)
        {
            try
            {
                string path = "~/Logs/" + fileName + DateTime.Today.ToString("dd-MM-yy") + ".txt";
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
