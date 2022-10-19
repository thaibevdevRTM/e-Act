﻿using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.MasterData;
using eForms.Presenter.AppCode;
using eForms.Presenter.MasterData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using static eActForm.Models.ApproveFlowModel;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ManagementFlowController : Controller
    {
        // GET: managementFlow
        public ActionResult Index()
        {
            ManagementFlow_Model model = new ManagementFlow_Model();
            if (UtilsAppCode.Session.User.isSuperAdmin)
            {
                model.companyList = managementFlowAppCode.getCompany();
                model.subjectList = new List<TB_Reg_Subject_Model>();
            }
            else
            {
                model.companyList = managementFlowAppCode.getCompany().Where(w => w.val1.Contains(UtilsAppCode.Session.User.empCompanyId)).ToList();
                model.subjectList = new List<TB_Reg_Subject_Model>();
            }

            return View(model);
        }

        public ActionResult dropDetail(string companyId, string typeFlow, string subjectId)
        {
            ManagementFlow_Model model = new ManagementFlow_Model();
            try
            {

                model.customerList = managementFlowAppCode.getCustomer(companyId);
                model.departmentMasterList = departmentMasterPresenter.getdepartmentMaster(AppCode.StrCon, companyId);
                model.cateList = managementFlowAppCode.getProductCate(companyId);

                model.productTypeList = managementFlowAppCode.getProductType();

                if (subjectId == ConfigurationManager.AppSettings["subjectSetPriceOMT"])
                {
                    model.regionList = QueryGetAllRegion.getAllRegion().Where(x => x.condition == "OMT").ToList();
                }
                else
                {
                    model.regionList = QueryGetAllRegion.getAllRegion().Where(x => x.condition == ConfigurationManager.AppSettings["conditionActBeer"] && x.nameShot == companyId).ToList();

                }

                if (ReportSummaryAppCode.getCompanyMTMList().Where(x => x.id.Equals(companyId)).Any())
                {
                    model.activityGroupList = BusinessLayer.QueryGetAllActivityGroup.getAllActivityGroup()
                   .Where(x => x.activityCondition.Contains(Activity_Model.activityType.MT.ToString()))
                   .GroupBy(item => item.activitySales)
                   .Select(grp => new Models.TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();
                }
                else
                {
                    model.activityGroupList = BusinessLayer.QueryGetAllActivityGroup.getAllActivityGroup()
                    .Where(x => x.activityCondition.Contains(ConfigurationManager.AppSettings["conditionActBeer"]))
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new Models.TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();
                }



                model.typeFlow = typeFlow;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("dropDetail => " + ex.Message);
            }
            return PartialView(model);
        }

        public ActionResult genDataApproveList(getDataList_Model model, string typeFlow)
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            try
            {
                
                management_Model.approveFlow = ApproveFlowAppCode.getFlowApproveGroupByType(model, typeFlow);
                management_Model.approveGroupList = managementFlowAppCode.getApproveGroup();
                management_Model.getDDLShowApproveList = managementFlowAppCode.getApproveShow();
                management_Model.getDDlApproveList = managementFlowAppCode.getApprove();
                management_Model.getDDLActiveList = managementFlowAppCode.getActive();
                management_Model.typeFlow = typeFlow;
                management_Model.p_productType = model.productTypeId;
                management_Model.p_productCateId = model.productCateId;
                management_Model.p_productBrandId = model.productBrandId;
                management_Model.p_flowLimitId = model.flowLimitId;
                management_Model.p_channelId = model.channelId;
                management_Model.p_subjectId = model.subjectId;
                management_Model.activityTypeId = model.activityGroup;
                management_Model.customerId = model.customerId;
                management_Model.p_deparmentId = model.deparmentId;
                management_Model.p_companyId = management_Model.approveFlow.flowDetail.Any() ? management_Model.approveFlow.flowDetail[0].companyId : model.companyId;

                TempData["management_Model"] = management_Model;
            }
            catch(Exception ex)
            {
                TempData["management_Model"] = new ManagementFlow_Model() ;
                ExceptionManager.WriteError("ManagementFlowController >> genDataApproveList => " + ex.Message);
            }
            return RedirectToAction("approveList");
        }


        public ActionResult approveList()
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            try
            {
                management_Model = (ManagementFlow_Model)TempData["management_Model"];
                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("approveList => " + ex.Message);
            }
            return PartialView(management_Model);
        }


        public JsonResult insertFlowApprove(ManagementFlow_Model model)
        {
            var result = new AjaxResult();
            try
            {
                result.Success = false;
                var countRow = 0;
                if (model.typeFlow == Activity_Model.typeFlow.flowAddOn.ToString())
                {
                    countRow = managementFlowAppCode.insertFlowApproveAddOn(model);
                }
                else
                {
                    countRow = managementFlowAppCode.insertFlowApprove(model);
                }



                if (countRow > 0)
                {
                    result.Success = true;
                }
                else
                {
                    result.Message = AppCode.StrMessFail;
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("insertFlowApprove => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult addRow()
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            try
            {
                management_Model = (ManagementFlow_Model)TempData["management_Model"];
                flowApproveDetail flowDetail_Model = new flowApproveDetail("");
                if (management_Model.approveFlow.flowDetail.Any())
                {
                    flowDetail_Model.rangNo = management_Model.approveFlow.flowDetail.OrderBy(x => x.rangNo).Last().rangNo + 1;

                }
                else
                {
                    flowDetail_Model.rangNo = 1;
                }
                flowDetail_Model.id = Guid.NewGuid().ToString();
                management_Model.approveFlow.flowDetail.Add(flowDetail_Model);

                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("addRow => " + ex.Message);
            }
            return RedirectToAction("approveList");
        }


        public ActionResult delRow(string id)
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            try
            {
                management_Model = (ManagementFlow_Model)TempData["management_Model"];
                flowApproveDetail flowDetail_Model = new flowApproveDetail("");
                management_Model.approveFlow.flowDetail.RemoveAll(r => r.id == id);
                var result = managementFlowAppCode.delFlowAddOnByEmpId(id);

                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("delRow => " + ex.Message);
            }
            return RedirectToAction("approveList");
        }

        public ActionResult delRowSwap(string id)
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            try
            {
                var result = managementFlowAppCode.delFlowApproveByEmpId(id);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("delRowSwap => " + ex.Message);
            }
            return RedirectToAction("approveList");
        }

        public JsonResult getLimitBySubject(string subjectId, string companyId)
        {
            var result = new AjaxResult();
            try
            {
                if (subjectId == ConfigurationManager.AppSettings["formTrvHcmId"] || subjectId == ConfigurationManager.AppSettings["formExpMedNumId"])
                {
                    companyId = "";
                }


                var lists = managementFlowAppCode.getLimit(subjectId, companyId);
                result.Data = lists;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getLimitBySubject => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEmp(string subjectId, string limitId, string channelId, string actType, string customerId, string companyId)
        {
            List<RequestEmpModel> empList = new List<RequestEmpModel>();
            try
            {
                empList = ApproveFlowAppCode.getEmpByConditon(subjectId, limitId, channelId, actType, customerId, companyId);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ManagementFlowController >> getEmp => " + ex.Message);
            }
            return Json(empList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCompanyByEmpId(string empId)
        {
            var result = new AjaxResult();
            try
            {
                List<ManagentFlowModel.flowSubject> flowSubjectList = new List<ManagentFlowModel.flowSubject>();
                flowSubjectList = pManagementFlowAppCode.getFlowApproveByEmpId(AppCode.StrCon, empId)
                    .GroupBy(item => new { item.companyName, item.companyId })
                    .Select((group, index) => new ManagentFlowModel.flowSubject
                    {
                        companyId = group.First().companyId,
                        companyName = group.First().companyName,
                    }).ToList();

                var resultData = new
                {
                    companyList = flowSubjectList.Select(x => new
                    {
                        Value = x.companyId,
                        Text = x.companyName
                    }).ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getFlowSwap(string empId, string[] companyList)
        {
            ManagentFlowModel flowSubject = new ManagentFlowModel();
            try
            {
                flowSubject.flowSubjectsList = pManagementFlowAppCode.getFlowApproveByEmpId(AppCode.StrCon, empId).Where(w => companyList.Contains(w.companyId)).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowSwap => " + ex.Message);
            }

            return PartialView(flowSubject);
        }


        public JsonResult insertFlowAddOn(string[] selectRow, string newEmpId)
        {
            var result = new AjaxResult();

            ManagentFlowModel flowSubject = new ManagentFlowModel();
            try
            {
                foreach (var item in selectRow)
                {

                    result.Code = pManagementFlowAppCode.updateSwapByApproveId(AppCode.StrCon, item, newEmpId, UtilsAppCode.Session.User.empId);

                }
                result.Success = true;
                //flowSubject.flowSubjectsList = pManagementFlowAppCode.getFlowApproveByEmpId(AppCode.StrCon, empId).Where(w => companyList.Contains(w.companyId)).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("insertFlowAddOn => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Index_EmpMoveCompany()
        {

            return View();
        }

        public ActionResult list_EmpMoveCompany(string empId)
        {
            ManagementFlow_Model model = new ManagementFlow_Model();
            try
            {

                model.flowSubjectList = pManagementFlowAppCode.getFlowbyEmpId(AppCode.StrCon, empId);
                model.companyList = managementFlowAppCode.getCompany();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("list_EmpMoveCompany => " + ex.Message);
            }
            return PartialView(model);

        }

        public JsonResult updateCompanyByEmpId(string flowId, string empId, string companyId)
        {
            var result = new AjaxResult();

            ManagentFlowModel flowSubject = new ManagentFlowModel();
            try
            {
                result.Code = pManagementFlowAppCode.updateCompanyFlowByEmpId(AppCode.StrCon, flowId, companyId, empId, UtilsAppCode.Session.User.empId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("updateCompanyByEmpId => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getChannelBySubject(string subjectId)
        {
            var result = new AjaxResult();
            try
            {
                var lists = QueryGetAllChanel.getChanelBySubjectId(subjectId);
                result.Data = lists;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getChannelBySubject => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getBrandBySubject(string subjectId)
        {
            var result = new AjaxResult();
            try
            {


                var lists = QueryGetAllBrand.GetBrandBySubject(subjectId);
                result.Data = lists;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getBrandBySubject => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult getSubjectByCompany(string companyId)
        {
            var result = new AjaxResult();
            try
            {

                var lists = managementFlowAppCode.getSubject(companyId);
                result.Data = lists;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getSubjectByCompany => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}