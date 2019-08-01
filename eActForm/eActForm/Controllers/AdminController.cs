﻿using eActForm.BusinessLayer;
using eActForm.BusinessLayer.CommandHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.productBrandList = QueryGetAllBrand.GetAllBrand();
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
            //add productprice.....
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

    }
}