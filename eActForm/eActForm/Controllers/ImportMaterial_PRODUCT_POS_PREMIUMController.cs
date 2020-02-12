using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ImportMaterial_PRODUCT_POS_PREMIUMController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ImportExcel importExcel)
        {
            if (ModelState.IsValid)
            {
                string folderKeepFile = "ImportMatProductPosPremium";
                string UploadDirectory = Server.MapPath("~") + "\\Uploadfiles\\" + folderKeepFile + "\\";
                string path = UploadDirectory + importExcel.file.FileName;
                importExcel.file.SaveAs(path);

                var dt = new DataTable();
                dt = ExcelAppCode.ReadExcel(path, "dataimport", "A:H");
                var countDataHaveRows = dt.Rows.Count;

                ViewBag.Result = "Successfully Imported";
            }
            //Thread.Sleep(3000);
            return View();
        }
    }
}