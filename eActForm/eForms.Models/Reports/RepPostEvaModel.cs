using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eForms.Models.Reports
{
    
    public class RepPostEvaModels
    {
        public List<RepPostEvaModel> repPostEvaLists { get; set; }
        public List<RepPostEvaModel> repPostEvaGroupBrand { get; set; }
    }

    public class RepPostEvaGroup
    {
        public string name { get; set; }
        public string value { get; set; }
        public string sumActSalesParti { get; set; }
        public double? sumNormalCase { get; set; }
        public double? sumPromotionCase { get; set; }
        public double? sumSalesInCase { get; set; }
        public string countGroup { get; set; }
    }
    public class RepPostEvaModel
    {
        public string id { get; set; }
        public string customerId { get; set; }
        public string customerName { get; set; }
        public string preLoadStock { get; set; }
        public string activitySales { get; set; }
        public string activityNo { get; set; }
        public string activityName { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string brandName { get; set; }
        public string groupName { get; set; }
        public string cusNameEN { get; set; }
        public string size { get; set; }
        public DateTime? activityPeriodSt { get; set; }
        public DateTime? activityPeriodEnd { get; set; }
        public string costPeriodSt { get; set; }
        public string costPeriodEnd { get; set; }
        public double? le { get; set; }
        public string unit { get; set; }
        public string compensate { get; set; }
        public double? normalCost { get; set; }
        public string productId { get; set; }
        public double? themeCost { get; set; }
        public double? total { get; set; }
        public double? tempAPNormalCost { get; set; }
        public double? estimateSaleBathAll { get; set; }
        public double? actReportQuantity { get; set; }
        public double? actVolumeQuantity { get; set; }
        public double? actAmount { get; set; }
        public double? billedQuantityMT { get; set; }
        public double? volumeMT { get; set; }
        public double? netValueMT { get; set; }
        public double? specialDiscountMT { get; set; }
        public double? presentToSale { get { return estimateSaleBathAll == 0 ? 0 : (total / estimateSaleBathAll) * 100; } set { } }
        public double? bathParti { get { return estimateSaleBathAll * (le / 100); } }
        public double? presentToSaleParti { get { return total / bathParti;  } }
        public double? presentSE { get { return activitySales == "Promotion Support" ? 0 : (specialDiscountMT / (netValueMT + specialDiscountMT)) * 100;  } } //([specialDiscountMT] / [netValueMT] + [specialDiscountMT]) * 100
        public double? salePartiCase { get { return activitySales == "Promotion Support" ? actReportQuantity * (le/100) : 0; } }
        public double? salePartiBath { get { return activitySales == "Promotion Support" ? actAmount * (le / 100) : 0; } }
        public double? accuracySaleCase { get { return activitySales == "Promotion Support" ? actReportQuantity / tempAPNormalCost : billedQuantityMT / tempAPNormalCost; } }
        public double? accuracySaleBath { get { return activitySales == "Promotion Support" ? actAmount / estimateSaleBathAll : 0; } }
        public double? accuracySpendingBath { get { return activitySales == "Promotion Support" ? 0 : specialDiscountMT/ estimateSaleBathAll; } }
        public double? saleActual { get; set; }
        public double? presentAcctual { get { return activitySales == "Promotion Support" ? 0 : saleActual / total; } }
        public string dayAddStart { get; set; }
        public string dayAddEnd { get; set; }
        public int countGroup { get; set; }
    }
}
