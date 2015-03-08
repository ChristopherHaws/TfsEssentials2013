using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using Spiral.TfsEssentials.Extentions;

namespace Spiral.TfsEssentials.Providers
{
	[Export]
	internal class PackageConfigurationManager
	{
		private readonly Package package;
		private const string RootKey = "TfsEssentials2013";

		[ImportingConstructor]
		public PackageConfigurationManager([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			package = serviceProvider.GetService<Package>();
		}

		public string GetValue(string key)
		{
			using (var registryKey = package.UserRegistryRoot.OpenSubKey(RootKey, false))
			{
				if (registryKey == null)
				{
					Debug.Write("Could not open registry key.");
					return null;
				}

				return registryKey.GetValue(key) as string;
			}
		}

		public void SetValue(string key, string value)
		{
			EnsureRootKeyExists();

			using (var registryKey = package.UserRegistryRoot.OpenSubKey(RootKey, true))
			{
				if (registryKey == null)
				{
					Debug.Write("Could not open registry key.");
					return;
				}

				registryKey.SetValue(key, value, RegistryValueKind.String);
			}
		}

		private void EnsureRootKeyExists()
		{
			using (var registryKey = package.UserRegistryRoot.OpenSubKey(RootKey))
			{
				if (registryKey == null)
				{
					package.UserRegistryRoot.CreateSubKey(RootKey);
				}
			}
		}
	}
}