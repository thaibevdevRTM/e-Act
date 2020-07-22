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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using WebLibrary;
using static eActForm.Models.TB_Act_Image_Model;

namespace eActForm.Models
{
    public class AppCode
    {

        public static string StrConAuthen = ConfigurationManager.ConnectionStrings["ActDBAuthen_ConnectionString"].ConnectionString;
        public static string StrCon = ConfigurationManager.ConnectionStrings["ActDB_ConnectionString"].ConnectionString;
        public static string StrMessFail = ConfigurationManager.AppSettings["messFail"].ToString();
        public static string nonAL = "1D1097F4-246F-4DC2-BB69-B7BB6E678299";
        public static string AL = "FC696EB5-B058-445E-B605-977C5067AEBA";
        public static string[] hcForm = { ConfigurationManager.AppSettings["formExpTrvNumId"], ConfigurationManager.AppSettings["formExpMedNumId"] };
        public static string[] compHcForm = { Activity_Model.groupCompany.NUM.ToString(), Activity_Model.groupCompany.POM.ToString(), Activity_Model.groupCompany.CVM.ToString() };

        public enum ApproveEmailype
        {
            approve
                , document
                , budget_form
        }
        public enum ApproveType
        {
            Activity_Form
                , Report_Detail
                , Report_Summary
                , Budget_form
        }
        public enum ApproveStatus
        {
            Draft = 1
            , รออนุมัติ = 2
            , อนุมัติ = 3
            , Success = 4
            , ไม่อนุมัติ = 5
            , เพิ่มเติม = 7 // for Report Detail
            , เรียนเพื่อทราบ
        }
        public enum StatusType
        {
            app, // approve
            doc // document
        }

        public enum Mode
        {
            addNew,
            edit
        }


        public static class ApproveGroup
        {
            public const string Applicant = "D0A49169-8697-4001-9CD4-896976E964F5";//ผู้ขอเบิก   
            public const string ApplicantApp = "A2D5B3D5-610D-4D7E-B311-25717B3457F8";// ผู้ขออนุมัติ  
            public const string Recorder = "8A6A0CA2-E9CC-4A6C-9E17-E0B212C12DAF";//ผู้บันทึก   
            public const string Director = "2354EFCB-174F-48FE-B634-2DE926D7F836";// ผู้บังคับบัญชา  
            public const string Approveby = "7C308168-B155-4684-A2CD-906EC94AA49C";//เรียน/อนุมัติ 
            public const string PettyCashApprover = "BD48756C-12CC-4267-AD6F-A6C37F9B2B32";//ผู้อนุมัติเงินสดย่อย
            public const string CreateBy = "1AFEFF8D-C980-4628-8550-78AE619AC31A";//ผู้จัดทำ
            public const string PettyCashVerify = "FA88EB4E-26C1-49FB-BEC7-D161AA9CC0A0";//ผู้ตรวจสอบเงินสดย่อย
        }
        public static class CodeHtml
        {
            public const string LabelHtml = "label";
            public const string TextboxHtml = "textbox";
            public const string DropdownHtml = "dropdown";


        }
        public static class Expenses
        {
            public const string Allowance = "06FF853F-EBB0-48E8-9620-520D0B8F6E0C";
            public const string Medical = "6BB0F68F-4B07-4E00-9B1E-B776D003D992";
        }
        public static string checkNullorEmpty(string p)
        {
            return p == "" || p == null || p == "0" || p == "0.00" || p == "0.000" || p == "0.0000" || p == "0.00000" ? "0" : p;
        }

        public static MemoryStream GetFileReportTomail_Preview(string GridHtml, Document pdfDoc, string serverMapPath)
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
                GridBuilder.Append(".fontt{font-family:TH SarabunPSK;}");
                GridBuilder.Append("</style>");
                GridBuilder.Append("<body class=\"fontt\">");
                GridBuilder.Append(GridHtml);
                GridBuilder.Append("</body>");
                GridBuilder.Append("</html>");

                // Replace 
                GridBuilder = GridBuilder.Replace("signa\">", "signa\"/>");

                GridBuilder.Append(sw.ToString());


                string path = serverMapPath + "\\Content\\" + "tablethin.css";
                string readText = System.IO.File.ReadAllText(path);

                using (var writer = PdfWriter.GetInstance(pdfDoc, ms))
                {
                    pdfDoc.Open();
                    //pdfDoc = new Document(PageSize.A4, 25, 25, 10, 10);
                    using (MemoryStream cssMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(readText)))
                    {
                        writer.CloseStream = false;
                        using (MemoryStream mss = new MemoryStream(Encoding.UTF8.GetBytes(GridBuilder.ToString().Replace(".png\">", ".png\"/>").Replace(".jpg\">", ".jpg\"/>").Replace(".jpeg\">", ".jpeg\"/>").Replace(".jfif\">", ".jfif\"/>"))))
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
                ms.Dispose();
                return ms;
            }
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
                GridBuilder.Append(".fontt{font-family:TH SarabunPSK;}");
                GridBuilder.Append("</style>");
                GridBuilder.Append("<body class=\"fontt\">");
                GridBuilder.Append(GridHtml);
                GridBuilder.Append("</body>");
                GridBuilder.Append("</html>");

                GridBuilder.Append(sw.ToString());


                string path = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Content\\" + "tablethin.css";
                string readText = System.IO.File.ReadAllText(path);

