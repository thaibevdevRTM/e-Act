using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
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

        //EPPlus On mobile ก็ Export ได้
        public static void ExportExcelEpPlus(DataTable dt, string excelSheetName, string fileName, string AuthorFile, string SubjectFile, HttpContextBase p,string typeProcess)
        {

            string currentDirectorypath = Environment.CurrentDirectory;
            string finalFileNameWithPath = string.Empty;
            finalFileNameWithPath = string.Format("{0}\\{1}.xlsx", currentDirectorypath, fileName);

            //Delete existing file with same file name.
            if (File.Exists(finalFileNameWithPath))
                File.Delete(finalFileNameWithPath);

            var newFile = new FileInfo(finalFileNameWithPath);

            //Step 1 : Create object of ExcelPackage class and pass file path to constructor.
            using (var package = new ExcelPackage(newFile))
            {
                //Step 2 : Add a new worksheet to ExcelPackage object and give a suitable name
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(excelSheetName);

                //Step 3 : Start loading datatable form A1 cell of worksheet.
                worksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.None);
                if(typeProcess== "MaterialProductPosPremium")
                {
                    worksheet.Column(12).Style.Numberformat.Format = "yyyy-mm-dd";
                    worksheet.Column(14).Style.Numberformat.Format = "yyyy-mm-dd";
                }

                //Step 4 : (Optional) Set the file properties like title, author and subject
                package.Workbook.Properties.Title = fileName; //@"This code is part of tutorials available at http://bytesofcode.hubpages.com";
                package.Workbook.Properties.Author = AuthorFile;
                package.Workbook.Properties.Subject = SubjectFile;

                //Step 5 : Save all changes to ExcelPackage object which will create Excel 2007 file.
                // package.Save(); //อันนี้ลงดิส

                //===========start========================Save To Web===============================================
                Byte[] fileBytes = package.GetAsByteArray(); //Read the Excel file in a byte array

                //Clear the response
                p.Response.ClearHeaders();
                p.Response.ClearContent();
                p.Response.Clear();

                //p.Response.Cookies.Clear();


                //Add the header & other information
                //p.Response.Cache.SetCacheability(System.Web.HttpCacheability.Private);
                //p.Response.CacheControl = "private";
                //p.Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
                //p.Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
                p.Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
                //p.Response.AppendHeader("Pragma", "cache");
                //p.Response.AppendHeader("Expires", "60");
                p.Response.AddHeader("Content-Disposition",
                "attachment; " +
                "filename=" + fileName + ".xlsx; " +
                "size=" + fileBytes.Length.ToString() + "; " +
                "creation-date=" + DateTime.Now.ToString("R").Replace(",", "") + "; " +
                "modification-date=" + DateTime.Now.ToString("R").Replace(",", "") + "; " +
                "read-date=" + DateTime.Now.ToString("R").Replace(",", ""));

                //p.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                p.Response.ContentType = "application/x-msexcel";

                //Write it back to the client
                p.Response.BinaryWrite(fileBytes);
                p.Response.Flush();
                p.Response.Close();
                //===========end========================Save To Web===============================================
            }
        }

    }
}