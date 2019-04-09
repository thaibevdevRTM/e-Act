using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using WebLibrary;

namespace eActForm.Models
{
    public class AppCode
    {
        public static string StrCon = ConfigurationManager.ConnectionStrings["ActDB_ConnectionString"].ConnectionString;
        public static string StrMessFail = ConfigurationManager.AppSettings["messFail"].ToString();

        public enum ApproveEmailType
        {
            Activity_Form
                , Report_Detail
                , Report_Summary
        }
        public enum ApproveStatus
        {
            Draft = 1
            , รออนุมัติ = 2
            , อนุมัติ = 3
            , Success = 4
            , ไม่อนุมัติ = 5
        }
        public enum StatusType
        {
            app, // approve
            doc // document
        }

        public static string checkNullorEmpty(string p)
        {
            return p == "" || p == null || p == "0" || p == "0.00" ? "0" : p;
        }

        public static MemoryStream GetFileReportTomail_Preview(string GridHtml, Document pdfDoc)
        {
            MemoryStream ms = new MemoryStream();
            try
            {

                UserControl LoadControl = new UserControl();
                StringWriter sw = new StringWriter();
                HtmlTextWriter myWriter = new HtmlTextWriter(sw);
                LoadControl.RenderControl(myWriter);
                StringReader sr = new StringReader(sw.ToString());



                StringBuilder GridBuilder = new StringBuilder();
                GridBuilder.Append("<html>");
                GridBuilder.Append("<style>");
                GridBuilder.Append(".fontt{font-family:Angsana New;}");
                GridBuilder.Append("</style>");
                GridBuilder.Append("<body class=\"fontt\">");
                GridBuilder.Append(GridHtml);
                GridBuilder.Append("</body>");
                GridBuilder.Append("</html>");

                GridBuilder.Append(sw.ToString());


                string path = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Content\\" + "tablethin.css";
                string readText = System.IO.File.ReadAllText(path);

                //Document pdfDoc = new Document(pageSize, 25, 25, 10, 10);
                using (var writer = PdfWriter.GetInstance(pdfDoc, ms))
                {

                    pdfDoc.Open();
                    using (MemoryStream cssMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(readText)))
                    {
                        using (MemoryStream mss = new MemoryStream(Encoding.UTF8.GetBytes(GridBuilder.ToString().Replace(".png\">", ".png\"/>"))))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, mss, cssMemoryStream, Encoding.UTF8);
                        }
                        pdfDoc.Close();
                    }
                }
                return ms;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> GetFileReportTomail_Preview");
                return ms;
            }

        }
        public static List<Attachment> genPdfFile(string GridHtml, Document doc, string activityId)
        {
            //GridHtml = GridHtml.Replace("\n", "");
            ContentType xlsxContent = new ContentType("application/pdf");
            MemoryStream msPreview = new MemoryStream();
            byte[] PreviewBytes = new byte[0];
            List<Attachment> files = new List<Attachment>();

            msPreview = GetFileReportTomail_Preview(GridHtml, doc);
            PreviewBytes = msPreview.ToArray();

            var rootPath = HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
            File.WriteAllBytes(rootPath, PreviewBytes);

            if (PreviewBytes.Length != 0)
            {
                Attachment data_RepCashofSale = new Attachment(new MemoryStream(PreviewBytes), xlsxContent);
                data_RepCashofSale.ContentDisposition.FileName = "eActForm.pdf";
                files.Add(data_RepCashofSale);
            }

            return files;
        }

        public static MemoryStream genFileToMemoryStream(string pathFile)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                if (File.Exists(pathFile))
                {
                    using (FileStream file = new FileStream(pathFile, FileMode.Open, FileAccess.Read))
                    {
                        file.CopyTo(ms);
                    }
                }
                return ms;
            }
            catch (Exception ex)
            {
                throw new Exception("genFileToMemoryStream >> " + ex.Message);
            }
        }

        public static List<Productcostdetail> getProductcostdetail(string p_productcateid, string p_size, string p_customerid)
        {
            DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getProductcost"
                 , new SqlParameter("@p_productcateid", p_productcateid)
                 , new SqlParameter("@p_size", p_size)
                 , new SqlParameter("@p_customerid", p_customerid));
            var lists = (from DataRow d in ds.Tables[0].Rows
                         select new Productcostdetail()
                         {
                             productId = d["productId"].ToString(),
                             productName = d["productName"].ToString(),
                             wholeSalesPrice = decimal.Parse(d["price"].ToString()),
                             normalCost = decimal.Parse(d["price"].ToString()),
                         });
            return lists.ToList();
        }



        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        public static void CheckFolder_CreateNotHave_Direct(string path)
        {

            DirectoryInfo ObjDirItemNo = new DirectoryInfo(path);

            if (ObjDirItemNo.Exists == false)
            {
                ObjDirItemNo.Create();
            }

        }

    }
}