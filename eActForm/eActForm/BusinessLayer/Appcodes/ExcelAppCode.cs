using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class ExcelAppCode
    {
        public static DataTable ReadExcel(string path, string sheetName, string fromColumnToColumn)
        {
            DataTable dt = new DataTable();
            using (OleDbConnection conn = new OleDbConnection())
            {

                //ใช้ xls เท่านั่นจะได้ไม่ติดปัญหา Register OLE หรือต้องติดตั้งอะไรเพิ่มเติม

                // if (path.Contains("xlsx"))
                // {
                //conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ path + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";
                // }
                // else
                // {
                //conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + path + " ; Extended Properties='Excel 8.0;IMEX=1;HDR=YES'";                
                conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\";";
                // }

                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + sheetName + "$" + fromColumnToColumn + "]";
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}