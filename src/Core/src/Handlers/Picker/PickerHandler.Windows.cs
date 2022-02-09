#nullable enable
using WBrush = Microsoft.UI.Xaml.Media.Brush;
using WSelectionChangedEventArgs = Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs;

namespace Microsoft.Maui.Handlers
{
	public partial class PickerHandler : ViewHandler<IPicker, MauiComboBox>
	{
		WBrush? _defaultForeground;

		protected override MauiComboBox CreatePlatformView()
		{
			var platformPicker = new MauiComboBox();

			if (VirtualView != null)
				platformPicker.ItemsSource = new ItemDelegateList<string>(VirtualView);

			return platformPicker;
		}

		protected override void ConnectHandler(MauiComboBox platformView)
		{
			platformView.SelectionChanged += OnControlSelectionChanged;
			SetupDefaults(platformView);
		}

		protected override void DisconnectHandler(MauiComboBox platformView)
		{
			platformView.SelectionChanged -= OnControlSelectionChanged;
		}

		void SetupDefaults(MauiComboBox platformView)
		{
			_defaultForeground = platformView.Foreground;
		}

		void Reload()
		{
			if (VirtualView == null || PlatformView == null)
				return;
			PlatformView.ItemsSource = new ItemDelegateList<string>(VirtualView);
		}

		public static void MapReload(PickerHandler handler, IPicker picker, object? args) => handler.Reload();

		public static void MapTitle(PickerHandler handler, IPicker picker) 
		{
			handler.PlatformView?.UpdateTitle(picker);
		}

		public static void MapTitleColor(PickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateTitle(picker);
		}

		public static void MapSelectedIndex(PickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateSelectedIndex(picker);
		}

		public static void MapCharacterSpacing(PickerHandler handler, IPicker picker) 
		{
			handler.PlatformView?.UpdateCharacterSpacing(picker);
		}

		public static void MapFont(PickerHandler handler, IPicker picker) 
		{
			var fontManager = handler.GetRequiredService<IFontManager>();

			handler.PlatformView?.UpdateFont(picker, fontManager);
		}

		public static void MapTextColor(PickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateTextColor(picker, handler._defaultForeground);
		}

		public static void MapHorizontalTextAlignment(PickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateHorizontalTextAlignment(picker);
		}
		
		public static void MapVerticalTextAlignment(PickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateVerticalTextAlignment(picker);
		}

		void OnControlSelectionChanged(object? sender, WSelectionChangedEventArgs e)
		{
			if (VirtualView != null && PlatformView != null)
				VirtualView.SelectedIndex = PlatformView.SelectedIndex;
		}
	}
}
