using Foundation;
using System;
using UIKit;
using SQLite;

namespace LeapUser
{
    public partial class OTPViewController : UIViewController
    {
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

        private int primary_key=0;

        private Session session;

        public OTPViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var db = new SQLiteConnection(dbPath);
            var plist = NSUserDefaults.StandardUserDefaults;
            primary_key = Convert.ToInt32(plist.StringForKey("primaryKey"));
            session = db.Table<Session>().Where(x => x.Id == primary_key).FirstOrDefault();
            textOTPSessionName.Text = session.Session_Name + " OTP";
           
        }

       
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
            var db = new SQLiteConnection(dbPath);
            var controller = segue.DestinationViewController as QuestionViewController;
            controller.dbPath = dbPath;
            controller.session_count = session_count;

		}

        partial void ButtonOTP_Activated(UIBarButtonItem sender)
        {
            try
            {
				if (textOTP.Text.Length == 4 && Convert.ToInt32(textOTP.Text) < 10000 && Convert.ToInt32(textOTP.Text) > 999 && Convert.ToInt32(textOTP.Text) == session.OTP)
                {
					PerformSegue("OTPSuccessful", null);
				}
				else
				{
					var displayAlert = UIAlertController.Create("Invalid OTP", "Please enter a valid OTP for the Session.", UIAlertControllerStyle.Alert);
					displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
					PresentViewController(displayAlert, true, null);

				}
            }
            catch(Exception)
            {
				var displayAlert = UIAlertController.Create("Invalid OTP", "Please enter a valid OTP for the Session.", UIAlertControllerStyle.Alert);
				displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
				PresentViewController(displayAlert, true, null);
            }

           
        }
    }
}