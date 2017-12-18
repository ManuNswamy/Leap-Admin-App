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
using Torman.Swipeable;
using Torman.Swipeable.Enums;

using Torman.Swipeable.Builders;
using Android.Graphics;

namespace LeapProjectUser
{
    class CustomAdapter : BaseAdapter
    {

        private List<string> array;
        private static List<int> completedSession = new List<int>();
        string dbPath;
        private Context context;

        public CustomAdapter(Context context, List<String> session_names, string dbPath)
        {
            this.context = context;
            this.array = session_names;
            this.dbPath = dbPath;
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

            topView.FindViewById<TextView>(Resource.Id.textView).Text = array[position];

            if(completedSession.Contains(position))
            {
                swipeableView.DisableSwipe();
            }
            else
            {

                swipeableView.SetOnlyRightSwipe().EnablePossibleSwipeAnimationOnClick(true).AddSwipeButton(new ButtonBuilder(() => takeTest(position), Color.Green)
                                .SetTitle("TAKE TEST")
                                .SetForegroundColor(Color.White)
                                .SetButtonSize(ButtonSize.Wrap),
                            SwipeDirection.Right);
            }


            return swipeableView;
        }

        private async void takeTest(int position)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(context);
            View mView = layoutInflater.Inflate(Resource.Layout.OTPLayout, null);
            Button buttonTakeTest = mView.FindViewById<Button>(Resource.Id.buttonStartTest);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(context);
            alertDialogBuilder.SetView(mView);
            var alert = alertDialogBuilder.Show();

            buttonTakeTest.Click += delegate
            {
                var OTP = mView.FindViewById<TextView>(Resource.Id.editOTP).Text;
                var db = new SQLiteConnection(dbPath);
                string name = array[position];
                var session = db.Table<Session>().Where(x => x.Session_Name == name).FirstOrDefault();
                if (session.OTP.ToString() == OTP)
                {
                    Toast.MakeText(context, "START SESSION", ToastLength.Short).Show();
                    var testActivity = new Intent(context, typeof(TestActivity));
                    testActivity.PutExtra("sessionName",name);
                    testActivity.PutExtra("dbPath",dbPath);
                    completedSession.Add(position);
                    alert.Dismiss();
                    context.StartActivity(testActivity);
                }
                else
                {
                    Toast.MakeText(context, "SORRY WRONG OTP", ToastLength.Short).Show();
                    alert.Dismiss();
                }
            };
        }


    }
}