using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class SharedMasterFormDetailController : Controller
    {
        public ActionResult textDetailsPay(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
        public ActionResult listDetails(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
        public ActionResult textDetailsAttachPay(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
        public ActionResult textDetailsCreateByV1(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
        public ActionResult textDetailsBlankRows(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
        public ActionResult showSignatureV1(MainFormModel mainFormModel)
        {
            return PartialView(mainFormModel);
        }
    }
}