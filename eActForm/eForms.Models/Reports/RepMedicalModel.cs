using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.Reports
{

    public class RepMedicalModels
    {
        public List<RepMedicalModel> repMedicalists { get; set; }
      //  public List<RepPostEvaModel> repPostEvaGroupBrand { get; set; }
    }

    //public class RepPostEvaGroup
    //{
    //    public string name { get; set; }
    //    public string value { get; set; }
    //    public string sumActSalesParti { get; set; }
    //    public double? sumNormalCase { get; set; }
    //    public double? sumPromotionCase { get; set; }
    //    public double? sumSalesInCase { get; set; }
    //    public string countGroup { get; set; }
    //}
    public class RepMedicalModel
    {
        public string id { get; set; }
        public string empId { get; set; }
        public string empName { get; set; }
        public string positionName { get; set; }
        public string level { get; set; }
        public string department { get; set; }
        //Doc

        //public string activityName { get; set; }
        //public string productCode { get; set; }
        //public string productName { get; set; }
        //public string brandName { get; set; }
        //public string groupName { get; set; }
        //public string cusNameEN { get; set; }
        //public string size { get; set; }
        //public DateTime? activityPeriodSt { get; set; }
        //public DateTime? activityPeriodEnd { get; set; }
        //public DateTime? costPeriodSt { get; set; }
        //public DateTime? costPeriodEnd { get; set; }
        //public double? le { get; set; }
        //public string unit { get; set; }
        //public string compensate { get; set; }
        //public double? normalCost { get; set; }
        //public string productId { get; set; }
        //public double? themeCost { get; set; }
        //public double? total { get; set; }
        //public double? tempAPNormalCost { get; set; }
        //public double? estimateSaleBathAll { get; set; }
        //public double? actReportQuantity { get; set; }
        //public double? actVolumeQuantity { get; set; }
        //public double? actAmount { get; set; }
        //public double? billedQuantityMT { get; set; }
        //public double? volumeMT { get; set; }
        //public double? netValueMT { get; set; }
        //public double? specialDiscountMT { get; set; }
        //public double? presentToSale { get { return estimateSaleBathAll == 0 ? 0 : (total / estimateSaleBathAll) * 100; } set { } }
        //public double? bathParti { get { return estimateSaleBathAll * (le / 100); } }
        //public double? presentToSaleParti { get { return total / bathParti; } }
        //public double? presentSE { get { return activitySales == "Promotion Support" ? 0 : (specialDiscountMT / (netValueMT + specialDiscountMT)) * 100; } } //([specialDiscountMT] / [netValueMT] + [specialDiscountMT]) * 100
        //public double? salePartiCase { get { return activitySales == "Promotion Support" ? actReportQuantity * (le / 100) : 0; } }
        //public double? salePartiBath { get { return activitySales == "Promotion Support" ? actAmount * (le / 100) : 0; } }
        //public double? accuracySaleCase { get { return activitySales == "Promotion Support" ? (actReportQuantity / themeCost) * 100  : (billedQuantityMT / themeCost) * 100; } }
        //public double? accuracySaleBath { get { return activitySales == "Promotion Support" ? (actAmount / estimateSaleBathAll) * 100 : 0; } }
        //public double? accuracySpendingBath { get { return activitySales == "Promotion Support" ? 0 : (specialDiscountMT / total) * 100; } }
        //public double? saleActual { get; set; }
        //public double? presentAcctual { get { return activitySales == "Promotion Support" ? 0 : saleActual / total; } }
        //public string dayAddStart { get; set; }
        //public string dayAddEnd { get; set; }
        //public int countGroup { get; set; }
    }
}
