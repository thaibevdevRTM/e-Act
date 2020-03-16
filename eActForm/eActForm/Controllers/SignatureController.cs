using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eActForm.Models;
using eActForm.BusinessLayer;
using System.Configuration;
using System.IO;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SignatureController : Controller
    {
        // GET: Signature
        public ActionResult Index(ActSignatureModel.SignModel Model)
        {
            if (string.IsNullOrEmpty(Model.empId))
            {
                Model.empId = UtilsAppCode.Session.User.empId;
                Model.empName = UtilsAppCode.Session.User.empFNameTH + " " + UtilsAppCode.Session.User.empLNameTH;
                Model.empCompanyName = QueryGet_empDetailById.getEmpDetailById(Model.empId).FirstOrDefault().companyName;
                Model.positionTitle = UtilsAppCode.Session.User.empPositionTitleTH;
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createSignature(ActSignatureModel.SignModel model, IEnumerable<HttpPostedFileBase> files)
        {
            model.id = Guid.NewGuid().ToString();
            model.createdByUserId = UtilsAppCode.Session.User.empId;
            model.createdDate = DateTime.Now;
            model.updatedByUserId = UtilsAppCode.Session.User.empId;
            model.updatedDate = DateTime.Now;
            List<ActSignatureModel.SignModel> list = new List<ActSignatureModel.SignModel>();
            list.Add(model);


            if (Request.Files[0].ContentLength > 0)
            {
                var f = files.ToList();
                MemoryStream target = new MemoryStream();
                f[0].InputStream.CopyTo(target);
                list[0].signature = target.ToArray();
            }

            int rtn = SignatureAppCode.signatureInsert(AppCode.ToDataTable(list));
            if (rtn > 0)
            {
                UtilsAppCode.Session.writeFileHistory(System.Web.HttpContext.Current.Server
                    , model.signature
                    , string.Format(ConfigurationManager.AppSettings["rootSignaURL"], model.empId));
            }

            model.signature = null;
            return RedirectToAction("Index", model);



        }

        public ActionResult listsSignature(string empId)
        {
            ActSignatureModel.SignModels models = new ActSignatureModel.SignModels();
            models.lists = SignatureAppCode.signatureGetByEmpId(empId);

            List<RequestEmpModel> emp = new List<RequestEmpModel>();
            emp = QueryGet_empDetailById.getEmpDetailById(empId);
            ActSignatureModel.SignModel singna = new ActSignatureModel.SignModel();
            if (!models.lists.Any())
            {

                singna.empId = emp[0].empId;
                singna.empName = emp[0].empName;
                singna.empCompanyName = emp[0].companyName;
                singna.positionTitle = emp[0].position;
               
                models.lists.Add(singna);
            }
            else
            {

                models.lists[0].empId = emp[0].empId;
                models.lists[0].empName = emp[0].empName;
                models.lists[0].empCompanyName = emp[0].companyName;
                models.lists[0].positionTitle = emp[0].position;
            }

            return PartialView(models);
        }

        public ActionResult currentSignature(string empId)
        {
            ActSignatureModel.SignModels models = SignatureAppCode.currentSignatureByEmpId(empId);
            ActSignatureModel.SignModel model = (models.lists == null || models.lists.Count == 0) ? null : models.lists[0];
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult searchEmp(ActSignatureModel.SignModel model)
        {
            //ActSignatureModel.SignModel model = new ActSignatureModel.SignModel();
            try
            {


                string empid = model.empId;
                List<RequestEmpModel> emp = new List<RequestEmpModel>();

                emp = QueryGet_empDetailById.getEmpDetailById(empid);

                model.empId = emp[0].empId;
                model.empName = emp[0].empName;
                model.empCompanyName = emp[0].companyName;
                model.positionTitle = emp[0].position;


            }
            catch (Exception ex)
            {

                // ExceptionManager.WriteError("insertDataActivityMainForm => " + ex.Message);
            }

            return RedirectToAction("Index", model);

            // return Json(result);
        }
     
    }
}