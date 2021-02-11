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

                dt = ExcelAppCode.ReadExcel(resultFilePath, "BG-L1", "A:Z");
                dtBrand = ExcelAppCode.ReadExcel(resultFilePath, "B_BRAND", "A:Z");

                var getLE = ImportBudgetControlAppCode.getLE_No(AppCode.StrCon);

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
                            BudgetControlModels modelBudget = new BudgetControlModels();
                            genId = Guid.NewGuid().ToString();
                            modelBudget.id = genId;
                            modelBudget.companyId = model.companyId;
                            modelBudget.brandId = dt.Rows[i]["brandId"].ToString();
                            modelBudget.budgetGroupType = ImportBudgetControlAppCode.channel;
                            modelBudget.amount = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i][dt.Columns[ii].ToString()].ToString()));
                            modelBudget.totalChannel = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i]["Total Channel"].ToString()));
                            modelBudget.totalBG = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i]["Total BG"].ToString()));
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

                            //if (BudgetLEList.Any())
                            //{

                            //    if (dt.Columns[ii].ToString().Contains("TT"))
                            //    {
                            //        for (int aa = 1; aa < dtCMKT_TT.Columns.Count; aa++)
                            //        {
                            //            BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                            //            bgActTypeModel.id = Guid.NewGuid().ToString();
                            //            bgActTypeModel.budgetId = genId;
                            //            bgActTypeModel.budgetLEId = genIdLE;
                            //            bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Contains(dtCMKT_TT.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                            //            bgActTypeModel.amount = dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_TT.Rows[i][dtCMKT_TT.Columns[aa].ToString()].ToString()));
                            //            bgActTypeModel.createdByUserId = UtilsAppCode.Session.User.empId;
                            //            bgActTypeList.Add(bgActTypeModel);
                            //        }
                            //    }

                            //    if (dt.Columns[ii].ToString().Contains("CVM"))
                            //    {
                            //        for (int aa = 1; aa < dtCMKT_CVM.Columns.Count; aa++)
                            //        {
                            //            BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                            //            bgActTypeModel.id = Guid.NewGuid().ToString();
                            //            bgActTypeModel.budgetId = genId;
                            //            bgActTypeModel.budgetLEId = genIdLE;
                            //            bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Contains(dtCMKT_CVM.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                            //            bgActTypeModel.amount = dtCMKT_CVM.Rows[i][dtCMKT_CVM.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_CVM.Rows[i][dtCMKT_CVM.Columns[aa].ToString()].ToString()));
                            //            bgActTypeModel.createdByUserId = UtilsAppCode.Session.User.empId;
                            //            bgActTypeList.Add(bgActTypeModel);
                            //        }
                            //    }
                            //    if (dt.Columns[ii].ToString().Contains("MT"))
                            //    {
                            //        for (int aa = 1; aa < dtCMKT_MTM.Columns.Count; aa++)
                            //        {
                            //            BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                            //            bgActTypeModel.id = Guid.NewGuid().ToString();
                            //            bgActTypeModel.budgetId = genId;
                            //            bgActTypeModel.budgetLEId = genIdLE;
                            //            bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Contains(dtCMKT_MTM.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                            //            bgActTypeModel.amount = dtCMKT_MTM.Rows[i][dtCMKT_MTM.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_MTM.Rows[i][dtCMKT_MTM.Columns[aa].ToString()].ToString()));
                            //            bgActTypeModel.createdByUserId = UtilsAppCode.Session.User.empId;
                            //            bgActTypeList.Add(bgActTypeModel);
                            //        }
                            //    }

                            //    if (dt.Columns[ii].ToString().Contains("ONT"))
                            //    {
                            //        for (int aa = 1; aa < dtCMKT_ONT.Columns.Count; aa++)
                            //        {
                            //            BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                            //            bgActTypeModel.id = Guid.NewGuid().ToString();
                            //            bgActTypeModel.budgetId = genId;
                            //            bgActTypeModel.budgetLEId = genIdLE;
                            //            bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Contains(dtCMKT_ONT.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                            //            bgActTypeModel.amount = dtCMKT_ONT.Rows[i][dtCMKT_ONT.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_ONT.Rows[i][dtCMKT_ONT.Columns[aa].ToString()].ToString()));
                            //            bgActTypeModel.createdByUserId = UtilsAppCode.Session.User.empId;
                            //            bgActTypeList.Add(bgActTypeModel);
                            //        }
                            //    }
                            //    if (dt.Columns[ii].ToString().Contains("SSC"))
                            //    {
                            //        for (int aa = 1; aa < dtCMKT_SSC.Columns.Count; aa++)
                            //        {
                            //            BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                            //            bgActTypeModel.id = Guid.NewGuid().ToString();
                            //            bgActTypeModel.budgetId = genId;
                            //            bgActTypeModel.budgetLEId = genIdLE;
                            //            bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Contains(dtCMKT_SSC.Columns[aa].ToString().ToLower())).FirstOrDefault().id;
                            //            bgActTypeModel.amount = dtCMKT_SSC.Rows[i][dtCMKT_SSC.Columns[aa].ToString()].ToString() == "" ? 0 : decimal.Parse(AppCode.checkNullorEmpty(dtCMKT_SSC.Rows[i][dtCMKT_SSC.Columns[aa].ToString()].ToString()));
                            //            bgActTypeModel.createdByUserId = UtilsAppCode.Session.User.empId;
                            //            bgActTypeList.Add(bgActTypeModel);
                            //        }
                            //    }

                            //}
                            //Add BudgetControl ActType
                            runingNo++;
                        }
                    }
                }

                //------------------------ Prepare data for BudgetControl by Brand -----------------

                for (int i = 0; i < dtBrand.Rows.Count; i++)
                {
                    BudgetControlModels modelBudget = new BudgetControlModels();
                    BudgetControl_LEModel modelLE = new BudgetControl_LEModel();

                    genId = Guid.NewGuid().ToString();
                    modelBudget.id = genId;
                    modelBudget.companyId = model.companyId;
                    modelBudget.brandId = dtBrand.Rows[i]["brandId"].ToString();
                    modelBudget.budgetGroupType = ImportBudgetControlAppCode.brand;
                    modelBudget.amount = decimal.Parse(AppCode.checkNullorEmpty(dtBrand.Rows[i]["Total MKT"].ToString()));
                    modelBudget.totalBG = decimal.Parse(AppCode.checkNullorEmpty(dt.Rows[i]["Total BG"].ToString()));
                    modelBudget.chanelId = "";
                    modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                    modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                    modelBudget.EO = ImportBudgetControlAppCode.genEO(AppCode.StrCon, modelBudget); //genauto
                    modelBudget.budgetNo = ImportBudgetControlAppCode.genBudgetNo(AppCode.StrCon, modelBudget, int.Parse(getLE) + 1);  //genauto
                    modelBudget.LE = int.Parse(getLE) + 1;
                    modelBudget.createdByUserId = UtilsAppCode.Session.User.empId;
                    budgetList.Add(modelBudget);
                    //runingNo + 1


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

                if (budgetList.Any())
                {
                    foreach (var item in budgetList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetControl(AppCode.StrCon, item);
                    }
                }

                if (BudgetLEList.Any())
                {
                    var delCount = +ImportBudgetControlAppCode.InsertBudgetLE_History(AppCode.StrCon);
                    foreach (var item in BudgetLEList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetLE(AppCode.StrCon, item);
                    }
                }
                if (bgActTypeList.Any())
                {
                    var delCount = +ImportBudgetControlAppCode.InsertBudgetActType_History(AppCode.StrCon);
                    foreach (var item in bgActTypeList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetActType(AppCode.StrCon, item);
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
    }
}