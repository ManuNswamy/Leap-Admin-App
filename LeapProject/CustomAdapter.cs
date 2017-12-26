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
using System.Collections.ObjectModel;
using Torman.Swipeable;
using Torman.Swipeable.Builders;
using Android.Graphics;
using Torman.Swipeable.Enums;
using Firebase.Xamarin.Database;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LeapProject
{
    class CustomAdapter : BaseAdapter
    {

        private static List<string> array;
        //private List<int> sessionsRating;
        private String editURL;
        private List<int> activeSessionsPosition;
        string FirebaseURL = "https://leapproject-b603d.firebaseio.com/";
        private Context context;
        private MainActivity mainActivity;
        private Session sessionObj1;

        public CustomAdapter(Context context, List<String> session_names, List<int> activeSessionsPosition)//,List<int> sessionsRating)
        {
            this.context = context;
            array = session_names;
            //this.sessionsRating = sessionsRating;
            this.activeSessionsPosition = activeSessionsPosition;
        }

       
        public override int Count => array.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return array[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var swipeableView = convertView as SwipeLayout;

            if (swipeableView == null)
            {
                swipeableView = new SwipeLayout(context);
            }

            var topView = swipeableView.SetTopView(Resource.Layout.RowLayout);
            //RatingBar ratingbar = topView.FindViewById<RatingBar>(Resource.Id.ratingBar);
            //ratingbar.Visibility = ViewStates.Gone;
            //ratingbar.Rating = sessionsRating[position];

          

            TextView textView = topView.FindViewById<TextView>(Resource.Id.textView);
            textView.Text = array[position];

           

            if(activeSessionsPosition.Contains(position))
            {
                topView.SetBackgroundColor(ColorHelper.Completed);
                //myButton.SetBackgroundColor(ColorHelper.Completed);

                swipeableView.SetClipToPadding(true);
                swipeableView.SetOnlyRightSwipe().EnablePossibleSwipeAnimationOnClick(true).AddSwipeButton(new ButtonBuilder(() => ViewSession(position), ColorHelper.Purple)
                            .SetTitle(" VIEW ")
                            .SetForegroundColor(Color.White)
                            .SetButtonSize(ButtonSize.Wrap),
                        SwipeDirection.Right).SetBackgroundColor(Color.DarkGreen); ;

            }
            else
            {
                topView.SetBackgroundColor(ColorHelper.Inactive);
                swipeableView.SetBothWaysSwipe().EnablePossibleSwipeAnimationOnClick(true).AddSwipeButton(new ButtonBuilder(() => DeleteSession(position), ColorHelper.Red)
                               .SetTitle(" DELETE ? ")
                               .SetForegroundColor(Color.White)
                               .SetButtonSize(ButtonSize.Wrap),
                           SwipeDirection.Left).AddSwipeButton(new ButtonBuilder(() => ShowEditDetails(position), ColorHelper.Green)
                               .SetTitle(" EDIT ")
                               .SetForegroundColor(Color.White)
                               .SetButtonSize(ButtonSize.Wrap),
                           SwipeDirection.Right);

            }

            return swipeableView;
        }      

        private async void ViewSession(int position)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View mView = layoutInflater.Inflate(Resource.Layout.ProgressLayout, null);
            //mView.SetBackgroundColor(Color.Transparent);
            Android.Support.V7.App.AlertDialog.Builder alertProgress = new Android.Support.V7.App.AlertDialog.Builder(context);
            alertProgress.SetView(mView).SetCancelable(false);
            var alert = alertProgress.Show();
            Session sessionObj = new Session();
            try
            {
                         
                var firebase = new FirebaseClient("https://leapproject-b603d.firebaseio.com/");
                var items = await firebase.Child("Session").OnceAsync<Session>();
                if(items.Count<1)
                {
                    alert.Dismiss();
                    Toast.MakeText(context, "Please Check the Internet Connection and Try Again.", ToastLength.Short).Show();
                }
                foreach (var item in items)
                {
                    if (item.Object.Session_Name == array[position])
                    {
                        sessionObj = item.Object;
                        alert.Dismiss();
                        displaySessionDetails(sessionObj);
                    }
                }

            }
            catch (Exception)
            {
                Toast.MakeText(context, "Couldn't Sync the Firebase Data", ToastLength.Short).Show();
            }
        }

        private void displaySessionDetails(Session sessionObj)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View mView = layoutInflater.Inflate(Resource.Layout.view_session_details, null);
            mView.FindViewById<TextView>(Resource.Id.displayTextOTP).Text = "OTP : "+sessionObj.OTP;
            mView.FindViewById<TextView>(Resource.Id.displayTextSessionName).Text = sessionObj.Session_Name;
            mView.FindViewById<TextView>(Resource.Id.displayTextInstructorName).Text = sessionObj.Instructor_Name;

            mView.FindViewById<TextView>(Resource.Id.displayTextQuestion).Text = sessionObj.Question;
            mView.FindViewById<TextView>(Resource.Id.displayTextOptionA).Text = "A )  "+sessionObj.OptionA;
            mView.FindViewById<TextView>(Resource.Id.displayTextOptionB).Text = "B )  " + sessionObj.OptionB;
            mView.FindViewById<TextView>(Resource.Id.displayTextOptionC).Text = "C )  " + sessionObj.OptionC;
            mView.FindViewById<TextView>(Resource.Id.displayTextOptionD).Text = "D )  " + sessionObj.OptionD;
            mView.FindViewById<TextView>(Resource.Id.displayTextCorrectOption).Text = "Correct Answer : "+sessionObj.CorrectAnswer;

            
            Button buttonCancel = mView.FindViewById<Button>(Resource.Id.buttonCancel);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(context);
            alertDialogBuilder.SetView(mView);
            var alert = alertDialogBuilder.Show();
            buttonCancel.Click += delegate
            {
                alert.Dismiss();
            };
           

        }

        private async void DeleteSession(int index)
        {
            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(context);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("Delete");
            alert.SetMessage("Are you Sure ?");
            alert.SetIcon(Android.Resource.Drawable.IcDialogAlert);
            Console.WriteLine("Click Detected " + array[index]);
            Session sessionObj = new Session();
            try
            {
                var firebase = new FirebaseClient("https://leapproject-b603d.firebaseio.com/");
                var items = await firebase.Child("Session").OnceAsync<Session>();
                if (items.Count < 1)
                {
                    alert.Dismiss();
                    Toast.MakeText(context, "Please Check the Internet Connection and Try Again.", ToastLength.Short).Show();
                }
                foreach (var item in items)
                {
                    if (item.Object.Session_Name == array[index])
                    {
                        sessionObj = item.Object;
                        editURL = "https://leapproject-b603d.firebaseio.com/" + "Session/" + item.Key + "/.json/";
                        Console.WriteLine("edit URL = " + editURL);    
                    }
                }
                alert.SetButton("DELETE", (c, ev) =>
                {
                    performDelete(editURL, array[index]);
                    array.Remove(array[index]);
                });
                alert.SetButton2("CANCEL", (c, ev) => { });
                alert.Show();
            }
            catch (Exception)
            {
                Toast.MakeText(context, "Please Check the Internet Connection and Try Again.", ToastLength.Short).Show();
                alert.Dismiss();
            }
          
        }

        private async void performDelete(string editURL,string sessionName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //post request
                    var result = await client.DeleteAsync(editURL);                   
                    NotifyDataSetChanged();
                    NotifyDataSetInvalidated();
                }
            }
            catch(Exception)
            {
                Toast.MakeText(context, "Please Check the Internet Connection and Try Again.", ToastLength.Short).Show();
            }
        }

        private async void ShowEditDetails(int index)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View mView = layoutInflater.Inflate(Resource.Layout.ProgressLayout, null);
            //mView.SetBackgroundColor(Color.Transparent);
            Android.Support.V7.App.AlertDialog.Builder alertProgress = new Android.Support.V7.App.AlertDialog.Builder(context);
            alertProgress.SetView(mView).SetCancelable(false);
            var alert = alertProgress.Show();
            Session sessionObj = new Session();
            try
            {
                var firebase = new FirebaseClient("https://leapproject-b603d.firebaseio.com/");
                var items = await firebase.Child("Session").OnceAsync<Session>();
                foreach (var item in items)
                {
                    if (item.Object.Session_Name == array[index])
                    {
                        sessionObj = item.Object;                        
                        editURL = "https://leapproject-b603d.firebaseio.com/" + "Session/" + item.Key + "/.json/";
                        alert.Dismiss();
                        handleEditDetails(sessionObj);
                    }
                }          

            }
            catch (Exception)
            {
                Toast.MakeText(context, "Please Check the Internet Connection and Try Again.", ToastLength.Short).Show();
            }
        }

        private void handleEditDetails(Session sessionObj)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View mView = layoutInflater.Inflate(Resource.Layout.edit_session_details, null);
            TextView textOTP = mView.FindViewById<TextView>(Resource.Id.textOTP);
            TextView textCorrectOption = mView.FindViewById<TextView>(Resource.Id.textCorrectOption);
            EditText editSessionName = mView.FindViewById<EditText>(Resource.Id.editSessionName);
            EditText editInstructorName = mView.FindViewById<EditText>(Resource.Id.editInstructorName);
            EditText editQuestion = mView.FindViewById<EditText>(Resource.Id.editQuestion);
            EditText editOptionA = mView.FindViewById<EditText>(Resource.Id.editOptionA);
            EditText editOptionB = mView.FindViewById<EditText>(Resource.Id.editOptionB);
            EditText editOptionC = mView.FindViewById<EditText>(Resource.Id.editOptionC);
            EditText editOptionD = mView.FindViewById<EditText>(Resource.Id.editOptionD);
            Button buttonEditSession = mView.FindViewById<Button>(Resource.Id.buttonEditSession);
            Button buttonCancelEdit = mView.FindViewById<Button>(Resource.Id.buttonCancelEdit);
            List<string> options = new List<String> { "Select an Option", "Option A", "Option B", "Option C", "Option D" };
            var adapterCorrectOption = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem, options);
            Spinner spinnerCorrectOption = mView.FindViewById<Spinner>(Resource.Id.spinnerCorrectOption);
            spinnerCorrectOption.Adapter = adapterCorrectOption;
            textOTP.Text = "OTP : " + sessionObj.OTP;
            editSessionName.Text = sessionObj.Session_Name;
            editInstructorName.Text = sessionObj.Instructor_Name;
            editQuestion.Text = sessionObj.Question;
            editOptionA.Text = sessionObj.OptionA;
            editOptionB.Text = sessionObj.OptionB;
            editOptionC.Text = sessionObj.OptionC;
            editOptionD.Text = sessionObj.OptionD;
            textCorrectOption.Text = "Correct Option :" + sessionObj.CorrectAnswer;
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(context);
            alertDialogBuilder.SetView(mView);
            var alert = alertDialogBuilder.Show();
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


            buttonEditSession.Click += async delegate
            {

                if (editSessionName.Text != "" && editQuestion.Text != "" && editQuestion.Text != "" && editOptionA.Text != "" && editOptionB.Text != "" && editOptionC.Text != "" && editOptionD.Text != "")
                {
                    alert.Dismiss();
                    mView = layoutInflater.Inflate(Resource.Layout.ProgressLayout, null);
                    //mView.SetBackgroundColor(Color.Transparent);
                    Android.Support.V7.App.AlertDialog.Builder alertProgress = new Android.Support.V7.App.AlertDialog.Builder(context);
                    alertProgress.SetView(mView).SetCancelable(false);
                    alert = alertProgress.Show();

                    try
                    {
                        using (var client = new HttpClient())
                        {
                            int index = array.IndexOf(sessionObj.Session_Name);
                            array[index] = editSessionName.Text;
                            sessionObj.Session_Name = editSessionName.Text;
                            sessionObj.Instructor_Name = editInstructorName.Text;
                            sessionObj.Question = editQuestion.Text;
                            sessionObj.OptionA = editOptionA.Text;
                            sessionObj.OptionB = editOptionB.Text;
                            sessionObj.OptionC = editOptionC.Text;
                            sessionObj.OptionD = editOptionD.Text;
                            if (correctOption != "")
                            {
                                sessionObj.CorrectAnswer = correctOption;
                            }
                            //convert the object to json
                            var json = JsonConvert.SerializeObject(sessionObj);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            //post request
                            var result = await client.PutAsync(editURL, content);

                            NotifyDataSetChanged();
                            NotifyDataSetInvalidated();
                            alert.Dismiss();

                        }
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(context, "Please Check the Internet Connection and Try Again.", ToastLength.Short).Show();
                        alert.Dismiss();
                    }
                }
                else
                {
                    Toast.MakeText(context, "Please fill in all the details", ToastLength.Short).Show();
                }


            };
            buttonCancelEdit.Click += delegate
            {
                alert.Dismiss();
            };
        }

        private void ToastButtonClicked(int row, string buttonName)
        {
            Toast.MakeText(context, $"{buttonName} from row {row + 1} clicked!", ToastLength.Short).Show();
        }
        
    }  
}