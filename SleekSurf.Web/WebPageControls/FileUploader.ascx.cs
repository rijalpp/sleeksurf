using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SleekSurf.Web.WebPageControls
{
    public partial class FileUploader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public FileUpload ImageControl
        {
            get { return this.fileUpload; }
        }
    }
}