using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using static eActForm.Models.AdminUserModel;

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
            adminUserModel.roleList = AdminUserAppCode.getAllRole();
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
                    foreach (var itemCompany in model.companyList)
                    {
                        foreach (var item in model.chkProductType)
                        {
                            if (itemCompany == ConfigurationManager.AppSettings["companyId_MT"])
                            {
                                if (model.custLi != null)
                                {
                                    foreach (var itemCust in model.custLi)
                                    {
                                        AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], itemCompany, itemCust, item, "");
                                    }
                                }
                                else
                                {
                                    AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], itemCompany, null, item, "");
                                }
                            }
      
                            if (itemCompany == ConfigurationManager.AppSettings["companyId_OMT"])
                            {
                                if (model.regionList == null)
                                {
                                    AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], itemCompany, null, item, "");
                                }
                                else
                                {
                                    foreach (var region in model.regionList)
                                    {
                                        AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], itemCompany, "", item, region);
                                    }
                                }
                            }


                        }
                    }
                }
                else
                {
                    //other Company
                    foreach (var itemCompany in model.companyList)
                    {
                        AdminUserAppCode.insertAuthorized(Request.Form["txtEmpCode"], itemCompany, null, "", "");
                    }
                }
            }
            catch (Exception ex)
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
                userModel.roleList = AdminUserAppCode.getAllRole();
                var resultData = new
                {
                    roleList = userModel.roleList,
                    userLists = userModel.userLists,
                    regionList = userModel.regionList,
                    customerLists = userModel.customerLists.GroupBy(grp => grp.cusId).Select(group => new
                    {
                        cusId = group.First().cusId,
                        customerName = group.First().customerName
                    }).OrderBy(x => x.customerName).ToList(),
                    productTypeList = customerLists,
                    companyList = customerLists.ToList(),
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