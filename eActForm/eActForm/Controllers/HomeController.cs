﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            SearchActivityModels models = new SearchActivityModels();
            models.approveStatusList = ApproveAppCode.getApproveStatus();
            models.productGroupList = QueryGetAllProductGroup.getAllProductGroup();
            models.customerslist = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameEN != "").ToList();
            models.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
            models.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                .GroupBy(item => item.activitySales)
                .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

            return View(models);
        }

        public ActionResult myDoc()
        {
            Activity_Model.actForms model;
            if (TempData["SearchDataModel"] != null)
            {
                model = (Activity_Model.actForms)TempData["SearchDataModel"];
            }
            else
            {
                model = new Activity_Model.actForms();
                model.actLists = ActFormAppCode.getActFormByEmpId(UtilsAppCode.Session.User.empId);
            }
            return PartialView(model);
        }

        public ActionResult requestDeleteDoc(string actId,string statusId)
        {
            //return RedirectToAction("index");
            AjaxResult result = new AjaxResult();
            result.Success = false; 
            if( statusId == "1")
            {
                // Draft
                if(ActFormAppCode.deleteActForm(actId, "request delete by user") > 0)
                {
                    result.Success = true;
                    TempData["SearchDataModel"] = null;
                }
            }
            else
            {

            }

            return RedirectToAction("myDoc");
        }

        public ActionResult searchActForm()
        {
            string count = Request.Form.AllKeys.Count().ToString();
            Activity_Model.actForms model = new Activity_Model.actForms();
            model.actLists = ActFormAppCode.getActFormByEmpId(UtilsAppCode.Session.User.empId);

            if( Request.Form["txtActivityNo"] != "")
            {
                model.actLists = model.actLists.Where(r => r.activityNo == Request.Form["txtActivityNo"]).ToList();                             
            }

            if( Request.Form["ddlStatus"] != "")
            {
                model.actLists = model.actLists.Where(r => r.statusId == Request.Form["ddlStatus"]).ToList();
            }

            TempData["SearchDataModel"] = model;
            return RedirectToAction("myDoc");
        }

        public ActionResult logOut()
        {
            UtilsAppCode.Session.User = null;
            return RedirectToAction("index", "home");
        }

        public ActionResult contact()
        {
            return View();
        }
    }
}