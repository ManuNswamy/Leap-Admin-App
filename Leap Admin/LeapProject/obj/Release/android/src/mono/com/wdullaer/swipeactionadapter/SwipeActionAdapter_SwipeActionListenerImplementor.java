package mono.com.wdullaer.swipeactionadapter;


public class SwipeActionAdapter_SwipeActionListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.wdullaer.swipeactionadapter.SwipeActionAdapter.SwipeActionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_hasActions:(ILcom/wdullaer/swipeactionadapter/SwipeDirection;)Z:GetHasActions_ILcom_wdullaer_swipeactionadapter_SwipeDirection_Handler:Com.Wdullaer.Swipeactionadapter.SwipeActionAdapter/ISwipeActionListenerInvoker, Binding_SwipeActionAdapter\n" +
			"n_onSwipe:([I[Lcom/wdullaer/swipeactionadapter/SwipeDirection;)V:GetOnSwipe_arrayIarrayLcom_wdullaer_swipeactionadapter_SwipeDirection_Handler:Com.Wdullaer.Swipeactionadapter.SwipeActionAdapter/ISwipeActionListenerInvoker, Binding_SwipeActionAdapter\n" +
			"n_shouldDismiss:(ILcom/wdullaer/swipeactionadapter/SwipeDirection;)Z:GetShouldDismiss_ILcom_wdullaer_swipeactionadapter_SwipeDirection_Handler:Com.Wdullaer.Swipeactionadapter.SwipeActionAdapter/ISwipeActionListenerInvoker, Binding_SwipeActionAdapter\n" +
			"";
		mono.android.Runtime.register ("Com.Wdullaer.Swipeactionadapter.SwipeActionAdapter+ISwipeActionListenerImplementor, Binding_SwipeActionAdapter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SwipeActionAdapter_SwipeActionListenerImplementor.class, __md_methods);
	}


	public SwipeActionAdapter_SwipeActionListenerImplementor ()
	{
		super ();
		if (getClass () == SwipeActionAdapter_SwipeActionListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Wdullaer.Swipeactionadapter.SwipeActionAdapter+ISwipeActionListenerImplementor, Binding_SwipeActionAdapter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean hasActions (int p0, com.wdullaer.swipeactionadapter.SwipeDirection p1)
	{
		return n_hasActions (p0, p1);
	}

	private native boolean n_hasActions (int p0, com.wdullaer.swipeactionadapter.SwipeDirection p1);


	public void onSwipe (int[] p0, com.wdullaer.swipeactionadapter.SwipeDirection[] p1)
	{
		n_onSwipe (p0, p1);
	}

	private native void n_onSwipe (int[] p0, com.wdullaer.swipeactionadapter.SwipeDirection[] p1);


	public boolean shouldDismiss (int p0, com.wdullaer.swipeactionadapter.SwipeDirection p1)
	{
		return n_shouldDismiss (p0, p1);
	}

	private native boolean n_shouldDismiss (int p0, com.wdullaer.swipeactionadapter.SwipeDirection p1);

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
