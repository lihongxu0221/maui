﻿using System;

namespace Microsoft.Maui.Platform
{
	public static class WindowExtensions
	{
		public static void UpdateTitle(this UI.Xaml.Window platformWindow, IWindow window)
		{
			platformWindow.Title = window.Title;

			var rootManager = window.Handler?.MauiContext?.GetNavigationRootManager();
			if(rootManager != null)
			{
				rootManager.SetWindowTitle(window.Title);
			}
		}

		public static IWindow? GetWindow(this UI.Xaml.Window platformWindow)
		{
			foreach(var window in MauiWinUIApplication.Current.Application.Windows)
			{
				if (window?.Handler?.PlatformView is UI.Xaml.Window win && win == platformWindow)
					return window;
			}

			return null;
		}
	}
}