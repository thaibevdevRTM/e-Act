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
                SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport();
                models.typeForm = typeForm;
               // ViewBag.TypeForm = typeForm;
                if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
                {
                    if (typeForm == Activity_Model.activityType.MT.ToString())
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
                DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-15) : DateTime.ParseExact(Request.Form["startDate"], "MM/dd/yyyy", null);
                DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "MM/dd/yyyy", null);
                models.actRepDetailLists = DocumentsAppCode.getActRepDetailLists(startDate, endDate, typeForm);

                if (Request.Form["txtActivityNo"] != "")
                {
                    models.actRepDetailLists = models.actRepDetailLists.Where(x => x.id == RepDetailAppCode.getRepdetailByActNo(Request.Form["txtActivityNo"].ToString())).ToList();
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
                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId + "_");
                GridHtml = GridHtml.Replace("<br>", "<br/>");
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