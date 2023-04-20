using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace eActForm.BusinessLayer
{
    public class GenPDFAppCode
    {
        public static void doGen(string gridHtml, string activityId, HttpServerUtilityBase server)
        {
            string log = "", htmlHeader="";
            try
            {
                Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
                activity_TBMMKT_Model = ReportAppCode.mainReport(activityId, "");


                gridHtml = gridHtml.Replace("<br>", "<br/>");
                gridHtml = gridHtml.Replace("undefined", "");
                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId + "_");
                log = "rootPathInsert >>" + rootPathInsert;



                if (activity_TBMMKT_Model != null)
                {
                    if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]
                        || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPurchaseTbm"])
                    {

                        htmlHeader = ApproveAppCode.RenderViewToString("MainReport", "styleView", null);
                        htmlHeader += " <table style=\"width: 100%;\" id=\"tabel_report\" class=\"formBorderStyle2\"><tr><td>";
                        htmlHeader += ApproveAppCode.RenderViewToString("PartialPaymentVoucher", "headerPv", activity_TBMMKT_Model);
                        htmlHeader += ApproveAppCode.RenderViewToString("PartialPaymentVoucher", "headerPvDetails", activity_TBMMKT_Model);
                        htmlHeader += "</td></tr></table>";

                    }
                }


                AppCode.genPdfFile(gridHtml, new Document(PageSize.A4, 25, 25, 30, 30), HostingEnvironment.MapPath(rootPathInsert), HostingEnvironment.MapPath("~"), htmlHeader);
                


                TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels
                {
                    tbActImageList = ImageAppCode.GetImage(activityId).Where(x => x.extension == ".pdf").ToList()
                };


                string[] pathFile = new string[getImageModel.tbActImageList.Count + 1];
                pathFile[0] = HostingEnvironment.MapPath(rootPathInsert);
                if (getImageModel.tbActImageList.Any())
                {
                    int i = 1;
                    foreach (var item in getImageModel.tbActImageList)
                    {
                        pathFile[i] = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                        i++;
                    }
                }
                var rootPathOutput = HostingEnvironment.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
                log += "rootPathOutput >>" + rootPathOutput;


                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id != ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]
                    && activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id != ConfigurationManager.AppSettings["formPurchaseTbm"])
                {
                    log += "call mergePDF";
                    var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile, activityId);
                }
                else
                {
                    log += "delete rootPathOutput >>" + rootPathOutput;
                    File.Delete(rootPathOutput);
                    string replace = rootPathOutput.Replace(".pdf", "_.pdf");
                    File.Copy(replace, rootPathOutput);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("doGen PDF >> " + ex.Message);
            }
        }
    }
}