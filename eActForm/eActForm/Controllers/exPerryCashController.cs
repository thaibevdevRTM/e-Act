using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class exPerryCashController : Controller
    {
        // GET: Expense
        public Activity_TBMMKT_Model getMasterExPerryCash()
        {

            Activity_TBMMKT_Model model = new Activity_TBMMKT_Model();
            return model;
        }


    }
}