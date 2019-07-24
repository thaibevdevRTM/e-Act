using eActForm.BusinessLayer;
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


        public JsonResult addNewProduct(string p_cateId, string p_groupId, string p_brandId, string p_size, string p_pack, string p_productName, string p_productCode)
        {
            var result = new AjaxResult();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateProduct(string p_cateId, string p_groupId, string p_brandId, string p_size, string p_pack, string p_productName, string p_productCode)
        {
            var result = new AjaxResult();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}