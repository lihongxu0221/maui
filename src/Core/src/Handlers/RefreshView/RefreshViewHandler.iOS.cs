﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Handlers
{
	public partial class RefreshViewHandler : ViewHandler<IRefreshView, MauiRefreshView>
	{
		protected override MauiRefreshView CreatePlatformView()
		{
			return new MauiRefreshView();
		}

		protected override void ConnectHandler(MauiRefreshView platformView)
		{
			platformView.RefreshControl.ValueChanged += OnRefresh;
			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(MauiRefreshView platformView)
		{
			platformView.RefreshControl.ValueChanged -= OnRefresh;
			base.DisconnectHandler(platformView);
		}

		public static void MapBackground(RefreshViewHandler handler, IRefreshView view)
			=> handler.PlatformView.RefreshControl.UpdateBackground(view);

		public static void MapIsRefreshing(RefreshViewHandler handler, IRefreshView refreshView)
			=> handler.UpdateIsRefreshing();

		public static void MapContent(RefreshViewHandler handler, IRefreshView refreshView)
			=> handler.UpdateContent();

		public static void MapRefreshColor(RefreshViewHandler handler, IRefreshView refreshView)
			=> handler.UpdateRefreshColor();

		public static void MapIsEnabled(RefreshViewHandler handler, IRefreshView refreshView)
			=> handler.PlatformView?.UpdateIsEnabled(refreshView.IsEnabled);

		void OnRefresh(object? sender, EventArgs e)
		{
			VirtualView.IsRefreshing = true;
		}

		void UpdateIsRefreshing()
		{
			PlatformView.IsRefreshing = VirtualView.IsRefreshing;
		}

		void UpdateContent() =>
			PlatformView.UpdateContent(VirtualView.Content, MauiContext);

		void UpdateRefreshColor()
		{
			var color = VirtualView?.RefreshColor?.ToColor()?.ToPlatform();

			if (color != null)
				PlatformView.RefreshControl.TintColor = color;
		}

	}
}
