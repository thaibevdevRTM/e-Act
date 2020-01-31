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
        public ActionResult Index(string activityId, string typeForm, string master_type_form_id)
        {

            Activity_TBMMKT_Model model = new Activity_TBMMKT_Model();
            model.activityFormModel.master_type_form_id = master_type_form_id;
            model.activityFormModel.typeForm = typeForm;
            return PartialView(model);
        }
    }
}