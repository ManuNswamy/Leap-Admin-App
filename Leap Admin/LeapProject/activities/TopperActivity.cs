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
using Plugin.Connectivity;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Android.Support.V7.App;

namespace LeapProject
{
    [Activity(Label = "TopperActivity")]
    public class TopperActivity : AppCompatActivity
    {
        private bool IsConnectedToNetwork;
        private Android.Support.V7.App.AlertDialog alertDialog;
        private string FirebaseURL = "https://leapproject-b603d.firebaseio.com/";

        List<String> topperList;
        List<string> active_session_names;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TopperLayout);
            Button button = FindViewById<Button>(Resource.Id.buttonRank);
            EditText editRank = FindViewById<EditText>(Resource.Id.editRank);

            IsConnectedToNetwork = CrossConnectivity.Current.IsConnected;

            if (IsConnectedToNetwork == false)
            {
                string message = "Please Check your Internet Connection And Try Again.";
                displayProgressDialog(message);
            }          

            button.Click += delegate{
                topperList = new List<String>();
                active_session_names = new List<string>();
                Console.WriteLine("RANK IS " + editRank.Text);
                displayTopper(Convert.ToInt32(editRank.Text));
            };
            
          
            CrossConnectivity.Current.ConnectivityChanged += delegate
            {

                if (CrossConnectivity.Current.IsConnected != true)
                {
                    string message = "Please Check your Internet Connection And Try Again.";
                    displayProgressDialog(message);
                    IsConnectedToNetwork = false;
                }
                if (CrossConnectivity.Current.IsConnected == true)
                {
                    if (alertDialog.IsShowing)
                        alertDialog.Dismiss();                    
                }
            };
        }
        private async void displayTopper(int rank)
        {
            string message = "Please wait ...";
            displayProgressDialog(message);
            try
            {
                var firebase = new FirebaseClient(FirebaseURL);
                var items = await firebase.Child("Session").OnceAsync<Session>();
                if (items.Count != 0)
                {
                    foreach (var item in items)
                    {
                        try
                        {
                            var active_session = await firebase.Child("Score").Child(item.Object.Session_Name).OnceAsync<SessionResponse>();
                            if (active_session.Count > 0)
                            {
                                active_session_names.Add(item.Object.Session_Name);
                                int count = 1;
                                List<String> topper = new List<string>();
                                foreach (var correct_answer in active_session)
                                {
                                    if (count <= rank && count<=active_session.Count)
                                    {
                                        topper.Add("[ " + item.Object.Session_Name + " ] " + correct_answer.Object.name + " " + correct_answer.Object.mobilenumber);
                                        count++;
                                    }
                                    else
                                        break;

                                }
                                topper.Reverse();
                                for (int i = 0; i < topper.Count; i++)
                                {
                                    topperList.Add(topper[i]);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                ListView topperListView = FindViewById<ListView>(Resource.Id.listViewTopper);
                topperList.Reverse();
                ArrayAdapter<String> topperAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, topperList);
                topperListView.Adapter = topperAdapter;
                if (alertDialog.IsShowing)
                    alertDialog.Dismiss();

            }
            catch (Exception)
            {
                if (alertDialog.IsShowing)
                    alertDialog.Dismiss();
                message = "Please check the internet Connection";
                displayProgressDialog(message);
            }
           
        }

        private void displayProgressDialog(string message)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View connectionErrorView = layoutInflater.Inflate(Resource.Layout.ProgressLayout, null);
            //mView.SetBackgroundColor(Color.Transparent);
            Android.Support.V7.App.AlertDialog.Builder alertProgress = new Android.Support.V7.App.AlertDialog.Builder(this);
            alertProgress.SetView(connectionErrorView).SetCancelable(false);
            connectionErrorView.FindViewById<TextView>(Resource.Id.displayLoading).Text = message;
            alertDialog = alertProgress.Show();
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(MainActivity));
            this.Finish();
        }
    }
}