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
using Android.Support.V7.App;
using Plugin.Connectivity;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Newtonsoft.Json;

namespace LeapProject
{
    [Activity(Label = "Create a Session")]
    public class CreateSessionActivity : AppCompatActivity
    {
        private bool IsConnectedToNetwork;
        private Android.Support.V7.App.AlertDialog alertDialog;
        private string FirebaseURL = "https://leapproject-b603d.firebaseio.com/";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            IsConnectedToNetwork = CrossConnectivity.Current.IsConnected;

            SetContentView(Resource.Layout.CreateSessionLayout);

            if (IsConnectedToNetwork == false)
            {
                string message = "Please Check your Internet Connection And Try Again.";
                displayProgressDialog(message);
            }
            else
            {
                addSession();
            }
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

                    addSession();

                    IsConnectedToNetwork = true;
                }
            };
        }

        private void addSession()
        {

            EditText editSessionName = FindViewById<EditText>(Resource.Id.editSessionName);
            EditText editInstructorName = FindViewById<EditText>(Resource.Id.editInstructorName);
            EditText editQuestion = FindViewById<EditText>(Resource.Id.editQuestion);
            EditText editOptionA = FindViewById<EditText>(Resource.Id.editOptionA);
            EditText editOptionB = FindViewById<EditText>(Resource.Id.editOptionB);
            EditText editOptionC = FindViewById<EditText>(Resource.Id.editOptionC);
            EditText editOptionD = FindViewById<EditText>(Resource.Id.editOptionD);
            Button buttonCreateSession = FindViewById<Button>(Resource.Id.buttonCreateSession);
            Button buttonCancelCreate = FindViewById<Button>(Resource.Id.buttonCancelCreate);
            Spinner spinnerCorrectOption = FindViewById<Spinner>(Resource.Id.spinnerCorrectOption);

            ///creating correct option Spinner
            List<string> options = new List<String> { "Select an Option", "Option A", "Option B", "Option C", "Option D" };
            var adapterCorrectOption = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, options);
            spinnerCorrectOption.Adapter = adapterCorrectOption;
            string correctOption = "";
            spinnerCorrectOption.ItemSelected += (sender, e) =>
            {
                switch (e.Position)
                {
                    case 1:
                        correctOption = "Option A";
                        break;

                    case 2:
                        correctOption = "Option B";
                        break;

                    case 3:
                        correctOption = "Option C";
                        break;

                    case 4:
                        correctOption = "Option D";
                        break;
                    default:
                        correctOption = "";
                        break;

                }
            };
            buttonCreateSession.Click += async delegate
            {
                try
                {
                    if (editSessionName.Text != "" && editQuestion.Text != "" && editQuestion.Text != "" && editOptionA.Text != "" && editOptionB.Text != "" && editOptionC.Text != "" && editOptionD.Text != "" && correctOption != "")
                    {
                        displayProgressDialog(" Please Wait...");
                        Random rand = new Random();
                        Session sessionObj = new Session();
                        sessionObj.OTP = rand.Next(1000, 9999);
                        sessionObj.Session_Name = editSessionName.Text;
                        sessionObj.Instructor_Name = editInstructorName.Text;
                        sessionObj.OptionA = editOptionA.Text;
                        sessionObj.OptionB = editOptionB.Text;
                        sessionObj.OptionC = editOptionC.Text;
                        sessionObj.OptionD = editOptionD.Text;
                        sessionObj.CorrectAnswer = correctOption;
                        sessionObj.Question = editQuestion.Text;
                        var firebase = new FirebaseClient(FirebaseURL);
                        var item = await firebase.Child("Session").PostAsync<Session>(sessionObj);                        //
                        var toMainActivity = new Intent(this, typeof(MainActivity));
                        toMainActivity.PutExtra("FromAddActivtiy", sessionObj.Session_Name); 
                        if (alertDialog.IsShowing)
                            alertDialog.Dismiss();
                        StartActivity(toMainActivity);
                    }
                    else
                    {                      
                        Toast.MakeText(this, "Please fill in all the details", ToastLength.Short).Show();
                    }
                }
                catch (Exception)
                {
                    string message = "Please Check your Internet Connection And Try Again.";
                    displayProgressDialog(message);
                }

            };

            buttonCancelCreate.Click += delegate
            {
                StartActivity(typeof(MainActivity));
            };
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
