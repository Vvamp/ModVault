using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModVault.Core
{
    public static class Helpers
    {
        public static Version TryGetVersion(this string item)
        {
            Version ver;
            bool success = Version.TryParse(item, out ver);
            if (success) return ver;
            return null;
        }
        
        public static Version[] TryGetVersions(this IEnumerable<string> items)
        {
            List<Version> versions = new List<Version>();
            foreach(var item in items)
            {
                Version ver;
                bool success = Version.TryParse(item, out ver);
                if (success) versions.Add(ver);

            }

            return versions.ToArray();
        }
    }
}
