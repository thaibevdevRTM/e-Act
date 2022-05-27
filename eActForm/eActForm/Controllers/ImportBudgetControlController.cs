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
            string typeImportCatch = "";
            int rowImport = 1;
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

                var getLE = ImportBudgetControlAppCode.getLE_No(AppCode.StrCon, MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]).Year, model.companyId);

                //------------------------ Prepare data for BudgetControl by Chanel -----------------
                List<BudgetControlModels> budgetList = new List<BudgetControlModels>();
                List<BudgetControl_LEModel> BudgetLEList = new List<BudgetControl_LEModel>();
                List<BudgetControl_ActType> bgActTypeList = new List<BudgetControl_ActType>();
                if (model.companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                {
                    dt = ExcelAppCode.ReadExcel(resultFilePath, "Chanel", "A:Z");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        


                        typeImportCatch = ImportBudgetControlAppCode.channel;
                        rowImport = rowImport++;
                        for (int ii = 2; ii < dt.Columns.Count; ii++)
                        {
                            BudgetControlModels modelBudget = new BudgetControlModels();
                            genId = Guid.NewGuid().ToString();
                            modelBudget.id = genId;
                            modelBudget.companyId = model.companyId;
                            typeImportCatch = "/// brand : " + dt.Rows[i]["BRAND"].ToString();

                            if (!QueryGetAllBrand.GetAllBrand().Where(x => x.id.Equals(dt.Rows[i]["brandId"].ToString())).Any())
                            {
                                resultAjax.Message += "brandId is null &&";
                                throw new Exception();
                            }


                            modelBudget.brandId = dt.Rows[i]["brandId"].ToString();
                            modelBudget.budgetGroupType = ImportBudgetControlAppCode.channel;
                            modelBudget.amount = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i][dt.Columns[ii].ToString()].ToString()));
                            modelBudget.chanelName = dt.Columns[ii].ToString();
                            modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelBudget.createdByUserId = UtilsAppCode.Session.User.empId;
                            modelBudget.LE = int.Parse(getLE) + 1;

                            if (model.companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                            {
                                typeImportCatch += "//// column name : " + dt.Columns[ii].ToString();
                                modelBudget.chanelId = QueryGetAllChanel.getAllChanel().Where(x => x.cust.Equals(dt.Columns[ii].ToString())).FirstOrDefault().id;
                                modelBudget.EO = ImportBudgetControlAppCode.genEO(AppCode.StrCon, modelBudget); //genauto
                            }
                            else
                            {
                                modelBudget.chanelId = dt.Columns[ii].ToString();
                                modelBudget.EO = ""; //genauto
                            }

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
                        }


                    }
                }

                //------------------------ Prepare data for BudgetControl by Brand -----------------
                rowImport = 1;

                dtBrand = ExcelAppCode.ReadExcel(resultFilePath, "Brand", "A:Z");
                for (int i = 0; i < dtBrand.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dtBrand.Rows[i]["BRAND"].ToString()))
                    {
                        BudgetControlModels modelBudget = new BudgetControlModels();
                        BudgetControl_LEModel modelLE = new BudgetControl_LEModel();
                        typeImportCatch = ImportBudgetControlAppCode.brand;
                        rowImport = rowImport + 1;

                        genId = Guid.NewGuid().ToString();
                        modelBudget.id = genId;
                        modelBudget.companyId = model.companyId;
                        typeImportCatch = "/// brand : " + dtBrand.Rows[i]["BRAND"].ToString();
                        if (!QueryGetAllBrand.GetAllBrand().Where(x => x.id.Equals(dtBrand.Rows[i]["brandId"].ToString())).Any())
                        {
                            resultAjax.Message += "brandId is null   &&   ";
                                throw new Exception();
                        }
                            

                        modelBudget.brandId = dtBrand.Rows[i]["brandId"].ToString();
                        modelBudget.budgetGroupType = ImportBudgetControlAppCode.brand;
                        modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                        modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                        modelBudget.LE = int.Parse(getLE) + 1;
                        modelBudget.createdByUserId = UtilsAppCode.Session.User.empId;
                        if (model.companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                        {
                            modelBudget.chanelId = "";
                            modelBudget.amount = 0;
                            modelBudget.EO = ImportBudgetControlAppCode.genEO(AppCode.StrCon, modelBudget); //genauto
                            modelBudget.budgetNo = ImportBudgetControlAppCode.genBudgetNo(AppCode.StrCon, modelBudget, int.Parse(getLE) + 1);  //genauto
                        }
                        else
                        {
                            modelBudget.chanelId = dtBrand.Rows[i]["channel"].ToString();
                            modelBudget.amount = 0;
                            modelBudget.EO = "";
                            modelBudget.budgetNo = "";
                        }


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
                            int setIndex = 2;
                            if (model.companyId != ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                            {
                                setIndex = 3;
                            }

                            for (int ii = setIndex; ii < dtBrand.Columns.Count; ii++)
                            {
                                BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                                bgActTypeModel.id = Guid.NewGuid().ToString();
                                bgActTypeModel.budgetId = genId;
                                bgActTypeModel.budgetLEId = genIdLE;
                                if (model.companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                                {
                                    typeImportCatch += "/// column : " + dtBrand.Columns[ii].ToString();
                                    bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Equals(dtBrand.Columns[ii].ToString().ToLower())).FirstOrDefault().id;
                                }
                                else
                                {
                                    typeImportCatch += "/// column : " + dtBrand.Columns[ii].ToString();
                                    bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals(ConfigurationManager.AppSettings["conditionActBeer"]) && x.activitySales.Substring(0, 2).Equals(dtBrand.Columns[ii].ToString().Substring(0, 2))).FirstOrDefault().id;
                                }
                                bgActTypeModel.amount = dtBrand.Rows[i][dtBrand.Columns[ii].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtBrand.Rows[i][dtBrand.Columns[ii].ToString()].ToString()));
                                bgActTypeModel.description = "";
                                bgActTypeModel.createdByUserId = UtilsAppCode.Session.User.empId;
                                bgActTypeList.Add(bgActTypeModel);

                                setIndex++;
                            }

                        }
                    }
                }



                if (budgetList.Any())
                {
                    var delCount = ImportBudgetControlAppCode.deleteBudgetTemp_ImportBudgetBG(AppCode.StrCon, model.companyId);
                    foreach (var item in budgetList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetControlTemp(AppCode.StrCon, item, model.companyId);
                    }
                }

                if (BudgetLEList.Any())
                {
                    //var delCount = +ImportBudgetControlAppCode.InsertBudgetLE_History(AppCode.StrCon, BudgetLEList.FirstOrDefault().endDate);
                    foreach (var item in BudgetLEList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetLETemp(AppCode.StrCon, item, model.companyId);
                    }
                }
                if (bgActTypeList.Any())
                {
                    //var delCount = +ImportBudgetControlAppCode.InsertBudgetActType_History(AppCode.StrCon , BudgetLEList.FirstOrDefault().endDate);
                    foreach (var item in bgActTypeList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetActTypeTemp(AppCode.StrCon, item, model.companyId);
                    }
                }


                resultAjax.Success = true;

            }
            catch (Exception ex)
            {
                resultAjax.Message += ex.Message + "TypeImport : " + typeImportCatch + " Row : " + rowImport;

            }
            return Json(resultAjax, "text/plain");
        }


        public ActionResult showImportBGlist(string companyId)
        {


            Models.ImportBudgetControlModel modelTemp = new Models.ImportBudgetControlModel();

            modelTemp.budgetReportList = ImportBudgetControlAppCode.getBudgetTemp(AppCode.StrCon, companyId);

            return PartialView(modelTemp);
        }


        public JsonResult confirmImportBudget(BudgetControlModels model, string companyId)
        {
            var resultAjax = new AjaxResult();
            try
            {

                var delCount = +ImportBudgetControlAppCode.InsertBudgetLE_History(AppCode.StrCon, companyId);
                delCount = +ImportBudgetControlAppCode.InsertBudgetActType_History(AppCode.StrCon, companyId);
                resultAjax.Code = ImportBudgetControlAppCode.confirmImportBudget(AppCode.StrCon, companyId);
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

            model.importTypeList = MainAppCode.getOhterMaster(AppCode.StrCon, "Import", "reportSap");
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
                        resultAjax.Code = i + 3;

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
                    if (model.typeImport.ToLower() == ConfigurationManager.AppSettings["typeImportYearly"])
                    {
                        modelBudgetRpt.commitment = decimal.Parse(dtBudget.Rows[i]["Commitment"].ToString());
                    }
                    else
                    {
                        modelBudgetRpt.commitment = dtBudget.Rows[i]["Person Responsible"].ToString().Trim().ToLower() == "Commit".Trim().ToLower() ? decimal.Parse(dtBudget.Rows[i]["Commitment"].ToString()) : 0;
                        modelBudgetRpt.nonCommitment = dtBudget.Rows[i]["Person Responsible"].ToString().Trim().ToLower() == "Non Commit".Trim().ToLower() ? decimal.Parse(dtBudget.Rows[i]["Commitment"].ToString()) : 0;
                    }

                    modelBudgetRpt.typeImport = importType;
                    var dateStr = BaseAppCodes.converStrToDatetimeWithFormat(model.dateStr + "-" + "01", "yyyy-MM-dd").ToString("dd/MM/yyyy");
                    modelBudgetRpt.date = BaseAppCodes.converStrToDatetimeWithFormat(dateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                    modelBudgetRpt.fiscalYear = modelBudgetRpt.date.Value.Year.ToString();
                    //modelBudgetRpt.chanelId = ImportBudgetControlAppCode.getChannelIdForTxt(AppCode.StrCon, dtChannel.Rows[i]["Bnam_Eng"].ToString());
                    //modelBudgetRpt.activityTypeId = ImportBudgetControlAppCode.getActivityIdIdForTxt(AppCode.StrCon, dtChannel.Rows[i]["Activity"].ToString());
                    modelBudgetRpt.createdByUserId = UtilsAppCode.Session.User.empId;
                    modelBudgetRptList.Add(modelBudgetRpt);
                }

                int result = 0;
                Models.ImportBudgetControlModel budgetModel = new Models.ImportBudgetControlModel();
                if (model.typeImport.ToLower() == ConfigurationManager.AppSettings["typeImportYearly"])
                {
                    budgetModel.typeImport = ConfigurationManager.AppSettings["typeImportYearly"];
                    result = +ImportBudgetControlAppCode.delBudgetRpt_Temp(AppCode.StrCon);
                    foreach (var item in modelBudgetRptList)
                    {

                        result = +ImportBudgetControlAppCode.InsertBudgetRpt_Temp(AppCode.StrCon, item);
                    }

                    budgetModel.budgetReportList = ImportBudgetControlAppCode.getBudgetList(AppCode.StrCon);
                    budgetModel.budgetReportList2 = ImportBudgetControlAppCode.getBudgetReportTBM_NotEO(AppCode.StrCon);
                }
                else
                {
                    budgetModel.typeImport = ConfigurationManager.AppSettings["typeImportMonthly"];
                    result = +ImportBudgetControlAppCode.delBudgetRptMonthly_Temp(AppCode.StrCon);
                    foreach (var item in modelBudgetRptList)
                    {
                        result = +ImportBudgetControlAppCode.InsertBudgetTemp_Monthly(AppCode.StrCon, item);
                    }

                    budgetModel.budgetReportList = ImportBudgetControlAppCode.getDataSapMonthlyList(AppCode.StrCon);
                }

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
            int result = 0;
            try
            {
                Models.ImportBudgetControlModel model = new Models.ImportBudgetControlModel();
                model = (Models.ImportBudgetControlModel)TempData["budgetTemp"];

                eForms.Models.MasterData.ImportBudgetControlModel modelBG_MD = new eForms.Models.MasterData.ImportBudgetControlModel();
                modelBG_MD.budgetReportList = model.budgetReportList;
                //modelBG_MD.budgetReportBrandList = model.budgetReportBrandList;

                if (model.typeImport == ConfigurationManager.AppSettings["typeImportYearly"])
                {
                    result = ImportBudgetControlAppCode.insertDateReportBudgetTBM(AppCode.StrCon, modelBG_MD);
                }
                else
                {
                    result = ImportBudgetControlAppCode.delBudgetMonthlyByDate(AppCode.StrCon, modelBG_MD);
                    result = ImportBudgetControlAppCode.insertDateReportMonthly_BudgetTBM(AppCode.StrCon, modelBG_MD);
                }

                resultAjax.Success = true;
            }
            catch (Exception ex)
            {

            }
            return Json(resultAjax, "text/plain");
        }

    }
}