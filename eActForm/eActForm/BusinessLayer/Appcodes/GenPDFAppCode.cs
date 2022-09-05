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
            string log = "";
            try
            {

                gridHtml = gridHtml.Replace("<br>", "<br/>");
                gridHtml = gridHtml.Replace("undefined", "");


                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId + "_");
                log = "rootPathInsert >>" + rootPathInsert;
                AppCode.genPdfFile(gridHtml, new Document(PageSize.A4, 25, 25, 10, 10), server.MapPath(rootPathInsert), server.MapPath("~"));


                TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels
                {
                    tbActImageList = ImageAppCode.GetImage(activityId).Where(x => x.extension == ".pdf").ToList()
                };


                string[] pathFile = new string[getImageModel.tbActImageList.Count + 1];
                pathFile[0] = server.MapPath(rootPathInsert);
                if (getImageModel.tbActImageList.Any())
                {
                    int i = 1;
                    foreach (var item in getImageModel.tbActImageList)
                    {
                        pathFile[i] = server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                        i++;
                    }
                }
                var rootPathOutput = server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
                log += "rootPathOutput >>" + rootPathOutput;

                List<ActivityForm> getActList = QueryGetActivityById.getActivityById(activityId);
                if (getActList.Any() && getActList.FirstOrDefault().master_type_form_id != ConfigurationManager.AppSettings["formPaymentVoucherTbmId"]
                    || getActList.FirstOrDefault().master_type_form_id != ConfigurationManager.AppSettings["formPurchaseTbm"])
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