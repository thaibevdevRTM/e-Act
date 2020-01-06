using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;
using static eActForm.Models.ApproveFlowModel;

namespace eActForm.Controllers
{
    public class ManagementFlowController : Controller
    {
        // GET: managementFlow
        public ActionResult Index()
        {
            ManagementFlow_Model model = new ManagementFlow_Model();
            try
            {
                model.companyList = managementFlowAppCode.getCompany().Where(w => w.val1.Contains(UtilsAppCode.Session.User.empCompanyId)).ToList();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ManagementFlow => Index => " + ex.Message);
            }
            return View(model);
        }

        public ActionResult dropDetail(string companyId)
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

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ManagementFlow => dropDetail => " + ex.Message);
            }
            return PartialView(model);
        }

        public ActionResult genDataApproveList(getDataList_Model model)
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            management_Model.approveFlow = ApproveFlowAppCode.getFlowApproveGroupByType(model);
            management_Model.approveGroupList = managementFlowAppCode.getApproveGroup();
            management_Model.getDDLShowApproveList = managementFlowAppCode.getApproveShow();
            management_Model.getDDlApproveList = managementFlowAppCode.getApprove();
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
                ExceptionManager.WriteError("ManagementFlow => approveList => " + ex.Message);
            }
            return PartialView(management_Model);
        }

       
        public JsonResult insertFlowApprove(ManagementFlow_Model model)
        {
            var result = new AjaxResult();
            try
            {
                result.Success = false;
                if (managementFlowAppCode.insertFlowApprove(model) > 0)
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
                ExceptionManager.WriteError("ManagementFlow => insertFlowApprove => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult addRow()
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            try
            {
                management_Model = (ManagementFlow_Model)TempData["management_Model"];
                flowApproveDetail flowDetail_Model = new flowApproveDetail();
                flowDetail_Model.rangNo = management_Model.approveFlow.flowDetail.OrderBy(x=>x.rangNo).Last().rangNo + 1;
                flowDetail_Model.id = Guid.NewGuid().ToString();
                management_Model.approveFlow.flowDetail.Add(flowDetail_Model);

                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ManagementFlow => addRow => " + ex.Message);
            }
            return RedirectToAction("approveList");
        }


        public ActionResult delRow(string id)
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            try
            {
                management_Model = (ManagementFlow_Model)TempData["management_Model"];
                flowApproveDetail flowDetail_Model = new flowApproveDetail();
                management_Model.approveFlow.flowDetail.RemoveAll(r => r.id == id);

                TempData.Keep();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ManagementFlow => delRow => " + ex.Message);
            }
            return RedirectToAction("approveList");
        }

        public JsonResult getLimitBySubject(string subjectId)
        {
            var result = new AjaxResult();
            try
            {
                var lists = managementFlowAppCode.getLimit(subjectId);
                result.Data = lists;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("ManagementFlow => getLimitBySubject => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}