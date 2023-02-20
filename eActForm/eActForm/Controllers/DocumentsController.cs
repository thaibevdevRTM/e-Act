using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using static eActForm.Models.RepDetailModel;

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

        public ActionResult reportDetail(string typeForm)
        {
            try
            {
                SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport(typeForm);
                models.typeForm = typeForm;
                // ViewBag.TypeForm = typeForm;
                if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
                {
                    if (typeForm == Activity_Model.activityType.MT.ToString() || typeForm == Activity_Model.activityType.SetPrice.ToString())
                    {
                        models.customerslist = QueryGetAllCustomers.getCustomersMT();
                    }
                    else
                    {
                        models.customerslist = QueryGetAllCustomers.getCustomersOMT();
                    }
                }
                return View(models);
            }
            catch (Exception ex)
            {
                UtilsAppCode.Session.User.exception = ex.Message;
                ExceptionManager.WriteError("reportDetail >> " + ex.Message);
            }

            return View();
        }

        public ActionResult searchActForm(string typeForm)
        {
            DocumentsModel.actRepDetailModels models = new DocumentsModel.actRepDetailModels();
            try
            {
                DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-15) : DateTime.ParseExact(Request.Form["startDate"], "dd/MM/yyyy", null);
                DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "dd/MM/yyyy", null);

                models.actRepDetailLists = DocumentsAppCode.getActRepDetailLists(startDate, endDate, typeForm);

                if (Request.Form["txtActivityNo"] != "")
                {
                    var getId = RepDetailAppCode.getRepdetailByActNo(Request.Form["txtActivityNo"].ToString());
                    models.actRepDetailLists = models.actRepDetailLists.Where(x => x.id == getId).ToList();
                }
                else
                {
                    #region filter
                    if (Request.Form["ddlStatus"] != "")
                    {
                        models.actRepDetailLists = models.actRepDetailLists.Where(x => x.statusId == Request.Form["ddlStatus"]).ToList();
                    }
                    if (Request.Form["ddlCustomer"] != "")
                    {
                        models.actRepDetailLists = models.actRepDetailLists.Where(x => x.customerId == Request.Form["ddlCustomer"]).ToList();
                    }

                    if (Request.Form["ddlProductType"] != "")
                    {
                        models.actRepDetailLists = models.actRepDetailLists.Where(x => x.productTypeId == Request.Form["ddlProductType"]).ToList();
                    }


                    #endregion
                }
                TempData["SearchDataRepDetail"] = models.actRepDetailLists;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("reportDetailListsView");
        }

        public ActionResult reportDetailListsView(string typeForm)
        {
            DocumentsModel.actRepDetailModels models = new DocumentsModel.actRepDetailModels();

            try
            {
                if (TempData["SearchDataRepDetail"] != null)
                {
                    models.actRepDetailLists = (List<DocumentsModel.actRepDetailModel>)TempData["SearchDataRepDetail"];
                }
                else
                {
                    models.actRepDetailLists = DocumentsAppCode.getActRepDetailLists(DateTime.Now.AddDays(-15), DateTime.Now, typeForm);
                }
                TempData["SearchDataModelSummary"] = null;
                return PartialView(models);
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
                GenPDFAppCode.doGen(GridHtml, activityId, Server);

                bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], activityId)));
                if (folderExists)
                    Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], activityId)), true);

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


        public ActionResult actDetail(string repId)
        {
            actApproveRepDetailModels model = new actApproveRepDetailModels();
            try
            {
                model.repDetailLists = RepDetailAppCode.getActNoByRepId(repId);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("actDetail >> " + ex.Message);
            }

            return PartialView(model);
        }
    }
}