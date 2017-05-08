using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SleekSurf.FrameWork
{
    public static class Configuration
    {
        public static object GetConfigurationSetting(string key, Type expectedType)
        {
            AppSettingsReader settingReader = new AppSettingsReader();
            return settingReader.GetValue(key, expectedType);
        }
    }
}
