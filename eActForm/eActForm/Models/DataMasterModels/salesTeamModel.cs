using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eForms.Models.MasterData;
namespace eActForm.Models
{
    public class salesTeamModel
    {
        public List<ProvinceModel> provinceList { get; set; }
        public List<SalesTeamCVMModel> saleTeamList { get; set; }

    }
}