using System;
using System.Web;
using System.Net;
using System.IO;
using System.Globalization;
using SleekSurf.Entity;
using SleekSurf.Manager;
using SleekSurf.FrameWork;


namespace SleekSurf.Web.WebPages.PayPal
{
    public class PayPalIPNHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (IsVerifiedNotification())
                {
                    string orderID = HttpContext.Current.Request.Params["custom"];
                    string status = HttpContext.Current.Request.Params["payment_status"];
                    string transactionID = HttpContext.Current.Request.Params["txn_id"];
                    decimal amount = Convert.ToDecimal(HttpContext.Current.Request.Params["mc_gross"], CultureInfo.CreateSpecificCulture("en-AU"));
                    decimal amountDeducted = Convert.ToDecimal(HttpContext.Current.Request.Params["mc_fee"]);
                    string currencyUsed = HttpContext.Current.Request.Params["mc_currency"];
                    PackageOrderDetails order = ClientPackageManager.SelectPackageOrder(orderID).EntityList[0];
                    order.OrderStatus = status;
                    order.TransactionID = transactionID;
                    if (amount >= order.FinalPrice)
                    {
                        order.OrderStatus = StatusOrder.Verified.ToString();
                        order.FinalPrice = amount;
                        order.AmountDeducted = amountDeducted;
                        order.FinalPriceAfterDeduction = order.FinalPrice - order.AmountDeducted;
                    }

                    ClientPackageManager.UpdatePackageOrder(order);
                    if (order.OrderStatus == StatusOrder.Verified.ToString())
                        ClientManager.PublishClient(order.Client.ClientID, Status.Activated.ToString());
                }
            }
            catch(Exception ex)
            {
                StreamWriter swr = new StreamWriter(HttpContext.Current.Server.MapPath("HandlertestAfterUpdate.txt"));
                swr.WriteLine("== error occured== Datetime "+System.DateTime.Now);
                swr.Dispose();
                string description = "An Error occured while processing the payment of Order ID : "+HttpContext.Current.Request.Params["custom"]+" Please check your paypal account and the Package Order Database for details.";
                Helpers.LogError(ex, description);
            }
        }

        private bool IsVerifiedNotification()
        {
            string response = "";

            string post = HttpContext.Current.Request.Form.ToString() + "&cmd=_notify-validate";
            string serverUrl = (Globals.Settings.Package.SandBoxMode ? "https://www.sandbox.paypal.com/us/cgi-bin/webscr" : "https://www.paypal.com/us/cgi-bin/webscr");

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(serverUrl);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = post.Length;

            StreamWriter writer = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            writer.Write(post);
            writer.Close();

            StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream());
            response = reader.ReadToEnd();
            reader.Close();

            return (response == "VERIFIED");

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}