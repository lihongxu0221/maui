﻿using System;
using System.Threading.Tasks;

namespace Microsoft.Maui.Handlers
{
	public partial class WebViewHandler : ViewHandler<IWebView, object>
	{
		protected override object CreatePlatformView() => throw new NotImplementedException();

		public static void MapSource(IViewHandler handler, IWebView webView) { }

		public static void MapGoBack(IViewHandler handler, IWebView webView, object? arg) { }
		public static void MapGoForward(IViewHandler handler, IWebView webView, object? arg) { }
		public static void MapReload(IViewHandler handler, IWebView webView, object? arg) { }
		public static void MapEval(IViewHandler handler, IWebView webView, object? arg) { }
		public static void MapEvaluateJavaScriptAsync(IViewHandler handler, IWebView webView, object? arg) { }
	}
}