using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;
using eActForm.BusinessLayer;
using eActForm.Models;
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
    }
}