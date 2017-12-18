using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace LeapProject
{
    class ColorHelper
    {
        public static Color Pink => new Color(233, 30, 99);
        
        public static Color Orange => new Color(242,167,77);

        public static Color Inactive => new Color(253, 217, 186);

        public static Color Completed => new Color(199, 196, 193);

        public static Color Red => new Color(244, 67, 54);

        public static Color Purple => new Color(156, 39, 176);

        public static Color Teal => new Color(0, 150, 136);

        public static Color Green => new Color(76, 175, 80);
    }
}