using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using eActForm.Models;
namespace eActForm.Controllers
{
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

        public ActionResult getPDF(string actId, string type)
        {
            string rootPath = "";
            if (type == AppCode.ApproveType.Report_Detail.ToString())
            {
                rootPath = ConfigurationManager.AppSettings["rootRepDetailPdftURL"];
            }
            else
            {
                rootPath = ConfigurationManager.AppSettings["rooPdftURL"];
            }
            var fileStream = new FileStream(Server.MapPath(string.Format(rootPath, actId)),
                                             FileMode.Open,
                                             FileAccess.Read
                                           );
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }

    }
}