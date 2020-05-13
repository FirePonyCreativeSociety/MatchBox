using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox
{
    public static class Version
    {
        public const int MajorVersion = 2;
        public const int MinorVersion = 1;
        public const int PatchVersion = 0;

        public static string FullVersion = $"{MajorVersion}.{MinorVersion}.{PatchVersion}";
        public const string ProductNameNoSpaces = "MatchBox";

        public static string ProductNameNoSpacesFor(string methodName)
        {
            return $"{ProductNameNoSpaces}.{methodName}";
        }
    }
}
