// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LeapUser
{
    [Register ("OTPViewController")]
    partial class OTPViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem buttonOTP { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textOTP { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOTPSessionName { get; set; }

        [Action ("ButtonOTP_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ButtonOTP_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (buttonOTP != null) {
                buttonOTP.Dispose ();
                buttonOTP = null;
            }

            if (textOTP != null) {
                textOTP.Dispose ();
                textOTP = null;
            }

            if (textOTPSessionName != null) {
                textOTPSessionName.Dispose ();
                textOTPSessionName = null;
            }
        }
    }
}