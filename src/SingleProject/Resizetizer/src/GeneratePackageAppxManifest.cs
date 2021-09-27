﻿#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.Maui.Resizetizer
{
	public class GeneratePackageAppxManifest : Task
	{
		private static readonly XNamespace xmlnsUap = "http://schemas.microsoft.com/appx/manifest/uap/windows10";

		[Required]
		public string IntermediateOutputPath { get; set; } = null!;

		[Required]
		public ITaskItem AppxManifest { get; set; } = null!;

		public string? GeneratedFilename { get; set; }

		public string? ApplicationId { get; set; }

		public string? ApplicationDisplayVersion { get; set; }

		public string? ApplicationVersion { get; set; }

		public string? ApplicationTitle { get; set; }

		public ITaskItem[]? AppIcon { get; set; }

		public ITaskItem[]? SplashScreen { get; set; }

		[Output]
		public ITaskItem GeneratedAppxManifest { get; set; } = null!;

		public override bool Execute()
		{
			try
			{
				Directory.CreateDirectory(IntermediateOutputPath);

				var filename = Path.Combine(IntermediateOutputPath, GeneratedFilename ?? "Package.appxmanifest");

				var appx = XDocument.Load(AppxManifest.ItemSpec);

				UpdateManifest(appx);

				appx.Save(filename);

				GeneratedAppxManifest = new TaskItem(filename);
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex);
			}

			return !Log.HasLoggedErrors;
		}

		void UpdateManifest(XDocument appx)
		{
			var appIconInfo = AppIcon?.Length > 0 ? ResizeImageInfo.Parse(AppIcon[0]) : null;
			var splashInfo = SplashScreen?.Length > 0 ? ResizeImageInfo.Parse(SplashScreen[0]) : null;
			var imageExtension = ".png";

			var xmlns = appx.Root!.GetDefaultNamespace();

			// <Identity Name="" Version="" />
			if (!string.IsNullOrEmpty(ApplicationId) || !string.IsNullOrEmpty(ApplicationDisplayVersion) || !string.IsNullOrEmpty(ApplicationVersion))
			{
				// <Identity>
				var xidentity = xmlns + "Identity";
				var identity = appx.Root.Element(xidentity);
				if (identity == null)
				{
					identity = new XElement(xidentity);
					appx.Root.Add(identity);
				}

				// Name=""
				if (!string.IsNullOrEmpty(ApplicationId))
				{
					Guid.Parse(ApplicationId);

					var xname = "Name";
					var attr = identity.Attribute(xname);
					if (attr == null || string.IsNullOrEmpty(attr.Value))
						identity.SetAttributeValue(xname, ApplicationId);
				}

				// Version=""
				if (!string.IsNullOrEmpty(ApplicationDisplayVersion) || !string.IsNullOrEmpty(ApplicationVersion))
				{
					var parts = ApplicationDisplayVersion?.Split('.') ?? Array.Empty<string>();
					var v = new int[4];
					for (var i = 0; i < 4 && i < parts.Length; i++)
						v[i] = int.Parse(parts[i]);
					if (!string.IsNullOrEmpty(ApplicationVersion))
						v[3] = int.Parse(ApplicationVersion);

					var xname = "Version";
					var attr = identity.Attribute(xname);
					if (attr == null || string.IsNullOrEmpty(attr.Value))
						identity.SetAttributeValue(xname, $"{v[0]:0}.{v[1]:0}.{v[2]:0}.{v[3]:0}");
				}
			}

			// <Properties>
			//   <DisplayName />
			//   <Logo />
			// </Properties>
			if (!string.IsNullOrEmpty(ApplicationTitle) || appIconInfo != null)
			{
				// <Properties>
				var xproperties = xmlns + "Properties";
				var properties = appx.Root.Element(xproperties);
				if (properties == null)
				{
					properties = new XElement(xproperties);
					appx.Root.Add(properties);
				}

				// <DisplayName>
				if (!string.IsNullOrEmpty(ApplicationTitle))
				{
					var xname = xmlns + "DisplayName";
					var xelem = properties.Element(xname);
					if (xelem == null || string.IsNullOrEmpty(xelem.Value))
						properties.SetElementValue(xname, ApplicationTitle);
				}

				// <Logo>
				if (appIconInfo != null)
				{
					var xname = xmlns + "Logo";
					var xelem = properties.Element(xname);
					if (xelem == null || string.IsNullOrEmpty(xelem.Value))
					{
						var dpi = DpiPath.Windows.StoreLogo[0];
						var path = Path.Combine(dpi.Path, appIconInfo.OutputName + dpi.NameSuffix + imageExtension);
						properties.SetElementValue(xname, path);
					}
				}
			}

			// <Applications>
			//   <Application>
			//     <uap:VisualElements DisplayName="" Description="" BackgroundColor="" Square150x150Logo="" Square44x44Logo="">
			//       <uap:DefaultTile Wide310x150Logo="" Square71x71Logo="" Square310x310Logo="" ShortName="">
			//         <uap:ShowNameOnTiles>
			//           <uap:ShowOn />
			//         </uap:ShowNameOnTiles>
			//       </uap:DefaultTile>
			//       <uap:SplashScreen Image="" />
			//     </uap:VisualElements>
			//   </Application>
			// </Applications>
			if (!string.IsNullOrEmpty(ApplicationTitle) || appIconInfo != null || splashInfo != null)
			{
				// <Applications>
				var xapplications = xmlns + "Applications";
				var applications = appx.Root.Element(xapplications);
				if (applications == null)
				{
					applications = new XElement(xapplications);
					appx.Root.Add(applications);
				}

				// <Application>
				var xapplication = xmlns + "Application";
				var application = applications.Element(xapplication);
				if (application == null)
				{
					application = new XElement(xapplication);
					applications.Add(application);
				}

				// <uap:VisualElements>
				var xvisual = xmlnsUap + "VisualElements";
				var visual = application.Element(xvisual);
				if (visual == null)
				{
					visual = new XElement(xvisual);
					application.Add(visual);
				}

				if (!string.IsNullOrEmpty(ApplicationTitle))
				{
					// DisplayName=""
					{
						var xname = "DisplayName";
						var attr = visual.Attribute(xname);
						if (attr == null || string.IsNullOrEmpty(attr.Value))
							visual.SetAttributeValue(xname, ApplicationTitle);
					}

					// Description=""
					{
						var xname = "Description";
						var attr = visual.Attribute(xname);
						if (attr == null || string.IsNullOrEmpty(attr.Value))
							visual.SetAttributeValue(xname, ApplicationTitle);
					}
				}

				// BackgroundColor=""
				{
					var xname = "BackgroundColor";
					var attr = visual.Attribute(xname);
					if (attr == null || string.IsNullOrEmpty(attr.Value))
						visual.SetAttributeValue(xname, "transparent");
				}

				if (appIconInfo != null)
				{
					// Square150x150Logo=""
					{
						var xname = "Square150x150Logo";
						var attr = visual.Attribute(xname);
						if (attr == null || string.IsNullOrEmpty(attr.Value))
						{
							var dpi = DpiPath.Windows.MediumTile[0];
							var path = Path.Combine(dpi.Path, appIconInfo.OutputName + dpi.NameSuffix + imageExtension);
							visual.SetAttributeValue(xname, path);
						}
					}

					// Square44x44Logo=""
					{
						var xname = "Square44x44Logo";
						var attr = visual.Attribute(xname);
						if (attr == null || string.IsNullOrEmpty(attr.Value))
						{
							var dpi = DpiPath.Windows.Logo[0];
							var path = Path.Combine(dpi.Path, appIconInfo.OutputName + dpi.NameSuffix + imageExtension);
							visual.SetAttributeValue(xname, path);
						}
					}
				}

				// <uap:DefaultTile>
				var xtile = xmlnsUap + "DefaultTile";
				var tile = visual.Element(xtile);
				if (tile == null)
				{
					tile = new XElement(xtile);
					visual.Add(tile);
				}

				if (appIconInfo != null)
				{
					// Wide310x150Logo=""
					{
						var xname = "Wide310x150Logo";
						var attr = tile.Attribute(xname);
						if (attr == null || string.IsNullOrEmpty(attr.Value))
						{
							var dpi = DpiPath.Windows.WideTile[0];
							var path = Path.Combine(dpi.Path, appIconInfo.OutputName + dpi.NameSuffix + imageExtension);
							tile.SetAttributeValue(xname, path);
						}
					}

					// Square71x71Logo=""
					{
						var xname = "Square71x71Logo";
						var attr = tile.Attribute(xname);
						if (attr == null || string.IsNullOrEmpty(attr.Value))
						{
							var dpi = DpiPath.Windows.SmallTile[0];
							var path = Path.Combine(dpi.Path, appIconInfo.OutputName + dpi.NameSuffix + imageExtension);
							tile.SetAttributeValue(xname, path);
						}
					}

					// Square310x310Logo=""
					{
						var xname = "Square310x310Logo";
						var attr = tile.Attribute(xname);
						if (attr == null || string.IsNullOrEmpty(attr.Value))
						{
							var dpi = DpiPath.Windows.LargeTile[0];
							var path = Path.Combine(dpi.Path, appIconInfo.OutputName + dpi.NameSuffix + imageExtension);
							tile.SetAttributeValue(xname, path);
						}
					}
				}

				// ShortName=""
				if (!string.IsNullOrEmpty(ApplicationTitle))
				{
					var xname = "ShortName";
					var attr = tile.Attribute(xname);
					if (attr == null || string.IsNullOrEmpty(attr.Value))
						tile.SetAttributeValue(xname, ApplicationTitle);
				}

				// <uap:ShowNameOnTiles>
				var xshowname = xmlnsUap + "ShowNameOnTiles";
				var showname = tile.Element(xshowname);
				if (showname == null)
				{
					showname = new XElement(xshowname);
					tile.Add(showname);
				}

				// <ShowOn>
				var xshowon = xmlnsUap + "ShowOn";
				var showons = showname.Elements(xshowon).ToArray();
				if (showons.All(x => x.Attribute("Tile")?.Value != "square150x150Logo"))
					showname.Add(new XElement(xshowon, new XAttribute("Tile", "square150x150Logo")));
				if (showons.All(x => x.Attribute("Tile")?.Value != "wide310x150Logo"))
					showname.Add(new XElement(xshowon, new XAttribute("Tile", "wide310x150Logo")));

				if (splashInfo != null)
				{
					// <uap:SplashScreen>
					var xsplash = xmlnsUap + "SplashScreen";
					var splash = visual.Element(xsplash);
					if (splash == null)
					{
						splash = new XElement(xsplash);
						visual.Add(splash);
					}

					// Image=""
					{
						var xname = "Image";
						var attr = splash.Attribute(xname);
						if (attr == null || string.IsNullOrEmpty(attr.Value))
						{
							var dpi = DpiPath.Windows.SplashScreen[0];
							var path = Path.Combine(dpi.Path, splashInfo.OutputName + dpi.NameSuffix + imageExtension);
							splash.SetAttributeValue(xname, path);
						}
					}
				}
			}
		}
	}
}