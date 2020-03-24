using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using eActForm.BusinessLayer;

namespace eActForm.Controllers
{
    public class orderProductController : Controller
    {
        string strRemark = "พนักงาน {0} ชื่อ {1} เบอร์ติดต่อ {2} email {3} สั่งสินค้าส่งจังหวัด {4} หน่วย {5}";
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
                model.activityFormModel.documentDate = DateTime.Now;
                model.activityFormModel.createdDate = DateTime.Now;
                model.activityFormModel.updatedDate = DateTime.Now;
                model.activityFormModel.updatedByUserId = model.activityFormModel.empId;
                model.activityFormModel.createdByUserId = model.activityFormModel.empId;
                model.activityFormModel.companyId = ConfigurationManager.AppSettings["companyId_Thaibev"];
                model.activityFormModel.activityDetail = string.Format(strRemark,model.activityFormModel.empId
                    , model.activityFormModel.customerName
                    , model.activityFormModel.empTel
                    , model.activityFormModel.empEmail
                    , model.activityFormModel.regionName
                    , model.activityFormModel.activityName);

                if ( ActivityFormCommandHandler.insertActivityForm(model.activityFormModel) > 0 ){
                    foreach (CostThemeDetail detail in model.costthemedetail)
                    {

                        detail.id = Guid.NewGuid().ToString();
                        detail.activityId = model.activityFormModel.id;
                        detail.createdDate = DateTime.Now;
                        detail.updatedDate = DateTime.Now;
                        detail.createdByUserId = model.activityFormModel.empId;
                        detail.updatedByUserId = model.activityFormModel.empId;
                        ActivityFormCommandHandler.insertEstimate(detail);
                    }
                }

                resultAjax.Code = 200;
            }
            catch(Exception ex)
            {
                resultAjax.Code = 500;
                resultAjax.Message = ex.Message;
            }
            return Json(resultAjax);
        }
    }
}