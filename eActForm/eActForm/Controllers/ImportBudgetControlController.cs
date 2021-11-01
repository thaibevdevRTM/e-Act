using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using eForms.Models.MasterData;
using eForms.Presenter.AppCode;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebLibrary;
using static eForms.Models.MasterData.ImportBudgetControlModel;

namespace eActForm.Controllers
{
    public class ImportBudgetControlController : Controller
    {
        // GET: ImportBudgetControl
        public ActionResult Index()
        {
            eForms.Models.MasterData.ImportBudgetControlModel model = new eForms.Models.MasterData.ImportBudgetControlModel();

            if (UtilsAppCode.Session.User.isSuperAdmin)
            {
                model.companyList = MainAppCode.getOhterMaster(AppCode.StrCon, "company", "");
            }
            else
            {
                model.companyList = MainAppCode.getOhterMaster(AppCode.StrCon, "company", "").Where(w => w.val1.Contains(UtilsAppCode.Session.User.empCompanyId)).ToList();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult ImportFlie_BudgetConttrol(BudgetControlModels model)
        {
            var resultAjax = new AjaxResult();
            try
            {
                string resultFilePath = "", genId = "", genIdLE = "";
                int CountFile = model.InputFiles.Count();
                for (int i = 0; i < CountFile; i++)
                {
                    string genUniqueName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + UtilsAppCode.Session.User.empId;
                    string extension = Path.GetExtension(model.InputFiles[i].FileName);
                    string resultFileName = genUniqueName + extension;
                    resultFilePath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles_BudgetControl"], resultFileName));
                    model.InputFiles[i].SaveAs(resultFilePath);
                }
                DataTable dt = new DataTable();
                DataTable dtBrand = new DataTable();

                dt = ExcelAppCode.ReadExcel(resultFilePath, "BG-L1", "A:Z");
                dtBrand = ExcelAppCode.ReadExcel(resultFilePath, "B_BRAND", "A:Z");

                var getLE = ImportBudgetControlAppCode.getLE_No(AppCode.StrCon, MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]).Year);

                //------------------------ Prepare data for BudgetControl by Chanel -----------------
                List<BudgetControlModels> budgetList = new List<BudgetControlModels>();
                List<BudgetControl_LEModel> BudgetLEList = new List<BudgetControl_LEModel>();
                List<BudgetControl_ActType> bgActTypeList = new List<BudgetControl_ActType>();

                int runingNo = ImportBudgetControlAppCode.getBudget_No(AppCode.StrCon);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int ii = 2; ii < dt.Columns.Count; ii++)
                    {

                        if (QueryGetAllChanel.getAllChanel().Where(x => x.cust.Equals(dt.Columns[ii].ToString())).Any())
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["brandId"].ToString()))
                            {

                                BudgetControlModels modelBudget = new BudgetControlModels();
                                genId = Guid.NewGuid().ToString();
                                modelBudget.id = genId;
                                modelBudget.companyId = model.companyId;
                                modelBudget.brandId = dt.Rows[i]["brandId"].ToString();
                                modelBudget.budgetGroupType = ImportBudgetControlAppCode.channel;
                                modelBudget.amount = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i][dt.Columns[ii].ToString()].ToString()));
                                //modelBudget.totalChannel = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i]["Total Channel"].ToString()));
                                modelBudget.chanelId = QueryGetAllChanel.getAllChanel().Where(x => x.cust.Equals(dt.Columns[ii].ToString())).FirstOrDefault().id;
                                modelBudget.chanelName = dt.Columns[ii].ToString();
                                modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                                modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                                modelBudget.createdByUserId = UtilsAppCode.Session.User.empId;
                                modelBudget.EO = ImportBudgetControlAppCode.genEO(AppCode.StrCon, modelBudget); //genauto
                                modelBudget.budgetNo = ImportBudgetControlAppCode.genBudgetNo(AppCode.StrCon, modelBudget, int.Parse(getLE) + 1);  //genauto
                                modelBudget.LE = int.Parse(getLE) + 1;
                                budgetList.Add(modelBudget);


                                if (budgetList.Any())
                                {
                                    //Add Model LE
                                    BudgetControl_LEModel modelLE = new BudgetControl_LEModel();
                                    genIdLE = Guid.NewGuid().ToString();
                                    modelLE.id = genIdLE;
                                    modelLE.budgetId = genId;
                                    modelLE.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                                    modelLE.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                                    modelLE.descripion = "";

                                    modelLE.createdByUserId = UtilsAppCode.Session.User.empId;
                                    BudgetLEList.Add(modelLE);
                                }
                                if (BudgetLEList.Any())
                                {
                                    BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                                    bgActTypeModel.id = Guid.NewGuid().ToString();
                                    bgActTypeModel.budgetId = genId;
                                    bgActTypeModel.budgetLEId = genIdLE;
                                    bgActTypeModel.actTypeId = "";
                                    bgActTypeModel.amount = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i][dt.Columns[ii].ToString()].ToString()));
                                    bgActTypeModel.createdByUserId = UtilsAppCode.Session.User.empId;
                                    bgActTypeList.Add(bgActTypeModel);
                                }


                                runingNo++;
                            }
                        }
                    }
                }

                //------------------------ Prepare data for BudgetControl by Brand -----------------

                for (int i = 0; i < dtBrand.Rows.Count; i++)
                {
                    BudgetControlModels modelBudget = new BudgetControlModels();
                    BudgetControl_LEModel modelLE = new BudgetControl_LEModel();

                    if (!string.IsNullOrEmpty(dtBrand.Rows[i]["brandId"].ToString()))
                    {
                        genId = Guid.NewGuid().ToString();
                        modelBudget.id = genId;
                        modelBudget.companyId = model.companyId;
                        modelBudget.brandId = dtBrand.Rows[i]["brandId"].ToString();
                        modelBudget.budgetGroupType = ImportBudgetControlAppCode.brand;
                        modelBudget.amount = decimal.Parse(AppCode.checkNullorEmpty(dtBrand.Rows[i]["Total MKT"].ToString()));
                        modelBudget.chanelId = "";
                        modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                        modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                        modelBudget.EO = ImportBudgetControlAppCode.genEO(AppCode.StrCon, modelBudget); //genauto
                        modelBudget.budgetNo = ImportBudgetControlAppCode.genBudgetNo(AppCode.StrCon, modelBudget, int.Parse(getLE) + 1);  //genauto
                        modelBudget.LE = int.Parse(getLE) + 1;
                        modelBudget.createdByUserId = UtilsAppCode.Session.User.empId;
                        budgetList.Add(modelBudget);



                        if (budgetList.Any())
                        {
                            //Add Model LE
                            genIdLE = Guid.NewGuid().ToString();
                            modelLE.id = genIdLE;
                            modelLE.budgetId = genId;
                            modelLE.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.descripion = "";
                            modelLE.createdByUserId = UtilsAppCode.Session.User.empId;
                            BudgetLEList.Add(modelLE);
                        }

                        if (BudgetLEList.Any())
                        {
                            for (int ii = 2; ii < dtBrand.Columns.Count; ii++)
                            {
                                if (QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Equals(dtBrand.Columns[ii].ToString().ToLower())).Any())
                                {
                                    BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                                    bgActTypeModel.id = Guid.NewGuid().ToString();
                                    bgActTypeModel.budgetId = genId;
                                    bgActTypeModel.budgetLEId = genIdLE;
                                    bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Equals(dtBrand.Columns[ii].ToString().ToLower())).FirstOrDefault().id;
                                    bgActTypeModel.amount = dtBrand.Rows[i][dtBrand.Columns[ii].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtBrand.Rows[i][dtBrand.Columns[ii].ToString()].ToString()));
                                    bgActTypeModel.description = "";
                                    bgActTypeModel.createdByUserId = UtilsAppCode.Session.User.empId;
                                    bgActTypeList.Add(bgActTypeModel);
                                }
                            }
                        }
                        runingNo++;
                    }
                }


                if (budgetList.Any())
                {
                    var delCount = ImportBudgetControlAppCode.deleteBudgetTemp_ImportBudgetBG(AppCode.StrCon);
                    foreach (var item in budgetList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetControlTemp(AppCode.StrCon, item);
                    }
                }

                if (BudgetLEList.Any())
                {

                    //var delCount = +ImportBudgetControlAppCode.InsertBudgetLE_History(AppCode.StrCon, BudgetLEList.FirstOrDefault().endDate);
                    foreach (var item in BudgetLEList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetLETemp(AppCode.StrCon, item);
                    }
                }
                if (bgActTypeList.Any())
                {
                    //var delCount = +ImportBudgetControlAppCode.InsertBudgetActType_History(AppCode.StrCon , BudgetLEList.FirstOrDefault().endDate);
                    foreach (var item in bgActTypeList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetActTypeTemp(AppCode.StrCon, item);
                    }
                }


                resultAjax.Success = true;

            }
            catch (Exception ex)
            {
                resultAjax.Message = ex.Message;

            }
            return Json(resultAjax, "text/plain");
        }


        public ActionResult showImportBGlist()
        {


            Models.ImportBudgetControlModel modelTemp = new Models.ImportBudgetControlModel();
            modelTemp.budgetReportList = ImportBudgetControlAppCode.getBudgetTemp(AppCode.StrCon);

            return PartialView(modelTemp);
        }


        public JsonResult confirmImportBudget(BudgetControlModels model)
        {
            var resultAjax = new AjaxResult();
            try
            {

                var delCount = +ImportBudgetControlAppCode.InsertBudgetLE_History(AppCode.StrCon);
                delCount = +ImportBudgetControlAppCode.InsertBudgetActType_History(AppCode.StrCon);
                resultAjax.Code = ImportBudgetControlAppCode.confirmImportBudget(AppCode.StrCon);
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax, "text/plain");
        }


        public ActionResult Inddex_ImportBGMarketing()
        {
            eForms.Models.MasterData.ImportBudgetControlModel model = new eForms.Models.MasterData.ImportBudgetControlModel();

            if (UtilsAppCode.Session.User.isSuperAdmin)
            {
                model.companyList = MainAppCode.getOhterMaster(AppCode.StrCon, "company", "");
            }
            else
            {
                model.companyList = MainAppCode.getOhterMaster(AppCode.StrCon, "company", "").Where(w => w.val1.Contains(UtilsAppCode.Session.User.empCompanyId)).ToList();
            }
            return View(model);
        }


        public JsonResult ImportFlie_BudgetConttrol_Rpt(BudgetControlModels model)
        {
            var resultAjax = new AjaxResult();
           
            try
            {
                string resultFilePath = "";
                TempData.Clear();
                int CountFile = model.InputFiles.Count();
                for (int i = 0; i < CountFile; i++)
                {
                    string genUniqueName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_BG_TBM_RPT_" + UtilsAppCode.Session.User.empId;
                    string extension = Path.GetExtension(model.InputFiles[i].FileName);
                    string resultFileName = genUniqueName + extension;
                    resultFilePath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles_BudgetControlRpt"], resultFileName));
                    model.InputFiles[i].SaveAs(resultFilePath);
                }
                DataTable dtBudget = new DataTable();
                //DataTable dtBrand = new DataTable();

                dtBudget = ExcelAppCode.ReadExcel(resultFilePath, "Sheet1", "A:Z");
                //dtBrand = ExcelAppCode.ReadExcel(resultFilePath, "BrandImport", "A:Z");

                List<BudgetControlModels> modelBudgetRptList = new List<BudgetControlModels>();
                for (int i = 0; i < dtBudget.Rows.Count; i++)
                {
                    BudgetControlModels modelBudgetRpt = new BudgetControlModels();

                    string getEO = dtBudget.Rows[i]["External Order No"].ToString();
                    string importType = "";
                    if (!string.IsNullOrEmpty(getEO) && getEO.Length > 10)
                    {


                        var getEO_Brand2digit = QueryGetAllBrand.GetAllBrand().Where(x => x.digit_EO.Contains(getEO.Substring(0, 4))).FirstOrDefault().digit_EO;
                        resultAjax.Code = i +3;

                        string subStrEO = getEO.Substring(4, 2);
                        if (subStrEO == "11")
                        {
                           
                            modelBudgetRpt.replaceEO = getEO_Brand2digit + getEO.Substring(4, 6);
                            importType = "chanel";
                        }
                        else
                        {

                            getEO = getEO.Substring(0, 10);
                            getEO = getEO.Remove(6, 2);
                            getEO = getEO.Insert(6, "{0}");

                            modelBudgetRpt.replaceEO = getEO_Brand2digit + getEO.Substring(4, 7);
                            importType = "brand";
                        }
                    }


                    modelBudgetRpt.EO = dtBudget.Rows[i]["External Order No"].ToString();
                    modelBudgetRpt.approveNo = dtBudget.Rows[i]["Approve No"].ToString();
                    modelBudgetRpt.orderNo = dtBudget.Rows[i]["Order No"].ToString();
                    modelBudgetRpt.description = dtBudget.Rows[i]["Description"].ToString();
                    modelBudgetRpt.budgetAmount = decimal.Parse(dtBudget.Rows[i]["Budget"].ToString());
                    modelBudgetRpt.returnAmount = decimal.Parse(dtBudget.Rows[i]["Return"].ToString());
                    modelBudgetRpt.actual = decimal.Parse(dtBudget.Rows[i]["Actual"].ToString());
                    modelBudgetRpt.accrued = decimal.Parse(dtBudget.Rows[i]["Accrued"].ToString());
                    modelBudgetRpt.commitment = decimal.Parse(dtBudget.Rows[i]["Commitment"].ToString());
                    modelBudgetRpt.PR_PO = decimal.Parse(dtBudget.Rows[i]["PR/PO Outstanding"].ToString());
                    modelBudgetRpt.prepaid = decimal.Parse(dtBudget.Rows[i]["Prepaid"].ToString());
                    modelBudgetRpt.available = decimal.Parse(dtBudget.Rows[i]["Available"].ToString());
                    modelBudgetRpt.fiscalYear = dtBudget.Rows[i]["Fiscal Year"].ToString();
                    modelBudgetRpt.typeImport = importType;
                    //modelBudgetRpt.chanelId = ImportBudgetControlAppCode.getChannelIdForTxt(AppCode.StrCon, dtChannel.Rows[i]["Bnam_Eng"].ToString());
                    //modelBudgetRpt.activityTypeId = ImportBudgetControlAppCode.getActivityIdIdForTxt(AppCode.StrCon, dtChannel.Rows[i]["Activity"].ToString());
                    modelBudgetRpt.createdByUserId = UtilsAppCode.Session.User.empId;
                    modelBudgetRptList.Add(modelBudgetRpt);
                }

                int result = 0;
                result = +ImportBudgetControlAppCode.delBudgetRpt_Temp(AppCode.StrCon);

                foreach (var item in modelBudgetRptList)
                {
                   
                    result = +ImportBudgetControlAppCode.InsertBudgetRpt_Temp(AppCode.StrCon, item);
                }

                Models.ImportBudgetControlModel budgetModel = new Models.ImportBudgetControlModel();
                //budgetModel.budgetReportChannelList = ImportBudgetControlAppCode.getBudgetChannelList(AppCode.StrCon);
                budgetModel.budgetReportList = ImportBudgetControlAppCode.getBudgetList(AppCode.StrCon);
                budgetModel.budgetReportList2 = ImportBudgetControlAppCode.getBudgetReportTBM_NotEO(AppCode.StrCon);


                TempData["budgetTemp"] = budgetModel;
                resultAjax.Success = true;

            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = ex.Message + "__Row" + resultAjax.Code;

            }
            return Json(resultAjax, "text/plain");
        }



        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportExcel(string gridHtml, string type)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return File(Encoding.UTF8.GetBytes(gridHtml), "application/vnd.ms-excel", "ReportTBM_" + type + ".xls");
        }

        public ActionResult showBudgetListAfterImport()
        {
            Models.ImportBudgetControlModel model = new Models.ImportBudgetControlModel();

            model = (Models.ImportBudgetControlModel)TempData["budgetTemp"];
            TempData.Keep();
            return PartialView(model);

        }
        public JsonResult confirmImportBudgetReport()
        {
            var resultAjax = new AjaxResult();
            try
            {
                Models.ImportBudgetControlModel model = new Models.ImportBudgetControlModel();
                model = (Models.ImportBudgetControlModel)TempData["budgetTemp"];

                eForms.Models.MasterData.ImportBudgetControlModel modelBG_MD = new eForms.Models.MasterData.ImportBudgetControlModel();
                modelBG_MD.budgetReportList = model.budgetReportList;
                //modelBG_MD.budgetReportBrandList = model.budgetReportBrandList;


                int result = ImportBudgetControlAppCode.insertDateReportBudgetTBM(AppCode.StrCon, modelBG_MD);

                resultAjax.Success = true;
            }
            catch (Exception ex)
            {

            }
            return Json(resultAjax, "text/plain");
        }

    }
}