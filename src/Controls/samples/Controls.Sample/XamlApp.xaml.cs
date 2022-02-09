using System;
using System.Diagnostics;
using System.IO;
using Maui.Controls.Sample.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace Maui.Controls.Sample
{
	public partial class XamlApp : Application
	{
		public XamlApp(IServiceProvider services, ITextService textService)
		{
			InitializeComponent();

			Services = services;

			Debug.WriteLine($"The injected text service had a message: '{textService.GetText()}'");

			RequestedThemeChanged += (sender, args) =>
			{
				// Respond to the theme change
				Debug.WriteLine($"Requested theme changed: {args.RequestedTheme}");
			};

			LoadAsset();
		}

		async void LoadAsset()
		{
			try
			{
				using var stream = await FileSystem.OpenAppPackageFileAsync("RawAsset.txt");
				using var reader = new StreamReader(stream);

				Debug.WriteLine($"The raw Maui asset contents: '{reader.ReadToEnd().Trim()}'");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error loading the raw Maui asset contents: {ex}");
			}
		}

		// Must not use MainPage for multi-window
		protected override Window CreateWindow(IActivationState activationState)
		{
			var window = new Window(Services.GetRequiredService<Page>());

			var menuBarItem = new MenuBarItem();
			menuBarItem.Text = "Bar Item";

			menuBarItem.Add(new MenuFlyoutItem()
			{
				Text = "Flyout Item"
			});

			window.MenuBar = new MenuBar()
			{
				menuBarItem
			};

			window.Title = ".NET MAUI Samples Gallery";
			return window;
		}

		public IServiceProvider Services { get; }
	}
}
