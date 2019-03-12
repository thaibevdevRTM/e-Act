using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;

namespace eActForm.Controllers
{
    public class ActivityViewerController : Controller
    {
        // GET: ActivityViewer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult activityPDFView(string actId)
        {
            //TempData["fileViewer"] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actId));
            TempData["actId"] = actId;
            return PartialView();
        }

        public ActionResult getPDF(string actId)
        {
            var fileStream = new FileStream(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], actId)),
                                             FileMode.Open,
                                             FileAccess.Read
                                           );
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }

    }
}