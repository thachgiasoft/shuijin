using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ZGameLib
{
    public class Global
    {
        class UnityAsset
        {

        }

        class UnityAsync
        {

        }

        class UnityLog
        {

        }

        public class UnityUI
        {
            public static string AssetDir { get; set; }
            public static GameObject Root2D { get; set; }
            public static GameObject Root3D { get; set; }

            public static string CombinePath(string name)
            {
                return string.Format("{0}/{1}.ui.asset", AssetDir, name);
            }
        }
    }
}
