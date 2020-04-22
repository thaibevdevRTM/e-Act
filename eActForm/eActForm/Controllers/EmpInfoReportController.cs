using eActForm.BusinessLayer;
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
    [LoginExpire]
    public class EmpInfoReportController : Controller
    {

        public ActionResult empInfoRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            //empDetailList = QueryGet_empDetailById.getEmpDetailById(empId).ToList();

            //List<RequestEmpModel> empDetailList = new List<RequestEmpModel>();
           // activity_TBMMKT_Model.empInfoModel = eActController.QueryGet_empDetailById.getEmpDetailById(activity_TBMMKT_Model.).ToList();
            return PartialView(activity_TBMMKT_Model);
        }
     
    }
}