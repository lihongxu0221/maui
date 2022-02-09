using System;
using Microsoft.Extensions.DependencyInjection;
#if __IOS__ || MACCATALYST
using NativeView = UIKit.UIWindow;
#elif MONOANDROID
using NativeView = Android.App.Activity;
#elif WINDOWS
using NativeView = Microsoft.UI.Xaml.Window;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial class WindowHandler : IWindowHandler
	{
		public static IPropertyMapper<IWindow, IWindowHandler> Mapper = new PropertyMapper<IWindow, IWindowHandler>(ElementHandler.ElementMapper)
		{
			[nameof(IWindow.Title)] = MapTitle,
			[nameof(IWindow.Content)] = MapContent,
#if ANDROID || WINDOWS
			[nameof(IToolbarElement.Toolbar)] = MapToolbar,
#endif
#if WINDOWS
			[nameof(IMenuBarElement.MenuBar)] = MapMenuBar,
#endif
		};

		public WindowHandler()
			: base(Mapper)
		{
		}

		public WindowHandler(IPropertyMapper? mapper = null)
			: base(mapper ?? Mapper)
		{
		}

#if !NETSTANDARD
		protected override NativeView CreateNativeElement() =>
			MauiContext?.Services.GetService<NativeView>() ?? throw new InvalidOperationException($"MauiContext did not have a valid window.");
#endif
	}
}