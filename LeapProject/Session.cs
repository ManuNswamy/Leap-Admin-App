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
using SQLite;
namespace LeapProject
{
    class Session
    {
        //public int Id { get; set; }
        public int OTP { get; set; }
        public string Session_Name { get; set; }
        public string Instructor_Name { get; set; }
        public string Question { get; set; }
        public string OptionA{ get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public int Session_Rating { get; set; }

    }
}