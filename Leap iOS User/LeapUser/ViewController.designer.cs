// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LeapUser
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem buttonRegister { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textMobileNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textName { get; set; }

        [Action ("ButtonRegister_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ButtonRegister_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (buttonRegister != null) {
                buttonRegister.Dispose ();
                buttonRegister = null;
            }

            if (textMobileNumber != null) {
                textMobileNumber.Dispose ();
                textMobileNumber = null;
            }

            if (textName != null) {
                textName.Dispose ();
                textName = null;
            }
        }
    }
}