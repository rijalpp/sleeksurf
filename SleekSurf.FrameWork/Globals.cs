using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace SleekSurf.FrameWork
{
    public static class Globals
    {
        public static string ThemesSelectorID = "";
        public readonly static ApplicationConfiguration Settings = (ApplicationConfiguration)WebConfigurationManager.GetSection("ApplicationMainSection");
    }
}
