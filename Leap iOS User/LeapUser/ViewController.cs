using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Foundation;
using SQLite;
using UIKit;

namespace LeapUser
{
    public partial class ViewController : UIViewController
    {

        static int session_count = 0;
		public string dbPath;
        private string FirebaseURL = "https://leapproject-b603d.firebaseio.com/";

		protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            var plist = NSUserDefaults.StandardUserDefaults;
            plist.RemoveObject("name");
            plist.RemoveObject("mobilenumber");

            //path to the local database
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			dbPath = Path.Combine(documents, "SessionDB.db");

           
           
        }

        //directly route to the OTP screen if the shared preference is already there
		public override async void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			var plist = NSUserDefaults.StandardUserDefaults;
			Console.WriteLine("" + plist.StringForKey("name"));
			Console.WriteLine("" + plist.DoubleForKey("mobilenumber"));
			if (plist.StringForKey("name") != null && plist.DoubleForKey("mobilenumber") > 0)
			{
				Console.WriteLine("Hello");
				PerformSegue("registerSuccessful", null);
			}
			if (session_count < 1)
			{
				while (session_count < 1)
				{
					await fetchData();
				}
			}
		}
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			var controller = segue.DestinationViewController as OTPViewController;
            controller.session_count = session_count;
            controller.dbPath = dbPath;

		}

        partial void ButtonRegister_Activated(UIBarButtonItem sender)
        {
            var plist = NSUserDefaults.StandardUserDefaults;
            try
            {
                //validation
				if (textName.Text.Length >= 3 && textMobileNumber.Text.Length == 10 && Convert.ToDouble(textMobileNumber.Text) < 10000000000)
				{
					string name = textName.Text;
					double mobileNumber = Convert.ToDouble(textMobileNumber.Text);
					plist.SetString(name, "name");
					plist.SetDouble(mobileNumber, "mobilenumber");
                    plist.SetString("1", "primaryKey");
                    PerformSegue("registerSuccessful", null);

				}
				else
				{
					var displayAlert = UIAlertController.Create("Invalid Sign Up", "Please enter a valid Name and Mobile Number", UIAlertControllerStyle.Alert);
					displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
					PresentViewController(displayAlert, true, null);

				}
            }
            //caught if the mobile number contains a alpha character or text
            catch(Exception ex)
            {
				var displayAlert = UIAlertController.Create("Exception", "Please enter a valid Name and Mobile Number", UIAlertControllerStyle.Alert);
				displayAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => Console.WriteLine("OK Button Clicked")));
				PresentViewController(displayAlert, true, null);
            }


        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

		private async Task fetchData()
		{
			var db = new SQLiteConnection(dbPath);
			db.DropTable<Session>();
			db.CreateTable<Session>();
			Console.WriteLine("Table created!");
			try
			{
                var firebase = new FirebaseClient(FirebaseURL);
				var items = await firebase.Child("Session").OnceAsync<Session>();
                Console.WriteLine(""+items);
                foreach (var item in items)
                {
                    db.Insert(item.Object);
                }
                session_count = items.Count;
			}
			catch
			{
				Console.WriteLine("Failed to update the database");
			}
		}

	

	}
}
