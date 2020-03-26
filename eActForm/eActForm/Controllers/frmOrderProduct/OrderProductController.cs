using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;
using eForms.Presenter.MasterData;
using eForms.Models.MasterData;

namespace eActForm.Controllers
{
    public class orderProductController : Controller
    {
        string strRemark = "พนักงาน {0} ชื่อ {1} เบอร์ติดต่อ {2} email {3} สั่งสินค้าส่งจังหวัด {4} หน่วย {5}";
        string strBodyDetail = "สินค้า {0} ราคาต่อหน่วย {1} จำนวน {2} ราคา {3} <br/>";
        // GET: OrderProduct
        public ActionResult index()
        {
            string actId = Guid.NewGuid().ToString();
            ViewBag.actId = actId;
            return View();
        }

        [HttpPost] //post method
        [ValidateAntiForgeryToken]
        public JsonResult submitOrder(Activity_Model model)
        {
            var resultAjax = new AjaxResult();
            try
            {
                string strProductDetail = "";
                model.activityFormModel.documentDate = DateTime.Now;
                model.activityFormModel.createdDate = DateTime.Now;
                model.activityFormModel.updatedDate = DateTime.Now;
                model.activityFormModel.updatedByUserId = model.activityFormModel.empId;
                model.activityFormModel.createdByUserId = model.activityFormModel.empId;
                model.activityFormModel.companyId = ConfigurationManager.AppSettings["companyId_Thaibev"];
                model.activityFormModel.activityDetail = string.Format(strRemark, model.activityFormModel.empId
                    , model.activityFormModel.customerName
                    , model.activityFormModel.empTel
                    , model.activityFormModel.empEmail
                    , model.activityFormModel.regionName
                    , model.activityFormModel.activityName);

                if (ActivityFormCommandHandler.insertActivityForm(model.activityFormModel) > 0)
                {
                    foreach (CostThemeDetail detail in model.costthemedetail)
                    {
                        detail.id = Guid.NewGuid().ToString();
                        detail.activityId = model.activityFormModel.id;
                        detail.createdDate = DateTime.Now;
                        detail.updatedDate = DateTime.Now;
                        detail.createdByUserId = model.activityFormModel.empId;
                        detail.updatedByUserId = model.activityFormModel.empId;
                        strProductDetail += detail.unit > 0 ? string.Format(strBodyDetail, detail.productDetail, detail.wholeSalesPrice, detail.unit, detail.total) : "";
                        ActivityFormCommandHandler.insertEstimate(detail);
                    }
                }

                List<SalesTeamCVMModel> contactModel = SalesTeamPresenter.getSalesTeamCVMBySaleTeamId(AppCode.StrConAuthen, model.activityFormModel.reference);
                string strBody = string.Format(ConfigurationManager.AppSettings["bodyEmailConfirmOrderProduct"]
                    , model.activityFormModel.customerName
                    , strProductDetail
                    , "จังหวัด " + model.activityFormModel.regionName + model.activityFormModel.activityName
                    , contactModel[0].nameCashier1
                    , contactModel[0].telCashier);

                EmailAppCodes.sendEmail(model.activityFormModel.empEmail
                    , ConfigurationManager.AppSettings["emailApproveCC"] // model.activityFormModel.contactEmail
                    , ConfigurationManager.AppSettings["subjectEmailConfirm"]
                    , strBody
                    , null);

                resultAjax.Code = 200;
            }
            catch (Exception ex)
            {
                resultAjax.Code = 500;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax);
        }
    }
}