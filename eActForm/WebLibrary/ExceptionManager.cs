using System;
using System.IO;
namespace WebLibrary
{
    public class ExceptionManager
    {
        public static void WriteError(string errorMessage)
        {
            try
            {
                //    OrclHelper.ExecuteNonQuery(connStr, CommandType.StoredProcedure, "WAP_PAYMENT.SectionPaymentLogsInsert"
                //, new OracleParameter[] {OracleDataAccress.OrclHelper.GetOracleParameter("p_file_name", fileName, OracleType.VarChar, ParameterDirection.Input) 
                //, OracleDataAccress.OrclHelper.GetOracleParameter("p_file_detail", resultMessage, OracleType.VarChar, ParameterDirection.Input) 
                //, OracleDataAccress.OrclHelper.GetOracleParameter("p_file_date", DateTime.Now, OracleType.DateTime, ParameterDirection.Input)});
                string path = "~/Error/" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    w.WriteLine(" IP :  {0} ", System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());
                    string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                                  ". Error Message:" + errorMessage;
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
