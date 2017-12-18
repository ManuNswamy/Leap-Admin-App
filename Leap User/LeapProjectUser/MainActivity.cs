using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using SQLite;
using Firebase.Xamarin.Database;
using System.IO;
using Torman.Swipeable;
using System.Collections.Generic;
using Java.Net;
using Android.Net;

namespace LeapProjectUser
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        string dbPath;
        SwipeableListView listView;
        CustomAdapter customAdapter;
        static List<string> session_names = new List<string>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Set your main view here
            SetContentView(Resource.Layout.main);
           
            Console.WriteLine("ON CREATE");

            dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"SessionDB.db3");
          
            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);

            //check whether the user is a registered user          
            if (ap.getValue("name") == "" && ap.getValue("mobilenumber") == "" && IsInternetAvailable())
            {
                insertData();
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View mView = layoutInflater.Inflate(Resource.Layout.user_pref, null);

                Button buttonRegitser = mView.FindViewById<Button>(Resource.Id.buttonRegister);
                Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialogBuilder.SetView(mView);
                alertDialogBuilder.SetCancelable(false);

                var alert = alertDialogBuilder.Show();

                buttonRegitser.Click += delegate
                {
                    var username = mView.FindViewById<TextView>(Resource.Id.editUsername).Text;
                    var mobilenumber = mView.FindViewById<TextView>(Resource.Id.editMobileNumber).Text;
                    if (username.Length > 3 && mobilenumber.Length == 10)
                    {
                        ap.saveValue("name", username);
                        ap.saveValue("mobilenumber", mobilenumber);
                        ap.saveValue("score", "0");
                        ap.saveValue("testAttempted", "0");
                        alert.Dismiss();
                    }
                    else
                    {
                        if (username.Length < 3)
                        {
                            Toast.MakeText(this, "Please enter a valid Name. The Name should have atleast 3 characters.", ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(this, "Please enter a valid Mobile Number", ToastLength.Short).Show();
                        }

                    }
                };
            }
            listView = FindViewById<SwipeableListView>(Resource.Id.listView);
            customAdapter = new CustomAdapter(this, session_names, dbPath);
            listView.Adapter = customAdapter;

           



        }


        protected override void OnResume()
        {
            base.OnResume();
            Console.WriteLine("ON RESUME");
        }
        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }
        private async void insertData()
        {
        
            var db = new SQLiteConnection(dbPath);           
            db.DropTable<Session>();
            db.CreateTable<Session>();
            Console.WriteLine("Table created!");
            try
            {
                var firebase = new FirebaseClient("https://leapproject-b603d.firebaseio.com/");
                var items = await firebase.Child("Session").OnceAsync<Session>();
                foreach (var item in items)
                {
                    db.Insert(item.Object);
                    session_names.Add(item.Object.Session_Name);
                }
               
            }
            catch
            {
                Console.WriteLine("Failed to update the database");
            }
        }

        public bool IsInternetAvailable()
        {
            try
            {
                ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
                NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;
                return networkInfo.IsConnected;
            }
            catch (Exception)
            {
                return false;
                
            }
        }
    }
}