using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Firebase.Xamarin.Database;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Torman.Swipeable;
using Firebase.Xamarin.Database.Query;
using Android.Graphics;
using Android.Content.PM;
using System.Diagnostics;
using Android.Net;
using System.Threading;
using Plugin.Connectivity;

namespace LeapProject
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,ScreenOrientation =ScreenOrientation.Portrait)]
    
    public class MainActivity : AppCompatActivity
    {
        static List<string> session_names = new List<string>();
        static List<int> active_session_position = new List<int>();
        //List<int> sessionsRating;      
        string editURL;
        ListView sessionListView;
        CustomAdapter myCustomAdapter;
        CustomAdapter customAdapter;
        SwipeableListView listView;
        string FirebaseURL = "https://leapproject-b603d.firebaseio.com/";     

        bool IsConnectedToNetwork = false;
        Android.Support.V7.App.AlertDialog alertDialog;
        
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Set your main view here
            SetContentView(Resource.Layout.MainLayout); 

            IsConnectedToNetwork = CrossConnectivity.Current.IsConnected;

            //check the initial state of connection

            Console.WriteLine("SESSION COUNT : " + session_names.Count);
            if(session_names.Count==0)
            {
                if (IsConnectedToNetwork == false)
                {
                    string message = "Please Check your Internet Connection And Try Again.";
                    displayProgressDialog(message);
                }
                else
                {
                    await refreshSession();                    
                }
            }
            else
            {
                if(Intent.GetStringExtra("FromAddActivtiy")!="" && IsConnectedToNetwork)
                {
                    session_names.Add(Intent.GetStringExtra("FromAddActivtiy"));
                    listView = FindViewById<SwipeableListView>(Resource.Id.listView);
                    myCustomAdapter = new CustomAdapter(this, session_names, active_session_position);//,sessionsRating);
                    listView.Adapter = myCustomAdapter;
                }
                else
                {
                    listView.Adapter = null;
                }
               
            }
         
              
            //to check whether the device is connected to network
            CrossConnectivity.Current.ConnectivityChanged += async delegate
              {
                 
                  if (CrossConnectivity.Current.IsConnected != true)
                  {  
                      string message = "Please Check your Internet Connection And Try Again.";
                      displayProgressDialog(message);
                      IsConnectedToNetwork = false;                    
                  }
                  if(CrossConnectivity.Current.IsConnected == true)
                  {
                      if (alertDialog.IsShowing)
                          alertDialog.Dismiss();
                     
                      await refreshSession();                      

                      IsConnectedToNetwork = true;                    
                  }
              };
            
        }

        public void displayProgressDialog(string message)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View connectionErrorView = layoutInflater.Inflate(Resource.Layout.ProgressLayout, null);
            //mView.SetBackgroundColor(Color.Transparent);
            Android.Support.V7.App.AlertDialog.Builder alertProgress = new Android.Support.V7.App.AlertDialog.Builder(this);
            alertProgress.SetView(connectionErrorView).SetCancelable(false);
            connectionErrorView.FindViewById<TextView>(Resource.Id.displayLoading).Text = message;
            alertDialog = alertProgress.Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change main_compat_menu
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //handling menu item select
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.action_add)
            {
                //invoke add activity
                StartActivity(typeof(CreateSessionActivity));
            }
            if (item.ItemId == Resource.Id.action_topper)
            {
                StartActivity(typeof(TopperActivity));
            }

            return base.OnOptionsItemSelected(item);
        }

        //to display the toppers List
        
        public async Task refreshSession()
        {
            Console.WriteLine("SESSION COUNT 12 : " + session_names.Count);
            List<String> temp_session_names= new List<string>();
            List<int> temp_active_session_position = new List<int>();
            var index = 0;
            try
            {
                string message = "    Loading...";
                displayProgressDialog(message);
                var firebase = new FirebaseClient(FirebaseURL);
                var items = await firebase.Child("Session").OnceAsync<Session>();
                foreach (var item in items)
                {   
                    temp_session_names.Add(item.Object.Session_Name);
                    if(!active_session_position.Contains(index))
                    {
                        var sessionItems = await firebase.Child(item.Object.Session_Name).Child("Score").OnceAsync<SessionResponse>();
                        if (sessionItems.Count > 0)
                            temp_active_session_position.Add(index);
                    }                
                    index++;                  
                }
                session_names = temp_session_names;
                active_session_position = temp_active_session_position;
                listView = FindViewById<SwipeableListView>(Resource.Id.listView);
                customAdapter = new CustomAdapter(this, session_names, active_session_position);//,sessionsRating);
                listView.Adapter = customAdapter;
                Console.WriteLine("SESSION COUNT 123123 : " + session_names.Count);
                if (alertDialog.IsShowing)
                    alertDialog.Dismiss();
            }
            catch (Exception)
            {
                if (alertDialog.IsShowing)
                    alertDialog.Dismiss();

                string message = "Please Check your Internet Connection And Try Again.";
                displayProgressDialog(message);
            }

        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
            this.Finish();
        }
    }
}

