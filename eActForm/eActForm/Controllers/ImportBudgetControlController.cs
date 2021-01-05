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
using System.Web;
using System.Web.Mvc;
using static eForms.Models.MasterData.ImportBudgetControlModel;

namespace eActForm.Controllers
{
    public class ImportBudgetControlController : Controller
    {
        // GET: ImportBudgetControl
        public ActionResult Index()
        {
            ImportBudgetControlModel model = new ImportBudgetControlModel();

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
                DataTable dtCMKT_TT = new DataTable();
                DataTable dtCMKT_CVM = new DataTable();
                DataTable dtCMKT_MTM = new DataTable();
                DataTable dtCMKT_ONT = new DataTable();
                DataTable dtCMKT_SSC = new DataTable();
                dt = ExcelAppCode.ReadExcel(resultFilePath, "BG-L1", "A:F");
                dtBrand = ExcelAppCode.ReadExcel(resultFilePath, "B_BRAND", "A:G");
                dtCMKT_TT = ExcelAppCode.ReadExcel(resultFilePath, "TT", "A:G");
                dtCMKT_CVM = ExcelAppCode.ReadExcel(resultFilePath, "CVM", "A:G");
                dtCMKT_MTM = ExcelAppCode.ReadExcel(resultFilePath, "MT", "A:G");
                dtCMKT_ONT = ExcelAppCode.ReadExcel(resultFilePath, "ONT", "A:G");
                dtCMKT_SSC = ExcelAppCode.ReadExcel(resultFilePath, "SSC", "A:G");


                string rtn = ConfigurationManager.AppSettings["chanelBudget"];//File.ReadAllText(path);
                JObject json = JObject.Parse(rtn);
                var lists = JsonConvert.DeserializeObject<List<chanelBudgetModel>>(json.SelectToken("chanel").ToString());

                string rtnActTypeBudget = ConfigurationManager.AppSettings["actTypeBudget"];//File.ReadAllText(path);
                JObject jsonActTypeBudget = JObject.Parse(rtnActTypeBudget);
                var listsActType = JsonConvert.DeserializeObject<List<chanelBudgetModel>>(jsonActTypeBudget.SelectToken("actType").ToString());


                //------------------------ Prepare data for BudgetControl by Chanel -----------------
                List<BudgetControlModels> budgetList = new List<BudgetControlModels>();
                List<BudgetControl_LEModel> BudgetLEList = new List<BudgetControl_LEModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BudgetControlModels modelBudget = new BudgetControlModels();
                    BudgetControl_LEModel modelLE = new BudgetControl_LEModel();
                    BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                    for (int ii = 2; ii < dt.Columns.Count; ii++)
                    {

                        if (lists.Where(x => x.name.Contains(dt.Columns[ii].ToString())).Any())
                        {
                            genId = Guid.NewGuid().ToString();
                            modelBudget.id = genId;
                            modelBudget.companyId = model.companyId;
                            modelBudget.brandId = dt.Rows[i]["brandId"].ToString();
                            modelBudget.budgetGroupType = "chanel";
                            modelBudget.amount = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i][dt.Columns[ii].ToString()].ToString()));
                            modelBudget.chanelId = lists.Where(x => x.name.Contains(dt.Columns[ii].ToString())).FirstOrDefault().id;
                            modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelBudget.createdByUserId = UtilsAppCode.Session.User.empId;
                            modelBudget.EO = ""; //genauto
                            modelBudget.budgetNo = ""; //genauto
                            budgetList.Add(modelBudget);
                        }

                        if (budgetList.Any())
                        {
                            //Add Model LE
                            genIdLE = Guid.NewGuid().ToString();
                            modelLE.id = genIdLE;
                            modelLE.budgetId = genId;
                            modelLE.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.descripion = "";
                            BudgetLEList.Add(modelLE);
                        }

                        if (BudgetLEList.Any())
                        {
                            if (modelBudget.chanelId == "")
                            {

                            }

                            if (dt.Columns[ii].ToString().Contains("TT"))
                            {
                                for (int aa = 1; aa < dtCMKT_TT.Columns.Count; aa++)
                                {
                                    bgActTypeModel.id = Guid.NewGuid().ToString();
                                    bgActTypeModel.budgetId = genId;
                                    bgActTypeModel.budgetLEId = genIdLE;
                                    bgActTypeModel.actTypeId = listsActType.Where(x => x.name.ToLower().Contains(dtCMKT_TT.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                                    bgActTypeModel.amount = dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[aa].ToString()].ToString()));

                                    //bgActTypeModel.amount = dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString()));
                                }
                            }

