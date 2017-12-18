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
    [Register ("QuestionViewController")]
    partial class QuestionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem buttonSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optionA { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optionB { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optionC { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optionD { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOptionA { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOptionB { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOptionC { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOptionD { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textQuestion { get; set; }

        [Action ("ButtonSubmit_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ButtonSubmit_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (buttonSubmit != null) {
                buttonSubmit.Dispose ();
                buttonSubmit = null;
            }

            if (optionA != null) {
                optionA.Dispose ();
                optionA = null;
            }

            if (optionB != null) {
                optionB.Dispose ();
                optionB = null;
            }

            if (optionC != null) {
                optionC.Dispose ();
                optionC = null;
            }

            if (optionD != null) {
                optionD.Dispose ();
                optionD = null;
            }

            if (textOptionA != null) {
                textOptionA.Dispose ();
                textOptionA = null;
            }

            if (textOptionB != null) {
                textOptionB.Dispose ();
                textOptionB = null;
            }

            if (textOptionC != null) {
                textOptionC.Dispose ();
                textOptionC = null;
            }

            if (textOptionD != null) {
                textOptionD.Dispose ();
                textOptionD = null;
            }

            if (textQuestion != null) {
                textQuestion.Dispose ();
                textQuestion = null;
            }
        }
    }
}