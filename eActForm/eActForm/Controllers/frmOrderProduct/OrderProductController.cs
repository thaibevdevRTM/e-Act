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
using eForms.Presenter.Forms;

namespace eActForm.Controllers
{
    public class orderProductController : Controller
    {
        string strRemark = "พนักงาน {0} ชื่อ {1} เบอร์ติดต่อ {2} email {3} สั่งสินค้าส่งจังหวัด {4} หน่วย {5}";
        string strBodyDetail = "สินค้า {0} ราคาต่อหน่วย {1} จำนวน {2} ราคา {3} <br/>";
        string strOrderDupInWeek = "สามารถสั่งได้สัปดาห์ละ 10 แพ็คค่ะ";
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
                DateTime baseDate = DateTime.Today;
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday: baseDate = baseDate.AddDays(-2); break;
                    case DayOfWeek.Tuesday: baseDate = baseDate.AddDays(-3); break;
                }
                var yesterday = baseDate.AddDays(-1);
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek).AddDays(+3);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                int countOrderUnit = activityFormOrderProductPresenter.getCountOrderProductOnweek(AppCode.StrCon, model.activityFormModel.empId, thisWeekStart.ToString(), thisWeekEnd.ToString());
                int sumOrder = model.costthemedetail.Select(x => x.unit).Sum();//(from CostThemeDetail x in model.costthemedetail select x).Sum();
                if ((countOrderUnit + sumOrder) > 10)
                {
                    resultAjax.Code = 501;
                    resultAjax.Message = strOrderDupInWeek;
                    resultAjax.Message += (countOrderUnit > 0) ? " ท่านได้สั่งซื้อไปแล้วจำนวน " + countOrderUnit + " แพ็คค่ะ " : "";
                }
                else
                {
                    #region order product
                    string strProductDetail = "";
                    model.activityFormModel.theme = model.activityFormModel.customerName;
                    model.activityFormModel.objective = model.activityFormModel.empTel;
                    model.activityFormModel.productCateId = model.activityFormModel.empEmail;
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
                        , "จังหวัด " + model.activityFormModel.regionName + " " + model.activityFormModel.activityName
                        , contactModel[0].nameCashier
                        , contactModel[0].telCashier
                        , string.Format(ConfigurationManager.AppSettings["bodyEmailLinkLocation"], contactModel[0].latitude, contactModel[0].longitude)
                        , contactModel[0].nameContact
                        , contactModel[0].emailContact
                        , contactModel[0].telContact
                        , thisWeekEnd.AddDays(4).ToString("dd/MM/yyyy")
                        );

                    EmailAppCodes.sendEmail(model.activityFormModel.empEmail
                        , ConfigurationManager.AppSettings["emailApproveCC"] // contactModel[0].emailCashier
                        , ConfigurationManager.AppSettings["subjectEmailConfirm"]
                        , strBody
                        , null);

                    resultAjax.Code = 200;
                    #endregion
                }
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