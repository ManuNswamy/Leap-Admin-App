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
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace LeapProjectUser
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/icon")]
    public class TestActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TakeTestLayout);
            
            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);

            TextView displayQuestion = FindViewById<TextView>(Resource.Id.displayQuestion);
            CheckBox optionA = FindViewById<CheckBox>(Resource.Id.checkBoxOptionA);
            CheckBox optionB = FindViewById<CheckBox>(Resource.Id.checkBoxOptionB);
            CheckBox optionC = FindViewById<CheckBox>(Resource.Id.checkBoxOptionC);
            CheckBox optionD = FindViewById<CheckBox>(Resource.Id.checkBoxOptionD);

            string sessionName = Intent.GetStringExtra("sessionName");
            string dbPath = Intent.GetStringExtra("dbPath");
            var db = new SQLiteConnection(dbPath);          
            var session = db.Table<Session>().Where(x => x.Session_Name == sessionName).FirstOrDefault();
            var sessionList = db.Table<Session>();
            
            displayQuestion.Text = session.Question;
            optionA.Text = session.OptionA;
            optionB.Text = session.OptionB;
            optionC.Text = session.OptionC;
            optionD.Text = session.OptionD;
            Button submit = FindViewById<Button>(Resource.Id.buttonSubmit);


            optionA.Click += delegate
            {
                optionA.Checked = true;
                optionB.Checked = false;
                optionC.Checked = false;
                optionD.Checked = false;

            };

            optionB.Click += delegate
            {
                optionB.Checked = true;
                optionA.Checked = false;
                optionC.Checked = false;
                optionD.Checked = false;

            };

            optionC.Click += delegate
            {
                optionC.Checked = true;
                optionA.Checked = false;
                optionB.Checked = false;
                optionD.Checked = false;

            };

            optionD.Click += delegate
            {
                optionD.Checked = true;
                optionA.Checked = false;
                optionB.Checked = false;
                optionC.Checked = false;

            };



            submit.Click += delegate
             {
                 ap.saveValue("testAttempted", "" + (Convert.ToInt32(ap.getValue("testAttempted")) + 1));
                 switch (session.CorrectAnswer)
                 {
                     case "Option A":
                         if (optionA.Checked)
                             ap.saveValue("score",""+(Convert.ToInt32(ap.getValue("score"))+1));
                         break;

                     case "Option B":
                         if (optionB.Checked)
                             ap.saveValue("score", "" + (Convert.ToInt32(ap.getValue("score")) + 1));
                         break;

                     case "Option C":
                         if (optionC.Checked)
                             ap.saveValue("score", "" + (Convert.ToInt32(ap.getValue("score")) + 1));
                         break;

                     case "Option D":
                         if (optionD.Checked)
                             ap.saveValue("score", "" + (Convert.ToInt32(ap.getValue("score")) + 1));
                         break;

                     default:
                         break;
                 }

                 LayoutInflater layoutInflater = LayoutInflater.From(this);
                 View mView = layoutInflater.Inflate(Resource.Layout.RatingLayout, null);
                 RatingBar ratingBar = mView.FindViewById<RatingBar>(Resource.Id.ratingBar1);
                 Button buttonRating = mView.FindViewById<Button>(Resource.Id.buttonRating);
                 Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                 alertDialogBuilder.SetView(mView);
                 var alert = alertDialogBuilder.Show();
                 alertDialogBuilder.SetCancelable(false);
                 buttonRating.Click += async delegate
                  {
                      try
                      {
                          var firebase = new FirebaseClient("https://leapproject-b603d.firebaseio.com/");
                          int rating = Convert.ToInt32(ratingBar.Rating);
                          var items = await firebase.Child(session.Session_Name).PostAsync<int>(rating);
                          //Toast.MakeText(this, "your Score is " + ap.getValue("score"), ToastLength.Short).Show();

                          if (ap.getValue("testAttempted") == "" + sessionList.Count())
                          {

                              Toast.MakeText(this, "WOWEQAW", ToastLength.Short).Show();
                            

                               for(int i=0;i<10;i++)
                              {
                                  Random rand = new Random();
                                  SessionResponse sessionResponse = new SessionResponse();
                                  sessionResponse.name = ap.getValue("name");
                                  sessionResponse.mobilenumber = Convert.ToDouble(ap.getValue("mobilenumber"));
                                  //sessionResponse.score = Convert.ToInt32(ap.getValue("score"));
                                  sessionResponse.score = rand.Next(1, 9);
                                  var scoreItem = await firebase.Child("Score").Child("" + sessionResponse.score).PostAsync<SessionResponse>(sessionResponse);
                                 
                              }

                          }

                          alert.Dismiss();
                          var mainActivity = new Intent(this, typeof(MainActivity));                          
                          StartActivity(mainActivity);
                      }
                      catch
                      {
                          Console.WriteLine("Failed to update the database");
                      }
                  };

                 
              
             };
            // Create your application here
        }
        public override void OnBackPressed()
        {
        }

    }
}