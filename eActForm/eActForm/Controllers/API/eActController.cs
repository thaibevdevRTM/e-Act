﻿using eActForm.BusinessLayer;
using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using eForms.Presenter.MasterData;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WebLibrary;

namespace eActForm.Controllers
{
    public partial class eActController : Controller
    {

        // GET: eAct
        public ActionResult Index(string activityId)
        {
            //Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            //activity_TBMMKT_Model = mainReport(activityId,null);
            return PartialView();
        }

        public ActionResult previewActMT(string activityId)
        {
            //Activity_Model activityModel = new Activity_Model();
            //activityModel = ReportAppCode.previewApprove(activityId,"70008316");
            return PartialView();
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
                        Text = x.brandName,
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

        public JsonResult getProductBrandByCompany(string p_groupId,string company)
        {
            var result = new AjaxResult();
            try
            {
                var getProductBrand = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId.Trim() == p_groupId.Trim() && x.companyId == company).ToList();
                var resultData = new
                {
                    getProductname = getProductBrand.Select(x => new
                    {
                        Value = x.id,
                        Text = x.brandName,
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

        public JsonResult getDetailBrandById(string brandId)
        {
            var result = new AjaxResult();
            try
            {
                var getProductBrand = QueryGetAllBrand.GetAllBrand().Where(x => x.id.Trim() == brandId.Trim()).ToList();
                var resultData = new
                {

                    Value = getProductBrand.FirstOrDefault().id,
                    Text = getProductBrand.FirstOrDefault().brandName,
                    EO = getProductBrand.FirstOrDefault().digit_EO
,
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




        public JsonResult getALLProductBrand(string p_txtBrand)
        {
            var result = new AjaxResult();
            try
            {
                var getProductBrand = QueryGetAllBrand.GetAllBrand().Where(x => x.brandName.ToLower().Trim().Contains(p_txtBrand.ToLower().Trim())).ToList();
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

        public JsonResult getAreaByRegion(string center, string txtCus, string type)
        {

            List<eForms.Models.MasterData.TB_Act_Area_Model> areaList = new List<eForms.Models.MasterData.TB_Act_Area_Model>();
            try
            {
                areaList = QueryGetArea.getAreaByCondition(AppCode.StrCon, type).Where(x => x.center == center && x.area.Contains(txtCus)).ToList();

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCustomerByRegion => " + ex.Message);
            }
            return Json(areaList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCustomerMT(string companyId)
        {

            List<TB_Act_Customers_Model.Customers_Model> customerList = new List<TB_Act_Customers_Model.Customers_Model>();
            try
            {
                customerList = QueryGetAllCustomers.getCustomersMT().ToList();

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCustomerMT => " + ex.Message);
            }
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEmpDetailById(string empId, string typeFormId = "")
        {
            bool langEn = false;
            if (Request.Cookies[ConfigurationManager.AppSettings["nameCookieLanguageEact"]] != null)
            {
                langEn = Request.Cookies[ConfigurationManager.AppSettings["nameCookieLanguageEact"]].Value.ToString() == ConfigurationManager.AppSettings["cultureEng"];
            }
            List<RequestEmpModel> empDetailList = new List<RequestEmpModel>();
            var result = new AjaxResult();
            try
            {

                empDetailList = typeFormId == "" ? QueryGet_empDetailById.getEmpDetailById(empId).ToList()
                                                 : QueryGet_empDetailById.getEmpDetailFlowById(empId, typeFormId).ToList();

                if (empDetailList.Any())
                {
                    var resultData = new
                    {
                        empName = !langEn ? empDetailList.FirstOrDefault().empName : empDetailList.FirstOrDefault().empNameEN,
                        position = !langEn ? empDetailList.FirstOrDefault().position : empDetailList.FirstOrDefault().positionEN,
                        level = empDetailList.FirstOrDefault().level,
                        department = !langEn ? empDetailList.FirstOrDefault().department : empDetailList.FirstOrDefault().departmentEN,
                        bu = !langEn ? empDetailList.FirstOrDefault().bu : empDetailList.FirstOrDefault().buEN,
                        companyName = !langEn ? empDetailList.FirstOrDefault().companyName : empDetailList.FirstOrDefault().companyNameEN,
                        compId = empDetailList.FirstOrDefault().compId,
                        email = empDetailList.FirstOrDefault().email,
                        //hireDate = empDetailList.FirstOrDefault().hireDate
                        //hireDate = DocumentsAppCode.convertDateTHToShowCultureDateEN(Convert.ToDateTime(BaseAppCodes.getEmpFromApi(empId).empProbationEndDate), ConfigurationManager.AppSettings["formatDateUse"]),//  empProbationEndDate
                        //api พัง ใช้อันนี้แทนเทสไปก่อน
                        hireDate = DocumentsAppCode.convertDateTHToShowCultureDateEN(Convert.ToDateTime(empDetailList.FirstOrDefault().hireDate), ConfigurationManager.AppSettings["formatDateUse"]),

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

        public JsonResult getAllRegion(string txtRegion, string condition)
        {

            List<TB_Act_Region_Model> regionList = new List<TB_Act_Region_Model>();
            try
            {
                if (UtilsAppCode.Session.User.isAdminBeer || UtilsAppCode.Session.User.isSuperAdmin)
                {
                    regionList = QueryGetAllRegion.getAllRegion().Where(x => x.descTh.Contains(txtRegion) && x.condition.Equals(condition)).ToList();

                }
                else
                {
                    regionList = QueryGetAllRegion.getAllRegion().Where(x => x.descTh.Contains(txtRegion) && x.descEn.Contains(Regex.Match(UtilsAppCode.Session.User.DepartmentName, @"\d+").Value) && x.condition.Equals(condition)).ToList();

                }

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
                ExceptionManager.WriteError("textThaiBaht => " + ex.Message);
            }
            return Json(txtBaht, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getOtherMasterByType(string type, string subType, string text)
        {
            List<TB_Act_Other_Model> getOtherList = new List<TB_Act_Other_Model>();
            try
            {
                getOtherList = QueryOtherMaster.getOhterMaster(type, subType).Where(x => x.displayVal.ToLower().Contains(text.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getOtherMasterByType => " + ex.Message);
            }
            return Json(getOtherList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCashLimitByEmpId(string empId, string empLvl)
        {
            List<CashEmpModel> cashEmpList = new List<CashEmpModel>();
            
            var result = new AjaxResult();
            try
            {
                if (empLvl != "")
                {
                    cashEmpList = QueryGetBenefit.getCashLimitByEmpId(empId, empLvl).ToList();
                    if (cashEmpList.Count > 0)
                    {
                        var resultData = new
                        {
                            ProductDetail_0 = cashEmpList[0].cashPerDay,
                            ProductDetail_1 = cashEmpList[1].cashPerDay,
                            PerDayUs = cashEmpList[1].cashPerDayUs,
                        };

                        result.Data = resultData;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCashLimitByEmpId => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getEmpByChannel(string subjectId, string channelId, string filter)
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

        public JsonResult getRegionalByCompany(string companyId)
        {
            var result = new AjaxResult();
            try
            {
                var getRegional = QueryGetRegional.getRegionalByCompanyId(companyId).ToList();
                var resultData = new
                {
                    regional = getRegional.ToList(),
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
        public JsonResult getRegionalOfFlow(string empId)
        {
            var result = new AjaxResult();
            try
            {
                var getRegional = QueryGetRegional.getRegionalByCompanyId(empId).ToList();
                var resultData = new
                {
                    regional = getRegional.ToList(),
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
        public JsonResult getFlowBytypeFormId(string typeFormId, string empId)
        {
            var result = new AjaxResult();
            try
            {
                var getFlowDetail = QueryGetFlow.getFlowDetailBytypeFormId(typeFormId).ToList().Where(x => x.empGroup == empId);
                var resultData = new
                {
                    flow = getFlowDetail.ToList(),
                    regional = getFlowDetail.ToList().Count == 0 ? null : QueryGetRegional.getRegionalByCompanyId((getFlowDetail.ToList().FirstOrDefault().companyId)).ToList(),
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
        public JsonResult getAllHospital(string text)
        {
            List<HospitalModel> getList = new List<HospitalModel>();
            try
            {
                getList = QueryGetAllHospital.getAllHospital().Where(x => x.hospNameTH.Contains(text)).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllHospital => " + ex.Message);
            }
            return Json(getList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getCashLimitByTypeId(string typeId, string hireDate, string jobLevel)
        {
            List<CashEmpModel> cashEmpList = new List<CashEmpModel>();
            var result = new AjaxResult();
            try
            {

                if (!string.IsNullOrEmpty(hireDate))
                {
                    hireDate = (BaseAppCodes.converStrToDatetimeWithFormat(hireDate, ConfigurationManager.AppSettings["formatDateUse"])).ToString();
                    cashEmpList = QueryGetBenefit.getCashLimitByTypeId(typeId, hireDate, jobLevel).ToList();
                    if (cashEmpList.Count > 0)
                    {
                        var resultData = new
                        {
                            cashPerDay = cashEmpList[0].cashPerDay,

                        };

                        result.Data = resultData;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCashLimitByTypeId => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getCumulativeByEmpId(string empId)
        {
            List<CashEmpModel> cashEmpList = new List<CashEmpModel>();
            var result = new AjaxResult();
            try
            {
                if (!string.IsNullOrEmpty(empId))
                {
                    cashEmpList = QueryGetBenefit.getCumulativeByEmpId(empId, DateTime.Today).ToList();
                    if (cashEmpList.Count > 0)
                    {
                        var resultData = new
                        {
                            cashPerDay = cashEmpList[0].cashPerDay,
                        };
                        result.Data = resultData;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCumulativeByEmpId => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getCashDetailByEmpId(string empId, string typeId, string hireDate, string jobLevel, string docDate)
        {
            List<CashEmpModel> cashEmpList = new List<CashEmpModel>();
            var result = new AjaxResult();
            try
            {
                decimal limit = 0, cumulative = 0, balance = 0;

                if (!string.IsNullOrEmpty(empId))
                {


                    //docDate = (BaseAppCodes.converStrToDatetimeWithFormat(docDate, ConfigurationManager.AppSettings["formatDateUse"])).ToString();
                    cashEmpList = QueryGetBenefit.getCashLimitByTypeId(typeId, hireDate, jobLevel).ToList();
                    if (cashEmpList != null && cashEmpList.Count > 0)
                    {
                        limit = cashEmpList[0].cashPerDay;
                    }

                    DateTime? docDatee = DateTime.ParseExact(docDate, "dd/MM/yyyy", null);
                    cashEmpList = QueryGetBenefit.getCumulativeByEmpId(empId, docDatee).ToList();
                    if (cashEmpList != null && cashEmpList.Count > 0)
                    {
                        cumulative = cashEmpList[0].cashPerDay;
                    }
                    balance = limit - cumulative;

                    var resultData = new
                    {
                        limit = limit,
                        cumulative = cumulative,
                        balance = balance,
                        cashPerDay = cumulative//cashEmpList[0].cashPerDay,
                    };
                    result.Data = resultData;

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getCashDetailByEmpId => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllActivityFormByEmpId(string typeFormId, string empId)
        {
            List<ActivityFormTBMMKT> activityFormTBMMKT = new List<ActivityFormTBMMKT>();
            var result = new AjaxResult();
            try
            {
                activityFormTBMMKT = QueryGetActivityByIdTBMMKT.getAllActivityFormByEmpId(typeFormId, empId).Where(x => x.statusId == 2).ToList();
                var resultData = new
                {
                    chk = activityFormTBMMKT.Count > 0 ? "false" : "true",

                };

                result.Data = resultData;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllActivityFormByEmpId => " + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDepartmentByCompId(string companyId)
        {
            var result = new AjaxResult();
            try
            {
                List<eForms.Models.MasterData.departmentMasterModel> departmentList = new List<eForms.Models.MasterData.departmentMasterModel>();
                departmentList = departmentMasterPresenter.getdepartmentByCompId(AppCode.StrCon, companyId);

                var resultData = new
                {
                    departmentList = departmentList.ToList(),
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
        public JsonResult getEmpByDepartment(string companyId, string department)
        {
            List<RequestEmpModel> empDetailList = new List<RequestEmpModel>();
            var result = new AjaxResult();
            try
            {
                empDetailList = QueryGet_empByComp.getEmpByDepartment(companyId, department).ToList();
                if (empDetailList.Any())
                {
                    var resultData = new
                    {
                        //empId = empDetailList.FirstOrDefault().empId,
                        //empName = empDetailList.FirstOrDefault().empName,                  
                        //department =empDetailList.FirstOrDefault().departmentEN,         
                        empList = empDetailList.ToList()
                    };
                    result.Data = resultData;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getEmpByDepartment => " + ex.Message);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult genImageStream(string empId)
        {
            try
            {
                var result = SignatureAppCode.currentSignatureByEmpId(empId);
                if (result.lists.Any())
                {
                    return File(result.lists[0].signature, "image/jpg");
                }
                else
                {
                    return File(Server.MapPath("~/images/noSig.jpg"), "image/jpg");
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("genImageStream => " + ex.Message);
                return File(Server.MapPath("~/images/noSigError.jpg"), "image/jpg");
            }


        }


        public JsonResult getMasterTypeIdByActId(string actId)
        {
            var result = new AjaxResult();
            try
            {
                var getActDetailList = QueryGetActivityById.getActivityById(actId);
                if (!string.IsNullOrEmpty(QueryGetActivityById.getActivityById(actId).FirstOrDefault().master_type_form_id))
                {
                    result.Success = true;
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult checkIOPV(string IO)
        {
            var result = new AjaxResult();
            try
            {
                result.Success = ActFormAppCode.checkIOPV(IO);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult checkSizeFile(string year)
        {
            var result = new AjaxResult();
            var getSize = ActFormAppCode.getActivityByYear("2022");

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}