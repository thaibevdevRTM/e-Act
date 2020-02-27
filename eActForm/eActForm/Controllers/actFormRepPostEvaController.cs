using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class actFormRepPostEvaController : Controller
    {
        // GET: actFormRepPostEva
        public ActionResult index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult searchActForm()
        {
            try
            {
                return View();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}