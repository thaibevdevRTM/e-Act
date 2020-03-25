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
    public class TravellingController : Controller
    {
        public ActionResult travellingDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {

            if (activity_TBMMKT_Model.placeDetailModel.Count == 0)
            {
                List<PlaceDetailModel> placeDetailModel = new List<PlaceDetailModel>();
                for (int i = 0; i < 3; i++)
                {
                    placeDetailModel.Add(new PlaceDetailModel() { place = "", forProject = "", period = "", departureDate = null, arrivalDate = null });
                }
                activity_TBMMKT_Model.placeDetailModel = placeDetailModel;
            }
            return PartialView(activity_TBMMKT_Model);
        }

    }
}