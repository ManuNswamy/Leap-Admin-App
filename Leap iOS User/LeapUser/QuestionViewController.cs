using Foundation;
using System;
using UIKit;
using SQLite;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace LeapUser
{
    public partial class QuestionViewController : UIViewController
    {
        String chosenAnswer;

		public string dbPath
		{
			get;
			set;

		}
		public int session_count
		{
			get;
			set;
		}

        private int primary_key;
        private Session session;
        private string FirebaseURL = "https://leapproject-b603d.firebaseio.com/";
		public QuestionViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			var plist = NSUserDefaults.StandardUserDefaults;
			primary_key = Convert.ToInt32(plist.StringForKey("primaryKey"));
            var db = new SQLiteConnection(dbPath);
            session = db.Table<Session>().Where(x => x.Id == primary_key).FirstOrDefault();

            textQuestion.Text = session.Question;
            textOptionA.Text = session.OptionA;
            textOptionB.Text = session.OptionB;
            textOptionC.Text = session.OptionC;
            textOptionD.Text = session.OptionD;
            chosenAnswer= "";

			optionA.TouchUpInside += delegate
            {
                optionB.SetState(false, true);
                optionC.SetState(false, true);
                optionD.SetState(false, true);
                chosenAnswer = "Option A";
            };
			optionB.TouchUpInside += delegate
		   {
			   optionA.SetState(false, true);
			   optionC.SetState(false, true);
			   optionD.SetState(false, true);
                chosenAnswer = "Option B";
		   };
			optionC.TouchUpInside += delegate
		   {
			   optionA.SetState(false, true);
			   optionB.SetState(false, true);
			   optionD.SetState(false, true);
                chosenAnswer = "Option C";
		   };
			optionD.TouchUpInside += delegate
		   {
			   optionA.SetState(false, true);
			   optionB.SetState(false, true);
			   optionC.SetState(false, true);
                chosenAnswer = "Option D";
		   };
        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			var controller = segue.DestinationViewController as OTPViewController;
            controller.session_count = session_count;
            controller.dbPath = dbPath;


		}

		partial void ButtonSubmit_Activated(UIBarButtonItem sender)
        {
			var plist = NSUserDefaults.StandardUserDefaults;
            primary_key= primary_key+1;
            if(chosenAnswer!="")
            {
                if(primary_key<=session_count)
                {
                    plist.SetString(""+primary_key, "primaryKey");
                    if(chosenAnswer==session.CorrectAnswer)
                    {
                        var firebase = new FirebaseClient(FirebaseURL);
                        SessionResponse response = new SessionResponse();
                        response.mobilenumber = plist.DoubleForKey("mobilenumber");
                        response.name = plist.StringForKey("name");
                        var item = firebase.Child(session.Session_Name).Child("Score").PostAsync<SessionResponse>(response);
                    }
                    try
                    {
                     PerformSegue("backOTP", null);   
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("PerformSegue() Question ViewController " + ex);
                    }
                }
                else
                {
                    
					var displayAlert = UIAlertController.Create("Session Complete", "Thank You for you Participation.", UIAlertControllerStyle.Alert);
					displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
					PresentViewController(displayAlert, true, null);
                }
               
            }
            else
            {
				var displayAlert = UIAlertController.Create("Submit Failed", "Please select an Option.", UIAlertControllerStyle.Alert);
				displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
				PresentViewController(displayAlert, true, null);
            }
        }
    }
}