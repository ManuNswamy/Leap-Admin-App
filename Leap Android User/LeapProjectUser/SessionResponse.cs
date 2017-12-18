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

namespace LeapProjectUser
{
    class SessionResponse
    {
        public int score { get; set; }
        public double mobilenumber { get; set; }
        public string name { get; set; }
    }
}