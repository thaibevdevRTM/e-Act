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

        public ActionResult expensesTrvDetail(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            if (activity_TBMMKT_Model.expensesDetailModel == null || activity_TBMMKT_Model.expensesDetailModel.costDetailLists == null || !activity_TBMMKT_Model.expensesDetailModel.costDetailLists.Any())
            {
                CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT
                {
                    costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
                };


                activity_TBMMKT_Model.listPiority = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice("E887A185-2BD5-4DA6-98CF-8EAF1BF35E49", "expensesTrv").OrderBy(x => x.orderNum).ToList();

                // listChoiceName,listChoiceId
                for (int i = 0; i < activity_TBMMKT_Model.listPiority.Count; i++)
                {
                    model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { listChoiceId = activity_TBMMKT_Model.listPiority[i].id, listChoiceName = activity_TBMMKT_Model.listPiority[i].name, productDetail = "", unit = 0, unitPrice = 0, total = 0 });
                }
                activity_TBMMKT_Model.expensesDetailModel = model;

            }
            return PartialView(activity_TBMMKT_Model);
        }
        //public ActionResult testExpensesDetails(Activity_TBMMKT_Model activity_TBMMKT_Model)
        //{
        //    return PartialView(activity_TBMMKT_Model);
        //}
    }
}