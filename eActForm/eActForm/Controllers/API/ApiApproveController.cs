using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class ApiApproveController : Controller
    {
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult doApprove(string gridHtml, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();

            string empId = UtilsAppCode.Session.User.empId;
            ApproveAppCode.setCountWatingApprove();
            HostingEnvironment.QueueBackgroundWorkItem(c => doGenFile(gridHtml, empId, statusId, activityId));

            return Json(resultAjax, "text/plain");
        }

        /// <summary>
        /// ******* BackGround Service can't  use session ************
        /// </summary>
        /// <param name="gridHtml"></param>
        /// <param name="empId"></param>
        /// <param name="statusId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        private async Task<AjaxResult> doGenFile(string gridHtml, string empId, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();
            try
            {

                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    EmailAppCodes.sendReject(activityId, AppCode.ApproveType.Activity_Form, empId);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId + "_");
                    gridHtml = gridHtml.Replace("<br>", "<br/>");
                    gridHtml = gridHtml.Replace("undefined", "");
                    AppCode.genPdfFile(gridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert), Server.MapPath("~"));

                    TB_Act_Image_Model.ImageModels getImageModel = new TB_Act_Image_Model.ImageModels();
                    getImageModel.tbActImageList = ImageAppCode.GetImage(activityId, ".pdf");
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
                }
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("apiApprove genPdfApprove >> " + ex.Message);
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
            }

            return resultAjax;
        }


        // GET: ApiApprove
        public ActionResult Index()
        {
            return View();
        }
    }
}