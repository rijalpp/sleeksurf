using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SleekSurf.FrameWork;
using System.IO;
using SleekSurf.Entity;
using SleekSurf.Manager;

namespace SleekSurf.Web
{
    /// <summary>
    /// Summary description for Upload
    /// </summary>
    public class Upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                HttpPostedFile uploadedFile = context.Request.Files["UploadedFile"];

                if (context.Request["ClientID"] != null)
                {
                    string clientID = context.Request["ClientID"].ToString();
                    string promotionID = context.Request["PromotionID"].ToString();
                    string promotionPhotoID = System.DateTime.Now.ToString("PP-ddMMyyy-HHmmssfff");
                    string dirUrl = BasePage.BaseUrl + "Uploads/" + clientID + "/Promotions/" + promotionID;

                    MediaGalleryDetails media = new MediaGalleryDetails();

                    media.MediaID = promotionPhotoID;
                    media.Title = "Title" + promotionID;
                    media.Caption = "Caption" + promotionID;
                    media.MediaType = "Image";
                    media.MediaUrl = dirUrl + "/" + promotionPhotoID + ".jpg";
                    media.Description = "Description" + promotionID;
                    media.IsActive = true;
                    media.Promotion = new PromotionDetails() { PromotionID = promotionID };
                    media.EventDetail = new EventDetails();


                    Result<MediaGalleryDetails> result = new Result<MediaGalleryDetails>();
                    result = EventManager.InsertMediaGallery(media);

                    if (result.Status == ResultStatus.Success)
                    {
                        string dirPath = context.Server.MapPath(dirUrl);
                        if (!Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath);

                        string outputfileName = dirPath + "/" + promotionPhotoID + ".jpg";
                        uploadedFile.SaveAs(outputfileName);
                        Helpers.ResizeImage(outputfileName, outputfileName, 600);
                    }

                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Successed");
                }
            }
            catch (Exception)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Failed");
            }

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