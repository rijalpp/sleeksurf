using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.Caching;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Xml.Linq;
using System.Net;
using System.Device.Location;
using System.Collections;

namespace SleekSurf.FrameWork
{
    public static class Helpers
    {
        public static string[] GetThemes()
        {
            if (HttpContext.Current.Cache["SiteThemes"] != null)
            {
                return (string[])HttpContext.Current.Cache["SiteThemes"];
            }
            else
            {
                string[] mainTheme = { "SleekTheme" };
                string themesDirPath = HttpContext.Current.Server.MapPath("~/App_Themes");
                //get the array of themes folders under /app_themes
                string[] allThemes = Directory.GetDirectories(themesDirPath);
                for (int i = 0; i <= allThemes.Length - 1; i++)
                {
                    allThemes[i] = Path.GetFileName(allThemes[i]);
                    //cache the array with a dependancy to the folder
                    CacheDependency dep = new CacheDependency(themesDirPath);
                    HttpContext.Current.Cache.Insert("SiteThemes", allThemes, dep);
                }

                return allThemes.Except(mainTheme).ToArray();
            }
        }
        public static string CurrentUserIP
        {
           // get { return HttpContext.Current.Request.UserHostAddress; }
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                                               ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                                               : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }
        //used to preserve password while registering user

        public enum Title
        {
            Master = 1,
            Miss,
            Mr,
            Mrs,
            Ms
        }

        public enum Occupation
        {
            Academic = 1,
            Accountant,
            Actor,
            Architect,
            Artist,
            Author,
            BusinessManager,
            Carpenter,
            ChiefExecutive,
            Cinematographer,
            CivilServant,
            Coach,
            Composer,
            Computerprogrammer,
            Cook,
            Counsellor,
            Doctor,
            Driver,
            Economist,
            Editor,
            Electrician,
            Engineer,
            ExecutiveProducer,
            Fixer,
            GraphicDesigner,
            Hairdresser,
            Headhunter,
            HRRecruitment,
            InformationOfficer,
            ITConsultant,
            Journalist,
            LawyerSolicitor,
            Lecturer,
            Librarian,
            Mechanic,
            Model,
            Musician,
            OfficeWorker,
            Performer,
            Photographer,
            Presenter,
            ProducerDirector,
            ProjectManager,
            Researcher,
            Salesman,
            SocialWorker,
            Soldier,
            Sportsperson,
            Student,
            Teacher,
            TechnicalCrew,
            TechnicalWriter,
            Therapist,
            Translator,
            Waitress_Waiter,
            WebDesigner,
            Writer,
            Other
        }

        public static byte[] ResizeImage(Stream image, int maxWidthOrHeight)
        {
            Graphics myGraphics = null;
            Bitmap mySourceBitmap = null, myTargetBitmap = null;
            try
            {
                Bitmap bmpSource = new Bitmap(image);
                Size originalSize = bmpSource.Size;
                Size newSize = new Size(0, 0);

                bmpSource.Dispose();

                decimal maxHeightDecimal = Convert.ToDecimal(maxWidthOrHeight);
                decimal maxWidthDecimal = Convert.ToDecimal(maxWidthOrHeight);

                decimal? resizeFactor = null;

                if (originalSize.Height > originalSize.Width)
                {
                    //Portrait
                    resizeFactor = Convert.ToDecimal(Decimal.Divide(originalSize.Height, maxHeightDecimal));
                    newSize.Height = maxWidthOrHeight;
                    newSize.Width = Convert.ToInt32(originalSize.Width / resizeFactor);
                }
                else
                {
                    //Landscape or square
                    resizeFactor = Convert.ToDecimal(Decimal.Divide(originalSize.Width, maxWidthDecimal));
                    newSize.Width = maxWidthOrHeight;
                    newSize.Height = Convert.ToInt32(originalSize.Height / resizeFactor);
                }

                mySourceBitmap = new Bitmap(image);
                int newWidth = newSize.Width;
                int newHeight = newSize.Height;

                myTargetBitmap = new Bitmap(newWidth, newHeight);

                myGraphics = Graphics.FromImage(myTargetBitmap);

                myGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                myGraphics.DrawImage(mySourceBitmap, new Rectangle(0, 0, newWidth, newHeight));
                mySourceBitmap.Dispose();

                MemoryStream mStream = new MemoryStream();

                //Save the new image
                myTargetBitmap.Save(mStream, ImageFormat.Jpeg);
                byte[] newImage = mStream.ToArray();

                return newImage;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                throw;
            }
            finally
            {
                if (myGraphics != null)
                    myGraphics.Dispose();
                if (mySourceBitmap != null)
                    mySourceBitmap.Dispose();
                if (myTargetBitmap != null)
                    myTargetBitmap.Dispose();
            }
        }

