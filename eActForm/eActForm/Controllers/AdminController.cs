﻿using eActForm.BusinessLayer;
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

    }
}