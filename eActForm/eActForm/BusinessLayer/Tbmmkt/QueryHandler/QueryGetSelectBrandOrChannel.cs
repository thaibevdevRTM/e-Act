using eActForm.Models;
using System;
using System.Collections.Generic;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class QueryGetSelectBrandOrChannel
    {
        public static List<TB_Act_ActivityForm_SelectBrandOrChannel> GetAllQueryGetSelectBrandOrChannel()
        {
            try
            {
                List<TB_Act_ActivityForm_SelectBrandOrChannel> tB_Act_ActivityForm_SelectBrand = new List<TB_Act_ActivityForm_SelectBrandOrChannel>
                {
                    new TB_Act_ActivityForm_SelectBrandOrChannel() { txt = "Channel", val = "Channel" }
                    ,new TB_Act_ActivityForm_SelectBrandOrChannel() { txt = "Brand", val = "Brand" }
                };
                return tB_Act_ActivityForm_SelectBrand;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetAllQueryGetSelectBrandOrChannel => " + ex.Message);
                return new List<TB_Act_ActivityForm_SelectBrandOrChannel>();
            }
        }
    }
}