using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eActForm.Models.Reports
{
    public class RepPostEvaModels
    {
        public List<RepPostEvaModel> repPostEvaLists { get; set; }
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
        public DateTime? costPeriodSt { get; set; }
        public DateTime? costPeriodEnd { get; set; }
        public string le { get; set; }
        public string unit { get; set; }
        public string compensate { get; set; }
        public double? normalCost { get; set; }
        public string productId { get; set; }
        public double? themeCost { get; set; }
        public double? total { get; set; }
        public double? tempAPNormalCost { get; set; }
        public double? estimateSaleBathAll { get; set; }
        public double? presentToSale { get; set; }
        public double? bathParti { get; set; }
        public double? presentToSaleParti { get; set; }
        public double? billedQuantityMT { get; set; }
        public double? volumeMT { get; set; }
        public double? netValueMT { get; set; }
        public double? specialDiscountMT { get; set; }
        public double? presentSE { get; set; }
        public double? salePartiCase { get; set; }
        public double? salePartiBath { get; set; }
        public double? accuracySaleCase { get; set; }
        public double? accuracySaleBath { get; set; }
        public double? accuracySpendingBath { get; set; }
        public double? saleActual { get; set; }
        public double? presentAcctual { get; set; }
        public string dayAddStart { get; set; }
        public string dayAddEnd { get; set; }
        public double? actReportQuantity { get; set; }
        public double? actVolumeQuantity { get; set; }
        public double? actAmount { get; set; }
    }
}
