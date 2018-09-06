using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Upgrade
{
    public class VersionExt
    {
        public VersionInfo Release;
    }

    public class VersionInfos
    {
        public VersionInfo[] Release;
    }

    public class VersionInfo
    {
        public string Name;
        public string Version;
        public string MD5;
    }
}
