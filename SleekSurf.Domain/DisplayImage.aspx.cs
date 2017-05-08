using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using SleekSurf.Manager;
using SleekSurf.Entity;

namespace SleekSurf.Domain
{

    public partial class DisplayImage : System.Web.UI.Page
    {
        string PID = "";
        string Section = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                PID = Request.QueryString["ID"];
                if (Request.QueryString["SECTION"] != null)
                {
                    Section = Request.QueryString["SECTION"];
                    ShowImage(PID, Section);
                }
                else
                    ShowImage(PID);
            }
        }

        private void ShowImage(string PID, string Section)
        {

            if (WebContext.ImageList != null && WebContext.ImageList.ContainsKey(PID))
            {
                ByteStruct temp = WebContext.ImageList[PID];
                Response.Clear();
                if (Section == "TITLE")
                {
                    if (temp.MainImage != null)
                    {
                        Response.BinaryWrite(temp.MainImage);
                    }
                }
                else
                {
                    if (temp.SupportImage != null)
                        Response.BinaryWrite(temp.SupportImage);
                }
            }
            else if (Request.QueryString["ClientID"] != null)
            {
                //GET PROMOTION DETAILS TO SAVE IMAGE IN SESSION
                PromotionDetails promotion = ClientManager.SelectPromotion(PID, Request.QueryString["ClientID"].ToString()).EntityList[0];
                if (promotion.TitleImage != null)
                {
                    ByteStruct temp = WebContext.ImageList[PID];
                    Response.Clear();
                    if (Section == "TITLE")
                    {
                        if (temp.MainImage != null)
                        {
                            Response.BinaryWrite(temp.MainImage);
                        }
                    }
                    else
                    {
                        if (temp.SupportImage != null)
                            Response.BinaryWrite(temp.SupportImage);
                    }
                }
                else
                {
                    Response.Redirect("~/App_Themes/Default/Images/PromotionDefault.png");
                }
            }

            else
            {
                Response.Redirect("~/App_Themes/Default/Images/IfDefaultPictureNotFound.png");
            }
        }

        private void ShowImage(string PID)
        {
            if (WebContext.ImageList != null && WebContext.ImageList.ContainsKey(PID))
            {
                ByteStruct temp = WebContext.ImageList[PID];
                if (temp.MainImage != null)
                {
                    Response.Clear();
                    Response.BinaryWrite(temp.MainImage);
                }
                else
                    Response.Redirect("~/App_Themes/Default/Images/IfDefaultPictureNotFound.png");
            }
        }
    }
}