                            if (dt.Columns[ii].ToString().Contains("CVM"))
                            {
                                for (int aa = 1; aa < dtCMKT_CVM.Columns.Count; aa++)
                                {
                                    bgActTypeModel.id = Guid.NewGuid().ToString();
                                    bgActTypeModel.budgetId = genId;
                                    bgActTypeModel.budgetLEId = genIdLE;
                                    bgActTypeModel.actTypeId = listsActType.Where(x => x.name.ToLower().Contains(dtCMKT_CVM.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                                    bgActTypeModel.amount = dtCMKT_CVM.Rows[i][dtCMKT_CVM.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_CVM.Rows[i][dtCMKT_CVM.Columns[aa].ToString()].ToString()));

                                    //bgActTypeModel.amount = dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString()));
                                }
                            }
                            if (dt.Columns[ii].ToString().Contains("MT"))
                            {
                                for (int aa = 1; aa < dtCMKT_MTM.Columns.Count; aa++)
                                {
                                    bgActTypeModel.id = Guid.NewGuid().ToString();
                                    bgActTypeModel.budgetId = genId;
                                    bgActTypeModel.budgetLEId = genIdLE;
                                    bgActTypeModel.actTypeId = listsActType.Where(x => x.name.ToLower().Contains(dtCMKT_MTM.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                                    bgActTypeModel.amount = dtCMKT_MTM.Rows[i][dtCMKT_MTM.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_MTM.Rows[i][dtCMKT_MTM.Columns[aa].ToString()].ToString()));

                                    //bgActTypeModel.amount = dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString()));
                                }
                            }

                            if (dt.Columns[ii].ToString().Contains("ONT"))
                            {
                                for (int aa = 1; aa < dtCMKT_ONT.Columns.Count; aa++)
                                {
                                    bgActTypeModel.id = Guid.NewGuid().ToString();
                                    bgActTypeModel.budgetId = genId;
                                    bgActTypeModel.budgetLEId = genIdLE;
                                    bgActTypeModel.actTypeId = listsActType.Where(x => x.name.ToLower().Contains(dtCMKT_ONT.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                                    bgActTypeModel.amount = dtCMKT_ONT.Rows[i][dtCMKT_ONT.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_ONT.Rows[i][dtCMKT_ONT.Columns[aa].ToString()].ToString()));

                                    //bgActTypeModel.amount = dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString()));
                                }
                            }
                            if (dt.Columns[ii].ToString().Contains("SSC"))
                            {
                                for (int aa = 1; aa < dtCMKT_SSC.Columns.Count; aa++)
                                {
                                    bgActTypeModel.id = Guid.NewGuid().ToString();
                                    bgActTypeModel.budgetId = genId;
                                    bgActTypeModel.budgetLEId = genIdLE;
                                    bgActTypeModel.actTypeId = listsActType.Where(x => x.name.ToLower().Contains(dtCMKT_SSC.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                                    bgActTypeModel.amount = dtCMKT_SSC.Rows[i][dtCMKT_SSC.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_SSC.Rows[i][dtCMKT_SSC.Columns[aa].ToString()].ToString()));

                                    //bgActTypeModel.amount = dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[a].ToString()].ToString()));
                                }
                            }



                        }
                        //Add BudgetControl ActType
                    }
                }

                //------------------------ Prepare data for BudgetControl by Brand -----------------
                for (int i = 0; i < dtBrand.Rows.Count; i++)
                {
                    BudgetControlModels modelBudget = new BudgetControlModels();
                    BudgetControl_LEModel modelLE = new BudgetControl_LEModel();
                    for (int ii = 1; ii < dtBrand.Columns.Count; ii++)
                    {
                        genId = Guid.NewGuid().ToString();
                        if (listsActType.Where(x => x.name.Contains(dtBrand.Columns[ii].ToString())).Any())
                        {
                            modelBudget.id = genId;
                            modelBudget.brandId = dtBrand.Rows[i]["brandId"].ToString();
                            modelBudget.budgetGroupType = "brand";
                            modelBudget.amount = dtBrand.Rows[i][dtBrand.Columns[ii].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtBrand.Rows[i][dtBrand.Columns[ii].ToString()].ToString()));
                            modelBudget.chanelId = "";
                            modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelBudget.EO = ""; //genauto
                            modelBudget.budgetNo = ""; //genauto
                            modelBudget.createdByUserId = UtilsAppCode.Session.User.empId;
                            budgetList.Add(modelBudget);
                        }

                        if (budgetList.Any())
                        {
                            //Add Model LE
                            genIdLE = Guid.NewGuid().ToString();
                            modelLE.id = genIdLE;
                            modelLE.budgetId = genId;
                            modelLE.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.descripion = "";
                            BudgetLEList.Add(modelLE);

                        }

                    }
                }


                //var result = ImportBudgetControlAppCode.InsertBudgetControl(AppCode.StrCon, modelBudget);

                resultAjax.Success = true;

            }
            catch (Exception ex)
            {
                resultAjax.Message = ex.Message;

            }
            return Json(resultAjax, "text/plain");
        }
    }
}