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
    [LoginExpire]
    public class AdminUserController : Controller
    {
        // GET: AdminUser
        public ActionResult Index()
        {
            AdminUserModel adminUserModel = new AdminUserModel();
            adminUserModel.userLists = AdminUserAppCode.getAllUserRole();


            return View(adminUserModel);

        }


        public ActionResult insertUsers(AdminUserModel.AdminUserModels model)
        {
            //Insert Role User
            if (model.chkRole.Any())
            {
                foreach (var item in model.chkRole)
                {
                    AdminUserAppCode.insertRole(Request.Form["txtEmpCode"], item);
                }
            }

            if (model.chkProductType.Any())
            {
                foreach (var item in model.chkProductType)
                {
                    foreach (var itemCust in model.custLi)
                    {
                        AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], Request.Form["ddlCompany"], itemCust, item);
                    }
                }
            }


            return RedirectToAction("Index");
        }

        public JsonResult getUserRoleByEmpId(string empId)
        {

            AdminUserModel userModel = new AdminUserModel();
            var result = new AjaxResult();
            try
            {
                userModel.userLists = AdminUserAppCode.getUserRoleByEmpId(empId);
                userModel.customerLists = AdminUserAppCode.getcustomerRoleByEmpId(empId);
                var resultData = new
                {
                    userLists = userModel.userLists,
                    customerLists = userModel.customerLists.GroupBy(grp => grp.cusId).Select(group => new
                    {
                        cusId = group.First().cusId,
                        customerName = group.First().customerName
                    }).ToList(),
                    productTypeList = userModel.customerLists,
                    companyId = userModel.customerLists.FirstOrDefault().companyId,
                    empId = userModel.userLists.FirstOrDefault().empId
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("getUserRoleByEmpId => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}