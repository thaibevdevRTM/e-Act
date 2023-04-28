using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchDeleteFile
{
    public class AppCode
    {
        public static List<fileModel> getFileList(string strCon)
        {
            bool result = false;
            try
            {

                int rtn = 0;
                Console.Write("waiting for get data");

                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getFileListNonActId");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new fileModel
                             {
                                 _fileName = d["_fileName"].ToString()
                             });
                return lists.ToList();


            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception("getFileList >>" + ex.Message);
            }

        }

        public static string LogsFileName()
        {

                var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                string str = string.Format(BatchDeleteFile.Properties.Resources.logsFileName, new string[] { Path.GetDirectoryName(location), DateTime.Now.ToString("ddMMyyyy") });
                return str;
            
        }
    }

    public class fileModel
    {
        public string _fileName { get; set; }

    }


}
