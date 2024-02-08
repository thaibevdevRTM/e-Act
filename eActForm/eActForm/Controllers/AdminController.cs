using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.CommandHandler;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Models.MasterData;
using eForms.Presenter.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebLibrary;
using static eActForm.Models.Activity_Model;
using static eActForm.Models.TB_Act_ProductPrice_Model;
using static iTextSharp.text.pdf.AcroFields;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(string typeForm)
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.productBrandList = QueryGetAllBrand.GetAllBrand().Where(x => x.companyId == activityType.MT.ToString()).ToList();
            activityModel.productSmellLists = new List<TB_Act_Product_Model.ProductSmellModel>();

            activityModel.productcatelist = QuerygetAllProductCate.getAllProductCate().ToList();
            activityModel.productGroupList = QueryGetAllProductGroup.getAllProductGroup();
            activityModel.activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                .GroupBy(item => item.activitySales)
                .Select(grp => new Models.TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList();

            ViewBag.TypeForm = typeForm;

            return View(activityModel);
        }
        public ActionResult ProductDetail()
        {
            TB_Act_Product_Model.ProductList productModel = new TB_Act_Product_Model.ProductList(); 

            productModel.productLists = QueryGetAllProduct.getAllProductSetting();

            return PartialView(productModel);
        }

        public ActionResult productPriceDetail(string productcode, string typeForm)
        {
            TB_Act_ProductPrice_Model model = new TB_Act_ProductPrice_Model();

            model.ProductPriceList = QueryGetAllPrice.getPriceByProductCode(productcode, typeForm);

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
            if (QueryGetAllProduct.getAllProductSetting().Where(x => x.productCode == p_productCode).Any())
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

            //add productprice MT
            result.Code = AdminCommandHandler.insertProduct(model);

            var getCustomerAll = QueryGetAllCustomers.getAllCustomers();
            if (getCustomerAll.Any())
            {
                int countinsert = 0;
                foreach (var item in getCustomerAll)
                {
                    //Insert ProductPrice MT
                    countinsert = AdminCommandHandler.insertProductPrice(model.productCode, item.id);
                    countinsert++;
                }
                //Insert ProductPrice OMT
                countinsert = AdminCommandHandler.insertProductPrice(model.productCode, Activity_Model.activityType.OMT.ToString());

            }





            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateProduct(TB_Act_Product_Model.Product_Model model)
        {
            var result = new AjaxResult();

            result.Code = AdminCommandHandler.updateProduct(model);

            List<TB_Act_ProductPrice_Model.ProductPrice> productPriceList = new List<TB_Act_ProductPrice_Model.ProductPrice>();
            productPriceList = QueryGetAllPrice.getPriceByProductCode(model.productCode, "");

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


        public ActionResult Index_ImportProductPrice()
        {

            return View();
        }

        public ActionResult View_ProductPrice()
        {
            TB_Act_ProductPrice_Model pList = new TB_Act_ProductPrice_Model();
            pList.ProductPriceList = (List<ProductPrice>)TempData["TempProductPriceList"];
            TempData.Remove("TempProductPriceList");
            return PartialView(pList);
        }


        public JsonResult ImportFlieProductPrice(ImportFlowModel.ImportFlowModels model)
        {
            var resultAjax = new AjaxResult();
            try
            {
               
                string resultFilePath = "";
                int resultInert = 0;
                int CountFile = model.InputFile.Count();
                for (int i = 0; i < CountFile; i++)
                {
                    string genUniqueName = "ProductPrice_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + UtilsAppCode.Session.User.empId;
                    string extension = Path.GetExtension(model.InputFile[i].FileName);
                    string resultFileName = genUniqueName + extension;
                    resultFilePath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles_ProductPrice"], resultFileName));
                    model.InputFile[i].SaveAs(resultFilePath);
                }
                DataTable dt = new DataTable();
                dt = ExcelAppCode.ReadExcel(resultFilePath, "ProductPrice", "A:AB");


                var rtnDelete = AdminUserAppCode.deleteTempProductPrice(AppCode.StrCon, UtilsAppCode.Session.User.empId);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductPrice modelProductPrice = new ProductPrice();
                    modelProductPrice.customerName = dt.Rows[i]["cusNameEN"].ToString();
                    modelProductPrice.productId = dt.Rows[i]["productId"].ToString();
                    modelProductPrice.normalCost = Convert.ToDecimal(dt.Rows[i]["normalCost"] == DBNull.Value ? 0m : dt.Rows[i]["normalCost"]);
                    modelProductPrice.wholeSalesPrice = Convert.ToDecimal(dt.Rows[i]["wholeSalesPrice"] == DBNull.Value ? 0m : dt.Rows[i]["wholeSalesPrice"]);
                    modelProductPrice.discount1 = Convert.ToDecimal(dt.Rows[i]["discount1"] == DBNull.Value ? 0m : dt.Rows[i]["discount1"]);
                    modelProductPrice.discount2 = Convert.ToDecimal(dt.Rows[i]["discount2"] == DBNull.Value ? 0m : dt.Rows[i]["discount2"]);
                    modelProductPrice.discount3 = Convert.ToDecimal(dt.Rows[i]["discount3"] == DBNull.Value ? 0m : dt.Rows[i]["discount3"]);
                    modelProductPrice.saleNormal = Convert.ToDecimal(dt.Rows[i]["saleNormal"] == DBNull.Value ? 0m : dt.Rows[i]["saleNormal"]);
                    modelProductPrice.createdByUserId = UtilsAppCode.Session.User.empId;
                    resultInert = AdminUserAppCode.InserToTempProductPrice(AppCode.StrCon, modelProductPrice);

                }

                var ProductPriceList = AdminUserAppCode.getProductPriceAterImport(AppCode.StrCon, UtilsAppCode.Session.User.empId);
                TempData["TempProductPriceList"] = ProductPriceList;

                if (ProductPriceList.Any())
                    resultAjax.Success = true;




                return Json(resultAjax, "text/plain");
            }
            catch(Exception ex)
            {
                resultAjax.Success = false;
                ExceptionManager.WriteError("ImportFlieProductPrice => " + ex.Message);
                return Json(resultAjax, "text/plain");
            }
        }



        public JsonResult ConfirmImportProductPrice()
        {
            var result = new AjaxResult();
            try
            {
                int countResult = AdminUserAppCode.confirmInsertProductPrice(AppCode.StrCon, UtilsAppCode.Session.User.empId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                ExceptionManager.WriteError("ConfirmImportProductPrice => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult updateSetDisableProduct(TB_Act_Product_Model.Product_Model model)
        {
            var result = new AjaxResult();
            try
            {
                int countResult = AdminCommandHandler.updateProduct(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                ExceptionManager.WriteError("updateSetDisableProduct => " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}