                using (var writer = PdfWriter.GetInstance(pdfDoc, ms))
                {
                    pdfDoc.Open();
                    using (MemoryStream cssMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(readText)))
                    {

                        using (MemoryStream mss = new MemoryStream(Encoding.UTF8.GetBytes(GridBuilder.ToString().Replace(".png\">", ".png\"/>").Replace(".jpg\">", ".jpg\"/>").Replace(".jpeg\">", ".jpeg\"/>"))))
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
                ms.Dispose();
                return ms;
            }
        }

        public static MemoryStream GetFileStream(string GridHtml, Document pdfDoc)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                //GridHtml = "testt";
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

                        using (MemoryStream mss = new MemoryStream(Encoding.UTF8.GetBytes(GridBuilder.ToString().Replace(".png\">", ".png\"/>").Replace(".jpg\">", ".jpg\"/>").Replace(".jpeg\">", ".jpeg\"/>"))))
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
                ms.Dispose();
                return ms;
            }

        }

        public static List<Attachment> genPdfFile(string GridHtml, Document doc, string rootPath)
        {
            return genPdfFile(GridHtml, doc, rootPath, HttpContext.Current.Server.MapPath("~"));
        }

        public static List<Attachment> genPdfFile(string GridHtml, Document doc, string rootPath, string serverMapPath)
        {

            ContentType xlsxContent = new ContentType("application/pdf");
            MemoryStream msPreview = new MemoryStream();
            byte[] PreviewBytes = new byte[0];
            List<Attachment> files = new List<Attachment>();

            msPreview = GetFileReportTomail_Preview(GridHtml, doc, serverMapPath);
            PreviewBytes = msPreview.ToArray();

            if (PreviewBytes.Length != 0)
            {
                Attachment data_RepCashofSale = new Attachment(new MemoryStream(PreviewBytes), xlsxContent);
                data_RepCashofSale.ContentDisposition.FileName = "eActForm.pdf";
                files.Add(data_RepCashofSale);
            }

            return files;
        }



        public static MemoryStream genPdfFileStream(string GridHtml, Document doc)
        {
            //GridHtml = GridHtml.Replace("\n", "");
            MemoryStream msPreview = new MemoryStream();
            msPreview = GetFileReportTomail_Preview(GridHtml, doc);
            return msPreview;
        }



        public static string mergePDF(string rootPathOutput, string[] pathFile)
        {
            string result = string.Empty;
            PdfReader reader = null/* TODO Change to default(_) if this is not a reference type */;
            Document sourceDocument = null/* TODO Change to default(_) if this is not a reference type */;
            PdfCopy pdfCopyProvider = null/* TODO Change to default(_) if this is not a reference type */;
            PdfImportedPage importedPage;
            sourceDocument = new Document();
            try
            {
                pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(rootPathOutput, System.IO.FileMode.Create));
                sourceDocument.Open();


                for (int f = 0; f <= (pathFile.Length - 1); f++)
                {

                    int pages = get_pageCcount(pathFile[f]);
                    reader = new PdfReader(pathFile[f]);
                    for (int i = 1; i <= pages; i++)
                    {
                        importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                        pdfCopyProvider.AddPage(importedPage);
                    }
                    reader.Close();
                }
                sourceDocument.Close();
                result = "success";
            }
            catch (Exception ex)
            {
                try
                {
                    sourceDocument.Close();
                    File.Delete(rootPathOutput);
                    string replace = rootPathOutput.Replace(".pdf", "_.pdf");
                    File.Copy(replace, rootPathOutput);
                }
                catch (Exception exc)
                {
                    result = "error" + exc.Message;
                    ExceptionManager.WriteError(exc.Message + ">> mergePDF >> CopyError");
                }
            }
            return result;
        }

        private static int get_pageCcount(string file)
        {
            //var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //using (StreamReader sr = new StreamReader(fs))
            //{
            //    Regex regex = new Regex(@"/Type\s*/Page[^s]");
            //    MatchCollection matches = regex.Matches(sr.ReadToEnd());
            //    return matches.Count;
            //}
            //แก้ปัญหามีเอกสารแนบ 4 หน้า ฟังก์ชันเดิมอ่านได้ 6 หน้า ทำให้ merge pdf ไม่ได้ By Kanokpun 2020622
            int resultPagesCount = 0;
            using ( var reader = new PdfReader(file))
            {
                resultPagesCount = reader.NumberOfPages;
            }
            return resultPagesCount;
    
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
                        file.Dispose();

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


        public static List<ImageModel> writeImagestoFile(HttpServerUtilityBase server, List<ImageModel> imgLists)
        {
            try
            {
                foreach (ImageModel dr in imgLists)
                {
                    if (dr._image != null && dr._image.Length > 0)
                    {
                        dr._fileName = writeFileHistory(server, dr._image, dr.activityId + "_" + dr.id + ".png").Replace("..", "");
                    }
                }
                return imgLists;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static string writeFileHistory(HttpServerUtilityBase server, byte[] imgByte, string fileName)
        {
            try
            {
                string filePath = "../uploadImages/" + fileName;
                if (imgByte.Length > 0)
                {
                    if (!File.Exists(server.MapPath(filePath)))
                    {
                        File.WriteAllBytes(server.MapPath(filePath), imgByte);
                    }
                }
                return filePath;
            }
            catch (Exception ex)
            {
                return server.MapPath(fileName) + ex.Message;
            }
        }
    }
}