using eActForm.BusinessLayer;
using eActForm.Models;
using eForms.Models.Forms;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class actFormRepDetailController : Controller
    {
        // GET: actFormRepDetail
        public ActionResult Index(string typeForm)
        {
            ViewBag.TypeForm = typeForm;
            SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport(typeForm);
            models.showUIModel = new searchParameterFilterModel();


            if (typeForm == Activity_Model.activityType.MT.ToString() || typeForm == Activity_Model.activityType.SetPrice.ToString())
            {
                models.customerslist = QueryGetAllCustomers.getCustomersMT();
            }
            else
            {
                models.customerslist = QueryGetAllCustomers.getAllRegion().Where(x => x.condition.Equals("OMT")).OrderBy(x => x.cusNameTH).ToList();
            }




            models.approveStatusList.Add(new ApproveModel.approveStatus()
            {
                id = "7",
                nameTH = "เพิ่มเติม",
                nameEN = "เพิ่มเติม",
            });
            return View(models);
        }

        public ActionResult searchActForm(string typeForm)
        {
            try
            {

                RepDetailModel.actFormRepDetails model = new RepDetailModel.actFormRepDetails();
                model = RepDetailAppCode.getRepDetailReportByCreateDateAndStatusId(Request.Form["startDate"], Request.Form["endDate"], typeForm);
                model.typeForm = typeForm;
                model.dateReport = Request.Form["reportDate"];

                if (Request.Form["txtActivityNo"] != "")
                {
                    model = RepDetailAppCode.getFilterRepDetailByActNo(model, Request.Form["txtActivityNo"]);
                }
                else
                {
                    #region filter
                    if (Request.Form["ddlStatus"] != "")
                    {
                        model = RepDetailAppCode.getFilterRepDetailByStatusId(model, Request.Form["ddlStatus"]);
                    }

                    if (Request.Form["ddlCustomer"] != "")
                    {
                        if (typeForm == Activity_Model.activityType.MT.ToString() || typeForm == Activity_Model.activityType.SetPrice.ToString())
                        {
                            model = RepDetailAppCode.getFilterRepDetailByCustomer(model, Request.Form["ddlCustomer"]);
                        }
                        else
                        {
                            model = RepDetailAppCode.getFilterRepDetailByRegion(model, Request.Form["ddlCustomer"]);
                        }
                    }

                    if (Request.Form["ddlTheme"] != "")
                    {
                        model = RepDetailAppCode.getFilterRepDetailByActivity(model, Request.Form["ddlTheme"]);
                    }
                    if (Request.Form["ddlProductType"] != "")
                    {
                        model = RepDetailAppCode.getFilterRepDetailByProductType(model, Request.Form["ddlProductType"]);
                    }
                    if (Request.Form["ddlProductGrp"] != "")
                    {
                        model = RepDetailAppCode.getFilterRepDetailByProductGroup(model, Request.Form["ddlProductGrp"]);
                    }

                    #endregion
                }

                if (model.actFormRepDetailGroupLists.Any())
                {

                    if (typeForm == Activity_Model.activityType.MT.ToString())
                    {
                        model.flowList = ApproveFlowAppCode.getFlowForReportDetail(
                                                ConfigurationManager.AppSettings["subjectReportDetailId"]
                                                , string.IsNullOrEmpty(Request.Form["ddlCustomer"]) ? model.actFormRepDetailLists.FirstOrDefault().customerId : Request.Form["ddlCustomer"]
                                                , Request.Form["ddlProductType"]);
                    }
                    else
                    {
                        model.flowList = ApproveFlowAppCode.getFlowForReportDetailOMT(
                                                ConfigurationManager.AppSettings["subjectReportDetailId"]
                                                , Request.Form["ddlCustomer"]
                                                , Request.Form["ddlProductType"]);
                    }
                }

                Session["ActFormRepDetail"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return RedirectToAction("repChooseView", new { startDate = Request.Form["reportDate"] });
        }
        public ActionResult repChooseView(string startDate)
        {
            RepDetailModel.actFormRepDetails model = null;
            try
            {
                ViewBag.MouthText = DateTime.ParseExact(startDate, "MM-yyyy", null).ToString("MMM yyyy");
                model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }

        public JsonResult repSetDelFlagRecodeDetail(string actId, bool delFlag)
        {
            var result = new AjaxResult();
            try
            {
                RepDetailModel.actFormRepDetails model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"];
                if (actId == "All")
                {
                    model.actFormRepDetailGroupLists.Select(r => r.delFlag = !delFlag
                        ).ToList();

                    model.actFormRepDetailLists.Select(r => r.delFlag = !delFlag
                        ).ToList();
                    result.Code = 001;
                }
                else
                {
                    model.actFormRepDetailGroupLists
                        .Where(r => r.id == actId)
                        .Select(r => r.delFlag = !delFlag
                        ).ToList();

                    model.actFormRepDetailLists
                        .Where(r => r.id == actId)
                        .Select(r => r.delFlag = !delFlag
                        ).ToList();
                }
                result.Success = delFlag;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }

            return Json(result);
        }

        public ActionResult approvePositionSignature(ApproveFlowModel.approveFlowModel flowModel)
        {
            return PartialView(flowModel);
        }

        public ActionResult repListView()
        {
            RepDetailModel.actFormRepDetails model = null;
            try
            {
                model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
                model.actFormRepDetailGroupLists.OrderBy(x => x.activityNo).ThenBy(x => x.rowNo).ToList();

                ViewBag.MouthText = DateTime.ParseExact(model.dateReport, "MM-yyyy", null).ToString("MMM yyyy");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }

        public ActionResult repListViewSetPrice()
        {
            RepDetailModel.actFormRepDetails model = null;
            try
            {
                model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
                ViewBag.MouthText = DateTime.ParseExact(model.dateReport, "MM-yyyy", null).ToString("MMM yyyy");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }


        public ActionResult repPreviewForApproveDetail(string startDate)
        {
            RepDetailModel.actFormRepDetails model = null;
            try
            {
                model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
                //ViewBag.MouthText = DateTime.ParseExact(startDate, "MM-yyyy", null).ToString("MMM yyyy");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }


        public ActionResult repPreviewForApprove(string startDate)
        {
            RepDetailModel.actFormRepDetails model = null;
            try
            {
                model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"] ?? new RepDetailModel.actFormRepDetails();
                ViewBag.MouthText = DateTime.ParseExact(startDate, "MM/dd/yyyy", null).ToString("MMM yyyy");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(model);
        }

        public ActionResult repListViewGroupBrand(RepDetailModel.actFormRepDetails model, string[] brandId)
        {
            RepDetailModel.actFormRepDetails rep = new RepDetailModel.actFormRepDetails();
            try
            {
                rep.actFormRepDetailLists = model.actFormRepDetailGroupLists.Where(r => brandId.Contains(r.brandId)).ToList();
                ViewBag.MouthText = DateTime.ParseExact(model.dateReport, "MM-yyyy", null).ToString("MMM yyyy");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return PartialView(rep);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult repListViewExportExcel(string gridHtml)
        {

            var data = Encoding.UTF8.GetBytes(gridHtml);
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();

            return File(result, "application/vnd.ms-excel", "DetailReport.xls");
        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult repListViewExportPDF(string gridHtml)
        {
            List<Attachment> file = new List<Attachment>();
            try
            {
                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rooPdftURL"], "");
                gridHtml = gridHtml.Replace("<br>", "<br/>");
                file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), "");

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
            return File(file[0].ContentStream, "application/pdf", "reportDetailPDF.pdf");

        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult repReportDetailApprove(string gridHtml, string gridOS, string gridEst, string gridWA, string gridSO, string customerId, string productTypeId, string startDate, string endDate, string typeForm)
        {
            var result = new AjaxResult();
            try
            {
                gridHtml = gridHtml.Replace("<br>", "<br/>");
                RepDetailModel.actFormRepDetails model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"];
                model.actFormRepDetailLists = model.actFormRepDetailLists.Where(r => r.delFlag == false).ToList();
                string actRepDetailId = ApproveRepDetailAppCode.insertActivityRepDetail(customerId, productTypeId, startDate, endDate, model, typeForm);
                if (ApproveRepDetailAppCode.insertApproveForReportDetail(customerId, productTypeId, actRepDetailId, typeForm) > 0)
                {
                    RepDetailAppCode.genFilePDFBrandGroup(actRepDetailId, gridHtml, gridOS, gridEst, gridWA, gridSO);
                    EmailAppCodes.sendApprove(actRepDetailId, AppCode.ApproveType.Report_Detail, false);
                    Session["ActFormRepDetail"] = null;
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Message = AppCode.StrMessFail;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }

            return Json(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult reGenReportDetail(string gridHtml, string actRepDetailId)
        {
            var result = new AjaxResult();
            try
            {
                gridHtml = gridHtml.Replace("<br>", "<br/>");
                RepDetailModel.actFormRepDetails model = (RepDetailModel.actFormRepDetails)Session["ActFormRepDetail"];


                RepDetailAppCode.reGenPDFReportDetail(actRepDetailId, gridHtml);
                Session["ActFormRepDetail"] = null;
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }

            return Json(result);
        }
        /// <summary>
        /// for approve page
        /// </summary>
        /// <param name="actId"></param>
        /// <returns></returns>
        public ActionResult repPreviewListView(string actId)
        {
            RepDetailModel.actFormRepDetails model = new RepDetailModel.actFormRepDetails();
            try
            {
                model = RepDetailAppCode.getRepDetailReportByCreateDateAndStatusId(actId);
                model.flowList = ApproveFlowAppCode.getFlowByActFormId(actId);
                Session["ActFormRepDetail"] = model;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }


            return RedirectToAction("repPreviewForApproveDetail", new { startDate = model.actFormRepDetailLists.Count > 0 ? model.actFormRepDetailLists[0].createdDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy") });
        }

        /// <summary>
        /// for approve page
        /// </summary>
        /// <param name="gridHtml"></param>
        /// <param name="actId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult repDetailGenPDF(string gridHtml, string actId)
        {
            var result = new AjaxResult();
            try
            {
                result.Success = true;
                if ((int)AppCode.ApproveStatus.ไม่อนุมัติ == (int)RepDetailAppCode.getRepDetailStatus(actId))
                {
                    EmailAppCodes.sendRejectRepDetail();
                }
                else
                {
                    var rootPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootRepDetailPdftURL"], actId));
                    List<Attachment> file = AppCode.genPdfFile(gridHtml, new Document(PageSize.A3.Rotate(), 25, 10, 10, 10), rootPath);
                    EmailAppCodes.sendApprove(actId, AppCode.ApproveType.Report_Detail, false);
                    Session["ActFormRepDetail"] = null;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }

            return Json(result);
        }

        public ActionResult genImageStream(byte[] p_image, string empId)
        {
            return File(p_image, "image/png");
        }

        public ActionResult mockupData(string typeForm)
        {
            ViewBag.TypeForm = typeForm;
            SearchActivityModels models = SearchAppCode.getMasterDataForSearchForDetailReport(typeForm);
            models.showUIModel = new searchParameterFilterModel();


            if (typeForm == Activity_Model.activityType.MT.ToString() || typeForm == Activity_Model.activityType.SetPrice.ToString())
            {
                models.customerslist = QueryGetAllCustomers.getCustomersMT();
            }
            else
            {
                models.customerslist = QueryGetAllCustomers.getAllRegion();
            }




            models.approveStatusList.Add(new ApproveModel.approveStatus()
            {
                id = "7",
                nameTH = "เพิ่มเติม",
                nameEN = "เพิ่มเติม",
            });
            return View(models);
        }

    }
}