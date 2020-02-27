using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            adminUserModel.getCompany = AdminUserAppCode.getCompany();

            return View(adminUserModel);

        }


        public ActionResult insertUsers(AdminUserModel.AdminUserModels model)
        {
            try
            {
                //clear user before insert
                AdminUserAppCode.delUserandAuthorByEmpId(Request.Form["txtEmpCode"]);
                //Insert Role User
                if (model.chkRole.Any())
                {
                    foreach (var item in model.chkRole)
                    {
                        AdminUserAppCode.insertRole(Request.Form["txtEmpCode"], item);
                    }
                }

                if (model.chkProductType != null)
                {
                    foreach (var item in model.chkProductType)
                    {
                        if (model.companyList[0] == @ConfigurationManager.AppSettings["companyId_MT"] || 
                            model.companyList[0] == @ConfigurationManager.AppSettings["companyId_TBM"])
                        {
                            if (model.custLi == null)
                            {
                                AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], model.companyList[0], null, item,"");
                            }
                            else
                            {
                                foreach (var itemCust in model.custLi)
                                {
                                    AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], model.companyList[0], itemCust, item,"");
                                }
                            }
                        }
                        else
                        {
                            if (model.regionList == null)
                            {
                                AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], model.companyList[0], null, item,"");
                            }
                            else
                            {
                                foreach (var region in model.regionList)
                                {
                                    AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], model.companyList[0], "", item, region);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //other Company
                    AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], model.companyList[0], null, "", "");
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("insertUsers => " + ex.Message);
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
                var customerLists = AdminUserAppCode.getcustomerRoleByEmpId(empId);
                userModel.customerLists = customerLists.Where(x => x.cusId != "").ToList();
                userModel.regionList = QueryGetAllRegion.getRegoinByEmpId(empId);
                var resultData = new
                {
                    userLists = userModel.userLists,
                    regionList = userModel.regionList,
                    customerLists = userModel.customerLists.GroupBy(grp => grp.cusId).Select(group => new
                    {
                        cusId = group.First().cusId,
                        customerName = group.First().customerName
                    }).OrderBy(x => x.customerName).ToList(),
                    productTypeList = customerLists,
                    companyId = customerLists.Any()? customerLists.FirstOrDefault().companyId: customerLists.FirstOrDefault().companyId,
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



        public JsonResult delUserandAuthor(string empId)
        {
            var result = new AjaxResult();
            try
            {
                result.Code = AdminUserAppCode.delUserandAuthorByEmpId(empId);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                ExceptionManager.WriteError("delUserandAuthor => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}