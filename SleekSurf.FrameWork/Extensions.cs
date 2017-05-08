using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.FrameWork
{
    public static class Extensions
    {
        public static string Encrypt(this string str)
        {
            return Cryptographer.Encrypt(str, false);
        }

        public static string Decrypt(this string str)
        {
            return Cryptographer.Decrypt(str, false);
        }
    }
}
