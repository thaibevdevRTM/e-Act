using eActForm.Models;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using WebLibrary;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class ActivityViewerController : Controller
    {
        // GET: ActivityViewer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult activityPDFView(string actId, string type)
        {
            //TempData["fileViewer"] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actId));
            ViewBag.actId = actId;
            ViewBag.type = type;
            return PartialView();
        }

        public ActionResult getPDF(string actId, string type, string extension)
        {
            string rootPath = "", mapPath = "";

            try
            {

                if (type == AppCode.ApproveType.Report_Detail.ToString())
                {
                    rootPath = ConfigurationManager.AppSettings["rootRepDetailPdftURL"];
                }
                else if (type == "UploadFile")
                {
                    rootPath = ConfigurationManager.AppSettings["rootUploadfiles"];
                }
                else if (type == AppCode.ApproveType.Report_Summary.ToString())
                {
                    rootPath = ConfigurationManager.AppSettings["rootSummaryDetailPdftURL"];
                }
                else
                {
                    rootPath = ConfigurationManager.AppSettings["rooPdftURL"];
                }


                if (type != "UploadFile")
                {
                    if (!System.IO.File.Exists(Server.MapPath(string.Format(rootPath, actId))))
                    {
                        actId = "fileNotFound";
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
            var fileStream = new FileStream(Server.MapPath(string.Format(rootPath, actId)),
                                                     FileMode.Open,
                                                     FileAccess.Read
                                                   );


            if (extension == ".pdf" || string.IsNullOrEmpty(extension))
            {
                return new FileStreamResult(fileStream, "application/pdf");
            }
            else
            {
                return File(fileStream, "application/vnd.ms-excel", actId);
            }

        }

    }
}