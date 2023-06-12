using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebLibrary;


namespace eActForm.Controllers  //update 21-04-2020
{
    [LoginExpire]
    public class BudgetInvoiceAppCode
    {

        public static List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel> BudgetInvoiceListForCreatePDF(string act_form_id)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetInvoiceListForCreatePDF"
                    , new SqlParameter("@act_form_id", act_form_id)
                    );
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Bud_Invoice_Document_Model.BudgetInvoiceModel()
                             {
                                 id = d["imageId"].ToString(),

                                 invoiceNo = (d["invoiceNo"].ToString() == null || d["invoiceNo"] is DBNull) ? "" : d["invoiceNo"].ToString(),
                                 imageType = d["imageType"].ToString(),
                                 _image = (d["_image"] == null || d["_image"] is DBNull) ? new byte[0] : (byte[])d["_image"],
                                 _fileName = d["_fileName"].ToString(),
                                 extension = d["extension"].ToString(),
                                 remark = d["remark"].ToString(),

                                 companyId = d["companyId"].ToString(),
                                 regionId = d["regionId"].ToString(),
                                 customerId = d["customerId"].ToString(),

                                 company = d["company"].ToString(),
                                 regionName = d["regionName"].ToString(),
                                 customerName = d["cusNameTH"].ToString(),

                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetInvoice => " + ex.Message);
                return new List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel>();
            }
        }

        public static List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel> BudgetInvoiceDetail(string imageId, string imageInvoiceNo, string budgetApproveId, string activityNo, string createdByUserId, string company, string customerId, string beginDateyyyymmdd, string endDateyyyymmdd)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetInvoiceList"
                    , new SqlParameter("@imageId", imageId)
                    , new SqlParameter("@imageInvoiceNo", imageInvoiceNo)
                    , new SqlParameter("@budgetApproveId", budgetApproveId)
                    , new SqlParameter("@activityNo", activityNo)
                    , new SqlParameter("@createdByUserId", createdByUserId)
                    , new SqlParameter("@company", company)
                    , new SqlParameter("@customerId", customerId)
                    //, new SqlParameter("@beginDateyyyymmdd", null)
                    //, new SqlParameter("@endDateyyyymmdd", null)
                    , new SqlParameter("@beginDateyyyymmdd", beginDateyyyymmdd)
                    , new SqlParameter("@endDateyyyymmdd", endDateyyyymmdd)
                    );
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Bud_Invoice_Document_Model.BudgetInvoiceModel()
                             {
                                 id = d["imageId"].ToString(),

                                 count_budgetActivityId = int.Parse(d["count_budgetActivityId"].ToString()),
                                 count_activityNo = int.Parse(d["count_activityNo"].ToString()),
                                 count_budgetApproveId = int.Parse(d["count_budgetApproveId"].ToString()),

                                 invoiceNo = (d["invoiceNo"].ToString() == null || d["invoiceNo"] is DBNull) ? "" : d["invoiceNo"].ToString(),
                                 imageType = d["imageType"].ToString(),
                                 _image = (d["_image"] == null || d["_image"] is DBNull) ? new byte[0] : (byte[])d["_image"],
                                 _fileName = d["_fileName"].ToString(),
                                 extension = d["extension"].ToString(),
                                 remark = d["remark"].ToString(),

                                 companyId = d["companyId"].ToString(),
                                 regionId = d["regionId"].ToString(),
                                 customerId = d["customerId"].ToString(),

                                 company = d["company"].ToString(),
                                 regionName = d["regionName"].ToString(),
                                 customerName = d["cusNameTH"].ToString(),

                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),

                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetInvoice => " + ex.Message);
                return new List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel>();
            }
        }

        public static int BudgetInvoiceInsert(TB_Bud_Invoice_Document_Model.BudgetInvoiceModel model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetInvoiceInsert"
                    , new SqlParameter[] {new SqlParameter("@imageType",model.imageType)
                    ,new SqlParameter("@image",model._image)
                    ,new SqlParameter("@fileName",model._fileName)
                    ,new SqlParameter("@extension",model.extension)
                    ,new SqlParameter("@remark",model.remark)
                    ,new SqlParameter("@company",model.company)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)

                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertBudgetInvoice");
            }

            return result;
        }

        public static int BudgetInvoiceUpdate(TB_Bud_Invoice_Document_Model.BudgetInvoiceModel model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetInvoiceUpdate"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@invoiceNo",model.invoiceNo)
                    ,new SqlParameter("@customerId",model.customerId)
                    ,new SqlParameter("@remark",model.remark)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)

                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateImageBudget");
            }

            return result;
        }

        public static int BudgetInvoiceDelete(string fileId, string empId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetInvoiceDelete"
                    , new SqlParameter[] {new SqlParameter("@Id",fileId)
                    ,new SqlParameter("@updatedByUserId",empId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteImageBudgetById");
            }

            return result;
        }

    }

    [LoginExpire]
    public class BudgetController : Controller
    {

        public ActionResult activityIndex(string typeForm)
        {
            SearchBudgetActivityModels models = BudgetReportController.getMasterDataForSearch(typeForm);
            return View(models);
        }

        public ActionResult searchBudgetActivityForm(string typeForm)
        {
            DateTime act_createdDateStart = DateTime.Now.AddYears(-10);
            DateTime act_createdDateEnd = DateTime.Now.AddYears(2);
            string act_budgetStatusIdin = null;
            string actYear = null;

            Budget_Activity_Model budget_activity = new Budget_Activity_Model();

            try
            {
                #region filter
                actYear = Request.Form["ddlActYear"];

                if (Request.Form["ddlFormType"] != "" && Request.Form["ddlFormType"] != "Select All")
                {
                    typeForm = Request.Form["ddlFormType"];
                }

                if (Request.Form["ddlBudgetStatusId"] != "" && Request.Form["ddlBudgetStatusId"] != "Select All")
                {
                    act_budgetStatusIdin = Request.Form["ddlBudgetStatusId"];
                }
                #endregion

                budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", null, null, null, typeForm, act_createdDateStart, act_createdDateEnd, act_budgetStatusIdin, actYear).ToList();
                TempData["searchBudgetActivityForm"] = budget_activity;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("searchBudgetActivityForm --> " + ex.Message);
            }

            return RedirectToAction("activityList");
        }

        public ActionResult activityList(string typeForm)
        {
            Budget_Activity_Model models = new Budget_Activity_Model();
            DateTime act_createdDateStart = DateTime.Now.AddYears(-10);
            DateTime act_createdDateEnd = DateTime.Now.AddYears(2);
            string act_budgetStatusIdin = null;
            string act_year = null;
            try
            {
                if (TempData["searchBudgetActivityForm"] != null)
                {
                    models = (Budget_Activity_Model)TempData["searchBudgetActivityForm"];
                }
                else
                {
                    if (act_year == null)
                    {
                        act_year = (DateTime.Now.AddMonths(3).Year + 543).ToString();
                    }
                    if (act_budgetStatusIdin == null)
                    {
                        act_budgetStatusIdin = "2";
                    }
                    models.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", null, null, null, typeForm, act_createdDateStart, act_createdDateEnd, act_budgetStatusIdin, act_year).ToList();
                }
                TempData["searchBudgetActivityForm"] = null;
                return PartialView(models);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("activityList --> " + ex.Message);
            }
            return PartialView(models);
        }





        public ActionResult activityProduct(string activityId)
        {
            Budget_Activity_Model budget_activity = new Budget_Activity_Model();
            string act_year = null;

            if (activityId == null) { activityId = Session["activityId"].ToString(); }

            if (activityId == null) return RedirectToAction("activityList", "Budget");
            else
            {
                try
                {
                    budget_activity.Budget_Activity_list = QueryGetBudgetActivity.getBudgetActivityList("3", activityId, null, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null, act_year).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(budget_activity);
        }
        public ActionResult activityProductList(string activityId)
        {

            Session["activityId"] = activityId;
            Budget_Activity_Model budget_activity = new Budget_Activity_Model();
            try
            {
                budget_activity.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityList(null, activityId, null, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null, null).FirstOrDefault();
                budget_activity.Budget_Activity_Product_list = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, null);
                budget_activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
                budget_activity.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PartialView(budget_activity);
        }
        public PartialViewResult activityProductInvoiceList(string activityId, string activityOfEstimateId)
        {
            Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
            budget_activity_model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityList(null, activityId, null, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null, null).FirstOrDefault();
            budget_activity_model.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, null);
            budget_activity_model.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault();

            return PartialView(budget_activity_model);
        }
        public PartialViewResult activityProductInvoiceEdit(string activityId, string activityOfEstimateId, string invoiceId)
        {
            Budget_Activity_Model Budget_Activity = new Budget_Activity_Model();

            Budget_Activity.Budget_Activity_Product = QueryGetBudgetActivity.getBudgetActivityProduct(activityId, activityOfEstimateId).FirstOrDefault();
            Budget_Activity.Budget_Activity_Ststus_list = QueryGetBudgetActivity.getBudgetActivityStatus();
            Budget_Activity.Budget_Count_Wait_Approve = QueryGetBudgetActivity.getBudgetActivityWaitApprove(activityId).FirstOrDefault();
            Budget_Activity.Budget_Activity_Invoice_list = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, null);

            if (!string.IsNullOrEmpty(invoiceId))
            {// get invoice for edit 
                Budget_Activity.Budget_Activity_Invoice = QueryGetBudgetActivity.getBudgetActivityInvoice(activityId, activityOfEstimateId, invoiceId).FirstOrDefault();
            }
            return PartialView(Budget_Activity);
        }







        public PartialViewResult previewBudgetInvoice(string activityId)
        {
            Session["activityId"] = activityId;

            Session["activityId"] = activityId;

            Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();

            Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityDetail(null, activityId, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null).FirstOrDefault();
            Budget_Model.Budget_Activity_Last_Approve = QueryGetBudgetActivity.getBudgetActivityLastApprove(activityId).FirstOrDefault();
            Budget_Model.Budget_Invoice_list = BudgetInvoiceAppCode.BudgetInvoiceListForCreatePDF(Budget_Model.Budget_Activity.act_form_id);
            Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(activityId, null);

            return PartialView(Budget_Model);
        }

        public JsonResult submitInvoice(Budget_Activity_Model.Budget_Activity_Invoice_Att budgetInvoiceModel)
        {
            var resultAjax = new AjaxResult();
            //int count_invo = 0;
            try
            {

                if (budgetInvoiceModel.invoiceId == null)
                {
                    // insert invoice
                    int countSuccess = BudgetFormCommandHandler.insertInvoiceProduct(budgetInvoiceModel);
                }
                else
                {
                    //update invoice
                    int countSuccess = BudgetFormCommandHandler.updateInvoiceProduct(budgetInvoiceModel);
                }

                if (string.IsNullOrEmpty(budgetInvoiceModel.invoiceNo) == false)
                {
                    TB_Bud_Invoice_Document_Model getBudImageModel = new TB_Bud_Invoice_Document_Model();
                    getBudImageModel.BudgetInvoiceList = BudgetInvoiceAppCode.BudgetInvoiceDetail(null, budgetInvoiceModel.invoiceNo, null, null, null, null, budgetInvoiceModel.actCustomerId, null, null);

                    if (getBudImageModel.BudgetInvoiceList.Any()) // True, the list is not empty
                    {
                        if (getBudImageModel.BudgetInvoiceList.ElementAtOrDefault(0).count_activityNo > 1)
                        {
                            resultAjax.Code = 2;
                            resultAjax.Message = getBudImageModel.BudgetInvoiceList.ElementAtOrDefault(0).invoiceNo;
                        }
                    }
                }
                else
                {
                    resultAjax.Code = 0;
                    resultAjax.Message = null;
                }
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax, "text/plain");
        }

        public JsonResult budgetProductInvoiceDelete(string actId, string estId, string invId, string delType)
        {
            var result = new AjaxResult();
            try
            {
                int countSuccess = BudgetFormCommandHandler.commBudgetProductInvoiceDelete(actId, estId, invId, delType);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteBudgetApproveByActNo(string actNo, string actId)
        {
            var result = new AjaxResult();
            try
            {
                int countSuccess = BudgetFormCommandHandler.deleteBudgetApproveByActNo(actNo);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getBudgetInvoiceFillter(string imgInvoiceNo, string companyEN, string customerId)
        {
            List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel> imgInvoiceList = new List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel>();
            try
            {
                imgInvoiceList = BudgetInvoiceAppCode.BudgetInvoiceDetail(null, null, null, null, null, companyEN, customerId, null, null)
                    .Where(x => x.invoiceNo.Contains(imgInvoiceNo))
                    .ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetInvoiceFillter => " + ex.Message);
            }

            return Json(imgInvoiceList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRegionInvoice(string nameEN)
        {
            List<TB_Act_Region_Model> regionList = new List<TB_Act_Region_Model>();
            try
            {
                regionList = QueryGetAllRegion.getAllRegion().Where(x => x.name.Contains(nameEN)).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getRegionInvoice => " + ex.Message);
            }

            return Json(regionList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult manageInvoiceIndex(String companyTH)
        {
            return View();
        }

        public ActionResult manageInvoiceListSearch()
        {
            TB_Bud_Invoice_Document_Model models = new TB_Bud_Invoice_Document_Model();
            try
            {
                #region filter

                string companyEN = Request.Form["var_companyEN"];

                DateTime date_inv_start = DateTime.ParseExact(Request.Form["startDate"].Trim(), ConfigurationManager.AppSettings["formatDateUse"], null);
                DateTime date_inv_end = DateTime.ParseExact(Request.Form["endDate"].Trim(), ConfigurationManager.AppSettings["formatDateUse"], null);
                
                string inv_createdDateStart = date_inv_start.ToString("yyyyMMdd");
                string inv_createdDateEnd = date_inv_end.ToString("yyyyMMdd");

                #endregion

                models.BudgetInvoiceList = BudgetInvoiceAppCode.BudgetInvoiceDetail(null, null, null, null, null, companyEN, null, inv_createdDateStart, inv_createdDateEnd);
                TempData["searchBudgetInvoiceList"] = models;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("searchBudgetInvoiceList --> " + ex.Message);
            }

            return RedirectToAction("manageInvoiceList");
        }

        public ActionResult manageInvoiceList(string companyEN)
        {

            TB_Bud_Invoice_Document_Model models = new TB_Bud_Invoice_Document_Model();

            DateTime act_createdDateStart = DateTime.Now.AddDays(-15);
            DateTime act_createdDateEnd = DateTime.Now;

            string inv_createdDateStart = act_createdDateStart.ToString("yyyyMMdd");
            string inv_createdDateEnd = act_createdDateEnd.ToString("yyyyMMdd");

            try
            {
                if (TempData["searchBudgetInvoiceList"] != null)
                {
                    models = (TB_Bud_Invoice_Document_Model)TempData["searchBudgetInvoiceList"];
                }
                else
                {
                    models.BudgetInvoiceList = BudgetInvoiceAppCode.BudgetInvoiceDetail(null, null, null, null, null, companyEN, null, inv_createdDateStart, inv_createdDateEnd);
                }
                TempData["searchBudgetInvoiceList"] = null;

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("activityList --> " + ex.Message);
            }
            return PartialView(models);
        }

        public JsonResult getCustomerInvoice(string customerTH, string companyEN, string regionId)
        {
            var result = new AjaxResult();
            List<TB_Act_Customers_Model.Customers_Model> customerList = new List<TB_Act_Customers_Model.Customers_Model>();
            try
            {
                if (companyEN == "MT")
                {
                    customerList = QueryGetAllCustomers.getCustomersMT().Where(x => x.cusNameTH.Contains(customerTH)).ToList();
                }
                else
                {
                    customerList = QueryGetAllCustomers.getCustomersOMT().Where(x => x.regionId == regionId).ToList();

                    var resultData = new
                    {
                        getCustomerName = customerList.Select(x => new
                        {
                            Value = x.id,
                            Text = x.cusNameTH
                        }).ToList(),
                    };
                    result.Data = resultData;

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getInvoiceCustomer => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult manageInvoiceView(string imageId)
        {
            TB_Bud_Invoice_Document_Model budgetImageModel = new TB_Bud_Invoice_Document_Model();
            try
            {

                budgetImageModel.BudgetInvoice = BudgetInvoiceAppCode.BudgetInvoiceDetail(imageId, null, null, null, null, null, null, null, null).FirstOrDefault();
                budgetImageModel.RegionList = QueryGetAllRegion.getAllRegion().ToList();

                if (UtilsAppCode.Session.User.regionId != "")
                {
                    budgetImageModel.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.id == UtilsAppCode.Session.User.regionId).ToList();
                }
                else
                {
                    budgetImageModel.regionGroupList = QueryGetAllRegion.getAllRegion().Where(x => x.condition == budgetImageModel.BudgetInvoice.company).ToList();
                }

                if (budgetImageModel.BudgetInvoice.company == "MT")
                {
                    budgetImageModel.CustomerList = QueryGetAllCustomers.getCustomersMT().ToList();
                }
                else
                {
                    budgetImageModel.CustomerList = QueryGetAllCustomers.getCustomersOMT().Where(x => x.regionId == budgetImageModel.BudgetInvoice.regionId).ToList();
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("manageInvoiceView => " + ex.Message);
            }
            return PartialView(budgetImageModel);
        }


        [HttpPost]
        public ActionResult manageInvoiceUpload(String company)
        {
            var result = new AjaxResult();
            try
            {
                byte[] binData = null;
                TB_Bud_Invoice_Document_Model.BudgetInvoiceModel imageFormModel = new TB_Bud_Invoice_Document_Model.BudgetInvoiceModel();
                foreach (string UploadedImage in Request.Files)
                {
                    HttpPostedFileBase httpPostedFile = Request.Files[UploadedImage];

                    string resultFilePath = "";
                    string extension = Path.GetExtension(httpPostedFile.FileName);
                    string strDateTime = DateTime.Now.ToString("ddMMyyHHmmssff");
                    int tmpFileNameIndex = httpPostedFile.FileName.LastIndexOf('.');
                    int indexGetFileName = 10; // limit file name 10 char

                    if (tmpFileNameIndex < 10) { indexGetFileName = tmpFileNameIndex; }

                    var _fileName = Path.GetFileName(httpPostedFile.FileName.Substring(0, indexGetFileName)) + "_" + strDateTime + extension;
                    string UploadDirectory = Server.MapPath(string.Format(System.Configuration.ConfigurationManager.AppSettings["rootUploadfilesBudget"].ToString(), _fileName));

                    if (extension == ".pdf")
                    {
                        resultFilePath = UploadDirectory;
                        BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
                        binData = b.ReadBytes(0);
                        httpPostedFile.SaveAs(resultFilePath);
                    }
                    else
                    {
                        resultFilePath = UploadDirectory;
                        BinaryReader b = new BinaryReader(httpPostedFile.InputStream);
                        binData = b.ReadBytes(0);
                        httpPostedFile.SaveAs(resultFilePath);

                        // convert image to pdf
                        string UploadDirectory_pdf = "";
                        UploadDirectory_pdf = UploadDirectory.Replace(extension, ".pdf");
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(UploadDirectory);
                        using (FileStream fs = new FileStream(UploadDirectory_pdf, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            //using (Document doc = new Document(PageSize.A4, 10, 10, 10, 10))
                            using (Document doc = new Document(PageSize.A4, 0, 0, 0, 0))
                            {
                                using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                                {
                                    writer.CloseStream = false;
                                    doc.Open();
                                    image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                                    image.SetAbsolutePosition((PageSize.A4.Width - image.ScaledWidth) / 2, (PageSize.A4.Height - image.ScaledHeight));
                                    writer.DirectContent.AddImage(image);
                                    doc.Close();
                                }
                            }
                        }
                    }

                    imageFormModel._image = binData;
                    imageFormModel.imageType = "UploadFile";
                    imageFormModel._fileName = _fileName.ToLower().Replace(extension, ".pdf");
                    imageFormModel.extension = ".pdf";
                    imageFormModel.remark = "";

                    imageFormModel.company = "";
                    if (company == "MT") { imageFormModel.company = "5600"; };
                    if (company == "OMT") { imageFormModel.company = "5601"; };

                    imageFormModel.delFlag = false;
                    imageFormModel.createdByUserId = UtilsAppCode.Session.User.empId;
                    imageFormModel.createdDate = DateTime.Now;
                    imageFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                    imageFormModel.updatedDate = DateTime.Now;

                    int resultImg = BudgetInvoiceAppCode.BudgetInvoiceInsert(imageFormModel);
                }

                //manageInvoiceList(null);
                //result.ActivityId = Session["activityId"].ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
                ExceptionManager.WriteError("manageInvoiceUpload => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult manageInvoiceDelete(string id)
        {
            var result = new AjaxResult();
            var empId = UtilsAppCode.Session.User.empId;

            int resultImg = BudgetInvoiceAppCode.BudgetInvoiceDelete(id, empId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult manageInvoiceEditSubmit(string id, string invoiceNo, string remark, string companyEN, string regionId, string customerId)
        {
            var resultAjax = new AjaxResult();
            try
            {
                TB_Bud_Invoice_Document_Model.BudgetInvoiceModel budgetInvoiceModel = new TB_Bud_Invoice_Document_Model.BudgetInvoiceModel();
                budgetInvoiceModel.id = id;
                budgetInvoiceModel.invoiceNo = invoiceNo;
                budgetInvoiceModel.remark = remark;
                budgetInvoiceModel.company = companyEN;

                budgetInvoiceModel.regionId = regionId;
                budgetInvoiceModel.customerId = customerId;

                budgetInvoiceModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                budgetInvoiceModel.updatedDate = DateTime.Now;

                TB_Bud_Invoice_Document_Model getBudImageModel = new TB_Bud_Invoice_Document_Model();
                getBudImageModel.BudgetInvoiceList = BudgetInvoiceAppCode.BudgetInvoiceDetail(null, budgetInvoiceModel.invoiceNo, null, null, null, budgetInvoiceModel.company, customerId, null, null);
                if (getBudImageModel.BudgetInvoiceList.Any())
                {
                    resultAjax.Code = 2;
                    resultAjax.Message = getBudImageModel.BudgetInvoiceList.ElementAtOrDefault(0).invoiceNo;

                }
                else
                {
                    //update image invoice
                    int countSuccess = BudgetInvoiceAppCode.BudgetInvoiceUpdate(budgetInvoiceModel);

                    resultAjax.Code = 0;
                    resultAjax.Message = null;
                }

                //resultAjax.ActivityId = Session["activityId"].ToString();
                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax, "text/plain");
        }

    }

    [LoginExpire]
    public class BudgetViewerController : Controller
    {
        // GET: BudgetViewer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult activityPDFView(string budgetApproveId)
        {

            //var var_budgetActivityId = BudgetApproveListController.getApproveBudgetId(budgetActivityId);
            TempData["budgetApproveId"] = budgetApproveId;
            ViewBag.budgetApproveId = budgetApproveId;
            return PartialView();
        }

        public PartialViewResult regenBudgetApprovePdf(string budgetApproveId, string activityId)
        {
            Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();
            Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(null, budgetApproveId);
            Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityDetail(null, activityId, null, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null).FirstOrDefault();

            Budget_Model.Budget_Approve_detail_list = QueryGetBudgetApprove.getBudgetApproveId(budgetApproveId);
            return PartialView(Budget_Model);

            //budgetApproveId
            //activityId

        }

        public ActionResult getPdfBudget(string budgetApproveId)
        {
            string rootPath = "";
            FileStream fileStream = null;
            FileStreamResult fsResult = null;

            try
            {
                rootPath = ConfigurationManager.AppSettings["rootBudgetPdftURL"];
                if (!System.IO.File.Exists(Server.MapPath(string.Format(rootPath, budgetApproveId))))
                {
                    budgetApproveId = "fileNotFound";
                }

                fileStream = new FileStream(Server.MapPath(string.Format(rootPath, budgetApproveId)),
                                         FileMode.Open,
                                         FileAccess.Read
                                       );

                fsResult = new FileStreamResult(fileStream, "application/pdf");

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getPdfBudget >>" + ex.Message);
            }


            return fsResult;
        }

        public ActionResult getInvoicePdfBudget(string fileName)
        {
            string rootPath = "";
            FileStream fileStream = null;
            FileStreamResult fsResult = null;

            try
            {
                rootPath = ConfigurationManager.AppSettings["rootUploadfilesBudget"];
                if (!System.IO.File.Exists(Server.MapPath(string.Format(rootPath, fileName))))
                {
                    fileName = "fileNotFound.pdf";
                }

                fileStream = new FileStream(Server.MapPath(string.Format(rootPath, fileName)),
                                                     FileMode.Open,
                                                     FileAccess.Read
                                                   );
                fsResult = new FileStreamResult(fileStream, "application/pdf");

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getInvoicePdfBudget >>" + ex.Message);
            }

            return fsResult;
        }

        //---------------------------------------------------------------------------------------------------

        public ActionResult invoicePDFView(string fileName)
        {

            TempData["fileName"] = fileName;
            ViewBag.fileName = fileName;
            return PartialView();
        }


    }

    [LoginExpire]
    public class BudgetApproveListController : Controller
    {
       
        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult searchActForm()
        {

            string count = Request.Form.AllKeys.Count().ToString();

            DateTime startDate = Request["startDate"] == null ? DateTime.Now.AddDays(-15) : DateTime.ParseExact(Request.Form["startDate"], "dd/MM/yyyy", null);
            DateTime endDate = Request["endDate"] == null ? DateTime.Now : DateTime.ParseExact(Request.Form["endDate"], "dd/MM/yyyy", null);

            Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
            model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveFormLists"];

            if (!string.IsNullOrEmpty(Request.Form["txtActivityNo"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlStatus"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.statusId == Request.Form["ddlStatus"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlCustomer"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.customerId == Request.Form["ddlCustomer"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlTheme"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.theme == Request.Form["ddlTheme"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlProductType"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.productTypeId == Request.Form["ddlProductType"]).ToList();
            }

            if (!string.IsNullOrEmpty(Request.Form["ddlProductGrp"]))
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.productGroupid == Request.Form["ddlProductGrp"]).ToList();
            }

            TempData["ApproveSearchResult"] = model.budgetFormLists;
            return RedirectToAction("budgetApproveList");

        }

        public ActionResult budgetApproveList()
        {

            Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
            if (TempData["ApproveSearchResult"] == null)
            {
                model = new Budget_Approve_Detail_Model.budgetForms();
                model.budgetFormLists = QueryGetBudgetApprove.getApproveListsByEmpId(UtilsAppCode.Session.User.empId);
                TempData["ApproveFormLists"] = model.budgetFormLists;
                model.budgetFormLists = BudgetApproveController.getFilterFormByStatusId(model.budgetFormLists, (int)AppCode.ApproveStatus.รออนุมัติ);
            }
            else
            {
                model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["ApproveSearchResult"];
            }
            return PartialView(model);
        }

        public static List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel> BudgetApproveInvoiceList(string budgetApproveId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveInvoiceList"
                    , new SqlParameter("@budgetApproveId", budgetApproveId)
                    );
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Bud_Invoice_Document_Model.BudgetInvoiceModel()
                             {
                                 id = d["imageId"].ToString(),

                                 invoiceNo = (d["invoiceNo"].ToString() == null || d["invoiceNo"] is DBNull) ? "" : d["invoiceNo"].ToString(),
                                 imageType = d["imageType"].ToString(),
                                 _image = (d["_image"] == null || d["_image"] is DBNull) ? new byte[0] : (byte[])d["_image"],
                                 _fileName = d["_fileName"].ToString(),
                                 extension = d["extension"].ToString(),
                                 remark = d["remark"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetInvoiceByApproveId => " + ex.Message);
                return new List<TB_Bud_Invoice_Document_Model.BudgetInvoiceModel>();
            }
        }
    }

    [LoginExpire]
    public class BudgetMyDocController : Controller
    {
        // GET: BudgetMyDoc

        public ActionResult Index()
        {
            SearchActivityModels models = SearchAppCode.getMasterDataForSearch();
            return View(models);
        }

        public ActionResult searchBudgetForm()
        {
            string count = Request.Form.AllKeys.Count().ToString();
            Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
            model = new Budget_Approve_Detail_Model.budgetForms();

            string companyEN = Session["var_companyEN"].ToString();
            DateTime startDate = DateTime.ParseExact(Request.Form["startDate"].Trim(), "MM/dd/yyyy", null);
            DateTime endDate = DateTime.ParseExact(Request.Form["endDate"].Trim(), "MM/dd/yyyy", null);

            if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
            {
                model.budgetFormLists = getBudgetListsByEmpId(null, companyEN, startDate, endDate);
            }
            else
            {
                model.budgetFormLists = getBudgetListsByEmpId(UtilsAppCode.Session.User.empId, companyEN, startDate, endDate);
            }

            if (Request.Form["txtActivityNo"] != "")
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();
            }

            if (Request.Form["ddlStatus"] != "")
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.statusId == Request.Form["ddlStatus"]).ToList();
            }

            if (Request.Form["ddlCustomer"] != "")
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.customerId == Request.Form["ddlCustomer"]).ToList();
            }

            if (Request.Form["ddlTheme"] != "")
            {
                model.budgetFormLists = model.budgetFormLists.Where(r => r.themeId == Request.Form["ddlTheme"]).ToList();
            }

            TempData["SearchDataModelBudget"] = model.budgetFormLists;
            return RedirectToAction("myDocBudget");
        }

        public ActionResult myDocBudget()
        {
            Budget_Approve_Detail_Model.budgetForms model = new Budget_Approve_Detail_Model.budgetForms();
            model = new Budget_Approve_Detail_Model.budgetForms();

            string companyEN = Session["var_companyEN"].ToString();
            DateTime startDate = DateTime.Now.AddDays(-30);
            DateTime endDate = DateTime.Now.AddDays(1);

            if (TempData["SearchDataModelBudget"] != null)
            {
                model.budgetFormLists = (List<Budget_Approve_Detail_Model.budgetForm>)TempData["SearchDataModelBudget"];
            }
            else
            {
                if (UtilsAppCode.Session.User.isAdmin || UtilsAppCode.Session.User.isSuperAdmin)
                {
                    model.budgetFormLists = getBudgetListsByEmpId(null, companyEN, startDate, endDate);
                }
                else
                {
                    model.budgetFormLists = getBudgetListsByEmpId(UtilsAppCode.Session.User.empId, companyEN, startDate, endDate);
                }
            }
            return PartialView(model);
        }

        public ActionResult myDocBudgetEmpApproveList(string actId)
        {
            var result = new AjaxResult();
            ApproveModel.approveModels models = ApproveAppCode.getApproveByActFormId(actId);
            return PartialView(models);
        }

        public static List<Budget_Approve_Detail_Model.budgetForm> getBudgetListsByEmpId(string empId, string companyEN, DateTime createdDateStart, DateTime createdDateEnd)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetDocumentList"
                    , new SqlParameter[] {
                    new SqlParameter("@empId", empId),
                    new SqlParameter("@companyEN", companyEN),
                    new SqlParameter("@createdDateStart", createdDateStart),
                    new SqlParameter("@createdDateEnd", createdDateEnd)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new Budget_Approve_Detail_Model.budgetForm()
                             {
                                 statusId = dr["statusId"].ToString(),
                                 statusName = dr["statusName"].ToString(),
                                 activityId = dr["activityId"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),

                                 budgetApproveId = dr["budgetApproveId"].ToString(),
                                 documentDate = dr["documentDate"] is DBNull ? null : (DateTime?)dr["documentDate"],

                                 reference = dr["reference"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 channelName = dr["channelName"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 productTypeNameEN = dr["productTypeNameEN"].ToString(),

                                 cusShortName = dr["cusShortName"].ToString(),
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 productCategory = dr["productCateText"].ToString(),
                                 productGroup = dr["productGroupId"].ToString(),
                                 productGroupName = dr["productGroupName"].ToString(),

                                 activityPeriodSt = dr["activityPeriodSt"] is DBNull ? null : (DateTime?)dr["activityPeriodSt"],
                                 activityPeriodEnd = dr["activityPeriodEnd"] is DBNull ? null : (DateTime?)dr["activityPeriodEnd"],
                                 costPeriodSt = dr["costPeriodSt"] is DBNull ? null : (DateTime?)dr["costPeriodSt"],
                                 costPeriodEnd = dr["costPeriodEnd"] is DBNull ? null : (DateTime?)dr["costPeriodEnd"],
                                 activityName = dr["activityName"].ToString(),

                                 themeId = dr["themeId"].ToString(),
                                 theme = dr["theme"].ToString(),
                                 objective = dr["objective"].ToString(),
                                 trade = dr["trade"].ToString(),
                                 activityDetail = dr["activityDetail"].ToString(),

                                 budgetActivityId = dr["budgetActivityId"].ToString(),
                                 approveId = dr["approveId"].ToString(),

                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString(),

                                 normalCost = dr["normalCost"] is DBNull ? 0 : (decimal?)dr["normalCost"],
                                 themeCost = dr["themeCost"] is DBNull ? 0 : (decimal?)dr["themeCost"],
                                 totalCost = dr["totalCost"] is DBNull ? 0 : (decimal?)dr["totalCost"],
                                 totalInvoiceApproveBath = dr["totalInvoiceApproveBath"] is DBNull ? 0 : (decimal?)dr["totalInvoiceApproveBath"]

                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetListsByEmpId >> " + ex.Message);
                return new List<Budget_Approve_Detail_Model.budgetForm>();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult genPdfApprove(string GridHtml, string budgetApproveId)
        {
            var resultAjax = new AjaxResult();
            try
            {

                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId + "_");
                GridHtml = GridHtml.Replace("<br>", "<br/>");
                AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

                //del signature file
                bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)));
                if (folderExists)
                    Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)), true);


                TB_Bud_Invoice_Document_Model getBudgetImageModel = new TB_Bud_Invoice_Document_Model();
                getBudgetImageModel.BudgetInvoiceList = BudgetApproveListController.BudgetApproveInvoiceList(budgetApproveId);

                string[] pathFile = new string[getBudgetImageModel.BudgetInvoiceList.Count + 1];
                pathFile[0] = Server.MapPath(rootPathInsert);

                if (getBudgetImageModel.BudgetInvoiceList.Any())
                {
                    int i = 1;
                    foreach (var item in getBudgetImageModel.BudgetInvoiceList)
                    {
                        if (System.IO.File.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName))))
                        {
                            pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName));
                        }
                        else
                        {
                            pathFile = pathFile.Where((val, idx) => idx != i).ToArray();
                        }
                        i++;
                    }
                }

                var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId));
                var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile, budgetApproveId);

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

    [LoginExpire]
    public class BudgetApproveController : Controller
    {

        // GET: Approve
        public ActionResult Index(string budgetApproveId)
        {
            if (budgetApproveId == null) return RedirectToAction("index", "BudgetApproveList");
            else
            {
                ApproveModel.approveModels models = getApproveByBudgetApproveId(budgetApproveId);
                models.approveStatusLists = ApproveAppCode.getApproveStatus(AppCode.StatusType.app).Where(x => x.id == "3" || x.id == "5").ToList();
                return View(models);
            }
        }

        public ActionResult approveLists(ApproveModel.approveModels models)
        {
            return PartialView(models);
        }

        public ActionResult approvePositionSignatureLists(string actId, string companyEN)
        {
            ApproveModel.approveModels models = getApproveByBudgetApproveId(actId);
            if (companyEN == "MT")
            {
                ApproveFlowModel.approveFlowModel flowModel = BudgetApproveController.getFlowIdBudgetByBudgetActivityId(ConfigurationManager.AppSettings["subjectBudgetFormId"], actId);
                models.approveFlowDetail = flowModel.flowDetail;
            }
            else
            {
                ApproveFlowModel.approveFlowModel flowModel = BudgetApproveController.getFlowIdBudgetByBudgetActivityIdOMT(ConfigurationManager.AppSettings["subjectBudgetFormId"], actId);
                models.approveFlowDetail = flowModel.flowDetail;
            }
            return PartialView(models);
        }

        public ActionResult approvePositionSignatureListsByBudgetApproveId(string budgetApproveId)
        {
            ApproveModel.approveModels models = new ApproveModel.approveModels();
            try
            {
                models = ApproveAppCode.getApproveByActFormId(budgetApproveId);
                ApproveFlowModel.approveFlowModel flowModel = BudgetApproveController.getFlowIdByBudgetApproveId(budgetApproveId);
                models.approveFlowDetail = flowModel.flowDetail;

                var modelApproveDetail = models.approveDetailLists.ToList();

                if (modelApproveDetail.Any())
                {
                    bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)));
                    if (!folderExists)
                        Directory.CreateDirectory(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)));

                    foreach (var item in modelApproveDetail)
                    {
                        UtilsAppCode.Session.writeFileHistory(System.Web.HttpContext.Current.Server
                            , item.signature
                            , string.Format(ConfigurationManager.AppSettings["rootSignaByActURL"], budgetApproveId, item.empId));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["approvePositionSignatureError"] = AppCode.StrMessFail + ex.Message;
            }
            return PartialView(models);
        }

        public PartialViewResult previewApproveBudget(string budgetApproveId)
        {
            Budget_Approve_Detail_Model Budget_Model = new Budget_Approve_Detail_Model();
            Budget_Model.Budget_Invoce_History_list = QueryGetBudgetApprove.getBudgetInvoiceHistory(null, budgetApproveId);
            Budget_Model.Budget_Activity = QueryGetBudgetActivity.getBudgetActivityDetail(null, null,  budgetApproveId, null, DateTime.Now.AddYears(-10), DateTime.Now.AddYears(2), null).FirstOrDefault();
            Budget_Model.Budget_Approve_detail_list = QueryGetBudgetApprove.getBudgetApproveId(budgetApproveId);
            return PartialView(Budget_Model);
        }

        public static bool getPremisionApproveByEmpid(List<ApproveModel.approveDetailModel> lists, string empId)
        {
            try
            {
                bool rtn = false;
                if (lists != null)
                {
                    var model = (from x in lists where x.empId.Equals(empId) select x).ToList();
                    rtn = model.Count > 0 ? true : false;
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("fillterApproveByEmpid >>" + ex.Message);
            }
        }

        public static string getBudgetActivityId(string budget_approve_Id)
        {
            try
            {
                Budget_Approve_Detail_Model models = new Budget_Approve_Detail_Model();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetActivitySearchId"
                    , new SqlParameter[] {
                        new SqlParameter("@budget_approve_id",budget_approve_Id)
                    });
                models.Budget_Approve_detail_list = (from DataRow dr in ds.Tables[0].Rows
                                                     select new Budget_Approve_Detail_Model.Budget_Approve_Detail_Att()
                                                     {
                                                         budgetActivityId = dr["budgetActivityId"].ToString()
                                                     }).ToList();
                return models.Budget_Approve_detail_list.ElementAt(0).budgetActivityId.ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public static string getApproveBudgetId(string budgetActivityId)
        {
            try
            {
                Budget_Approve_Detail_Model models = new Budget_Approve_Detail_Model();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveIdSelect"
                    , new SqlParameter[] {
                        new SqlParameter("@budgetActivityId",budgetActivityId)
                    });
                models.Budget_Approve_detail_list = (from DataRow dr in ds.Tables[0].Rows
                                                     select new Budget_Approve_Detail_Model.Budget_Approve_Detail_Att()
                                                     {
                                                         budgetActivityId = dr["id"].ToString()
                                                     }).ToList();
                return models.Budget_Approve_detail_list.ElementAt(0).budgetActivityId.ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public static List<ApproveModel.approveDetailModel> getUserCreateBudgetForm(string budgetApproveId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetCreateUserSelect"
                    , new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveModel.approveDetailModel("")
                             {
                                 empId = dr["empId"].ToString(),
                                 empName = dr["empName"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 delFlag = (bool)dr["delFlag"],
                                 createdDate = (DateTime?)dr["createdDate"],
                                 createdByUserId = dr["createdByUserId"].ToString(),
                                 updatedDate = (DateTime?)dr["updatedDate"],
                                 updatedByUserId = dr["updatedByUserId"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getUserCreateBudgetForm >>" + ex.Message);
            }
        }

        public static ApproveModel.approveModels getApproveByBudgetApproveId(string budgetApproveId)
        {
            try
            {
                ApproveModel.approveModels models = new ApproveModel.approveModels();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveDetailList"
                    , new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId) });
                models.approveDetailLists = (from DataRow dr in ds.Tables[0].Rows
                                             select new ApproveModel.approveDetailModel("")
                                             {
                                                 id = dr["id"].ToString(),
                                                 approveId = dr["approveId"].ToString(),
                                                 rangNo = (int)dr["rangNo"],
                                                 empId = dr["empId"].ToString(),
                                                 empName = dr["empName"].ToString(),
                                                 empEmail = dr["empEmail"].ToString(),
                                                 statusId = dr["statusId"].ToString(),
                                                 statusName = dr["statusName"].ToString(),
                                                 isSendEmail = (bool)dr["isSendEmail"],
                                                 remark = dr["remark"].ToString(),
                                                 signature = (dr["signature"] == null || dr["signature"] is DBNull) ? new byte[0] : (byte[])dr["signature"],
                                                 ImgName = string.Format(ConfigurationManager.AppSettings["rootgetSignaURL"], dr["empId"].ToString()), //rootgetSignaURL , rootSignaURL
                                                 delFlag = (bool)dr["delFlag"],
                                                 createdDate = (DateTime?)dr["createdDate"],
                                                 createdByUserId = dr["createdByUserId"].ToString(),
                                                 updatedDate = (DateTime?)dr["updatedDate"],
                                                 updatedByUserId = dr["updatedByUserId"].ToString(),

                                             }).ToList();


                if (models.approveDetailLists.Count > 0)
                {
                    ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveDetailSelect"
                   , new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId) });

                    var empDetail = models.approveDetailLists.Where(r => r.empId == UtilsAppCode.Session.User.empId).ToList(); //
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new ApproveModel.approveModel()
                                 {
                                     id = dr["id"].ToString(),
                                     flowId = dr["flowId"].ToString(),
                                     actFormId = dr["actFormId"].ToString(),
                                     actNo = dr["activityNo"].ToString(),
                                     statusId = (empDetail.Count > 0) ? empDetail.FirstOrDefault().statusId : "",
                                     delFlag = (bool)dr["delFlag"],
                                     createdDate = (DateTime?)dr["createdDate"],
                                     createdByUserId = dr["createdByUserId"].ToString(),
                                     updatedDate = (DateTime?)dr["updatedDate"],
                                     updatedByUserId = dr["updatedByUserId"].ToString(),
                                     isPermisionApprove = getPremisionApproveByEmpid(models.approveDetailLists, UtilsAppCode.Session.User.empId)
                                 }).ToList();

                    models.approveModel = lists[0];

                }

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getApproveByBudgetApproveId >>" + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowIdBudgetByBudgetActivityId(string subId, string budgetActivityId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetFlowSelect"
                    , new SqlParameter[] {
                          new SqlParameter("@subId",subId)
                        , new SqlParameter("@budgetActivityId",budgetActivityId)
                    });

                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApprove()
                             {
                                 id = dr["id"].ToString(),
                             }).ToList();
                if (lists.Count > 0)
                {
                    model.flowMain = lists[0];
                    model.flowDetail = getFlowDetailBudget(model.flowMain.id, budgetActivityId);

                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlow Budget By BudgetActivityId >>" + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowIdBudgetByBudgetActivityIdOMT(string subId, string budgetActivityId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetFlowSelectOMT"
                    , new SqlParameter[] {
                          new SqlParameter("@subId",subId)
                        , new SqlParameter("@budgetActivityId",budgetActivityId)
                    });

                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApprove()
                             {
                                 id = dr["id"].ToString(),
                             }).ToList();
                if (lists.Count > 0)
                {
                    model.flowMain = lists[0];
                    model.flowDetail = getFlowDetailBudget(model.flowMain.id, budgetActivityId);
                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlow Budget By OMT >>" + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowIdByBudgetApproveId(string budget_approve_id)
        {
            try
            {

                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetFlowIdSelect"
                    , new SqlParameter[] {
                        new SqlParameter("@budgetApproveId",budget_approve_id)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApprove()
                             {
                                 id = dr["id"].ToString(),
                             }).ToList();
                if (lists.Count > 0)
                {
                    var budgetActivityId = getBudgetActivityId(budget_approve_id);
                    model.flowMain = lists[0];
                    model.flowDetail = getFlowDetailBudget(model.flowMain.id, budgetActivityId);
                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("get FlowId by BudgetApproveId >>" + ex.Message);
            }
        }

        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailBudget(string flowId, string budgetActivityId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetFlowDetailSelect"
                    , new SqlParameter[] {
                        new SqlParameter("@flowId", flowId)
                        ,new SqlParameter("@budgetActivityId", budgetActivityId)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail(dr["empId"].ToString())
                             {
                                 id = dr["id"].ToString(),
                                 rangNo = (int)dr["rangNo"],
                                 empId = dr["empId"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 empFNameTH = dr["empFNameTH"].ToString(),
                                 empLNameTH = dr["empLNameTH"].ToString(),
                                 //empPositionTitleTH = dr["empPositionTitleTH"].ToString(),
                                 approveGroupId = dr["approveGroupId"].ToString(),
                                 approveGroupName = dr["approveGroupName"].ToString(),
                                 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
                                 isShowInDoc = (bool)dr["showInDoc"],
                                 empGroup = dr["empGroup"].ToString(),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetail >>" + ex.Message);
            }
        }

        public static List<Budget_Approve_Detail_Model.budgetForm> getFilterFormByStatusId(List<Budget_Approve_Detail_Model.budgetForm> lists, int statusId)
        {
            try
            {
                return lists.Where(r => r.statusId == statusId.ToString()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterFormByStatusId >> " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult genPdfApproveBudget(string GridHtml, string statusId, string budgetApproveId)
        {
            var resultAjax = new AjaxResult();
            try
            {

                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId + "_");
                GridHtml = GridHtml.Replace("<br>", "<br/>");
                AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

                //del signature file
                bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)));
                if (folderExists)
                    Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budgetApproveId)), true);

                TB_Bud_Invoice_Document_Model getBudgetImageModel = new TB_Bud_Invoice_Document_Model();
                getBudgetImageModel.BudgetInvoiceList = BudgetApproveListController.BudgetApproveInvoiceList(budgetApproveId);

                string[] pathFile = new string[getBudgetImageModel.BudgetInvoiceList.Count + 1];
                pathFile[0] = Server.MapPath(rootPathInsert);

                if (getBudgetImageModel.BudgetInvoiceList.Any())
                {
                    int i = 1;
                    foreach (var item in getBudgetImageModel.BudgetInvoiceList)
                    {
                        if (System.IO.File.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName))))
                        {
                            pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName));
                        }
                        else
                        {
                            pathFile = pathFile.Where((val, idx) => idx != i).ToArray();
                        }
                        i++;
                    }
                }

                var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budgetApproveId));
                var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile, budgetApproveId);



                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    EmailAppCodes.sendRejectBudget(budgetApproveId, AppCode.ApproveType.Budget_form);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    EmailAppCodes.sendApproveBudget(budgetApproveId, AppCode.ApproveType.Budget_form, false);
                }


                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
                ExceptionManager.WriteError("genPdfApproveBudget => " + ex.Message);
            }
            return Json(resultAjax, "text/plain");
        }

        [HttpPost]
        public JsonResult insertApprove()
        {
            var result = new AjaxResult();
            result.Success = false;
            try
            {
                if (updateApprove(Request.Form["lblActFormId"], Request.Form["ddlStatus"], Request.Form["txtRemark"], Request.Form["lblApproveType"]) > 0)
                {
                    setCountWatingApproveBudget();
                    result.Success = true;
                }
                else
                {
                    result.Message = AppCode.StrMessFail;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }

        public static int insertApproveBudgetDetail(string budgetActivityId)
        {
            try
            {
                int rtn = 0;

                SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveInsert"
                , new SqlParameter[]
                {
                 new SqlParameter("@budgetActivityId", budgetActivityId)
                ,new SqlParameter("@createdByUserId", UtilsAppCode.Session.User.empId)
                });

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static int insertApproveByFlowBudget(ApproveFlowModel.approveFlowModel flowModel, string budgetId, Int32 count_req_app)
        {
            try
            {
                string statusId = "";
                int rtn = 0;

                if (count_req_app == 0) { statusId = "3"; }

                List<ApproveModel.approveModel> list = new List<ApproveModel.approveModel>();
                ApproveModel.approveModel model = new ApproveModel.approveModel();
                model.id = Guid.NewGuid().ToString();
                model.flowId = flowModel.flowMain.id;
                model.actFormId = budgetId;
                model.delFlag = false;
                model.createdDate = DateTime.Now;
                model.createdByUserId = UtilsAppCode.Session.User.empId;
                model.updatedDate = DateTime.Now;
                model.updatedByUserId = UtilsAppCode.Session.User.empId;
                list.Add(model);
                DataTable dt = AppCode.ToDataTable(list);
                foreach (DataRow dr in dt.Rows)
                {
                    rtn += SqlHelper.ExecuteNonQueryTypedParams(AppCode.StrCon, "usp_insertApprove", dr);
                }

                // insert approve detail
                foreach (ApproveFlowModel.flowApproveDetail m in flowModel.flowDetail)
                {
                    rtn += SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertApproveDetail"
                        , new SqlParameter[] {new SqlParameter("@id",Guid.NewGuid().ToString())
                            ,new SqlParameter("@approveId",model.id)
                            ,new SqlParameter("@rangNo",m.rangNo)
                            ,new SqlParameter("@empId",m.empId)
                            ,new SqlParameter("@statusId",statusId)
                            ,new SqlParameter("@isSendEmail",false)
                            ,new SqlParameter("@remark","")

                            ,new SqlParameter("@isApproved",true)

                            ,new SqlParameter("@delFlag",false)
                            ,new SqlParameter("@createdDate",DateTime.Now)
                            ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                            ,new SqlParameter("@updatedDate",DateTime.Now)
                            ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                                                        ,new SqlParameter("@showInDoc",m.isShowInDoc)
                            ,new SqlParameter("@approveGroupId",m.approveGroupId)
                        });
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int insertApproveForBudgetForm(string budgetActivityId, string companyEN, Int32 count_req_app)
        {
            try
            {
                insertApproveBudgetDetail(budgetActivityId);
                var budget_approve_id = getApproveBudgetId(budgetActivityId);

                // check alredy approve
                if (BudgetApproveController.getApproveByBudgetApproveId(budget_approve_id).approveDetailLists.Count == 0)
                {
                    if (companyEN == "MT")
                    {
                        ApproveFlowModel.approveFlowModel flowModel = getFlowIdBudgetByBudgetActivityId(ConfigurationManager.AppSettings["subjectBudgetFormId"], budgetActivityId);
                        return insertApproveByFlowBudget(flowModel, budget_approve_id, count_req_app);
                    }
                    else
                    {
                        ApproveFlowModel.approveFlowModel flowModel = getFlowIdBudgetByBudgetActivityIdOMT(ConfigurationManager.AppSettings["subjectBudgetFormId"], budgetActivityId);
                        return insertApproveByFlowBudget(flowModel, budget_approve_id, count_req_app);
                    }
                }
                else return 999; // alredy approve
            }
            catch (Exception ex)
            {
                throw new Exception("insertApproveBudget >> " + ex.Message);
            }
        }

        public static int updateApprove(string actFormId, string statusId, string remark, string approveType)
        {
            try
            {
                // update approve detail
                var var_budget_approve_id = actFormId;
                int rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveUpdate"
                   , new SqlParameter[] {new SqlParameter("@actFormId",var_budget_approve_id)
                    , new SqlParameter("@empId",UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@statusId",statusId)
                    ,new SqlParameter("@remark",remark)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)
                   });


                if (approveType == "ActivityBudget")
                {
                    rtn = updateBudgetFormStatus(statusId, var_budget_approve_id);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("updateApprove >> " + ex.Message);
            }
        }

        private static int updateBudgetFormStatus(string statusId, string budgetApproveId)
        {
            try
            {
                int rtn = 0;
                // update activity form
                if (statusId == ConfigurationManager.AppSettings["statusReject"])
                {
                    // update reject
                    rtn += updateBudgetFormWithApproveReject(budgetApproveId);
                }
                else if (statusId == ConfigurationManager.AppSettings["statusApprove"])
                {
                    // update approve
                    rtn += updateBudgetFormWithApproveDetail(budgetApproveId);

                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("updateBudgetFormStatus >> " + ex.Message);
            }
        }

        public static int updateBudgetFormWithApproveReject(string budgetApproveId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveIsReject"
                    , new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateBudgetFormWithApproveReject >> " + ex.Message);
            }
        }

        public static int updateBudgetFormWithApproveDetail(string budgetApproveId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetApproveIsSubmit"
                    , new SqlParameter[] { new SqlParameter("@budgetApproveId", budgetApproveId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateBudgetFormWithApproveDetail >> " + ex.Message);
            }
        }

        public static int updateApproveWaitingByRangNo(string budgetId)
        {
            try
            {

                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateWaitingApproveByRangNo"
                    , new SqlParameter[] {new SqlParameter("@rangNo",1)
                    ,new SqlParameter("@actId", budgetId)
                    ,new SqlParameter("@updateBy", UtilsAppCode.Session.User.empId)
                    ,new SqlParameter("@updateDate", DateTime.Now)
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("updateApproveWaitingByRangNo >>" + ex.Message);
            }
        }

        public static void setCountWatingApproveBudget()
        {
            try
            {
                if (UtilsAppCode.Session.User != null)
                {
                    DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_mtm_BudgetCountWatingApproveOfEmp"
                        , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });

                    UtilsAppCode.Session.User.countWatingBudgetForm = "";
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        UtilsAppCode.Session.User.countWatingBudgetForm = ds.Tables[0].Rows[0]["actFormId"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("setCountWatingApproveBudget >>" + ex.Message);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<JsonResult> submitPreviewBudget(string GridHtml, string budgetActivityId, string companyEN, string count_req_approve)
        {

            var resultAjax = new AjaxResult();
            try
            {
                string budget_approve_id = "";
                int count_req_app = Int32.Parse(count_req_approve);

                if (BudgetApproveController.insertApproveForBudgetForm(budgetActivityId, companyEN, count_req_app) > 0) //usp_insertApproveDetail
                {
                    budget_approve_id = BudgetApproveController.getApproveBudgetId(budgetActivityId); // get last approve id
                    BudgetApproveController.updateApproveWaitingByRangNo(budget_approve_id);

                    TB_Bud_Invoice_Document_Model getBudgetImageModel = new TB_Bud_Invoice_Document_Model();
                    getBudgetImageModel.BudgetInvoiceList = BudgetApproveListController.BudgetApproveInvoiceList(budget_approve_id);


                    var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id + "_");
                    GridHtml = GridHtml.Replace("<br>", "<br/>");

                    AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));

                    //del signature file
                    bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budget_approve_id)));
                    if (folderExists)
                        Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budget_approve_id)), true);


                    string[] pathFile = new string[getBudgetImageModel.BudgetInvoiceList.Count + 1];
                    pathFile[0] = Server.MapPath(rootPathInsert);

                    if (getBudgetImageModel.BudgetInvoiceList.Any())
                    {
                        int i = 1;
                        foreach (var item in getBudgetImageModel.BudgetInvoiceList)
                        {
                            if (System.IO.File.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName))))
                            {
                                pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName));
                            }
                            else
                            {
                                pathFile = pathFile.Where((val, idx) => idx != i).ToArray();
                            }
                            i++;
                        }
                    }

                    var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id));
                    var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile, budget_approve_id);

                    BudgetApproveController.setCountWatingApproveBudget();
                    if (count_req_app > 0)
                    {
                        EmailAppCodes.sendApproveBudget(budget_approve_id, AppCode.ApproveType.Budget_form, false);
                    }
                }


                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }
            return Json(resultAjax, "text/plain");
        }


        [HttpPost]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<JsonResult> submitPreviewBudgetRegenPdf(string GridHtml, string budgetActivityId, string companyEN, string count_req_approve)
        {

            var resultAjax = new AjaxResult();
            try
            {
                string budget_approve_id = "";

                budget_approve_id = BudgetApproveController.getApproveBudgetId(budgetActivityId); // get last approve id


                var rootPathInsert = string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id + "_");
                GridHtml = GridHtml.Replace("<br>", "<br/>");
                GridHtml = GridHtml.Replace("undefined", "");
                AppCode.genPdfFile(GridHtml, new Document(PageSize.A4, 25, 25, 10, 10), Server.MapPath(rootPathInsert));
                // del signature file
                bool folderExists = Directory.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budget_approve_id)));
                if (folderExists)
                    Directory.Delete(Server.MapPath(@"" + string.Format(ConfigurationManager.AppSettings["rootCreateSubSigna"], budget_approve_id)), true);

                TB_Bud_Invoice_Document_Model getBudgetImageModel = new TB_Bud_Invoice_Document_Model();
                getBudgetImageModel.BudgetInvoiceList = BudgetApproveListController.BudgetApproveInvoiceList(budget_approve_id);

                string[] pathFile = new string[getBudgetImageModel.BudgetInvoiceList.Count + 1];
                pathFile[0] = Server.MapPath(rootPathInsert);

                if (getBudgetImageModel.BudgetInvoiceList.Any())
                {
                    int i = 1;
                    foreach (var item in getBudgetImageModel.BudgetInvoiceList)
                    {
                        if (System.IO.File.Exists(Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName))))
                        {
                            pathFile[i] = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfilesBudget"], item._fileName));
                        }
                        else
                        {
                            pathFile = pathFile.Where((val, idx) => idx != i).ToArray();
                        }
                        i++;
                    }
                }

                var rootPathOutput = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootBudgetPdftURL"], budget_approve_id));
                var resultMergePDF = AppCode.mergePDF(rootPathOutput, pathFile, budget_approve_id);


                resultAjax.Success = true;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
                ExceptionManager.WriteError(ex.Message);
            }
            return Json(resultAjax, "text/plain");
        }

    }

}
