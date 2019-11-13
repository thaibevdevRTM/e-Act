using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class SaleActualController : Controller
    {
        // GET: SaleActual
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult sale()
        {
            Activity_Model activityModel = new Activity_Model();
            activityModel.customerslist = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameEN != "").ToList();
            activityModel.productlist = QueryGetAllProduct.getAllProduct("");
            activityModel.productcatelist = QuerygetAllProductCate.getAllProductCate()
                .Select(group => new TB_Act_Product_Cate_Model.Product_Cate_Model
                {
                    cateName = group.cateName,   
                }).ToList();

            return View(activityModel);
        }
    }
}