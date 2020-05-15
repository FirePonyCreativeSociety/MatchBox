using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox
{
    public static class Version
    {
        public const int MajorVersion = 3;
        public const int MinorVersion = 0;
        public const int PatchVersion = 2;

        public const string ProductId = "MatchBox";        

        public static string FullVersion = $"{MajorVersion}.{MinorVersion}.{PatchVersion}";
    }
}
