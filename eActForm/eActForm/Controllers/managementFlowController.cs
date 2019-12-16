using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public class ManagementFlowController : Controller
    {
        // GET: managementFlow
        public ActionResult Index()
        {
            ManagementFlow_Model model = new ManagementFlow_Model();
            model.companyList = managementFlowAppCode.getCompany();

            return View(model);
        }

        public ActionResult dropDetail(string companyId)
        {
            ManagementFlow_Model model = new ManagementFlow_Model();
            try
            {
                model.subjectList = managementFlowAppCode.getSubject(companyId);
                model.customerList = managementFlowAppCode.getCustomer(companyId);
                model.getLimitList = managementFlowAppCode.getLimit();
                model.cateList = managementFlowAppCode.getProductCate(companyId);
                model.chanelList = managementFlowAppCode.getChanel("data");
                model.productBrandList = managementFlowAppCode.getProductBrand();


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("dropDetail => " + ex.Message);
            }
            return PartialView(model);
        }

        //public JsonResult getApproveFlowDataList()
        //{
        //    var result = new AjaxResult();
        //    try
        //    {
        //        //var result = ApproveFlowAppCode.getFlowApproveGroupByType();
        //    }
        //    catch(Exception ex)
        //    {

        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult approveList(getDataList_Model model)
        {
            ManagementFlow_Model management_Model = new ManagementFlow_Model();
            management_Model.approveFlow = ApproveFlowAppCode.getFlowApproveGroupByType(model);
            management_Model.approveGroupList = managementFlowAppCode.getApproveGroup();
            management_Model.getDDLShowApproveList = managementFlowAppCode.getApproveShow();
            management_Model.getDDlApproveList = managementFlowAppCode.getApprove();

            return PartialView(management_Model);
        }
    }
}