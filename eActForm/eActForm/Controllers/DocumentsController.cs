using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;
using eActForm.BusinessLayer;
using eActForm.Models;
using System.Configuration;
using iTextSharp.text;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class DocumentsController : Controller
    {
        // GET: Documents
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult reportDetail()
        {
            try
            {
                SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
                return View(models);
            }
            catch (Exception ex)
            {
                UtilsAppCode.Session.User.exception = ex.Message;
                ExceptionManager.WriteError("reportDetail >> " + ex.Message);
            }

            return View();
        }

        public ActionResult reportDetailListsView()
        {
            DocumentsModel.actRepDetailModels models = new DocumentsModel.actRepDetailModels();
            try
            {

                models.actRepDetailLists = DocumentsAppCode.getActRepDetailLists();

            }
            catch (Exception ex)
            {
                UtilsAppCode.Session.User.exception = ex.Message;
                ExceptionManager.WriteError("reportDetailListsView >>" + ex.Message);
            }
            return PartialView(models);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult genPdfApprove(string GridHtml, string statusId, string activityId)
        {
            var resultAjax = new AjaxResult();
            try
            {

                    var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId + "_");
                    GridHtml = GridHtml.Replace("<br>", "<br/>");
                    AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

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
    }
}