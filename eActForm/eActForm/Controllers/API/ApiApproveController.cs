using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class ApiApproveController : Controller
    {
        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> doApprove(string GridHtml, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();
            try
            {
                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    EmailAppCodes.sendReject(activityId, AppCode.ApproveType.Activity_Form);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId + "_");
                    GridHtml = GridHtml.Replace("<br>", "<br/>");
                    GridHtml = GridHtml.Replace("undefined", "");
                    AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

                    TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
                    getImageModel.tbActImageList = ImageAppCode.GetImage(activityId).Where(x => x.extension == ".pdf").ToList();
                    string[] pathFile = new string[getImageModel.tbActImageList.Count + 1];
                    pathFile[0] = Server.MapPath(rootPathInsert);
                    if (getImageModel.tbActImageList.Any())
                    {
                        int i = 1;
                        foreach (var item in getImageModel.tbActImageList)
                        {
                            pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], item._fileName));
                            i++;
                        }
                    }
                    var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
                    var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile);

                    EmailAppCodes.sendApprove(activityId, AppCode.ApproveType.Activity_Form, false);
                    ApproveAppCode.setCountWatingApprove();
                }
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("genPdfApprove >> " + ex.Message);
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax, "text/plain");
        }

        // GET: ApiApprove
        public ActionResult Index()
        {
            return View();
        }
    }
}