﻿using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{

    public partial class partialActivityDetailReportController
    {
        public ActionResult partialactSetPriceMTRpt(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            return PartialView(activity_TBMMKT_Model);
        }

    }

}