        public static void ResizeImage(string originalImageStream, string outputFilename, int maxWidthOrHeight)
        {
            Bitmap bmpSource = new Bitmap(originalImageStream);
            Size originalSize = bmpSource.Size;
            Size newSize = new Size(0, 0);

            bmpSource.Dispose();

            decimal maxHeightDecimal = Convert.ToDecimal(maxWidthOrHeight);
            decimal maxWidthDecimal = Convert.ToDecimal(maxWidthOrHeight);

            decimal? resizeFactor = null;

            if (originalSize.Height > originalSize.Width)
            {
                //Portrait
                resizeFactor = Convert.ToDecimal(Decimal.Divide(originalSize.Height, maxHeightDecimal));
                newSize.Height = maxWidthOrHeight;
                newSize.Width = Convert.ToInt32(originalSize.Width / resizeFactor);
            }
            else
            {
                //Landscape or square
                resizeFactor = Convert.ToDecimal(Decimal.Divide(originalSize.Width, maxWidthDecimal));
                newSize.Width = maxWidthOrHeight;
                newSize.Height = Convert.ToInt32(originalSize.Height / resizeFactor);
            }

            Bitmap mySourceBitmap = null, myTargetBitmap = null;
            Graphics myGraphics = null;

            try
            {
                mySourceBitmap = new Bitmap(originalImageStream);
                int newWidth = newSize.Width;
                int newHeight = newSize.Height;

                myTargetBitmap = new Bitmap(newWidth, newHeight);

                myGraphics = Graphics.FromImage(myTargetBitmap);

                myGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                myGraphics.DrawImage(mySourceBitmap, new Rectangle(0, 0, newWidth, newHeight));
                mySourceBitmap.Dispose();

                //Save the new image
                myTargetBitmap.Save(outputFilename, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                throw;
            }
            finally
            {
                if (myGraphics != null)
                    myGraphics.Dispose();
                if (mySourceBitmap != null)
                    mySourceBitmap.Dispose();
                if (myTargetBitmap != null)
                    myTargetBitmap.Dispose();
            }
        }

        private static string ExtractHTML(string htmlString)
        {
            return Regex.Replace(htmlString, @"<(.|\n)*?>", string.Empty);
        }

        public static string GetShortDescription(string description, int numberOfWords)
        {
            string[] Words = ExtractHTML(description).Split(' ');
            string sReturn = string.Empty;

            if (Words.Length <= numberOfWords)
            {
                sReturn = string.Join(" ", Words);
                return sReturn;
            }
            else
            {
                for (int i = 0; i < numberOfWords; i++)
                {
                    sReturn += Words.GetValue(i).ToString() + " ";
                }
                return sReturn + " ...";
            }

        }

        public static string ExtractSubDomain(Uri url)
        {
            string subDomain = string.Empty;
            if (url.HostNameType == UriHostNameType.Dns && (!(url.HostNameType == UriHostNameType.Unknown)))
            {
                string host = url.Host;
                if (host.ToLower().StartsWith("http://"))
                    host = host.Replace(host.Substring(0, 7), "");

                string[] tempHostSplit = host.Split('.');
                int length = tempHostSplit.Length;

                if (length > 2 && !tempHostSplit[length - 3].ToLower().Equals("www"))
                    subDomain = tempHostSplit[length - 3];
            }

            return subDomain;
        }

        public static void LogError(Exception ex)
        {
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
            string country = Configuration.GetConfigurationSetting("HomeCountry", typeof(string)) as string;
            StreamReader bodyReader = new StreamReader(appPath + "EmailTemplates/LogError.txt");
            string emailbody = bodyReader.ReadToEnd();
            bodyReader.Close();
            emailbody = emailbody.Replace("<%Logo%>", logoUrl);
            emailbody = emailbody.Replace("<%TopBackGround%>", topBackGroundUrl);
            emailbody = emailbody.Replace("<%DateOfError%>", DateTime.Now.ToLongDateString());
            emailbody = emailbody.Replace("<%TimeOfError%>", DateTime.Now.ToShortTimeString());
            emailbody = emailbody.Replace("<%Country%>", country);
            emailbody = emailbody.Replace("<%PageLocation%>", HttpContext.Current.Request.RawUrl);
            emailbody = emailbody.Replace("<%ErrorMessage%>", ex.Message);
            emailbody = emailbody.Replace("<%Source%>", ex.Source);
            emailbody = emailbody.Replace("<%TargetSite%>", ex.TargetSite.ToString());
            emailbody = emailbody.Replace("<%StackTrace%>", ex.StackTrace);

            string from = Globals.Settings.ErrorLogForm.From;
            string fromName = "SleekSurf Crash Alert";
            string to = Globals.Settings.ErrorLogForm.ErrorLogEmail;
            string subject = "Website Crash Information";

            Helpers.SendEmail(from, fromName, to, subject, emailbody);
        }

        public static void LogError(Exception ex, string customMessage)
        {
            string logoUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/LogoSleekSurf.png";
            string topBackGroundUrl = BasePage.FullBaseUrl + "App_Themes/SleekTheme/Images/MessageBoxTopBackground.png";
            string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
            string country = Configuration.GetConfigurationSetting("HomeCountry", typeof(string)) as string;
            StreamReader bodyReader = new StreamReader(appPath + "EmailTemplates/LogError.txt");
            string emailbody = bodyReader.ReadToEnd();
            bodyReader.Close();
            emailbody = emailbody.Replace("<%Logo%>", logoUrl);
            emailbody = emailbody.Replace("<%TopBackGround%>", topBackGroundUrl);
            emailbody = emailbody.Replace("<%DateOfError%>", DateTime.Now.ToLongDateString());
            emailbody = emailbody.Replace("<%TimeOfError%>", DateTime.Now.ToShortTimeString());
            emailbody = emailbody.Replace("<%Country%>", country);
            emailbody = emailbody.Replace("<%PageLocation%>", HttpContext.Current.Request.RawUrl);
            emailbody = emailbody.Replace("<%ErrorMessage%>", "<span style='dispaly:block;font-weight:bold;color:red;'>Note: " + customMessage + "</span>" + ex.Message);
            emailbody = emailbody.Replace("<%Source%>", ex.Source);
            emailbody = emailbody.Replace("<%TargetSite%>", ex.TargetSite.ToString());
            emailbody = emailbody.Replace("<%StackTrace%>", ex.StackTrace);

            string from = Globals.Settings.ErrorLogForm.From;
            string fromName = "SleekSurf Crash Alert";
            string to = Globals.Settings.ErrorLogForm.ErrorLogEmail;
            string subject = "Website Crash Information";

            Helpers.SendEmail(from, fromName, to, subject, emailbody);
        }

        public static bool SendEmail(List<UserInfoPartial> users, string mailSender, string subject, string body)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(Globals.Settings.ContactForm.MailTo, mailSender);
                //mailMessage.To.Add(new MailAddress(registeredUser.Email, registeredUser.FirstName+" "+registeredUser.MiddleName+" "+registeredUser.LastName));
                foreach (UserInfoPartial u in users)
                {
                    mailMessage.To.Add(new MailAddress(u.Email, u.FirstName + " " + u.MiddleName + " " + u.LastName));
                }
                mailMessage.Subject = string.Format(Globals.Settings.ContactForm.MailSubject, mailSender, subject);
                mailMessage.Body = body;
                new SmtpClient().Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool SendEmail(UserInfoPartial registeredUser, string mailSender, string subject, string body)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(Globals.Settings.ContactForm.MailTo, mailSender);
                mailMessage.To.Add(new MailAddress(registeredUser.Email, registeredUser.FirstName + " " + registeredUser.MiddleName + " " + registeredUser.LastName));
                mailMessage.Subject = string.Format(Globals.Settings.ContactForm.MailSubject, mailSender, subject);
                mailMessage.Body = body;
                new SmtpClient().Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool SendEmail(string from, string fromName, List<UserInfoPartial> users, string subject, string body)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(from, fromName);

                foreach (UserInfoPartial u in users)
                {
                    mailMessage.To.Add(new MailAddress(u.Email, u.FirstName + " " + u.MiddleName + " " + u.LastName));
                }

                mailMessage.Subject = subject;
                mailMessage.Body = body;
                new SmtpClient().Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendEmail(string from, string fromName, string to, string subject, string body)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(new MailAddress(from, fromName), new MailAddress(to));
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                new SmtpClient().Send(mailMessage);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static bool SendEmail(string from, string fromName, List<string> toEmailList, List<string> ccEmailList, List<string> bccEmailList, string subject, string body, List<Attachment> attachmentList)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(from, fromName);

                foreach (string to in toEmailList)
                {
                    mailMessage.To.Add(new MailAddress(to));
                }

                foreach (string cc in ccEmailList)
                {
                    mailMessage.CC.Add(new MailAddress(cc));
                }

                foreach (string bcc in bccEmailList)
                {
                    mailMessage.Bcc.Add(new MailAddress(bcc));
                }

                foreach (Attachment attachment in attachmentList)
                {
                    mailMessage.Attachments.Add(attachment);
                }

                mailMessage.Subject = subject;
                mailMessage.Body = body;
                new SmtpClient().Send(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static XElement GetGeocodingSearchResults(string address)
        {
            var url = String.Format("http://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false", HttpContext.Current.Server.UrlEncode(address));
            var results = XElement.Load(url);

            // Check the status
            var status = results.Element("status").Value;
            if (status != "OK" && status != "ZERO_RESULTS")
                // Whoops, something else was wrong with the request...
                throw new ApplicationException("There was an error with Google's Geocoding Service: " + status);

            return results;
        }

        public static string SendSMS(string url, string username, string password, string source, string destination, string text)
        {
            string status = "";

            status = SMSSend(url, SMSDetails(username, password, source, destination, text));

            return status;
        }

        private static string SMSDetails(string username, string password, string source, string destination, string text)
        {

            StringBuilder postData = new StringBuilder("action=sendsms");

            postData.Append("&user=");
            postData.Append(HttpUtility.UrlEncode(username));
            postData.Append("&password=");
            postData.Append(HttpUtility.UrlEncode(password));
            postData.Append("&from=");
            postData.Append(HttpUtility.UrlEncode(source));
            postData.Append("&to=");
            postData.Append(HttpUtility.UrlEncode(destination));
            postData.Append("&text=");
            postData.Append(HttpUtility.UrlEncode(text));

            return postData.ToString();

        }

        private static string SMSSend(string url, string postData)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest smsRequest = (HttpWebRequest)WebRequest.Create(url);
            smsRequest.Method = "POST";
            smsRequest.ContentType = "application/x-www-form-urlencoded";
            smsRequest.ContentLength = data.Length;

            System.IO.Stream smsDataStream = null;
            smsDataStream = smsRequest.GetRequestStream();
            smsDataStream.Write(data, 0, data.Length);
            smsDataStream.Close();

            System.Net.WebResponse smsResponse = smsRequest.GetResponse();

            byte[] responseBuffer = new byte[smsResponse.ContentLength];
            //smsResponse.GetResponseStream().Read(responseBuffer, 0, smsResponse.ContentLength - 1);
            smsResponse.GetResponseStream().Read(responseBuffer, 0, Convert.ToInt32(smsResponse.ContentLength - 1));
            smsResponse.Close();

            return encoding.GetString(responseBuffer);

        }


        public static int GetTotalPages(int totalRows, int pageSize)
        {
            int totalPageno = (int)Math.Ceiling((double)totalRows / pageSize);
            return totalPageno;
        }

        private static string buildAbsoluteUri(string relativeUri)
        {
            //get current uri
            Uri uri = HttpContext.Current.Request.Url;
            //build absolute path
            string app = HttpContext.Current.Request.ApplicationPath;
            if (!app.EndsWith("/"))
                app += "/";
            relativeUri = relativeUri.TrimStart('/');
            return HttpUtility.UrlPathEncode(string.Format("http://{0}:{1}{2}{3}", uri.Host, uri.Port, app, relativeUri));
        }

        public static string ToSearch(string page)
        {
            if (page == "1")
                return buildAbsoluteUri(String.Format("WebPages/BusinessCatalog.aspx"));
            else
                return buildAbsoluteUri(String.Format("WebPages/BusinessCatalog.aspx?Page={0}", page));
        }

        public static string ToSearch(string Page, string Name, string Location)
        {
            return buildAbsoluteUri(String.Format("WebPages/BusinessCatalog.aspx?Page={0}&Name={1}&Location={2}", Page, Name, Location));
        }

        public static string ToSearch(string Page, string Name, string Cat, string Location)
        {
            return buildAbsoluteUri(String.Format("WebPages/BusinessCatalog.aspx?Page={0}&Name={1}&Cat={2}&Location={3}", Page, Name, Cat, Location));
        }

        public static string FormatPrice(object price)
        {
            return Convert.ToDecimal(price).ToString("N2") + " " + Globals.Settings.Package.CurrencyCode;
        }

        public static string currencySymbol = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol;

        public static GeoCoordinate CurrentDeviceLocation
        {
            get {
                GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
                watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
                return watcher.Position.Location;
            }
        }

        /// <summary>
        /// Code repeated in BizObject, TODO: remove for BizObject.
        /// </summary>
        public static Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }

        public static void PurgeCacheItems(string prefix)
        {
            List<string> itemsToRemove = new List<string>();
            IDictionaryEnumerator enumerator = Helpers.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLower().StartsWith(prefix.ToLower()))
                    itemsToRemove.Add(enumerator.Key.ToString());
            }
            foreach (string itemToRemove in itemsToRemove)
                Helpers.Cache.Remove(itemToRemove);
        }

        public static void CacheData(string key, object data)
        {
            Helpers.Cache.Insert(key, data, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
        }

    }
}
