using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.CommandHandler;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;
using static eActForm.Models.Activity_Model;

namespace eActForm.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.productBrandList = QueryGetAllBrand.GetAllBrand().Where(x => x.companyId == activityType.MT.ToString()).ToList();
            activityModel.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();

            activityModel.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
            activityModel.productGroupList = QueryGetAllProductGroup.getAllProductGroup();
            activityModel.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                .GroupBy(item => item.activitySales)
                .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

            return View(activityModel);
        }
        public ActionResult ProductDetail()
        {
            TB_Act_Product_Model.ProductList productModel = new TB_Act_Product_Model.ProductList();

            productModel.productLists = QueryGetAllProduct.getAllProduct("");

            return PartialView(productModel);
        }

        public ActionResult productPriceDetail(string productcode)
        {
            TB_Act_ProductPrice_Model model = new TB_Act_ProductPrice_Model();

            model.ProductPriceList = QueryGetAllPrice.getPriceByProductCode(productcode);

            return PartialView(model);
        }

        public JsonResult onchangePrice(TB_Act_ProductPrice_Model.ProductPrice model)
        {
            var result = new AjaxResult();
            try
            {

                AdminCommandHandler.updatePriceProduct(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult delProductAndPrice(string productId)
        {
            var result = new AjaxResult();
            try
            {

                AdminCommandHandler.delProductMasterAndPrice(productId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult checkProduct(string p_productCode)
        {
            var result = new AjaxResult();
            TB_Act_Product_Model.ProductList productModel = new TB_Act_Product_Model.ProductList();
            if (QueryGetAllProduct.getAllProduct("").Where(x => x.productCode == p_productCode).Any())
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult addNewProduct(TB_Act_Product_Model.Product_Model model)
        {
            var result = new AjaxResult();


            result.Code = AdminCommandHandler.insertProduct(model);
            //add productprice MT
            var getCustomerAll = QueryGetAllCustomers.getAllCustomers();
            if (getCustomerAll.Any())
            {
                int countinsert = 0;
                foreach (var item in getCustomerAll)
                {
                    countinsert = AdminCommandHandler.insertProductPrice(model.productCode, item.id);
                    countinsert++;
                }

            }





            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateProduct(TB_Act_Product_Model.Product_Model model)
        {
            var result = new AjaxResult();

            result.Code = AdminCommandHandler.updateProduct(model);

            List<TB_Act_ProductPrice_Model.ProductPrice> productPriceList = new List<TB_Act_ProductPrice_Model.ProductPrice>();
            productPriceList = QueryGetAllPrice.getPriceByProductCode(model.productCode);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult manageCustomer()
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.regionGroupList = QueryGetAllRegion.getAllRegion();
            activityModel.companyList = managementFlowAppCode.getCompany().Where(x => x.val1.Contains(UtilsAppCode.Session.User.empCompanyId)).ToList();

            return View(activityModel);
        }

        public ActionResult customerList(string companyId)
        {

            Activity_Model activityModel = new Activity_Model();
            if (!string.IsNullOrEmpty(companyId))
            {
                if (companyId == ConfigurationManager.AppSettings["companyId_OMT"])
                {
                    activityModel.customerslist = QueryGetAllCustomers.getCustomersOMT().OrderBy(x => x.regionId).ToList();
                }
                else
                {
                    activityModel.customerslist = QueryGetAllCustomers.getCustomersMT().ToList();
                }
            }
            else
            {
                activityModel.customerslist = new List<TB_Act_Customers_Model.Customers_Model>();
            }
            return PartialView(activityModel);
        }


        public JsonResult insertCustomer(Activity_Model model)
        {
            var result = new AjaxResult();
            try
            {
                string fineReplace = model.customerModel.cusNameTH;
                int count = fineReplace.IndexOf("(");
                if (count != -1)
                {
                    model.customerModel.cusNameTH = fineReplace.Substring(0, count);
                }
                int countResult = AdminUserAppCode.insertCustomer(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                ExceptionManager.WriteError("getFlowSwap => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult delCustomer(string id)
        {
            var result = new AjaxResult();
            try
            {
                int countResult = AdminUserAppCode.delCustomer(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                ExceptionManager.WriteError("delCustomer => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}