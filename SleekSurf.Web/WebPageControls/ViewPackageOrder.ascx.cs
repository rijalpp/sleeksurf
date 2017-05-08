using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SleekSurf.Web.WebPageControls
{
    public partial class ViewPackageOrder : System.Web.UI.UserControl
    {

        #region CLIENT DETAILS

        public string ClientName
        {
            get { return lblClientName.Text; }
            set { lblClientName.Text = value; }
        }

        public string ContactPerson
        {
            get { return lblContactPerson.Text; }
            set { lblContactPerson.Text = value; }
        }

        public string ContactPersonEmail
        {
            get { return hlContactPersonEmail.Text; }
            set 
            { 
                hlContactPersonEmail.Text = value;
                hlContactPersonEmail.NavigateUrl = "~/Admin/SendEmail.aspx?To=" + value;
            }
        }

        public string ClientContactNo
        {
            get { return lblContactNo.Text; }
            set { lblContactNo.Text = value; }
        }

        public string ClientEmail
        {
            get { return hlClientEmail.Text; }
            set
            {
                hlClientEmail.Text = value;
                hlClientEmail.NavigateUrl = "~/Admin/SendEmail.aspx?To=" + value;
            }
        }

        public string ClientAddress
        {
            get { return lblAddress.Text; }
            set { lblAddress.Text = value; }
        }

        #endregion

        #region PACKAGE-TRANSACTION DETAILS

        public string PackageName
        {
            get { return lblPackageName.Text; }
            set { lblPackageName.Text = value; }
        }

        public string PackageDuration
        {
            get { return ltrPackageDuration.Text; }
            set {
                if (PackageName.Trim() == "SMS" || PackageName.Trim() == "SMS BySleekSurf")
                {
                    PackageName = PackageName.Replace(" BySleekSurf", "");
                    ltrPackageDuration.Text = value + " SMS Credits";
                }
                else if (PackageName.Trim().EndsWith(" BySleekSurf"))
                {
                    PackageName = PackageName.Replace(" BySleekSurf", "");
                    ltrPackageDuration.Text = value + " Days";
                }
                else
                {
                    if (value.ToString() == "0")
                        ltrPackageDuration.Text = "Matched";
                    else
                        ltrPackageDuration.Text = value + " Months";
                }
            }
        }

        public decimal StandardPrice
        {
            get
            {
                if (lblStandardPrice.Text.Length > 0)
                {
                    return decimal.Parse(lblStandardPrice.Text);
                }
                else
                    return 0.00M;
            }

            set
            {
                lblStandardPrice.Text = string.Format("{0:c}", value);
            }

        }

        public DateTime? ExpiryDate
        {
            get
            {
                if (lblExpiryDate.Text.Length > 0)
                    return Convert.ToDateTime(lblExpiryDate.Text);
                else
                    return null;
            }
            set
            {
                lblExpiryDate.Text = string.Format("{0:d}", value);
            }
        }

        public string PromoCode
        {
            get { return lblPromoCode.Text; }
            set { lblPromoCode.Text = value; }
        }

        public DateTime? PromoCodeStartDate
        {
            get
            {
                if (lblPromoCodeStartDate.Text.Length > 0)
                {
                    return Convert.ToDateTime(lblPromoCodeStartDate.Text);
                }
                else
                    return null;
            }

            set
            {
                lblPromoCodeStartDate.Text = string.Format("{0:d}", value);
            }
        }

        public DateTime? PromoCodeEndDate
        {
            get
            {
                if (lblPromoCodeEndDate.Text.Length > 0)
                {
                    return Convert.ToDateTime(lblPromoCodeEndDate.Text);
                }
                else
                    return null;
            }

            set
            {
                lblPromoCodeEndDate.Text = string.Format("{0:d}", value);
            }
        }

        public double? DiscountPercentage
        {
            get
            {
                if (lblDiscountPercentage.Text.Length > 0)
                {
                    return Convert.ToDouble(lblDiscountPercentage.Text);
                }
                else
                    return 0.0;
            }
            set
            {
                lblDiscountPercentage.Text = string.Format("{0:0.0}", value);
            }
        }

        public decimal PromoDiscountedAmount
        {
            get
            {
                if (lblDiscountedAmount.Text.Length > 0)
                {
                    return decimal.Parse(lblDiscountedAmount.Text);
                }
                else
                    return 0.00M;
            }

            set
            {
                lblDiscountedAmount.Text = string.Format("{0:c}", value);
            }
        }

        public decimal FinalPrice
        {
            get
            {
                if (lblFinalPrice.Text.Length > 0)
                {
                    return decimal.Parse(lblFinalPrice.Text);
                }
                else
                    return 0.00M;
            }

            set
            {
                lblFinalPrice.Text = string.Format("{0:c}", value);
            }

        }

        public string InvoiceID
        {
            get
            {
                return lblInvoiceID.Text;
            }
            set
            {
                lblInvoiceID.Text = value;
            }
        }

        public string TransactionID
        {
            get
            {
                if (lblTransactionID.Text.Length > 0)
                    return lblTransactionID.Text;
                else
                    return "Not created yet";
            }
            set { lblTransactionID.Text = value; }
        }

        public string PurchasedMethod
        {
            get { return lblPurchasedMethod.Text; }
            set { lblPurchasedMethod.Text = value; }
        }

        public decimal AmountDeducted
        {
            get
            {
                if (lblAmountDeducted.Text.Length > 0)
                {
                    return decimal.Parse(lblAmountDeducted.Text);
                }
                else
                    return 0.00M;
            }

            set
            {
                lblAmountDeducted.Text = string.Format("{0:c}", value);
            }
        }

        public string PurchasedStatus
        {
            get { return lblPurchasedStatus.Text; }
            set { lblPurchasedStatus.Text = value; }
        }

        public decimal AmountPaid
        {
            get
            {
                if (lblAmountPaid.Text.Length > 0)
                    return decimal.Parse(lblAmountPaid.Text);
                else
                    return 0.00M;
            }
            set
            {
                lblAmountPaid.Text = string.Format("{0:c}", value);
            }
        }

        public decimal AmountPaidByClient
        {
            get
            {
                if (lblActualAmountPaid.Text.Length > 0)
                    return decimal.Parse(lblActualAmountPaid.Text);
                else
                    return 0.00M;
            }
            set
            {
                lblActualAmountPaid.Text = string.Format("{0:c}", value);
            }
        }

        public string PurchasedType
        {
            get { return lblPurchasedType.Text; }
            set { lblPurchasedType.Text = value; }
        }

        public string PurchasedBy
        {
            get { return lblPurchasedBy.Text; }
            set { lblPurchasedBy.Text = value; }
        }

        public DateTime? PurchasedDate
        {
            get
            {
                if (lblPurchasedDate.Text.Length > 0)
                    return Convert.ToDateTime(lblPurchasedDate.Text);
                else
                    return null;
            }

            set
            {
                lblPurchasedDate.Text = string.Format("{0:d}", value);
            }
        }

        #endregion

        public void SetClientPanelVisibility(bool visibility)
        {
            divClientDetails.Visible = visibility;
        }

        public void DisplayRefundedPanel(bool display)
        {
            spanRefundedCase.Visible = display;
        }
    }
}