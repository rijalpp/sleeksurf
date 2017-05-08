using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SleekSurf.FrameWork;
using System.Web.UI.HtmlControls;

namespace SleekSurf.Web.WebPageControls
{
    public partial class NewEditAdvertisement : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindDimension();

            if (WebContext.CurrentUser.IsInRole("MarketingOfficer"))
                chkPublished.Enabled = false;
        }

        public HtmlGenericControl ImageHolder
        {
            get { return imageHolder; }
            set { imageHolder = value; }
        }

        public string AddName
        {
            get { return txtAdName.Text; }
            set { txtAdName.Text = value; }
        }

        public string Advertiser
        {
            get { return txtAdvertiser.Text; }
            set { txtAdvertiser.Text = value; }
        }

        public string ContactDetail
        {
            get { return txtContactDetails.Text; }
            set { txtContactDetails.Text = value; }
        }

        public string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }

        public FileUpload AddUrlImage
        {
            get { return ucFileUpload.ImageControl; }
        }

        public Image ImgThumb
        {
            get { return imgThumb; }
            set { imgThumb = value; }
        }

        public string NavigateUrl
        {
            get { return txtNavigateUrl.Text; }
            set { txtNavigateUrl.Text = value; }
        }

        public DateTime? StartDate
        {
            get { return Convert.ToDateTime(txtStartDate.Text); }
            set { txtStartDate.Text = value.ToString(); }
        }

        public TextBox TxtStartDate
        {
            get { return txtStartDate; }
        }

        public TextBox TxtEndDate
        {
            get { return txtEndDate; }
        }

        public DateTime? EndDate
        {
            get { return Convert.ToDateTime(txtEndDate.Text); }
            set { txtEndDate.Text = value.ToString(); }
        }

        public decimal AmountPaid
        {
            get { return Convert.ToDecimal(txtAmountPaid.Text); }
            set { txtAmountPaid.Text = string.Format("{0:0.00}", value); }
        }

        public string DisplayPosition
        {
            get { return ddlDisplayPosition.SelectedValue; }
            set { ddlDisplayPosition.SelectedValue = value; }
        }

        public int Dimension
        {
            get { return int.Parse(ddlDimension.SelectedValue); }
            set 
            { 
                ddlDimension.SelectedValue = value.ToString();
                string[] dimension;
                if (string.Compare(ddlDisplayPosition.SelectedValue, "right", true) == 0)
                    dimension = Enum.GetName(typeof(AdDimensionRight), value).Replace('d', ' ').Trim().Split('x');
                else
                    dimension = Enum.GetName(typeof(AdDimensionFooter), value).Replace('d', ' ').Trim().Split('x');

                imgThumb.Width = Convert.ToInt32(dimension[0]);
                imgThumb.Height = Convert.ToInt32(dimension[1]);
            }
        }

        public bool Published
        {
            get { return chkPublished.Checked; }
            set { chkPublished.Checked = value; }
        }

        public string Comments
        {
            get { return txtComment.Text; }
            set { txtComment.Text = value; }
        }

        public DropDownList DropDownDisplayPosition
        {
            get { return ddlDisplayPosition; }
            set { ddlDisplayPosition = value; }
        }

        public DropDownList DropDownDimension
        {
            get { return ddlDimension; }
            set { ddlDimension = value; }
        }

        public string Amount
        {
            get { return txtAmountPaid.Text; }
            set { txtAmountPaid.Text = value; }
        }

        public Panel PanelUpdateMode
        {
            get { return pnlUpdateMode; }
        }

        protected void ddlDisplayPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDimension();
        }

        private void BindDimension()
        {
            switch (ddlDisplayPosition.SelectedValue.Trim().ToLower())
            {
                case "right":
                    ddlDimension.Enabled = ddlDisplayPosition.Enabled;
                    ddlDimension.Items.Clear();
                    foreach (AdDimensionRight dimension in Enum.GetValues(typeof(AdDimensionRight)))
                    {
                        string tempDimension = Enum.GetName(typeof(AdDimensionRight), dimension).Replace('d', ' ').Trim();
                        tempDimension = string.Join("px X ", tempDimension.Split('x')) + "px";
                        ddlDimension.Items.Add(new ListItem(tempDimension, ((int)dimension).ToString()));
                    }
                    ddlDimension.DataBind();
                    ddlDimension.Items.Insert(0, new ListItem("Select Below", "0"));
                    break;
                case "footer":
                    ddlDimension.Enabled = ddlDisplayPosition.Enabled;
                    ddlDimension.Items.Clear();
                    foreach (AdDimensionFooter dimension in Enum.GetValues(typeof(AdDimensionFooter)))
                    {
                        string tempDimension = Enum.GetName(typeof(AdDimensionFooter), dimension).Replace('d', ' ').Trim();
                        tempDimension = string.Join("px X ", tempDimension.Split('x')) + "px";
                        ddlDimension.Items.Add(new ListItem(tempDimension, ((int)dimension).ToString()));
                    }
                    ddlDimension.DataBind();
                    ddlDimension.Items.Insert(0, new ListItem("Select Below", "0"));
                    break;
                default:
                    ddlDimension.Enabled = false;
                    ddlDimension.Items.Clear();
                    break;
            }
        }
    }
}