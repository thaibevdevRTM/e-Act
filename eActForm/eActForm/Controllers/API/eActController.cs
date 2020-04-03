﻿using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public partial class eActController : Controller
    {

        // GET: eAct
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getChanel(string Id)
        {
            var result = new AjaxResult();
            try
            {
                var getcustomer = QueryGetAllCustomers.getAllCustomers().Where(x => x.id == Id).FirstOrDefault();
                var getchanel = QueryGetAllChanel.getAllChanel().Where(x => x.id == getcustomer.chanel_Id).FirstOrDefault();
                result.Data = new
                {
                    Chanel_group = getchanel.chanelGroup,
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, "text/plain");
        }


        public JsonResult getProductGroup(string cateId)
        {
            var result = new AjaxResult();
            try
            {
                var getProductGroup = QueryGetAllProductGroup.getAllProductGroup().Where(x => x.cateId.Trim() == cateId.Trim()).ToList();
                var resultData = new
                {
                    //productGroup = getProductGroup.GroupBy(item => item.productGroup)
                    //.Select(group => new TB_Act_Product_Cate_Model.Product_Cate_Model
                    //{
                    //    id = group.First().id,
                    //    productGroup = group.First().productGroup,
                    //}).ToList(),
                    productGroup = getProductGroup.ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getProductSmell(string productGroupId)
        {
            var result = new AjaxResult();
            try
            {
                var lists = QueryGetAllProduct.getProductSmellByGroupId(productGroupId).OrderBy(x => x.nameTH).ToList();
                result.Data = lists;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getProductBrand(string p_groupId)
        {
            var result = new AjaxResult();
            try
            {
                var getProductBrand = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId.Trim() == p_groupId.Trim()).ToList();
                var resultData = new
                {
                    getProductname = getProductBrand.Select(x => new
                    {
                        Value = x.id,
                        Text = x.brandName
                    }).ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult getddlSize(string Id)
        //{
        //    var result = new AjaxResult();
        //    try
        //    {
        //        List<TB_Act_Product_Model.Product_Model> productModel = new List<TB_Act_Product_Model.Product_Model>();

        //        productModel = QueryGetAllProduct.getAllProduct().Where(x => x.brandId == Id).ToList();
        //        var resultData = new
        //        {
        //            getProductSize = productModel.Select(x => new
        //            {
        //                Value = x.id,
        //                Text = x.size
        //            }).ToList(),
        //        };
        //        result.Data = resultData;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Success = false;
        //        result.Message = ex.Message;
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult getProductDetail(string brandId, string smellId, string productGroupId)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_Product_Model.Product_Model> getProductDetail = new List<TB_Act_Product_Model.Product_Model>();
                if (smellId != "" && brandId != "")
                {
                    getProductDetail = QueryGetAllProduct.getProductBySmellIdAndBrandId(smellId, brandId);
                }
                else if (smellId != "")
                {
                    getProductDetail = QueryGetAllProduct.getProductBySmellId(smellId, productGroupId);
                }
                else
                {
                    getProductDetail = QueryGetAllProduct.getProductByBrandId(brandId);
                }

                var getProductsizee = getProductDetail.GroupBy(item => item.size)
                      .Select(group => new TB_Act_Product_Model.Product_Model
                      {
                          id = group.First().brandId,
                          size = group.First().size,
                      }).OrderByDescending(x => x.size).ToList();
                getProductsizee = getProductsizee.OrderByDescending(x => x.size).ToList();


                var resultData = new
                {
                    getProductsize = getProductDetail.GroupBy(item => item.size)
                     .Select(group => new TB_Act_Product_Model.Product_Model
                     {
                         id = group.First().brandId,
                         size = group.First().size,
                     }).OrderByDescending(x => x.size).ToList(),

                    getProductname = getProductDetail.Select(x => new
                    {
                        Value = x.id,
                        Text = x.productName
                    }).OrderBy(x => x.Text).ToList(),
                };

                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getddlProduct(string size, string brandId, string smellId, string productGroupId)
        {
            int psize = size == "" ? 0 : int.Parse(size);
            var result = new AjaxResult();
            try
            {
                List<TB_Act_Product_Model.Product_Model> productModel = new List<TB_Act_Product_Model.Product_Model>();
                if (size != "")
                {

                    if (smellId != "" && brandId != "")
                    {
                        productModel = QueryGetAllProduct.getAllProduct(productGroupId).Where(x => x.brandId == brandId && x.smellId == smellId && x.size == psize).ToList();
                    }
                    else if (smellId == "")
                    {
                        productModel = QueryGetAllProduct.getAllProduct(productGroupId).Where(x => (x.brandId == brandId) && x.size == psize).ToList();
                    }
                    else if (brandId == "")
                    {
                        productModel = QueryGetAllProduct.getAllProduct(productGroupId).Where(x => (x.smellId == smellId) && x.size == psize).ToList();
                    }
                    else
                    {
                        productModel = QueryGetAllProduct.getAllProduct(productGroupId).Where(x => (x.brandId == brandId || x.smellId == smellId) && x.size == psize).ToList();
                    }
                }
                else
                {
                    productModel = QueryGetAllProduct.getAllProduct(productGroupId).Where(x => x.brandId == brandId).ToList();
                    if (smellId != "")
                    {
                        productModel = productModel.Where(x => x.smellId == smellId).ToList();
                    }
                }

                var resultData = new
                {
                    getProductname = productModel.Select(x => new
                    {
                        Value = x.id,
                        Text = x.productName
                    }).ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCustomerByCompany(string companyId, string txtCus)
        {


            List<TB_Act_Customers_Model.Customers_Model> customerList = new List<TB_Act_Customers_Model.Customers_Model>();
            try
            {
                if (companyId == "5600")
                {
                    customerList = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameTH.Contains(txtCus)).ToList();
                }
                else
                {
                    customerList = QueryGetAllCustomers.getCustomersOMT().Where(x => x.cusNameTH.Contains(txtCus)).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCustomerByRegion => " + ex.Message);
            }
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }




        public JsonResult getCustomerByRegion(string regionId, string txtCus)
        {

            List<TB_Act_Customers_Model.Customers_Model> customerList = new List<TB_Act_Customers_Model.Customers_Model>();
            try
            {
                customerList = QueryGetAllCustomers.getCustomersOMT().Where(x => x.regionId == regionId && x.cusNameTH.Contains(txtCus)).ToList();

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCustomerByRegion => " + ex.Message);
            }
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEmpDetailById(string empId)
        {
            bool langEn = Request.Cookies[ConfigurationManager.AppSettings["nameCookieLanguageEact"]].Value.ToString() == ConfigurationManager.AppSettings["cultureEng"];
            List<RequestEmpModel> empDetailList = new List<RequestEmpModel>();
            var result = new AjaxResult();
            try
            {
                empDetailList = QueryGet_empDetailById.getEmpDetailById(empId).ToList();

                var resultData = new
                {
                    empName = !langEn ? empDetailList.FirstOrDefault().empName : empDetailList.FirstOrDefault().empNameEN,
                    position = !langEn ? empDetailList.FirstOrDefault().position : empDetailList.FirstOrDefault().positionEN,
                    level = empDetailList.FirstOrDefault().level,
                    department = !langEn ? empDetailList.FirstOrDefault().department : empDetailList.FirstOrDefault().departmentEN,
                    bu = !langEn ? empDetailList.FirstOrDefault().bu : empDetailList.FirstOrDefault().buEN,
                    companyName = !langEn ? empDetailList.FirstOrDefault().companyName : empDetailList.FirstOrDefault().companyNameEN,
                    compId = empDetailList.FirstOrDefault().compId,
                    email = empDetailList.FirstOrDefault().email
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpDetailById => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAllRegion(string txtRegion)
        {

            List<TB_Act_Region_Model> regionList = new List<TB_Act_Region_Model>();
            try
            {
                regionList = QueryGetAllRegion.getAllRegion().Where(x => x.descTh.Contains(txtRegion)).ToList();

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllRegion => " + ex.Message);
            }
            return Json(regionList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult textThaiBaht(string txtBaht)
        {
            try
            {
                txtBaht = ActFormAppCode.convertThaiBaht(decimal.Parse(txtBaht));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllRegion => " + ex.Message);
            }
            return Json(txtBaht, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getOtherMasterByType(string type, string subType, string text)
        {
            List<TB_Act_Other_Model> getOtherList = new List<TB_Act_Other_Model>();
            try
            {
                getOtherList = QueryOtherMaster.getOhterMaster(type, subType).Where(x => x.displayVal.Contains(text)).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllRegion => " + ex.Message);
            }
            return Json(getOtherList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCashLimitByEmpId(string empId)
        {
            List<CashEmpModel> cashEmpList = new List<CashEmpModel>();
            var result = new AjaxResult();
            try
            {
                cashEmpList = QueryGetBenafit.getCashLimitByEmpId(empId).ToList();
                if (cashEmpList.Count > 0)
                {
                    var resultData = new
                    {
                        ProductDetail_0 = cashEmpList[0].cashPerDay,
                        ProductDetail_1 = cashEmpList[1].cashPerDay,
                    };

                    result.Data = resultData;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpDetailById => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEmpByChannel(string subjectId, string channelId,string filter)
        {
            List<RequestEmpModel> empList = new List<RequestEmpModel>();
            try
            {
                empList = exPerryCashAppCode.getEmpByChannel(subjectId, channelId, filter);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpByChannel => " + ex.Message);
            }
            return Json(empList, JsonRequestBehavior.AllowGet);
        }


    }
}