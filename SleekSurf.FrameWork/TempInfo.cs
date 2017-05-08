using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;

namespace SleekSurf.FrameWork
{
    public class TempInfo
    {

        public string Password { get; set; }
        public Stream ProfileStream { get; set; }
        public Stream LogoStream { get; set; }
        public FileUpload ProfileImageStream { get; set; }
        public FileUpload LogoImageStream { get; set; }
        public string ProfileImageName { get; set; }
        public string LogoImageName { get; set; }
        public bool SuccessStatus { get; set; }
        public string ClientID { get; set; }
        public ByteStruct ImageStruct { get; set; }
    }

    public struct ByteStruct
    {
        public byte[] MainImage { get; set; }
        public byte[] SupportImage { get; set; }
    }
}
