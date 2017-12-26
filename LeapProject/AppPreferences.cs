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
using Android.Preferences;

namespace LeapProject
{
    class AppPreferences
    {
        private ISharedPreferences nameSharedPref;
        private ISharedPreferencesEditor namePrefEditor;
        private Context mContext;

        //private static string PREFERCENCE_ACCESS_KEY = "PREFERCENCE_ACCESS_KEY";
        //private static string NAME = "NAME";


        public AppPreferences(Context context)
        {
            this.mContext = context;
            nameSharedPref = PreferenceManager.GetDefaultSharedPreferences(mContext);
            namePrefEditor = nameSharedPref.Edit();
        }

        public void saveValue(string key, string value)//save data value
        {
            namePrefEditor.PutString(key, value);
            namePrefEditor.Commit();
        }
        public string getValue(string key)//return the data value
        {
            return nameSharedPref.GetString(key, "");
        }

        public void removeValue(string key)//save data value
        {
            namePrefEditor.Remove(key);
            namePrefEditor.Commit();
        }

    }
}