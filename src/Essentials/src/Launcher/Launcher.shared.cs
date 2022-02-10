using System;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Essentials
{
	/// <include file="../../docs/Microsoft.Maui.Essentials/Launcher.xml" path="Type[@FullName='Microsoft.Maui.Essentials.Launcher']/Docs" />
	public static partial class Launcher
	{
		/// <include file="../../docs/Microsoft.Maui.Essentials/Launcher.xml" path="//Member[@MemberName='CanOpenAsync'][0]/Docs" />
		public static Task<bool> CanOpenAsync(string uri)
		{
			if (string.IsNullOrWhiteSpace(uri))
				throw new ArgumentNullException(nameof(uri));

			return PlatformCanOpenAsync(new Uri(uri));
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/Launcher.xml" path="//Member[@MemberName='CanOpenAsync'][1]/Docs" />
		public static Task<bool> CanOpenAsync(Uri uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			return PlatformCanOpenAsync(uri);
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/Launcher.xml" path="//Member[@MemberName='OpenAsync'][0]/Docs" />
		public static Task OpenAsync(string uri)
		{
			if (string.IsNullOrWhiteSpace(uri))
				throw new ArgumentNullException(nameof(uri));

			return PlatformOpenAsync(new Uri(uri));
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/Launcher.xml" path="//Member[@MemberName='OpenAsync'][1]/Docs" />
		public static Task OpenAsync(Uri uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			return PlatformOpenAsync(uri);
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/Launcher.xml" path="//Member[@MemberName='OpenAsync'][2]/Docs" />
		public static Task OpenAsync(OpenFileRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));
			if (request.File == null)
				throw new ArgumentNullException(nameof(request.File));

			return PlatformOpenAsync(request);
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/Launcher.xml" path="//Member[@MemberName='TryOpenAsync'][0]/Docs" />
		public static Task<bool> TryOpenAsync(string uri)
		{
			if (string.IsNullOrWhiteSpace(uri))
				throw new ArgumentNullException(nameof(uri));

			return PlatformTryOpenAsync(new Uri(uri));
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/Launcher.xml" path="//Member[@MemberName='TryOpenAsync'][1]/Docs" />
		public static Task<bool> TryOpenAsync(Uri uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			return PlatformTryOpenAsync(uri);
		}
	}

	/// <include file="../../docs/Microsoft.Maui.Essentials/OpenFileRequest.xml" path="Type[@FullName='Microsoft.Maui.Essentials.OpenFileRequest']/Docs" />
	public class OpenFileRequest
	{
		/// <include file="../../docs/Microsoft.Maui.Essentials/OpenFileRequest.xml" path="//Member[@MemberName='.ctor'][0]/Docs" />
		public OpenFileRequest()
		{
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/OpenFileRequest.xml" path="//Member[@MemberName='.ctor'][2]/Docs" />
		public OpenFileRequest(string title, ReadOnlyFile file)
		{
			Title = title;
			File = file;
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/OpenFileRequest.xml" path="//Member[@MemberName='.ctor'][1]/Docs" />
		public OpenFileRequest(string title, FileBase file)
		{
			Title = title;
			File = new ReadOnlyFile(file);
		}

		/// <include file="../../docs/Microsoft.Maui.Essentials/OpenFileRequest.xml" path="//Member[@MemberName='Title']/Docs" />
		public string Title { get; set; }

		/// <include file="../../docs/Microsoft.Maui.Essentials/OpenFileRequest.xml" path="//Member[@MemberName='File']/Docs" />
		public ReadOnlyFile File { get; set; }

		/// <include file="../../docs/Microsoft.Maui.Essentials/OpenFileRequest.xml" path="//Member[@MemberName='PresentationSourceBounds']/Docs" />
		public Rectangle PresentationSourceBounds { get; set; } = Rectangle.Zero;
	}
}
