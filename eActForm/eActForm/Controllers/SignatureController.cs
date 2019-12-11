﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
using System.Configuration;
namespace eActForm.Controllers
{
    [LoginExpire]
    public class SignatureController : Controller
    {
        // GET: Signature
        public ActionResult Index()
        {
            ActSignatureModel.SignModel model = new ActSignatureModel.SignModel();
            model.empId = UtilsAppCode.Session.User.empId;
            return View(model);
        }

        public ActionResult createSignature(ActSignatureModel.SignModel model)
        {
            model.id = Guid.NewGuid().ToString();
            model.createdByUserId = UtilsAppCode.Session.User.empId;
            model.createdDate = DateTime.Now;
            model.updatedByUserId = UtilsAppCode.Session.User.empId;
            model.updatedDate = DateTime.Now;
            List<ActSignatureModel.SignModel> list = new List<ActSignatureModel.SignModel>();
            list.Add(model);
            int rtn = SignatureAppCode.signatureInsert(AppCode.ToDataTable(list));
            if (rtn > 0)
            {
                UtilsAppCode.Session.writeFileHistory(System.Web.HttpContext.Current.Server
                    , model.signature
                    , string.Format(ConfigurationManager.AppSettings["rootSignaURL"], UtilsAppCode.Session.User.empId));
            }
            return RedirectToAction("Index");
        }

        public ActionResult listsSignature(string empId)
        {
            ActSignatureModel.SignModels models = new ActSignatureModel.SignModels();
            models.lists = SignatureAppCode.signatureGetByEmpId(empId);
            return PartialView(models);
        }

        public ActionResult currentSignature(string empId)
        {
            ActSignatureModel.SignModels models = SignatureAppCode.currentSignatureByEmpId(empId);
            ActSignatureModel.SignModel model = (models.lists == null || models.lists.Count == 0) ? null : models.lists[0];
            return PartialView(model);
        }
    }
}