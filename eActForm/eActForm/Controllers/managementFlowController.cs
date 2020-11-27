using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using eForms.Models.MasterData;
using eForms.Presenter.AppCode;
using System;
using System.Collections.Generic;
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
            }
            else
            {
                model.companyList = managementFlowAppCode.getCompany().Where(w => w.val1.Contains(UtilsAppCode.Session.User.empCompanyId)).ToList();
            }

            return View(model);
        }

        public ActionResult dropDetail(string companyId, string typeFlow)
        {
            ManagementFlow_Model model = new ManagementFlow_Model();
            try
            {
                model.subjectList = managementFlowAppCode.getSubject(companyId);
                model.customerList = managementFlowAppCode.getCustomer(companyId);
                //model.getLimitList = managementFlowAppCode.getLimit();
                model.cateList = managementFlowAppCode.getProductCate(companyId);
                model.chanelList = managementFlowAppCode.getChanel("data");
                model.productBrandList = managementFlowAppCode.getProductBrand();
                model.productTypeList = managementFlowAppCode.getProductType();
                model.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                    .Where(x => x.activityCondition.Contains("mtm".ToLower()))
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();
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

            management_Model.p_companyId = management_Model.approveFlow.flowDetail.Any() ? management_Model.approveFlow.flowDetail[0].companyId : model.companyId;

            TempData["management_Model"] = management_Model;
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

        public JsonResult getEmp(string subjectId, string limitId, string channelId)
        {
            List<RequestEmpModel> empList = new List<RequestEmpModel>();
            try
            {
                empList = ApproveFlowAppCode.getEmpByConditon(subjectId, limitId, channelId);
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


        public ActionResult getFlowSwap(string empId,string[] companyList )
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


        public JsonResult insertFlowAddOn(string[] ApproveIdList, string[] selectRow,string newEmpId)
        {
            var result = new AjaxResult();

            ManagentFlowModel flowSubject = new ManagentFlowModel();
            try
            {
                int i = 0;

                foreach(var item in ApproveIdList)
                {
                    if(selectRow[i].ToString() == "true" && i != ApproveIdList.Count())
                    {
                        result.Code = pManagementFlowAppCode.updateSwapByApproveId(AppCode.StrCon, item, newEmpId , UtilsAppCode.Session.User.empId);
                    }
                    i++;
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
    }
}