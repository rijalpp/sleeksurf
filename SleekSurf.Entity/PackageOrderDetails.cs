using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class PackageOrderDetails
    {
        public string OrderID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string OrderStatus { get; set; }
        public ClientDetails Client { get; set; }
        public string ReceiptNo { get; set; }
        public string TransactionID { get; set; }
        public string PaymentOption { get; set; }
        public decimal StandardPrice { get; set; }
        public double DiscountPercentage { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal AmountDeducted { get; set; }
        public decimal FinalPriceAfterDeduction { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string Duration { get; set; }
        public string PromoCode { get; set; }
        public DateTime? PromoCodeStartDate { get; set; }
        public DateTime? PromoCodeEndDate { get; set; }
        public string Comments { get; set; }
    }
}
