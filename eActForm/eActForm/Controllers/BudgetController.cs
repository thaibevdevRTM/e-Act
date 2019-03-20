using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebLibrary;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class BudgetController : Controller
    {
		// GET: Budget
		//public ActionResult activityList()
		//{
		//    return View();
		//}

		public ActionResult PreviewBudgetInvoice(string activityId, string productId)
		{
			Budget_Activity_Model budget_Model = new Budget_Activity_Model();
			budget_Model.Budget_Activity_Product_list = QueryBudgetBiz.getBudgetActivityProduct(activityId, productId);

			return PartialView(budget_Model);
		}


		public ActionResult activityList()
        {
            Session["activityId"] = Guid.NewGuid().ToString();
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
            budget_activity_model.budget_activity_list = QueryBudgetBiz.getBudgetActivity("2",null).ToList();
		
			return View(budget_activity_model);
        }

		public ActionResult EditForm(string activityId, string activityNo)
		{

			Session["activityId"] = activityId;
			Session["activityNo"] = activityNo;
			Budget_Activity_Model budget_activity_model = new Budget_Activity_Model();
			budget_activity_model.budget_activity_list = QueryBudgetBiz.getBudgetActivity("2", null).ToList();
			budget_activity_model.Budget_Activity_Product_list = QueryBudgetBiz.getBudgetActivityProduct(activityId,null);

			return View(budget_activity_model);
		}
	}

  

}