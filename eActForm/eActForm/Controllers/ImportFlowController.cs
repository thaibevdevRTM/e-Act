using eActForm.BusinessLayer;
using eActForm.Models;
using eForms.Models.MasterData;
using eForms.Presenter.AppCode;
using eForms.Presenter.MasterData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class ImportFlowController : Controller
    {
        // GET: ImportFlow
        public ActionResult Index()
        {
            ImportFlowModel model = new ImportFlowModel();
            if (UtilsAppCode.Session.User.isSuperAdmin)
            {
                model.masterTypeFormList = QueryGetMasterForm.getAllMasterTypeForm(AppCode.StrCon);
            }
            else if (AppCode.compPomForm.Contains(UtilsAppCode.Session.User.empCompanyGroup))
            {
                model.masterTypeFormList = QueryGetMasterForm.getAllMasterTypeForm(AppCode.StrCon).Where(x => x.department == ConfigurationManager.AppSettings["conditionActBeer"]).ToList();
            }
            else if (UtilsAppCode.Session.User.isAdminTBM)
            {
                model.masterTypeFormList = QueryGetMasterForm.getAllMasterTypeForm(AppCode.StrCon).Where(x => x.companyId == ConfigurationManager.AppSettings["companyId_TBM"]).ToList();
            }


            return View(model);
        }

        public ActionResult detailList()
        {

            ImportFlowModel.ImportFlowModels model = (ImportFlowModel.ImportFlowModels)TempData["importFlowModel"];
            TempData.Keep();
            return PartialView(model);
        }

        public JsonResult ImportFlie(ImportFlowModel.ImportFlowModels model)
        {
            var resultAjax = new AjaxResult();
            try
            {
                string resultFilePath = "";
                int resultInert = 0;
                int CountFile = model.InputFile.Count();
                for (int i = 0; i < CountFile; i++)
                {
                    string genUniqueName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + UtilsAppCode.Session.User.empId;
                    string extension = Path.GetExtension(model.InputFile[i].FileName);
                    string resultFileName = genUniqueName + extension;
                    resultFilePath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles"], resultFileName));
                    model.InputFile[i].SaveAs(resultFilePath);
                }
                DataTable dt = new DataTable();
                dt = ExcelAppCode.ReadExcel(resultFilePath, "Import", "A:AB");


                var rtnDelete = ImportFlowPresenter.deleteTempFlow(AppCode.StrCon, UtilsAppCode.Session.User.empId);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["company"].ToString()))
                    {
                        ImportFlowModel.ImportFlowModels modelFlow = new ImportFlowModel.ImportFlowModels();

                        modelFlow.masterTypeId = model.masterTypeId;
                        modelFlow.company = dt.Rows[i]["company"].ToString();
                        modelFlow.companyId = dt.Rows[i]["companyId"].ToString();
                        modelFlow.actType = dt.Rows[i]["actType"].ToString();
                        modelFlow.subject = dt.Rows[i]["subject"].ToString();
                        modelFlow.customer = dt.Rows[i]["customer"].ToString();
                        modelFlow.customerId = ImportFlowPresenter.checkValueForImport(dt.Rows[i]["customerId"].ToString());
                        modelFlow.productCate = dt.Rows[i]["productCate"].ToString();
                        modelFlow.productCateId = ImportFlowPresenter.checkValueForImport(dt.Rows[i]["productCateId"].ToString());
                        modelFlow.productType = dt.Rows[i]["productType"].ToString();
                        modelFlow.productTypeId = ImportFlowPresenter.checkValueForImport(dt.Rows[i]["productTypeId"].ToString());
                        modelFlow.productBrand = dt.Rows[i]["productBrand"].ToString();
                        modelFlow.productBrandId = ImportFlowPresenter.checkValueForImport(dt.Rows[i]["productBrandId"].ToString());
                        modelFlow.channel = dt.Rows[i]["channel"].ToString();
                        modelFlow.channelId = ImportFlowPresenter.checkValueForImport(dt.Rows[i]["channelId"].ToString());
                        modelFlow.department = dt.Rows[i]["department"].ToString();
                        modelFlow.departmentId = ImportFlowPresenter.checkValueForImport(dt.Rows[i]["departmentId"].ToString());
                        modelFlow.limitBegin = dt.Rows[i]["limitBegin"].ToString();
                        modelFlow.limitTo = dt.Rows[i]["limitTo"].ToString();
                        modelFlow.limitDisplay = dt.Rows[i]["limitDisplay"].ToString();
                        modelFlow.rang = dt.Rows[i]["rank"].ToString();
                        modelFlow.approveGroup = dt.Rows[i]["approveGroup"].ToString();
                        modelFlow.approveGroupId = ImportFlowPresenter.checkValueForImport(dt.Rows[i]["approveGroupId"].ToString());
                        modelFlow.IsShow = dt.Rows[i]["IsShow"].ToString().ToLower() == "Yes".ToLower() ? "1" : "0";
                        modelFlow.IsApprove = dt.Rows[i]["IsApprove"].ToString().ToLower() == "Yes".ToLower() ? "1" : "0";
                        modelFlow.empId = dt.Rows[i]["empId"].ToString().Trim();
                        modelFlow.empGroup = dt.Rows[i]["empGroup"].ToString().Trim();
                        modelFlow.name = dt.Rows[i]["name"].ToString();
                        modelFlow.createdByUserId = UtilsAppCode.Session.User.empId;
                        //modelFlow.flowId = ImportFlowPresenter.getFlowIdByDetail(AppCode.StrCon, modelFlow, false, "");

                        resultInert = ImportFlowPresenter.InserToTemptFlow(AppCode.StrCon, modelFlow);

                    }

                }

                model.importFlowList = ImportFlowPresenter.getFlowAterImport(AppCode.StrCon, UtilsAppCode.Session.User.empId);
                TempData["importFlowModel"] = model;

                if (model.importFlowList.Any())
                    resultAjax.Success = true;


            }
            catch (Exception ex)
            {
                resultAjax.Message = ex.Message;

            }
            return Json(resultAjax, "text/plain");
        }

        public JsonResult InsertFlow()
        {
            int result = 0;
            var resultAjax = new AjaxResult();
            string keepFlow = "", strSubject = "", limitBegin = "",channel="", getSubjectId = "";
            bool checkSubject = false;
            try
            {
                ImportFlowModel.ImportFlowModels model = (ImportFlowModel.ImportFlowModels)TempData["importFlowModel"];

                if (model.importFlowList.Any())
                {
                    foreach (var item in model.importFlowList)
                    {
                        if (item.checkFlowExist != true)
                        {
                            if (model.masterTypeId == MainAppCode.masterTypePaymentVoucher && !string.IsNullOrEmpty(item.productBrandId))
                            {
                                //ใบสั่งจ่าย ช่องทาง brand จะมี subjectId แค่อันเดียว
                                item.flowId = ImportFlowPresenter.getFlowIdByDetail(AppCode.StrCon, item, true, MainAppCode.subjectPaymentVoucherId);
                            }
                            else if (model.masterTypeId == MainAppCode.masterTypeActivityBudget)
                            {
                                if (!string.IsNullOrEmpty(item.productBrandId))
                                {
                                    //budget TBM สำหรับช่องทาง Brand 
                                    var getSubject = SubjectQuery.getAllSubject(AppCode.StrCon).Where(x => x.nameTH.Contains("งบประมาณกิจกรรม")).ToList();
                                    getSubjectId = getSubject.Where(x => x.nameTH.Contains(item.actType)).FirstOrDefault().id;
                                    item.flowId = ImportFlowPresenter.getFlowIdByDetail(AppCode.StrCon, item, true, getSubjectId);
                                }
                                else
                                {
                                    var getSubject = SubjectQuery.getAllSubject(AppCode.StrCon).Where(x => x.nameTH.Contains(item.subject)).ToList();
                                    
                                    if(!getSubject.Any())
                                    {
                                        getSubjectId = ImportFlowPresenter.insertSubject(AppCode.StrCon, item);
                                        item.flowId = ImportFlowPresenter.getFlowIdByDetail(AppCode.StrCon, item, true, getSubject.FirstOrDefault().id);
                                    }
                                    else
                                    {
                                        item.flowId = ImportFlowPresenter.getFlowIdByDetail(AppCode.StrCon, item, true, getSubject.FirstOrDefault().id);
                                    }


                                }
                            }
                            else if (ImportFlowPresenter.checkFormAddSubject(AppCode.StrCon, model.masterTypeId))
                            {
                                if ((item.subject != strSubject && item.limitBegin == limitBegin) || (item.subject != strSubject && item.limitBegin != limitBegin))
                                {

                                    if (model.masterTypeId == ConfigurationManager.AppSettings["formCR_IT_FRM_314"] || model.masterTypeId == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])
                                    {
                                        var getSubject = SubjectQuery.getAllSubject(AppCode.StrCon).Where(x => x.nameTH.Contains(item.subject)).ToList();
                                        if (!getSubject.Any())
                                        {
                                            getSubjectId = ImportFlowPresenter.insertSubject(AppCode.StrCon, item);
                                        }
                                        else
                                        {
                                            getSubjectId = getSubject.FirstOrDefault().id;
                                        }

                                    }
                                    else
                                    {
                                        //check insert subject
                                        getSubjectId = ImportFlowPresenter.insertSubject(AppCode.StrCon, item);
                                    }

                                    checkSubject = true;

                                    item.flowId = ImportFlowPresenter.getFlowIdByDetail(AppCode.StrCon, item, checkSubject, getSubjectId);
                                    keepFlow = item.flowId;
                                }
                                else if (item.subject == strSubject && item.limitBegin != limitBegin || channel != item.channel)
                                {
                                    checkSubject = true;
                                    item.flowId = ImportFlowPresenter.getFlowIdByDetail(AppCode.StrCon, item, checkSubject, getSubjectId);
                                    keepFlow = item.flowId;
                                }
                                else
                                {
                                    item.flowId = keepFlow;
                                }
                            }
                            else
                            {
                                item.flowId = ImportFlowPresenter.getFlowIdByDetail(AppCode.StrCon, item, checkSubject, "");
                            }
                            result += ImportFlowPresenter.InsertFlow(AppCode.StrCon, item);
                            strSubject = item.subject;
                            limitBegin = item.limitBegin;
                            channel = item.channel;

                        }
                    }
                    resultAjax.Success = true;
                }

            }
            catch (Exception ex)
            {
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax, "text/plain");
        }
    }
}