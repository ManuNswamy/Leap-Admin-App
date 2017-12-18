package md50dc39590a61df7caff8777c8cde1d171;


public class CustomGestureDetector
	extends android.view.GestureDetector.SimpleOnGestureListener
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnTouchListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouch:(Landroid/view/View;Landroid/view/MotionEvent;)Z:GetOnTouch_Landroid_view_View_Landroid_view_MotionEvent_Handler:Android.Views.View/IOnTouchListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Torman.Swipeable.Gesture.CustomGestureDetector, Torman.Swipeable, Version=2.0.2.0, Culture=neutral, PublicKeyToken=null", CustomGestureDetector.class, __md_methods);
	}


	public CustomGestureDetector () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CustomGestureDetector.class)
			mono.android.TypeManager.Activate ("Torman.Swipeable.Gesture.CustomGestureDetector, Torman.Swipeable, Version=2.0.2.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CustomGestureDetector (md5dffab06e463ecdf2166e607e9f99ac64.SwipeLayout p0, float p1, android.content.Context p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == CustomGestureDetector.class)
			mono.android.TypeManager.Activate ("Torman.Swipeable.Gesture.CustomGestureDetector, Torman.Swipeable, Version=2.0.2.0, Culture=neutral, PublicKeyToken=null", "Torman.Swipeable.SwipeLayout, Torman.Swipeable, Version=2.0.2.0, Culture=neutral, PublicKeyToken=null:System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean onTouch (android.view.View p0, android.view.MotionEvent p1)
	{
		return n_onTouch (p0, p1);
	}

	private native boolean n_onTouch (android.view.View p0, android.view.MotionEvent p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
