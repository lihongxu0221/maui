﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using WPanel = Microsoft.UI.Xaml.Controls.Panel;
using WFrameworkElement = Microsoft.UI.Xaml.FrameworkElement;
using WWindow = Microsoft.UI.Xaml.Window;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.FlyoutPage)]
	public partial class FlyoutPageTests : HandlerTestBase
	{
		[Fact(DisplayName = "FlyoutPage Initializes with PaneFooter Set")]
		public async Task FlyoutPageInitializesWithPaneFooterSet()
		{
			SetupBuilder();
			var flyoutPage = CreateBasicFlyoutPage();
			await InvokeOnMainThreadAsync(async () =>
			{
				await CreateHandlerAndAddToWindow<FlyoutViewHandler>(flyoutPage, (handler) =>
				{
					Assert.NotNull(handler.PlatformView.PaneFooter);
					return Task.CompletedTask;
				});
			});
		}


		[Fact(DisplayName = "FlyoutPage Initializes with Header Set")]
		public async Task FlyoutPageInitializesWithHeaderSet()
		{
			SetupBuilder();
			var flyoutPage = CreateBasicFlyoutPage();

			await InvokeOnMainThreadAsync(async () =>
			{
				await CreateHandlerAndAddToWindow<WindowHandlerStub>(new Window(flyoutPage), (handler) =>
				{
					var navView = GetMauiNavigationView(handler.MauiContext);
					Assert.NotNull(navView.Header);
					return Task.CompletedTask;
				});
			});
		}
		NavigationView FindPlatformFlyoutView(WFrameworkElement aView) =>
			aView.GetParentOfType<NavigationView>();

		FlyoutPage CreateBasicFlyoutPage()
		{
			return new FlyoutPage()
			{
				Detail = new NavigationPage(new ContentPage() { Title = "Detail" }) { Title = "NavigationPage Detail" },
				Flyout = new ContentPage() { Title = "Flyout" }
			};
		}
	}
}
