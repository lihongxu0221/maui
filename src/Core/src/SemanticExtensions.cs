﻿using System;
#if __IOS__ || MACCATALYST
using UIKit;
using PlatformView = UIKit.UIView;
#elif __ANDROID__
using Android.Text;
using Android.Views;
using Android.Views.Accessibility;
using PlatformView = Android.Views.View;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif NETSTANDARD
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui
{
	public static partial class SemanticExtensions
	{
		public static void SetSemanticFocus(this IView element)
		{
			if (element?.Handler?.PlatformView is not PlatformView nativeView)
				throw new NullReferenceException("Can't access view from a null handler");

#if __ANDROID__
			nativeView.SendAccessibilityEvent(EventTypes.ViewHoverEnter);
#elif __IOS__ || MACCATALYST
			UIAccessibility.PostNotification(UIAccessibilityPostNotification.LayoutChanged, nativeView);
#endif
		}
	}
}